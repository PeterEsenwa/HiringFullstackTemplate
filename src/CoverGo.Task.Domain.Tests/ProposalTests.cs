namespace CoverGo.Task.Domain.Tests;

public class ProposalTests
{
    private readonly Plan _samplePlan = new()
    {
        Id = "1",
        Name = "Plan Name",
        Cost = 0
    };

    [Fact]
    public void Constructor_InitializesProperties()
    {
        const string companyName = "Test Company";

        var proposal = new Proposal(companyName);

        Assert.Equal(companyName, proposal.CompanyName);
        Assert.NotEqual(Guid.Empty, proposal.Id);
    }

    [Fact]
    public void AddInsuredGroup_AddsInsuredGroupToList()
    {
        var proposal = new Proposal("Test Company");
        const int numberOfEmployees = 10;

        proposal.AddInsuredGroup(numberOfEmployees, _samplePlan);

        Assert.Single(proposal.InsuredGroups);
        var insuredGroup = proposal.InsuredGroups.First();
        Assert.Equal(numberOfEmployees, insuredGroup.NumberOfEmployees);
        Assert.Equal(_samplePlan, insuredGroup.Plan);
    }

    [Fact]
    public void RemoveInsuredGroup_RemovesGroupWithSpecifiedId()
    {
        var proposal = new Proposal("Test Company");
        proposal.AddInsuredGroup(10, _samplePlan);
        var insuredGroupId = proposal.InsuredGroups.First().Id;

        proposal.RemoveInsuredGroup(insuredGroupId);

        Assert.Empty(proposal.InsuredGroups);
    }

    [Fact]
    public void RemoveInsuredGroup_DoesNothingWhenIdNotFound()
    {
        var proposal = new Proposal("Test Company");
        proposal.AddInsuredGroup(10, _samplePlan);
        var nonExistingId = Guid.NewGuid();

        proposal.RemoveInsuredGroup(nonExistingId);

        Assert.Single(proposal.InsuredGroups);
    }

    [Fact]
    public void UpdateInsuredGroup_UpdatesGroupWithSpecifiedId()
    {
        var proposal = new Proposal("Test Company");
        proposal.AddInsuredGroup(10, _samplePlan);
        var insuredGroupId = proposal.InsuredGroups.First().Id;
        const int newNumberOfEmployees = 20;
        var newPlan = new Plan
        {
            Id = "1",
            Name = "New Plan Name",
            Cost = 0
        };

        proposal.UpdateInsuredGroup(insuredGroupId, newNumberOfEmployees, newPlan);

        var insuredGroup = proposal.InsuredGroups.First();
        Assert.Equal(newNumberOfEmployees, insuredGroup.NumberOfEmployees);
        Assert.Equal(newPlan, insuredGroup.Plan);
    }

    [Fact]
    public void UpdateInsuredGroup_ThrowsExceptionWhenIdNotFound()
    {
        var proposal = new Proposal("Test Company");
        proposal.AddInsuredGroup(10, _samplePlan);
        var nonExistingId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() =>
            proposal.UpdateInsuredGroup(nonExistingId, 20, new Plan
            {
                Id = "1",
                Name = "New Plan Name",
                Cost = 0
            }));
    }

    [Fact]
    public void TotalCost_ReturnsSumOfInsuredGroupsCost()
    {
        var proposal = new Proposal("Test Company", isDiscounted: false);
        proposal.AddInsuredGroup(10, _samplePlan);
        proposal.AddInsuredGroup(5, _samplePlan);
        var expectedTotalCost = proposal.InsuredGroups.Sum(ig => ig.Plan.Cost * ig.NumberOfEmployees);

        var totalCost = proposal.TotalCost;

        Assert.Equal(expectedTotalCost, totalCost);
    }

    [Fact]
    public void TotalCost_AppliesDiscountCorrectly()
    {
        var proposal = new Proposal("Test Company", isDiscounted: true);
        proposal.AddInsuredGroup(10, _samplePlan);
        var expectedTotalCost =
            proposal.InsuredGroups.Sum(ig => ig.Plan.Cost * (ig.NumberOfEmployees - 1)); // Discount applied

        var totalCost = proposal.TotalCost;

        Assert.Equal(expectedTotalCost, totalCost);
    }
}