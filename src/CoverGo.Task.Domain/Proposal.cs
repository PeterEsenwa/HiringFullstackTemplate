namespace CoverGo.Task.Domain;

public class Proposal
{
    private decimal _discount;

    public Proposal(string companyName)
    {
        Id = Guid.NewGuid();
        CompanyName = companyName;
        InsuredGroups = new List<InsuredGroup>();
    }

    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public List<InsuredGroup> InsuredGroups { get; }

    public Discounts? AppliedDiscount { get; private set; }

    public decimal TotalCost => InsuredGroups.Sum(x => (x.Plan.Cost * x.NumberOfEmployees) - _discount);

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

    public void AddDiscount(Discounts discounts)
    {
        AppliedDiscount = discounts;
        CalculateDiscount();
    }

    public void AddDiscount(Dictionary<string, int> discountedPlans, decimal discount)
    {
        AppliedDiscount = new Discounts(discountedPlans, discount);
        CalculateDiscount();
    }

    public void RemoveDiscount()
    {
        AppliedDiscount = null;
        CalculateDiscount();
    }

    private bool IsInsuredGroupDiscounted(InsuredGroup insuredGroup)
    {
        if (AppliedDiscount == null)
        {
            return false;
        }

        if (!AppliedDiscount.DiscountedPlans.ContainsKey(insuredGroup.Plan.Id))
        {
            return false;
        }

        var discountedPlan = AppliedDiscount.DiscountedPlans[insuredGroup.Plan.Id];

        return insuredGroup.NumberOfEmployees >= discountedPlan;
    }

    private bool IsDiscountedPlanAccountedFor(string planId, List<InsuredGroup> eligibleGroups)
    {
        var isAccountedFor = false;

        foreach (var eligibleGroup in eligibleGroups)
        {
            if (planId != eligibleGroup.Plan.Id) continue;

            isAccountedFor = true;
            break;
        }

        return isAccountedFor;
    }

    private void CalculateDiscount()
    {
        if (AppliedDiscount == null)
        {
            _discount = 0;
            return;
        }

        // Check each InsuredGroup if it is eligible for discount
        var eligibleGroups = InsuredGroups.Where(x => IsInsuredGroupDiscounted(x)).ToList();

        var allPlansAccountedFor = true;

        // every plan in the DiscountedPlans dictionary must have at least one InsuredGroup that is eligible for discount
        foreach (var discountedPlan in AppliedDiscount.DiscountedPlans)
        {
            if (IsDiscountedPlanAccountedFor(discountedPlan.Key, eligibleGroups)) continue;

            allPlansAccountedFor = false;
            break;
        }

        if (!allPlansAccountedFor)
        {
            _discount = 0;
            return;
        }

        _discount = AppliedDiscount.Discount;
    }
}