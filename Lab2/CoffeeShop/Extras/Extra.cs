using CoffeeShop.Models;

namespace CoffeeShop.Extras;

public abstract class Extra : IOrderItem
{
    protected readonly IOrderItem item;

    protected Extra(IOrderItem item)
    {
        this.item = item;
    }

    public virtual string Description => item.Description;
    public virtual decimal Price => item.Price;

    public IReadOnlyList<IOrderItem> Parts => item.Parts;

    public abstract IOrderItem Clone();
}
