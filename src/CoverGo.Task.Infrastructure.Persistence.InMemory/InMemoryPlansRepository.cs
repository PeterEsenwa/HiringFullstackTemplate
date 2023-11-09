using System.Collections.Immutable;

using CoverGo.Task.Application;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class InMemoryPlansRepository : IPlansQuery, IPlansWriteRepository
{
    private readonly ImmutableList<Plan> _seedwork = new List<Plan>
    {
        new() { Id = "0", Name = "Base", Cost = 500 },
        new() { Id = "1", Name = "Premium", Cost = 750 },
    }.ToImmutableList();

    public ValueTask<Plan> GetById(string id, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(_seedwork.Single(it => it.Id == id));
    }

    public ValueTask<List<Plan>> ExecuteAsync()
    {
        return ValueTask.FromResult(_seedwork.ToList());
    }

    public ValueTask<Plan?> ExecuteAsync(string planId)
    {
        return ValueTask.FromResult(_seedwork.SingleOrDefault(it => it.Id == planId.ToString()));
    }
}
