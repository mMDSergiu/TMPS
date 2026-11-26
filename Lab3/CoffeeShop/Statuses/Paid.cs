using CoffeeShop.Models;

namespace CoffeeShop.Statuses;

public class Paid : OrderStatus
{
    public override string Name => "paid";

    public override void StartPreparing(Order order)
    {
        order.SetStatus(new Preparing());
    }

    public override void Cancel(Order order)
    {
        order.SetStatus(new Cancelled());
    }
}
