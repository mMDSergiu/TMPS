using CoffeeShop.Makers;
using CoffeeShop.Models;
using CoffeeShop.Pricing;

namespace CoffeeShop.Services;

public sealed class Menu : IMenu
{
    private static Menu? instance;
    private static readonly object locker = new object();

    private readonly Dictionary<string, decimal> prices = new();
    private readonly List<IDrinkPriceRule> rules = new();

    private Menu()
    {
        Console.WriteLine("(loading menu...)");
    }

    public static Menu Instance
    {
        get
        {
            lock (locker)
            {
                if (instance == null)
                    instance = new Menu();
                return instance;
            }
        }
    }

    public Menu AddDrink(string name, decimal basePrice)
    {
        lock (locker) prices[name] = basePrice;
        return this;
    }

    public Menu AddRule(IDrinkPriceRule rule)
    {
        lock (locker) rules.Add(rule);
        return this;
    }

    public IReadOnlyDictionary<string, decimal> Prices => prices;

    public DrinkMaker Make(string drinkName)
    {
        if (!prices.ContainsKey(drinkName))
            throw new ArgumentException("We dont have " + drinkName);

        return new DrinkMaker(drinkName, prices[drinkName], rules.ToArray());
    }

    public DrinkMaker Modify(Drink drink)
    {
        return Make(drink.Name).StartingFrom(drink);
    }
}
