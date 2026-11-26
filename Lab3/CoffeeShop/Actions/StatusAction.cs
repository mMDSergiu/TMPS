using CoffeeShop.Models;
using CoffeeShop.Statuses;

namespace CoffeeShop.Actions;

public abstract class StatusAction : IOrderAction
{
    protected readonly Order order;
    private OrderStatus? before;

    protected StatusAction(Order order)
    {
        this.order = order;
    }

    public abstract string Name { get; }

    public void Execute()
    {
        var old = order.Status;
        Apply();
        before = old;
    }

    public void Undo()
    {
        if (before == null)
            return;

        order.SetStatus(before);
        before = null;
    }

    protected abstract void Apply();
}
