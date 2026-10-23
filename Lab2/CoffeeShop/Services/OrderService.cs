using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;

namespace CoffeeShop.Services;

public class OrderService
{
    private readonly IOrderRepository repo;
    private readonly INotifier notifier;
    private readonly PriceCalculator calculator;
    private readonly ReceiptPrinter printer;

    public OrderService(IOrderRepository repo, INotifier notifier, PriceCalculator calculator, ReceiptPrinter printer)
    {
        this.repo = repo;
        this.notifier = notifier;
        this.calculator = calculator;
        this.printer = printer;
    }

    public bool ProcessOrder(Order order, IDiscount discount, IPaymentMethod payment)
    {
        decimal subtotal = calculator.GetSubtotal(order);
        decimal total = calculator.GetTotal(order, discount);

        printer.Print(order, subtotal, total, discount.Name);

        bool paid = payment.Pay(total);
        if (!paid)
        {
            notifier.Send(order.CustomerName, "payment failed, order canceled");
            return false;
        }

        repo.Add(order);
        notifier.Send(order.CustomerName, "your order is being prepared :)");
        return true;
    }
}
