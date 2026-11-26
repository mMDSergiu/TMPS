namespace CoffeeShop.Models;

public interface IOrderItem : ICopyable<IOrderItem>
{
    string Description { get; }
    decimal Price { get; }

    IReadOnlyList<IOrderItem> Parts { get; }
}
