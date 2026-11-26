using CoffeeShop.Output;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;

namespace CoffeeShop.Services;

public class SalesReport
{
    private readonly IOrderRepository orders;
    private readonly IPriceCalculator calculator;
    private readonly IOutput output;

    public SalesReport(IOrderRepository orders, IPriceCalculator calculator, IOutput output)
    {
        this.orders = orders;
        this.calculator = calculator;
        this.output = output;
    }

    public void Print()
    {
        var all = orders.GetAll();
        decimal sum = 0;

        output.WriteLine("SALES TODAY");
        foreach (var order in all)
        {
            var value = calculator.GetSubtotal(order);
            sum += value;
            output.WriteLine($"  #{order.Id} {order.CustomerName,-10}{value,6} lei");
        }
        output.WriteLine($"{all.Count} orders, {sum} lei (before discounts)");
    }
}
