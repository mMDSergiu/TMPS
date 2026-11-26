using CoffeeShop.Listeners;
using CoffeeShop.Makers;
using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Output;
using CoffeeShop.Pricing;

namespace CoffeeShop.Tests;

class TestOutput : IOutput
{
    public List<string> Lines { get; } = new();
    public void WriteLine(string text) => Lines.Add(text);
}

class TestNotifier : INotifier
{
    public List<string> Sent { get; } = new();

    public void Send(string to, string message) => Sent.Add(to + ": " + message);
}

class TestListener : IOrderListener
{
    public List<string> Seen { get; } = new();

    public void StatusChanged(Order order) => Seen.Add(order.Status.Name);
}

static class Make
{
    public static Drink Latte() => new DrinkMaker("Latte", 40, Array.Empty<IDrinkPriceRule>()).Build();

    public static Combo Breakfast() =>
        new Combo("Breakfast", 10).Add(Latte()).Add(new Pastry("Croissant", 25));

    public static Order PaidOrder(string customer = "Ana")
    {
        var order = new Order(customer);
        order.AddItem(Latte());
        order.Pay();
        return order;
    }
}
