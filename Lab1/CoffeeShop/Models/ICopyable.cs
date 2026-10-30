namespace CoffeeShop.Models;

public interface ICopyable<T>
{
    T Clone();
}
