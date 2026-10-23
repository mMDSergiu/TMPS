using CoffeeShop.Models;

namespace CoffeeShop.Repositories;

public interface IOrderRepository
{
    void Add(Order order);
    List<Order> GetAll();
}
