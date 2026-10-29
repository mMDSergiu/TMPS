using CoffeeShop.Builders;
using CoffeeShop.Models;

namespace CoffeeShop.Services;

public interface IMenu
{
    IReadOnlyDictionary<string, decimal> Prices { get; }
    DrinkBuilder Make(string drinkName);
    DrinkBuilder Modify(Drink drink);
}
