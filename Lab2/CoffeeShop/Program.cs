using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;
using CoffeeShop.Services;

var repo = new OrderRepository();
var service = new OrderService(repo, new ConsoleNotifier(), new PriceCalculator(), new ReceiptPrinter());

var menu = Menu.Instance;
var sameMenu = Menu.Instance;
Console.WriteLine("Same menu object? " + ReferenceEquals(menu, sameMenu));
menu.Print();
Console.WriteLine();


var o1 = new Order("Ana");
o1.Drinks.Add(menu.Make("Latte").Size(DrinkSize.Large).WithMilk("oat").AddSyrup("vanilla").Build());
o1.Drinks.Add(menu.Make("Espresso").ExtraShot().Build());
service.ProcessOrder(o1, new StudentDiscount(), new CardPayment("4111222233334444"));
Console.WriteLine();

var o2 = new Order("Ion");
var cappuccino = menu.Make("Cappuccino").Size(DrinkSize.Small).Build();
o2.Drinks.Add(cappuccino);
o2.Drinks.Add(cappuccino.Clone());
o2.Drinks.Add(menu.Make("Tea").Build());
service.ProcessOrder(o2, new HappyHourDiscount(), new CashPayment(100));
Console.WriteLine();


var o3 = o1.Clone();
service.ProcessOrder(o3, new NoDiscount(), new CashPayment(150));

Console.WriteLine("Same order? " + ReferenceEquals(o1, o3) + ", same first drink? " + ReferenceEquals(o1.Drinks[0], o3.Drinks[0]));
o3.Drinks[0].Syrups.Add("caramel");
Console.WriteLine("o1 drink: " + o1.Drinks[0]);
Console.WriteLine("o3 drink: " + o3.Drinks[0]);
Console.WriteLine();


var smsService = new OrderService(repo, new SmsNotifier(), new PriceCalculator(), new ReceiptPrinter());
var o4 = o1.Clone();
o4.CustomerName = "Victor";
smsService.ProcessOrder(o4, new NoDiscount(), new CardPayment("5555666677778888"));

Console.WriteLine("\nTotal orders saved: " + repo.GetAll().Count);
