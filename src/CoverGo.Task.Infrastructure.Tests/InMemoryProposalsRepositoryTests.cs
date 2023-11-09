using STask = System.Threading.Tasks.Task;
using CoverGo.Task.Infrastructure.Persistence.InMemory;

namespace CoverGo.Task.Infrastructure.Tests;

public class InMemoryProposalsRepositoryTests
{
    [Fact]
    public async STask AddAsync_AddsProposal()
    {
        // Arrange
        var repository = new InMemoryProposalsRepository();
        var proposal = new Proposal("Test Company");

        // Act
        var result = await repository.AddAsync(proposal);

        // Assert
        Assert.Equal(proposal, result);
    }
}