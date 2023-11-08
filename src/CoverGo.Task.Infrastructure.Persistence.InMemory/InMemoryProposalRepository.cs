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
    private readonly ConcurrentDictionary<Guid, Proposal> _proposals = new ConcurrentDictionary<Guid, Proposal>();

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

    public ValueTask<Proposal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _proposals.TryGetValue(id, out var proposal);
        return ValueTask.FromResult(proposal);
    }
}