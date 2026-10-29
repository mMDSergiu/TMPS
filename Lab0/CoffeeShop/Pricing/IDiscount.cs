namespace CoffeeShop.Pricing;

public interface IDiscount
{
    string Name { get; }
    decimal Apply(decimal price);
}
