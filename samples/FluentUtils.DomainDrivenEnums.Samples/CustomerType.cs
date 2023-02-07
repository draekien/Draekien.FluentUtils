namespace FluentUtils.DomainDrivenEnums.Samples;

public sealed class CustomerType : DomainDrivenEnum
{
    public static readonly CustomerType Standard = new(0, "Standard", 0.0, 4);
    public static readonly CustomerType Premium = new(1, "Premium", 0.1, 2);
    public static readonly CustomerType Vip = new(2, "VIP", 0.2, 1);

    private CustomerType(int value, string displayName, double discount, int serviceLevelAgreementDays) : base(
        value,
        displayName)
    {
        Discount = discount;
        ServiceLevelAgreementDays = serviceLevelAgreementDays;
    }

    public int ServiceLevelAgreementDays { get; }

    public double Discount { get; }
}

public sealed class Customer
{
    public Customer(Guid id, CustomerType? customerType)
    {
        Id = id;
        CustomerType = customerType ?? CustomerType.Standard;
    }

    public Guid Id { get; }

    public CustomerType CustomerType { get; }
}

public sealed class UsageExample
{
    public IEnumerable<Customer> GetBySlaGreaterThan2Days(IEnumerable<Customer> customers)
    {
        return customers.Where(c => c.CustomerType.ServiceLevelAgreementDays >= 2);
    }

    public IEnumerable<Customer> GetPremiumCustomers(IEnumerable<Customer> customers)
    {
        return customers.Where(c => c.CustomerType == CustomerType.Premium);
    }
}
