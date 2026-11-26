using CoffeeShop.Models;

namespace CoffeeShop.Statuses;

public class Placed : OrderStatus
{
    public override string Name => "placed";
    public override bool CanChangeItems => true;

    public override void Pay(Order order) => order.SetStatus(new Paid());

    public override void Cancel(Order order) => order.SetStatus(new Cancelled());
}
