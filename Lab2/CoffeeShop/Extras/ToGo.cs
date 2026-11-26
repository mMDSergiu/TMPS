using CoffeeShop.Models;

namespace CoffeeShop.Extras;

public class ToGo : Extra
{
    public ToGo(IOrderItem item) : base(item) { }

    public override string Description => item.Description + ", to go";
    public override decimal Price => item.Price + 3;

    public override IOrderItem Clone() => new ToGo(item.Clone());
}
