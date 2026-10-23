using CoffeeShop.Builders;

namespace CoffeeShop.Services;

public sealed class Menu
{
    private static Menu? instance;
    private static readonly object locker = new object();

    private readonly Dictionary<string, decimal> prices;

    private Menu()
    {
        Console.WriteLine("(loading menu...)");
        prices = new Dictionary<string, decimal>
        {
            { "Espresso", 25 },
            { "Latte", 40 },
            { "Cappuccino", 38 },
            { "Tea", 20 },
        };
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


    public DrinkBuilder Make(string drinkName)
    {
        if (!prices.ContainsKey(drinkName))
            throw new ArgumentException("We dont have " + drinkName);

        return new DrinkBuilder(drinkName, prices[drinkName]);
    }

    public void Print()
    {
        Console.WriteLine("MENU");
        foreach (var p in prices)
            Console.WriteLine($"  {p.Key,-12}{p.Value,5} lei");
    }
}
