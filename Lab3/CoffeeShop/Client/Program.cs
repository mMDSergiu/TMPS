using CoffeeShop.Actions;
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

Order NewOrder(string customer)
{
    var order = new Order(customer);
    order.Subscribe(screen);
    return order;
}


var o1 = NewOrder("Ana");
o1.Subscribe(sms);
o1.AddItem(new ToGo(menu.Combo("Breakfast")));
o1.AddItem(menu.Make("Espresso").ExtraShot().Build());
service.ProcessOrder(o1, new StudentDiscount(), new CardPayment("4111222233334444", output));
Console.WriteLine();

var o2 = NewOrder("Ion");
o2.AddItem(new Combo("Family", discount: 12)
    .Add(menu.Combo("Breakfast"))
    .Add(menu.Combo("Breakfast"))
    .Add(menu.Make("Tea").Size(DrinkSize.Large).Build()));
service.ProcessOrder(o2, new HappyHourDiscount(), new CashPayment(120, output));
Console.WriteLine();

var o3 = NewOrder("Victor");
o3.Subscribe(sms);
o3.AddItem(new GiftBox(new ToGo(menu.Combo("Sweet"))));
service.ProcessOrder(o3, new NoDiscount(), new CashPayment(50, output));
Console.WriteLine();


output.WriteLine("BARISTA");
var till = new Till(output);

till.Run(new StartPreparing(o1));
till.Run(new MarkReady(o2));
till.Run(new StartPreparing(o2));
till.Run(new MarkReady(o1));
till.Run(new HandOver(o1));

output.WriteLine("(oops, Ana is still waiting for the espresso)");
till.Undo();

output.WriteLine("(Ion wants an sms, then changes his mind)");
o2.Subscribe(sms);
o2.Unsubscribe(sms);
till.Run(new MarkReady(o2));

till.Run(new HandOver(o1));
till.Run(new HandOver(o2));
till.Run(new CancelOrder(o2));

try
{
    o1.AddItem(new Pastry("Cookie", 15));
}
catch (InvalidOperationException e)
{
    output.WriteLine("cant add a cookie: " + e.Message);
}

output.WriteLine("history: " + string.Join(", ", till.History));
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
Console.WriteLine();


output.WriteLine("YOUR TURN, you are the barista now");
var guest = NewOrder("Guest");
guest.AddItem(menu.Combo("Sweet"));
service.ProcessOrder(guest, new NoDiscount(), new CardPayment("4000123412341234", output));

var actions = new Dictionary<string, Func<Order, IOrderAction>>
{
    ["prepare"] = o => new StartPreparing(o),
    ["ready"] = o => new MarkReady(o),
    ["give"] = o => new HandOver(o),
    ["cancel"] = o => new CancelOrder(o)
};
var myTill = new Till(output);

output.WriteLine("commands: prepare, ready, give, cancel, undo, sms on, sms off, history, exit");
while (true)
{
    Console.Write("> ");
    string? line = Console.ReadLine()?.Trim().ToLower();
    if (line == null || line == "exit")
        break;

    if (actions.ContainsKey(line))
        myTill.Run(actions[line](guest));
    else if (line == "undo")
        myTill.Undo();
    else if (line == "sms on")
    {
        guest.Subscribe(sms);
        output.WriteLine("ok, sms is on");
    }
    else if (line == "sms off")
    {
        guest.Unsubscribe(sms);
        output.WriteLine("ok, sms is off");
    }
    else if (line == "history")
        output.WriteLine(string.Join(", ", myTill.History));
    else if (line != "")
        output.WriteLine("dont know '" + line + "'");
}
