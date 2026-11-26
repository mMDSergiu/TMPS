using CoffeeShop.Models;

namespace CoffeeShop.Statuses;

public class Preparing : OrderStatus
{
    public override string Name => "preparing";

    public override void MarkReady(Order order) => order.SetStatus(new Ready());
}
