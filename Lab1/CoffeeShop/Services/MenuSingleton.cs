using CoffeeShop.Builders;
using CoffeeShop.Models;
using CoffeeShop.Pricing;

namespace CoffeeShop.Services;

public sealed class MenuSingleton : IMenu
{
    private static MenuSingleton? instance;
    private static readonly object locker = new object();

    private readonly Dictionary<string, decimal> prices = new();
    private readonly List<IDrinkPriceRule> rules = new();

    private MenuSingleton()
    {
        Console.WriteLine("(loading menu...)");
    }

    public static MenuSingleton Instance
    {
        get
        {
            lock (locker)
            {
                if (instance == null)
                    instance = new MenuSingleton();
                return instance;
            }
        }
    }

    public MenuSingleton AddDrink(string name, decimal basePrice)
    {
        lock (locker) prices[name] = basePrice;
        return this;
    }

    public MenuSingleton AddRule(IDrinkPriceRule rule)
    {
        lock (locker) rules.Add(rule);
        return this;
    }

    public IReadOnlyDictionary<string, decimal> Prices => prices;

    public DrinkBuilder Make(string drinkName)
    {
        if (!prices.ContainsKey(drinkName))
            throw new ArgumentException("We dont have " + drinkName);

        return new DrinkBuilder(drinkName, prices[drinkName], rules.ToArray());
    }

    public DrinkBuilder Modify(Drink drink)
    {
        return Make(drink.Name).StartingFrom(drink);
    }
}
