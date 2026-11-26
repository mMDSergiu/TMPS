using CoffeeShop.Models;

namespace CoffeeShop.Extras;


public class GiftBox : Extra
{
    public GiftBox(IOrderItem item) : base(item)
    {
    }

    public override string Description => item.Description + ", in a gift box";

    public override decimal Price
    {
        get { return item.Price + 15; }
    }

    public override IOrderItem Clone()
    {
        return new GiftBox(item.Clone());
    }
}
