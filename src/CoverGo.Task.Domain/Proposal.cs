public class Proposal
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    // public List<InsuredGroup> InsuredGroups { get; private set; }

    public Proposal(string companyName)
    {
        Id = Guid.NewGuid();
        CompanyName = companyName;
        // InsuredGroups = new List<InsuredGroup>();
    }

    // Additional methods like adding insured groups will be implemented here.
}