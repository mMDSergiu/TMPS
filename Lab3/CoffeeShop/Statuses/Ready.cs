using CoffeeShop.Models;

namespace CoffeeShop.Statuses;

public class Ready : OrderStatus
{
    public override string Name => "ready";


    public override void HandOver(Order order)
    {
        order.SetStatus(new Collected());
    }
}
