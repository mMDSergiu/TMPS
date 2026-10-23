namespace CoffeeShop.Models;

public interface IPrototype<T>
{
    T Clone();
}
