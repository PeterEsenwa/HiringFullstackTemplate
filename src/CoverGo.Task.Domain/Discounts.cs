namespace CoverGo.Task.Domain;

public class Discounts
{
    public Discounts(Dictionary<string, int> discountedPlans, decimal discount)
    {
        DiscountedPlans = discountedPlans;
        Discount = discount;
    }

    public Guid Id { get; private set; }
    
    public Dictionary<string, int> DiscountedPlans { get; private set; }
    
    public decimal Discount { get; private set; }
}