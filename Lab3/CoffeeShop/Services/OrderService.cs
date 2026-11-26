using CoffeeShop.Models;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;

namespace CoffeeShop.Services;

public class OrderService
{
    private readonly IOrderRepository repo;
    private readonly IPriceCalculator calculator;
    private readonly IReceiptPrinter printer;

    public OrderService(IOrderRepository repo, IPriceCalculator calculator, IReceiptPrinter printer)
    {
        this.repo = repo;
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
            order.Cancel();
            return false;
        }

        order.Pay();
        repo.Add(order);
        return true;
    }
}
