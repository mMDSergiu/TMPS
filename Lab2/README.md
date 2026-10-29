# Lab 2, Creational Design Patterns

Student: Cătănoi Sergiu
Group: FAF-242

## Task

In this lab we had to implement 3 design patterns from this list: Singleton, Builder, Prototype, Factory Method and Abstract Factory. We could make a new project or continue the old one, so I continued my coffee shop app from lab 1 becuase it already had orders and drinks and the patterns fit there quite natural.

I picked:
* Singleton
* Builder
* Prototype

Everything from lab 1 (discounts, payments, notifiers, OrderService) is still there and works the same, I only added new stuff and changed the Drink class.

## What changed from lab 1

In lab 1 a drink was just a name and a price. That is not very realistic, in a real coffee shop you can pick the size, the milk, add syrup or an extra shot of espresso, and all of this changes the price. So now:

* there is a `Menu` with the base prices of all drinks (Singleton)
* drinks are made with a `DrinkBuilder` (Builder)
* orders and drinks can be cloned, so a client can repeat an order (Prototype)

New files:

```
CoffeeShop/
  Builders/DrinkBuilder.cs
  Models/IPrototype.cs
  Output/IOutput.cs
  Pricing/DrinkPriceRules.cs
  Pricing/IPriceCalculator.cs
  Services/IMenu.cs
  Services/Menu.cs
  Services/MenuPrinter.cs
  Services/IReceiptPrinter.cs
```

and `Drink.cs` / `Order.cs` were changed.

## Singleton

A coffee shop has one menu. If every part of the program made its own Menu object, it could happen that two places have different prices, so it makes sense to have only one instance.

The constructor is private so nobody can do `new Menu()`, and the only way to get it is through `Menu.Instance`. The object is created the first time someone asks for it (lazy). I also added a `lock` so if two threads call Instance at the same time it still creates only one object.

```csharp
public sealed class Menu
{
    private static Menu? instance;
    private static readonly object locker = new object();

    private Menu() { ... }

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
}
```

In Program.cs I call `Menu.Instance` two times and check with `ReferenceEquals`, it prints True, and the "(loading menu...)" message appears only once, so the constructor ran one time.

## Builder

A drink now has a lot of optional parts: size, milk, extra shots, syrups. Making a constructor with all of them would look like `new Drink("Latte", DrinkSize.Large, "oat", 0, new List<string>{"vanilla"}, 61)` which is hard to read and easy to mess up the order of parameters. Also the price depends on all the options so I dont want to calculate it by hand every time.

With the builder you set only what you need, step by step, and at the end `Build()` creates the drink and calculates the price:

```csharp
var latte = menu.Make("Latte")
    .Size(DrinkSize.Large)
    .WithMilk("oat")
    .AddSyrup("vanilla")
    .Build();
```

Every method returns `this` so the calls can be chained. `Build()` creates the drink and then asks a list of price rules (see the OCP part below) how much each option adds: small is 5 lei cheaper, large is +10, plant milk +5, extra shot +8, each syrup +6.

The setters in `Drink` are `internal` so outside code cant just change the price of a drink, it has to go through the builder.

I connected the Builder with the Singleton, `Menu.Make("Latte")` returns a new DrinkBuilder that already has the base price from the menu and the price rules. And if you ask for a drink that is not on the menu it throws an exception.

## Prototype

Clients often order the same thing again, so instead of building every drink again there is a `Clone()` method. I made a small generic interface for it:

```csharp
public interface IPrototype<T>
{
    T Clone();
}
```

Both `Drink` and `Order` implement it.

The important part was to make a **deep copy**. `Drink` uses `MemberwiseClone()`, but that copies the Syrups list only by reference, so if you added a syrup to the copy it would also appear in the original. Thats why I create a new list:

```csharp
public Drink Clone()
{
    var copy = (Drink)MemberwiseClone();
    copy.Syrups = new List<string>(Syrups);
    return copy;
}
```

`Order.Clone()` creates a new order (so it gets a new Id) and clones every drink inside it.

In Program.cs Ana repeats her order with `o1.Clone()`, then I add caramel to the first drink of the copy (through `menu.Modify`) and print both. The original stays without caramel, so the copy is really independent. Victor also gets a clone of Ana's order, I just change the CustomerName.

## SOLID in this lab

The new code has to follow the same rules as in lab 1, so after the first version I checked it and fixed these things:

* **OCP**: at first the price rules were `if/else` inside `DrinkBuilder.Build()`, so a new option (for example a new size) meant editing the builder. Now every rule is a class that implements `IDrinkPriceRule` (`SizeRule`, `MilkRule`, `ExtraShotRule`, `SyrupRule`) and `Build()` just sums them up. A new option is a new class, the builder stays the same.

```csharp
public interface IDrinkPriceRule
{
    decimal ExtraFor(Drink drink);
}

drink.Price = basePrice + rules.Sum(r => r.ExtraFor(drink));
```

* **SRP**: `Menu` was the singleton, the price list, the builder factory and it also printed itself. Printing moved to `MenuPrinter`, so `Menu` only keeps prices and gives out builders.
* **DIP**: `Program` used the concrete `Menu` class. Now there is an `IMenu` interface and `Program` / `MenuPrinter` depend on it. `Menu` is still a singleton (private constructor, `Menu.Instance`), but the code that uses the menu does not care how it is created.

* **OCP (menu data)**: the drinks and price rules were hardcoded in the `Menu` constructor. Now `Menu` starts empty and `Program.cs` (the place where objects are created) fills it with `AddDrink(...)` and `AddRule(...)`, so a new drink or rule does not change `Menu`.
* **DIP (service)**: `OrderService` took the concrete `PriceCalculator` and `ReceiptPrinter`. I added `IPriceCalculator` and `IReceiptPrinter`, now it depends only on interfaces.
* **SRP / DIP (output)**: `CashPayment`, `CardPayment`, `ReceiptPrinter` and `MenuPrinter` wrote to `Console` directly. Now they get an `IOutput` in the constructor (`ConsoleOutput` in the app), so they only decide *what* to print, not *where*.
* **Encapsulation of the price**: in the first version `Program` did `drink.Syrups.Add("caramel")`, which added the syrup but left the old price. Now `Drink.Syrups` is `IReadOnlyList<string>` and a drink is changed with `menu.Modify(drink).AddSyrup("caramel").Build()`, which goes through the rules and gives a new drink with the right price (67 lei instead of 61).

```csharp
o3.Drinks[0] = menu.Modify(o3.Drinks[0]).AddSyrup("caramel").Build();
```

Discounts, payment methods, notifiers, the repository are unchanged from lab 1, so SRP, OCP and DIP still hold there. The only place that still uses `Console` directly is the "(loading menu...)" line in the `Menu` constructor, because a singleton with a private constructor cannot receive dependencies, and I keep it on purpose to show that the constructor runs only once.

## Running

```
cd CoffeeShop
dotnet run
```

Output:

```
(loading menu...)
Same menu object? True
MENU
  Espresso       25 lei
  Latte          40 lei
  Cappuccino     38 lei
  Tea            20 lei

Order #1
Client: Ana
  Latte large, oat milk, vanilla              61 lei
  Espresso medium, +1 shot                    33 lei
Subtotal: 94 lei
Discount: student 10%
Total: 84.6 lei
Paid 84.6 lei with card ****4444
>> Ana: your order is being prepared :)

Order #2
Client: Ion
  Cappuccino small                            33 lei
  Cappuccino small                            33 lei
  Tea medium                                  20 lei
Subtotal: 86 lei
Discount: happy hour 20%
Total: 68.8 lei
Paid cash: 100 lei, change: 31.2 lei
>> Ion: your order is being prepared :)

Order #3
Client: Ana
  Latte large, oat milk, vanilla              61 lei
  Espresso medium, +1 shot                    33 lei
Subtotal: 94 lei
Total: 94 lei
Paid cash: 150 lei, change: 56 lei
>> Ana: your order is being prepared :)
Same order? False, same first drink? False
o1 drink: Latte large, oat milk, vanilla
o3 drink: Latte large, oat milk, vanilla, caramel

Order #4
Client: Victor
  Latte large, oat milk, vanilla              61 lei
  Espresso medium, +1 shot                    33 lei
Subtotal: 94 lei
Total: 94 lei
Paid 94 lei with card ****8888
[SMS to Victor] your order is being prepared :)

Total orders saved: 4
```

## Conclusion

In this lab I added three creational patterns to the coffee shop. Singleton made sure there is only one menu with one set of prices. Builder made creating drinks with many options readable and moved the price calculation in one place. Prototype made it easy to repeat orders, and I understood the diference between shallow and deep copy, because at first my clone shared the syrup list with the original and that was a bug.

The patterns also work well with the SOLID stuff from lab 1, for example adding a new option to drinks means adding a price rule class and a builder method, and not touching OrderService or PriceCalculator.
