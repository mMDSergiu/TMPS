using CoffeeShop.Models;
using CoffeeShop.Output;
using CoffeeShop.Statuses;

namespace CoffeeShop.Listeners;

public class PickupScreen : IOrderListener
{
    private readonly IOutput output;

    public PickupScreen(IOutput output)
    {
        this.output = output;
    }

    public void StatusChanged(Order order)
    {
        string text = order.Status is Ready ? "READY, come and get it" : order.Status.Name;
        output.WriteLine($"[screen] #{order.Id} {order.CustomerName}: {text}");
    }
}
