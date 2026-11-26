using CoffeeShop.Models;

namespace CoffeeShop.Listeners;

public interface IOrderListener
{
    void StatusChanged(Order order);
}
