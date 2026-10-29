using CoffeeShop.Models;

namespace CoffeeShop.Pricing;

public class PriceCalculator
{
    public decimal GetSubtotal(Order order)
    {
        decimal sum = 0;

        foreach (var d in order.Drinks)
            sum += d.Price;

        return sum;
    }


    public decimal GetTotal(Order order, IDiscount discount)
    {
        var total = discount.Apply(GetSubtotal(order));
        return Math.Round(total, 2);
    }
}
