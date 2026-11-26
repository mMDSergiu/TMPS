using CoffeeShop.Extras;
using CoffeeShop.Listeners;
using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Output;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;
using CoffeeShop.Services;

IOutput output = new ConsoleOutput();

var shopMenu = Menu.Instance
    .AddDrink("Espresso", 25)
    .AddDrink("Latte", 40)
    .AddDrink("Cappuccino", 38)
    .AddDrink("Tea", 20)
    .AddRule(new SizeRule())
    .AddRule(new MilkRule())
    .AddRule(new ExtraShotRule())
    .AddRule(new SyrupRule());

shopMenu
    .AddCombo(new Combo("Breakfast", discount: 10)
        .Add(shopMenu.Make("Latte").Build())
        .Add(new Pastry("Croissant", 25)))
    .AddCombo(new Combo("Sweet", discount: 8)
        .Add(shopMenu.Make("Cappuccino").Build())
        .Add(new Pastry("Cookie", 15)));

IMenu menu = shopMenu;
new MenuPrinter(output).Print(menu);
Console.WriteLine();

var cashier = new Staff("Maria", IsManager: false);
var manager = new Staff("Dan", IsManager: true);

var allOrders = new OrderRepository();
var calculator = new PriceCalculator();
var service = new OrderService(new ProtectedOrders(allOrders, cashier), calculator, new ReceiptPrinter(output));

var screen = new PickupScreen(output);
var sms = new SmsAlert(new SmsNotifier());


var o1 = new Order("Ana");
o1.Subscribe(screen);
o1.Subscribe(sms);
o1.AddItem(new ToGo(menu.Combo("Breakfast")));
o1.AddItem(menu.Make("Espresso").ExtraShot().Build());
service.ProcessOrder(o1, new StudentDiscount(), new CardPayment("4111222233334444", output));
Console.WriteLine();

var o2 = new Order("Ion");
o2.Subscribe(screen);
o2.AddItem(new Combo("Family", discount: 12)
    .Add(menu.Combo("Breakfast"))
    .Add(menu.Combo("Breakfast"))
    .Add(menu.Make("Tea").Size(DrinkSize.Large).Build()));
service.ProcessOrder(o2, new HappyHourDiscount(), new CashPayment(120, output));
Console.WriteLine();

o1.StartPreparing();
o1.MarkReady();
o1.HandOver();
Console.WriteLine();

try
{
    new SalesReport(new ProtectedOrders(allOrders, cashier), calculator, output).Print();
}
catch (UnauthorizedAccessException e)
{
    output.WriteLine("Access denied: " + e.Message);
}

new SalesReport(new ProtectedOrders(allOrders, manager), calculator, output).Print();
