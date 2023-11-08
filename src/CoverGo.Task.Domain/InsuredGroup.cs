namespace CoverGo.Task.Domain;

public class InsuredGroup
{
    public Guid Id { get; private set; }
    public int NumberOfEmployees { get; private set; }
    public Plan Plan { get; private set; }

    public InsuredGroup(int numberOfEmployees, Plan plan)
    {
        Id = Guid.NewGuid();
        NumberOfEmployees = numberOfEmployees;
        Plan = plan;
    }
    
    public void Update(int numberOfEmployees, Plan plan)
    {
        NumberOfEmployees = numberOfEmployees;
        Plan = plan;
    }
}