using CoffeeShop.Models;

namespace CoffeeShop.Builders;

public class DrinkBuilder
{
    private readonly string name;
    private readonly decimal basePrice;

    private DrinkSize size = DrinkSize.Medium;
    private string? milk;
    private int shots;
    private List<string> syrups = new();

    public DrinkBuilder(string name, decimal basePrice)
    {
        this.name = name;
        this.basePrice = basePrice;
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
        decimal price = basePrice;

        if (size == DrinkSize.Small) price -= 5;
        else if (size == DrinkSize.Large) price += 10;

        if (milk != null && milk != "regular")
            price += 5;
        price += shots * 8;
        price += syrups.Count * 6;

        return new Drink
        {
            Name = name,
            Size = size,
            Milk = milk,
            ExtraShots = shots,
            Syrups = new List<string>(syrups),
            Price = price
        };
    }
}
