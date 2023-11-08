using CoverGo.Task.Domain;

namespace CoverGo.Task.Application;

public interface IProposalsWriteRepository
{
    public ValueTask<Proposal> AddAsync(Proposal proposal, CancellationToken cancellationToken = default);
}
