namespace CoverGo.Task.Domain;

public class Proposal
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    private List<InsuredGroup> InsuredGroups { get; }

    private bool IsDiscounted { get; }

    public Proposal(string companyName, bool isDiscounted = false)
    {
        Id = Guid.NewGuid();
        CompanyName = companyName;
        IsDiscounted = isDiscounted;
        InsuredGroups = new List<InsuredGroup>();
    }

    public void AddInsuredGroup(int numberOfEmployees, Plan plan)
    {
        InsuredGroups.Add(new InsuredGroup(numberOfEmployees, plan));
    }
    
    public void RemoveInsuredGroup(Guid insuredGroupId)
    {
        InsuredGroups.RemoveAll(x => x.Id == insuredGroupId);
    }
    
    public void UpdateInsuredGroup(Guid insuredGroupId, int numberOfEmployees, Plan plan)
    {
        var insuredGroup = InsuredGroups.FirstOrDefault(x => x.Id == insuredGroupId);
        if (insuredGroup == null)
        {
            throw new ArgumentException($"Insured group with id {insuredGroupId} does not exist");
        }
        
        insuredGroup.Update(numberOfEmployees, plan);
    }
    
    public decimal TotalCost => InsuredGroups.Sum(x => x.Plan.Cost * (IsDiscounted ? x.NumberOfEmployees - 1 : x.NumberOfEmployees));

}