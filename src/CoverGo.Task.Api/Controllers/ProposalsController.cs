using CoverGo.Task.Api.DTOs;
using CoverGo.Task.Application;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProposalsController : ControllerBase
{
    private readonly IPlansQuery _plansQuery;
    private readonly IProposalsQuery _proposalsQuery;
    private readonly IProposalsWriteRepository _proposalsWriteRepository;

    public ProposalsController(IProposalsQuery proposalsQuery, IProposalsWriteRepository proposalsWriteRepository, IPlansQuery plansQuery)
    {
        _proposalsQuery = proposalsQuery;
        _proposalsWriteRepository = proposalsWriteRepository;
        _plansQuery = plansQuery;
    }

    [HttpGet(Name = "GetProposals")]
    public async ValueTask<ActionResult<List<Proposal>>> GetAll()
    {
        return await _proposalsQuery.GetAllAsync();
    }

    [HttpGet("{proposalId}", Name = "GetProposal")]
    public async ValueTask<ActionResult<Proposal?>> GetOne(Guid proposalId)
    {
        return await _proposalsQuery.GetByIdAsync(proposalId);
    }

    [HttpPost(Name = "CreateProposal")]
    public async ValueTask<ActionResult<Proposal>> CreateProposal([FromBody] string clientCompanyName, CancellationToken cancellationToken)
    {
        var proposal = new Proposal(clientCompanyName);
        await _proposalsWriteRepository.AddAsync(proposal, cancellationToken);

        return CreatedAtAction(nameof(GetOne), new { proposalId = proposal.Id }, proposal);
    }
    
    [HttpPost("{proposalId}/insured-groups", Name = "AddInsuredGroup")]
    public async ValueTask<ActionResult<Proposal>> AddInsuredGroup(Guid proposalId, [FromBody] AddInsuredGroupDto dto, CancellationToken cancellationToken)
    {
        var proposal = await _proposalsQuery.GetByIdAsync(proposalId, cancellationToken);
        if (proposal == null)
        {
            return NotFound();
        }
        
        var plan = await _plansQuery.ExecuteAsync(dto.PlanId);
        
        proposal.AddInsuredGroup(dto.NumberOfEmployees, plan);
        await _proposalsWriteRepository.UpdateAsync(proposal, cancellationToken);

        return CreatedAtAction(nameof(GetOne), new { proposalId = proposal.Id }, proposal);
    }
}