# Lab 1, SOLID principles

Student: Cătănoi Sergiu 
Group: FAF-242

## Task

For the first lab we had to make a small project in any language and use at least 3 of the SOLID principels. I chose C# and made a simple console app for a coffee shop where you can make orders, apply a discount and pay by cash or card.

The principles I used are:
* Single Responsibility (SRP)
* Open/Closed (OCP)
* Dependency Inversion (DIP)

SOLID is 5 principles for object oriented design that help make code easier to change and maintain:

* **S**, Single Responsibility: a class should have only one reason to change
* **O**, Open/Closed: classes should be open for extension, but closed for modification
* **L**, Liskov Substitution: a subclass should work everywhere the parent class works
* **I**, Interface Segregation: better to have more small interfaces than one big one
* **D**, Dependency Inversion: high level modules should depend on abstractions, not on concrete classes

## How the app works

The program creates a few orders. Each order has a customer and some drinks (espresso, latte, cappuccino, tea). When an order is processed the program calculates the price, applies the discount, prints a receipt, takes the payment and then sends a notification to the customer. If the payment fails (not enough cash), the order is canceled and not saved.

Project files:

```
CoffeeShop/
  Program.cs
  Models/         Drink, Order
  Pricing/        IDiscount, Discounts, PriceCalculator
  Payments/       IPaymentMethod, CashPayment, CardPayment
  Notifications/  INotifier (+ ConsoleNotifier, SmsNotifier)
  Repositories/   IOrderRepository, OrderRepository
  Services/       OrderService, ReceiptPrinter
```

## SRP

At first I wanted to put everything in one OrderService class (calculate, print, save...), but then it would have like 5 reasons to change so I split it up. Now every class does one thing:

* `PriceCalculator` only calculates subtotal and total
* `ReceiptPrinter` only prints the receipt
* `OrderRepository` only stores orders
* `ConsoleNotifier` / `SmsNotifier` only send messages
* `OrderService` just calls the others in the right order

So for example if I want to change how the receipt looks I only touch ReceiptPrinter and nothing else.

```csharp
public class PriceCalculator
{
    public decimal GetSubtotal(Order order)
    {
        decimal sum = 0;
        foreach (var d in order.Drinks)
            sum += d.Price;
        return sum;
    }

    public decimal GetTotal(Order order, IDiscount discount)
    {
        var total = discount.Apply(GetSubtotal(order));
        return Math.Round(total, 2);
    }
}
```

## OCP

Discounts are made with an interface `IDiscount`. Every discount is a seperate class (NoDiscount, StudentDiscount, HappyHourDiscount). If the coffee shop wants a new discount, for example for loyalty card, I just write a new class and dont need to change PriceCalculator at all.

```csharp
public interface IDiscount
{
    string Name { get; }
    decimal Apply(decimal price);
}

public class HappyHourDiscount : IDiscount
{
    public string Name => "happy hour 20%";

    public decimal Apply(decimal price)
    {
        return price * 0.8m;
    }
}
```

If I didnt use this, the calculator would have something like this and every new discount = editing the old code:

```csharp
if (type == "student") total = total * 0.9m;
else if (type == "happyhour") total = total * 0.8m;
// ...
```

Same thing for payments, there is `IPaymentMethod` with CashPayment and CardPayment, so adding something like PayPal later is just one more class.

## DIP

`OrderService` doesnt create its dependencies with `new`, it gets them through the constructor. And repository and notifier are interfaces, so the service doesnt know if it saves to a list or to a database, or if it sends an sms or just prints to console.

```csharp
public OrderService(IOrderRepository repo, INotifier notifier, PriceCalculator calculator, ReceiptPrinter printer)
```

All the concrete objects are created in Program.cs. To show that it works, for the last order I made another OrderService with `SmsNotifier` instead of `ConsoleNotifier` and the OrderService code stayed exactly the same.

```csharp
var smsService = new OrderService(repo, new SmsNotifier(), new PriceCalculator(), new ReceiptPrinter());
```

## Running

```
cd CoffeeShop
dotnet run
```

Output:

```
Order #1
Client: Ana
  Latte             40 lei
  Espresso          25 lei
Subtotal: 65 lei
Discount: student 10%
Total: 58.5 lei
Paid 58.5 lei with card ****4444
>> Ana: your order is being prepared :)

Order #2
Client: Ion
  Cappuccino        38 lei
  Cappuccino        38 lei
  Tea               20 lei
Subtotal: 96 lei
Discount: happy hour 20%
Total: 76.8 lei
Paid cash: 100 lei, change: 23.2 lei
>> Ion: your order is being prepared :)

Order #3
Client: Maria
  Latte             40 lei
Subtotal: 40 lei
Total: 40 lei
Not enough money! need 40 lei, got 20
>> Maria: payment failed, order canceled

Order #4
Client: Victor
  Espresso          25 lei
Subtotal: 25 lei
Total: 25 lei
Paid 25 lei with card ****8888
[SMS to Victor] your order is being prepared :)

Total orders saved: 3
```

## Conclusion

In this lab I learned how to use SRP, OCP and DIP in practice. The project is small but it was still noticable that splitting the code into small classes and using interfaces makes it much easier to add new things, like a new discount or a new type of notification, without breaking what already works. In the begining it seemed like too many files for such a simple app, but I think for bigger projects it really pays off. Also I think this project can be continued in the next labs with design patterns, for example a Decorator for adding milk or syrup to drinks.
