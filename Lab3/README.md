# Lab 3, Behavioral Design Patterns

Student: Cătănoi Sergiu
Group: FAF-242

Still the same coffee shop, continued from lab 2. This time the focus is on what happens with an order after it is paid: the barista prepares it, it gets ready, the client takes it. I added 3 behavioral patterns:

* **State**: the order has a status (placed, paid, preparing, ready, collected, cancelled) and the status decides what can be done with the order
* **Observer**: the pickup screen and the SMS alert are told every time the status of an order changes
* **Command**: the barista actions (prepare, ready, give, cancel) are objects that the till runs, keeps in a history and can undo

Everything from the previous labs is still used: Singleton menu, Builder for drinks, Prototype for combos (lab 1), combos, extras and the protected sales list (lab 2).

The diagrams, code and tests are explained in [REPORT.md](REPORT.md).

## How to build and run

You need the .NET 9 SDK.

```
cd Lab3
dotnet build
dotnet run --project CoffeeShop
```

The first part of the demo runs by itself. At the end there is a part where you are the barista and type commands for a new order (`prepare`, `ready`, `give`, `cancel`, `undo`, `sms on`, `sms off`, `history`, `exit`).

Tests:

```
cd Lab3
dotnet test
```

## Project structure

```
Lab3/
  CoffeeShop.sln
  CoffeeShop/
    Client/        Program.cs (demo + the part where you type commands)
    Statuses/      OrderStatus, Placed, Paid, Preparing, Ready, Collected, Cancelled
    Listeners/     IOrderListener, PickupScreen, SmsAlert
    Actions/       IOrderAction, StatusAction, StartPreparing, MarkReady, HandOver, CancelOrder, Till
    Models/        Order, Drink, Pastry, Combo, IOrderItem, Staff, ICopyable
    Extras/        Extra, ToGo, GiftBox
    Makers/        DrinkMaker
    Repositories/  IOrderRepository, OrderRepository, ProtectedOrders
    Services/      Menu, IMenu, MenuPrinter, OrderService, ReceiptPrinter, SalesReport
    Pricing/ Payments/ Notifications/ Output/   (from lab 0 and 1)
  CoffeeShop.Tests/  xUnit tests
```

What changed from lab 2:

* `Order` has a `Status` and methods `Pay`, `StartPreparing`, `MarkReady`, `HandOver`, `Cancel` that ask the current status what to do. Items can be added only while the order is not paid (`AddItem`).
* `Order` keeps a list of listeners (`Subscribe` / `Unsubscribe`) and tells them when the status changes.
* `OrderService` doesnt send messages to the client anymore, it only changes the status (`order.Pay()` or `order.Cancel()`) and the listeners react. So it doesnt need `INotifier` now.
* New folders: `Statuses/`, `Listeners/`, `Actions/`.

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
[screen] #1 Ana: paid

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
[screen] #2 Ion: paid

Order #3
Client: Victor
  Sweet combo, to go, in a gift box           63 lei
    - Cappuccino medium
    - Cookie
Subtotal: 63 lei
Total: 63 lei
Not enough money! need 63 lei, got 50
[screen] #3 Victor: cancelled
[SMS to Victor] your order #3 was cancelled

BARISTA
till: prepare #1
[screen] #1 Ana: preparing
till: ready #2
  not allowed, cant mark as ready order #2, it is paid
till: prepare #2
[screen] #2 Ion: preparing
till: ready #1
[screen] #1 Ana: READY, come and get it
[SMS to Ana] your order #1 is ready :)
till: give #1 to Ana
[screen] #1 Ana: collected
(oops, Ana is still waiting for the espresso)
till: undo give #1 to Ana
[screen] #1 Ana: READY, come and get it
[SMS to Ana] your order #1 is ready :)
(Ion wants an sms, then changes his mind)
till: ready #2
[screen] #2 Ion: READY, come and get it
till: give #1 to Ana
[screen] #1 Ana: collected
till: give #2 to Ion
[screen] #2 Ion: collected
till: cancel #2
  not allowed, cant cancel order #2, it is collected
cant add a cookie: order #1 is collected, the items cant be changed
history: prepare #1, prepare #2, ready #1, give #1 to Ana, undo give #1 to Ana, ready #2, give #1 to Ana, give #2 to Ion

Access denied: Maria is not a manager, cant see the sales
SALES TODAY
  #1 Ana           91 lei
  #2 Ion          128 lei
2 orders, 219 lei (before discounts)

YOUR TURN, you are the barista now
Order #4
Client: Guest
  Sweet combo                                 45 lei
    - Cappuccino medium
    - Cookie
Subtotal: 45 lei
Total: 45 lei
Paid 45 lei with card ****1234
[screen] #4 Guest: paid
commands: prepare, ready, give, cancel, undo, sms on, sms off, history, exit
> give
till: give #4 to Guest
  not allowed, cant hand over order #4, it is paid
> prepare
till: prepare #4
[screen] #4 Guest: preparing
> sms on
ok, sms is on
> ready
till: ready #4
[screen] #4 Guest: READY, come and get it
[SMS to Guest] your order #4 is ready :)
> undo
till: undo ready #4
[screen] #4 Guest: preparing
> sms off
ok, sms is off
> ready
till: ready #4
[screen] #4 Guest: READY, come and get it
> give
till: give #4 to Guest
[screen] #4 Guest: collected
> cancel
till: cancel #4
  not allowed, cant cancel order #4, it is collected
> history
prepare #4, ready #4, undo ready #4, ready #4, give #4 to Guest
> exit
```
