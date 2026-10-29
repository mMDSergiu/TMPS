using CoffeeShop.Models;
using CoffeeShop.Pricing;

namespace CoffeeShop.Builders;

public class DrinkBuilder
{
    private readonly string name;
    private readonly decimal basePrice;

    private DrinkSize size = DrinkSize.Medium;
    private string? milk;
    private int shots;
    private List<string> syrups = new();

    private readonly IEnumerable<IDrinkPriceRule> rules;

    public DrinkBuilder(string name, decimal basePrice, IEnumerable<IDrinkPriceRule> rules)
    {
        this.name = name;
        this.basePrice = basePrice;
        this.rules = rules;
    }

    public DrinkBuilder StartingFrom(Drink drink)
    {
        size = drink.Size;
        milk = drink.Milk;
        shots = drink.ExtraShots;
        syrups = new List<string>(drink.Syrups);
        return this;
    }

    public DrinkBuilder Size(DrinkSize size)
    {
        this.size = size;
        return this;
    }
    public DrinkBuilder WithMilk(string milk)
    {
        this.milk = milk;
        return this;
    }

    public DrinkBuilder ExtraShot()
    {
        shots++;
        return this;
    }

    public DrinkBuilder AddSyrup(string syrup)
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
