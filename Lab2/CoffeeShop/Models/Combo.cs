namespace CoffeeShop.Models;

public class Combo : IOrderItem
{
    private readonly List<IOrderItem> items = new();

    public string Name { get; }
    public decimal Discount { get; }


    public Combo(string name, decimal discount = 0)
    {
        Name = name;
        Discount = discount;
    }

    public Combo Add(IOrderItem item)
    {
        items.Add(item);
        return this;
    }

    public bool Remove(IOrderItem item) => items.Remove(item);

    public string Description => Name + " combo";

    public IReadOnlyList<IOrderItem> Parts => items;

    public decimal Price
    {
        get
        {
            decimal sum = items.Sum(i => i.Price);
            return Math.Max(0, sum - Discount);
        }
    }

    public IOrderItem Clone()
    {
        var copy = new Combo(Name, Discount);
        foreach (var item in items)
            copy.Add(item.Clone());

        return copy;
    }
}
