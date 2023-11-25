using System.Collections.Concurrent;
using CoverGo.Task.Application;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class InMemoryProposalsRepository : IProposalsQuery, IProposalsWriteRepository
{
    private const string DemoProposalName = "CoverGo";
    private readonly ConcurrentDictionary<Guid, Proposal> _storedProposals = new(new[]
    {
        new KeyValuePair<Guid, Proposal>(Guid.NewGuid(), new Proposal(DemoProposalName)),
    });

    public ValueTask<Proposal> AddAsync(Proposal newProposal, CancellationToken cancellationToken = default)
    {
        if (!_storedProposals.TryAdd(newProposal.Id, newProposal))
        {
            throw new InvalidOperationException("Could not add proposal.");
        }

        return ValueTask.FromResult(newProposal);
    }

    public ValueTask<List<Proposal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var allProposals = _storedProposals.Values.ToList();
        return ValueTask.FromResult(allProposals);
    }

    public ValueTask<Proposal?> GetByIdAsync(Guid proposalId, CancellationToken cancellationToken = default)
    {
        var existingProposal = _storedProposals.Values.FirstOrDefault(proposal => proposal.Id == proposalId);
        return ValueTask.FromResult(existingProposal);
    }

    public ValueTask<Proposal> UpdateAsync(Proposal updatedProposal, CancellationToken cancellationToken = default)
    {
        try
        {
            _storedProposals[updatedProposal.Id] = updatedProposal;
        }
        catch (Exception exception)
        {
            LogException(exception);
            throw;
        }
        
        return ValueTask.FromResult(updatedProposal);
    }
    
    private static void LogException(Exception exception)
    {
        Console.WriteLine(exception);
    }
}