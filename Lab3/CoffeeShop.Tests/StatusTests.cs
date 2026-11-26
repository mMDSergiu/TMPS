using CoffeeShop.Models;
using CoffeeShop.Statuses;

namespace CoffeeShop.Tests;

public class StatusTests
{
    [Fact]
    public void Order_goes_through_the_normal_flow()
    {
        var order = new Order("Ana");
        Assert.IsType<Placed>(order.Status);

        order.Pay();
        Assert.IsType<Paid>(order.Status);

        order.StartPreparing();
        Assert.IsType<Preparing>(order.Status);

        order.MarkReady();
        Assert.IsType<Ready>(order.Status);

        order.HandOver();
        Assert.IsType<Collected>(order.Status);
    }

    [Fact]
    public void Forbidden_moves_throw_and_the_status_stays_the_same()
    {
        var order = new Order("Ion");

        Assert.Throws<InvalidOperationException>(() => order.MarkReady());
        Assert.Throws<InvalidOperationException>(() => order.HandOver());
        Assert.IsType<Placed>(order.Status);

        order.Pay();
        order.StartPreparing();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
        Assert.IsType<Preparing>(order.Status);
    }

    [Fact]
    public void Items_can_be_changed_only_before_payment()
    {
        var order = new Order("Victor");
        order.AddItem(Make.Latte());

        order.Pay();

        Assert.Throws<InvalidOperationException>(() => order.AddItem(new Pastry("Cookie", 15)));
        Assert.Single(order.Items);
    }

    [Fact]
    public void Cancelled_order_is_final()
    {
        var order = new Order("Dan");
        order.Cancel();

        Assert.Throws<InvalidOperationException>(() => order.Pay());
        Assert.IsType<Cancelled>(order.Status);
    }
}
