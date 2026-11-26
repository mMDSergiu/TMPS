using CoffeeShop.Models;

namespace CoffeeShop.Actions;

public class StartPreparing : StatusAction
{
    public StartPreparing(Order order) : base(order) { }

    public override string Name => "prepare #" + order.Id;
    protected override void Apply() => order.StartPreparing();
}

public class MarkReady : StatusAction
{
    public MarkReady(Order order) : base(order) { }

    public override string Name => "ready #" + order.Id;
    protected override void Apply() => order.MarkReady();
}


public class HandOver : StatusAction
{
    public HandOver(Order order) : base(order)
    {
    }

    public override string Name => $"give #{order.Id} to {order.CustomerName}";

    protected override void Apply()
    {
        order.HandOver();
    }
}

public class CancelOrder : StatusAction
{
    public CancelOrder(Order order) : base(order) { }

    public override string Name => "cancel #" + order.Id;
    protected override void Apply() => order.Cancel();
}
