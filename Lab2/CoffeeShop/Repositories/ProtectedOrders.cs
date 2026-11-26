using CoffeeShop.Models;

namespace CoffeeShop.Repositories;

public class ProtectedOrders : IOrderRepository
{
    private readonly IOrderRepository orders;
    private readonly Staff user;

    public ProtectedOrders(IOrderRepository orders, Staff user)
    {
        this.orders = orders;
        this.user = user;
    }

    public void Add(Order order) => orders.Add(order);

    public List<Order> GetAll()
    {
        if (!user.IsManager)
            throw new UnauthorizedAccessException(user.Name + " is not a manager, cant see the sales");

        return orders.GetAll();
    }
}
