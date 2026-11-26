using CoffeeShop.Models;

namespace CoffeeShop.Statuses;

public abstract class OrderStatus
{
    public abstract string Name { get; }

    public virtual bool CanChangeItems => false;


    public virtual void Pay(Order order) => Refuse(order, "pay");
    public virtual void StartPreparing(Order order) => Refuse(order, "start preparing");
    public virtual void MarkReady(Order order) => Refuse(order, "mark as ready");
    public virtual void HandOver(Order order) => Refuse(order, "hand over");
    public virtual void Cancel(Order order) => Refuse(order, "cancel");

    protected void Refuse(Order order, string action)
    {
        throw new InvalidOperationException($"cant {action} order #{order.Id}, it is {Name}");
    }

    public override string ToString() => Name;
}
