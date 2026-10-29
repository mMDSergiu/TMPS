using CoffeeShop.Models;

namespace CoffeeShop.Pricing;

public interface IPriceCalculator
{
    decimal GetSubtotal(Order order);
    decimal GetTotal(Order order, IDiscount discount);
}
