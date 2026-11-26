using CoffeeShop.Models;

namespace CoffeeShop.Pricing;

public interface IDrinkPriceRule
{
    decimal ExtraFor(Drink drink);
}

public class SizeRule : IDrinkPriceRule
{
    public decimal ExtraFor(Drink drink) => drink.Size switch
    {
        DrinkSize.Small => -5,
        DrinkSize.Large => 10,
        _ => 0
    };
}

public class MilkRule : IDrinkPriceRule
{
    public decimal ExtraFor(Drink drink) =>
        drink.Milk != null && drink.Milk != "regular" ? 5 : 0;
}

public class ExtraShotRule : IDrinkPriceRule
{
    public decimal ExtraFor(Drink drink) => drink.ExtraShots * 8;
}

public class SyrupRule : IDrinkPriceRule
{
    public decimal ExtraFor(Drink drink) => drink.Syrups.Count * 6;
}
