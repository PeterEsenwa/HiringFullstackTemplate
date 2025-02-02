using CoverGo.Task.Domain;

namespace CoverGo.Task.Application;

public interface IPlansQuery
{
    public ValueTask<List<Plan>> ExecuteAsync();    
    
    public ValueTask<Plan?> ExecuteAsync(string planId);
}
