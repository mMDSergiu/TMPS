using CoffeeShop.Models;
using CoffeeShop.Repositories;

namespace CoffeeShop.Tests;

public class ProtectedOrdersTests
{
    private readonly OrderRepository real = new();


    [Fact]
    public void Cashier_cannot_read_the_orders()
    {
        IOrderRepository orders = new ProtectedOrders(real, new Staff("Maria", false));

        Assert.Throws<UnauthorizedAccessException>(() => orders.GetAll());
    }

    [Fact]
    public void Manager_gets_exactly_what_the_real_repository_has()
    {
        real.Add(new Order("Ana"));
        IOrderRepository orders = new ProtectedOrders(real, new Staff("Dan", true));

        Assert.Same(real.GetAll(), orders.GetAll());
    }

    [Fact]
    public void Cashier_can_still_save_orders()
    {
        var order = new Order("Ion");
        IOrderRepository orders = new ProtectedOrders(real, new Staff("Maria", false));

        orders.Add(order);

        Assert.Contains(order, real.GetAll());
    }
}
