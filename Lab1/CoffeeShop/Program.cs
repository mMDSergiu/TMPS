using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Output;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;
using CoffeeShop.Services;

IOutput output = new ConsoleOutput();
var repo = new OrderRepository();
var service = new OrderService(repo, new ConsoleNotifier(), new PriceCalculator(), new ReceiptPrinter(output));

var configuredMenu = MenuSingleton.Instance
    .AddDrink("Espresso", 25)
    .AddDrink("Latte", 40)
    .AddDrink("Cappuccino", 38)
    .AddDrink("Tea", 20)
    .AddRule(new SizeRule())
    .AddRule(new MilkRule())
    .AddRule(new ExtraShotRule())
    .AddRule(new SyrupRule());

IMenu menu = configuredMenu;
var sameMenu = MenuSingleton.Instance;
Console.WriteLine("Same menu object? " + ReferenceEquals(menu, sameMenu));
new MenuPrinter(output).Print(menu);
Console.WriteLine();


var o1 = new Order("Ana");
o1.Drinks.Add(menu.Make("Latte").Size(DrinkSize.Large).WithMilk("oat").AddSyrup("vanilla").Build());
o1.Drinks.Add(menu.Make("Espresso").ExtraShot().Build());
service.ProcessOrder(o1, new StudentDiscount(), new CardPayment("4111222233334444", output));
Console.WriteLine();

var o2 = new Order("Ion");
var cappuccino = menu.Make("Cappuccino").Size(DrinkSize.Small).Build();
o2.Drinks.Add(cappuccino);
o2.Drinks.Add(cappuccino.Clone());
o2.Drinks.Add(menu.Make("Tea").Build());
service.ProcessOrder(o2, new HappyHourDiscount(), new CashPayment(100, output));
Console.WriteLine();


var o3 = o1.Clone();
service.ProcessOrder(o3, new NoDiscount(), new CashPayment(150, output));

Console.WriteLine("Same order? " + ReferenceEquals(o1, o3) + ", same first drink? " + ReferenceEquals(o1.Drinks[0], o3.Drinks[0]));
o3.Drinks[0] = menu.Modify(o3.Drinks[0]).AddSyrup("caramel").Build();
Console.WriteLine("o1 drink: " + o1.Drinks[0]);
Console.WriteLine("o3 drink: " + o3.Drinks[0]);
Console.WriteLine();


var smsService = new OrderService(repo, new SmsNotifier(), new PriceCalculator(), new ReceiptPrinter(output));
var o4 = o1.Clone();
o4.CustomerName = "Victor";
smsService.ProcessOrder(o4, new NoDiscount(), new CardPayment("5555666677778888", output));

Console.WriteLine("\nTotal orders saved: " + repo.GetAll().Count);
