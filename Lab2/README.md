# Lab 2, Structural Design Patterns

Student: Cătănoi Sergiu
Group: FAF-242

This is the same coffee shop from lab 0 and lab 1, I just continued it. In this lab I added 3 structural patterns:

* **Composite**: combos (a combo can have drinks, pastries or even other combos)
* **Decorator**: extras for any item (to go, gift box)
* **Proxy**: only a manager can see the list of orders (sales report)

The patterns from lab 1 are still used: the menu is a Singleton, the drinks are made with the Builder (`DrinkMaker`) and the combos on the menu are copied with Prototype (`Clone()`) every time someone orders one.

Everything about the patterns (diagrams, code, tests) is in [REPORT.md](REPORT.md).

## How to build and run

You need the .NET 9 SDK.

```
cd Lab2
dotnet build
dotnet run --project CoffeeShop
```

Tests (one command, runs all of them):

```
cd Lab2
dotnet test
```

## Project structure

```
Lab2/
  CoffeeShop.sln
  CoffeeShop/
    Client/        Program.cs (the demo, the only client)
    Models/        Drink, Pastry, Combo, IOrderItem, Order, Staff, ICopyable
    Extras/        Extra, ToGo, GiftBox
    Makers/        DrinkMaker
    Repositories/  IOrderRepository, OrderRepository, ProtectedOrders
    Services/      Menu, IMenu, MenuPrinter, OrderService, ReceiptPrinter, SalesReport
    Pricing/       discounts, price rules, PriceCalculator
    Payments/      cash, card
    Notifications/ INotifier
    Output/        IOutput, ConsoleOutput
  CoffeeShop.Tests/  xUnit tests
```

New in this lab: `Models/IOrderItem.cs`, `Models/Pastry.cs`, `Models/Combo.cs`, `Models/Staff.cs`, the `Extras/` folder, `Repositories/ProtectedOrders.cs`, `Services/SalesReport.cs` and the tests. `Order` now has `Items` instead of `Drinks`, and the receipt also prints what is inside a combo.

## Output

```
(loading menu...)
MENU
  Espresso       25 lei
  Latte          40 lei
  Cappuccino     38 lei
  Tea            20 lei
COMBOS
  Breakfast combo     55 lei   (Latte medium + Croissant)
  Sweet combo         45 lei   (Cappuccino medium + Cookie)

Order #1
Client: Ana
  Breakfast combo, to go                      58 lei
    - Latte medium
    - Croissant
  Espresso medium, +1 shot                    33 lei
Subtotal: 91 lei
Discount: student 10%
Total: 81.9 lei
Paid 81.9 lei with card ****4444
>> Ana: your order is being prepared :)

Order #2
Client: Ion
  Family combo                               128 lei
    - Breakfast combo
      - Latte medium
      - Croissant
    - Breakfast combo
      - Latte medium
      - Croissant
    - Tea large
Subtotal: 128 lei
Discount: happy hour 20%
Total: 102.4 lei
Paid cash: 120 lei, change: 17.6 lei
>> Ion: your order is being prepared :)

Order #3
Client: Victor
  Sweet combo, to go, in a gift box           63 lei
    - Cappuccino medium
    - Cookie
  Cookie                                      15 lei
Subtotal: 78 lei
Total: 78 lei
Paid 78 lei with card ****8888
>> Victor: your order is being prepared :)

Access denied: Maria is not a manager, cant see the sales
SALES TODAY
  #1 Ana           91 lei
  #2 Ion          128 lei
  #3 Victor        78 lei
3 orders, 297 lei (before discounts)
```
