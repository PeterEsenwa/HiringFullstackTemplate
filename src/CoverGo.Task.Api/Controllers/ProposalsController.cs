using CoverGo.Task.Application;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProposalsController : ControllerBase
{
    private readonly IProposalsQuery _proposalsQuery;
    private readonly IProposalsWriteRepository _proposalsWriteRepository;

    public ProposalsController(IProposalsQuery proposalsQuery, IProposalsWriteRepository proposalsWriteRepository)
    {
        _proposalsQuery = proposalsQuery;
        _proposalsWriteRepository = proposalsWriteRepository;
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

    // CreateProposal
    [HttpPost(Name = "CreateProposal")]
    public async ValueTask<ActionResult<Proposal>> CreateProposal([FromBody] string clientCompanyName, CancellationToken cancellationToken)
    {
        var proposal = new Proposal(clientCompanyName);
        await _proposalsWriteRepository.AddAsync(proposal, cancellationToken);

        // The route values object should have the same property name as the parameter in the GetOne method
        return CreatedAtAction(nameof(GetOne), new { proposalId = proposal.Id }, proposal);
    }
}