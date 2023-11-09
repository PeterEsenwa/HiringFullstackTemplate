using STask = System.Threading.Tasks.Task;
using CoverGo.Task.Api.Controllers;
using CoverGo.Task.Domain;
using CoverGo.Task.Application;
using CoverGo.Task.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CoverGo.Task.Api.Tests;

public class ProposalsControllerTest
{
    private readonly Mock<IProposalsQuery> _mockProposalsQuery;
    private readonly Mock<IProposalsWriteRepository> _mockProposalsWriteRepository;
    private readonly Mock<IPlansQuery> _mockPlansQuery;
    private readonly ProposalsController _controller;

    public ProposalsControllerTest()
    {
        _mockProposalsQuery = new Mock<IProposalsQuery>();
        _mockProposalsWriteRepository = new Mock<IProposalsWriteRepository>();
        _mockPlansQuery = new Mock<IPlansQuery>();
        _controller = new ProposalsController(_mockProposalsQuery.Object, _mockProposalsWriteRepository.Object,
            _mockPlansQuery.Object);
    }

    [Fact]
    public async STask GetAll_ShouldReturnProposals()
    {
        var proposals = new List<Proposal> { new Proposal("Test Company") };
        _mockProposalsQuery.Setup(repo => repo.GetAllAsync(CancellationToken.None)).ReturnsAsync(proposals);

        var actionResult = await _controller.GetAll();

        var result = Assert.IsType<ActionResult<List<Proposal>>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(proposals, okResult.Value);
    }

    [Fact]
    public async STask GetOne_ShouldReturnProposal()
    {
        var proposalId = Guid.NewGuid();
        var proposal = new Proposal("Test Company");
        _mockProposalsQuery.Setup(repo => repo.GetByIdAsync(proposalId, CancellationToken.None)).ReturnsAsync(proposal);

        var actionResult = await _controller.GetOne(proposalId);

        var result = Assert.IsType<ActionResult<Proposal?>>(actionResult);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(proposal, okResult.Value);
    }

    [Fact]
    public async STask CreateProposal_ShouldReturnCreatedProposal()
    {
        const string clientCompanyName = "New Company";
        _mockProposalsWriteRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Proposal>(), CancellationToken.None))
            .ReturnsAsync((Proposal proposal, CancellationToken ct) => proposal); // Return the passed proposal

        var actionResult = await _controller.CreateProposal(clientCompanyName, CancellationToken.None);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedProposal = Assert.IsType<Proposal>(createdAtActionResult.Value);

        Assert.Equal(clientCompanyName, returnedProposal.CompanyName);
        Assert.Equal("GetOne", createdAtActionResult.ActionName); // Make sure this matches the actual action name
    }


    [Fact]
    public async STask AddInsuredGroup_ShouldReturnUpdatedProposal()
    {
        var proposalId = Guid.NewGuid();
        var dto = new AddInsuredGroupDto { NumberOfEmployees = 5, PlanId = "Test Plan" };
        var plan = new Plan
        {
            Id = dto.PlanId,
            Name = "Plan Name",
            Cost = 0
        };
        var proposal = new Proposal("Test Company");

        _mockProposalsQuery.Setup(repo => repo.GetByIdAsync(proposalId, CancellationToken.None)).ReturnsAsync(proposal);
        _mockPlansQuery.Setup(repo => repo.ExecuteAsync(dto.PlanId)).ReturnsAsync(plan);
        _mockProposalsWriteRepository.Setup(repo => repo.UpdateAsync(proposal, CancellationToken.None))
            .ReturnsAsync(proposal);

        var actionResult = await _controller.AddInsuredGroup(proposalId, dto, CancellationToken.None);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(proposal, createdAtActionResult.Value);
        Assert.Equal(nameof(ProposalsController.GetOne), createdAtActionResult.ActionName);
    }
}