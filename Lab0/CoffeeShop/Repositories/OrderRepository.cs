using CoffeeShop.Models;

namespace CoffeeShop.Repositories;

public class OrderRepository : IOrderRepository
{
    private List<Order> orders = new();

    public void Add(Order order) => orders.Add(order);
    public List<Order> GetAll()
    {
        return orders;
    }
}
