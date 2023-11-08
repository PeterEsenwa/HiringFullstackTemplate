using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoverGo.Task.Application;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class InMemoryProposalsRepository : IProposalsQuery, IProposalsWriteRepository
{
    private readonly ConcurrentDictionary<Guid, Proposal> _proposals = new(new[]
    {
        new KeyValuePair<Guid, Proposal>(Guid.NewGuid(), new Proposal("CoverGo")),
    });

    public ValueTask<Proposal> AddAsync(Proposal proposal, CancellationToken cancellationToken = default)
    {
        if (_proposals.TryAdd(proposal.Id, proposal))
        {
            return ValueTask.FromResult(proposal);
        }
        else
        {
            throw new InvalidOperationException("Could not add proposal.");
        }
    }

    public ValueTask<List<Proposal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_proposals.Values.ToList());
    }

    public ValueTask<Proposal?> GetByIdAsync(Guid proposalId, CancellationToken cancellationToken = default)
    {
        var proposal = _proposals.Values.FirstOrDefault(x => x.Id == proposalId);
        
        return ValueTask.FromResult(proposal);
    }
    
    public ValueTask<Proposal> UpdateAsync(Proposal proposal, CancellationToken cancellationToken = default)
    {
        try
        {
            _proposals[proposal.Id] = proposal;
            
            return ValueTask.FromResult(proposal);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}