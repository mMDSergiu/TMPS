using CoffeeShop.Models;
using CoffeeShop.Pricing;

namespace CoffeeShop.Makers;

public class DrinkMaker
{
    private readonly string name;
    private readonly decimal basePrice;

    private DrinkSize size = DrinkSize.Medium;
    private string? milk;
    private int shots;
    private List<string> syrups = new();

    private readonly IEnumerable<IDrinkPriceRule> rules;

    public DrinkMaker(string name, decimal basePrice, IEnumerable<IDrinkPriceRule> rules)
    {
        this.name = name;
        this.basePrice = basePrice;
        this.rules = rules;
    }

    public DrinkMaker StartingFrom(Drink drink)
    {
        size = drink.Size;
        milk = drink.Milk;
        shots = drink.ExtraShots;
        syrups = new List<string>(drink.Syrups);
        return this;
    }

    public DrinkMaker Size(DrinkSize size)
    {
        this.size = size;
        return this;
    }
    public DrinkMaker WithMilk(string milk)
    {
        this.milk = milk;
        return this;
    }

    public DrinkMaker ExtraShot()
    {
        shots++;
        return this;
    }

    public DrinkMaker AddSyrup(string syrup)
    {
        syrups.Add(syrup);

        return this;
    }

    public Drink Build()
    {
        var drink = new Drink
        {
            Name = name,
            Size = size,
            Milk = milk,
            ExtraShots = shots,
            Syrups = syrups.ToArray()
        };

        drink.Price = basePrice + rules.Sum(r => r.ExtraFor(drink));
        return drink;
    }
}
