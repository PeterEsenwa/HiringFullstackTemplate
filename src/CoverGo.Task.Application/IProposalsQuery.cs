using CoverGo.Task.Domain;

namespace CoverGo.Task.Application;

public interface IProposalsQuery
{
    public ValueTask<Proposal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public ValueTask<List<Proposal>> GetAllAsync(CancellationToken cancellationToken = default);
}
