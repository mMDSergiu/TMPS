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

class SilentNotifier : INotifier
{
    public void Send(string to, string message) { }
}

static class Make
{
    public static Drink Latte() => new DrinkMaker("Latte", 40, Array.Empty<IDrinkPriceRule>()).Build();

    public static Combo Breakfast() =>
        new Combo("Breakfast", 10).Add(Latte()).Add(new Pastry("Croissant", 25));
}
