using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;
using CoffeeShop.Services;

var repo = new OrderRepository();
var service = new OrderService(repo, new ConsoleNotifier(), new PriceCalculator(), new ReceiptPrinter());


var espresso = new Drink("Espresso", 25);
var latte = new Drink("Latte", 40);
var cappuccino = new Drink("Cappuccino", 38);
var tea = new Drink("Tea", 20);

var o1 = new Order("Ana");
o1.Drinks.Add(latte);
o1.Drinks.Add(espresso);
service.ProcessOrder(o1, new StudentDiscount(), new CardPayment("4111222233334444"));
Console.WriteLine();


var o2 = new Order("Ion");
o2.Drinks.Add(cappuccino);
o2.Drinks.Add(cappuccino);
o2.Drinks.Add(tea);
service.ProcessOrder(o2, new HappyHourDiscount(), new CashPayment(100));
Console.WriteLine();

var o3 = new Order("Maria");
o3.Drinks.Add(latte);
service.ProcessOrder(o3, new NoDiscount(), new CashPayment(20));
Console.WriteLine();


var smsService = new OrderService(repo, new SmsNotifier(), new PriceCalculator(), new ReceiptPrinter());
var o4 = new Order("Victor");
o4.Drinks.Add(espresso);
smsService.ProcessOrder(o4, new NoDiscount(), new CardPayment("5555666677778888"));

Console.WriteLine("\nTotal orders saved: " + repo.GetAll().Count);

