namespace CoffeeShop.Models;

public class Pastry : IOrderItem
{
    public string Name { get; }
    public decimal Price { get; }

    public Pastry(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public string Description => Name;
    public IReadOnlyList<IOrderItem> Parts => Array.Empty<IOrderItem>();

    public IOrderItem Clone() => new Pastry(Name, Price);
}
