using CoffeeShop.Makers;
using CoffeeShop.Models;

namespace CoffeeShop.Services;

public interface IMenu
{
    IReadOnlyDictionary<string, decimal> Prices { get; }
    IEnumerable<IOrderItem> Combos { get; }

    DrinkMaker Make(string drinkName);
    DrinkMaker Modify(Drink drink);

    IOrderItem Combo(string name);
}
