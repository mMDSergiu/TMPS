namespace CoffeeShop.Statuses;

public class Collected : OrderStatus
{
    public override string Name => "collected";
}

public class Cancelled : OrderStatus
{
    public override string Name => "cancelled";
}
