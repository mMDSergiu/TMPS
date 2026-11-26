using CoffeeShop.Extras;
using CoffeeShop.Models;
using CoffeeShop.Payments;
using CoffeeShop.Pricing;
using CoffeeShop.Repositories;
using CoffeeShop.Services;
using CoffeeShop.Statuses;

namespace CoffeeShop.Tests;

public class CheckoutTests
{
    [Fact]
    public void Order_with_a_combo_to_go_is_priced_printed_and_saved_through_the_guard()
    {
        var real = new OrderRepository();
        var output = new TestOutput();
        var service = new OrderService(new ProtectedOrders(real, new Staff("Maria", false)),
            new PriceCalculator(), new ReceiptPrinter(output));

        var order = new Order("Ana");
        order.AddItem(new ToGo(Make.Breakfast()));

        bool ok = service.ProcessOrder(order, new NoDiscount(), new CardPayment("4111222233334444", output));

        Assert.True(ok);
        Assert.IsType<Paid>(order.Status);
        Assert.Contains(order, real.GetAll());
        Assert.Contains("Total: 58 lei", output.Lines);
        Assert.Contains(output.Lines, l => l.Trim() == "- Croissant");
    }

    [Fact]
    public void Failed_payment_cancels_the_order_and_it_is_not_saved()
    {
        var real = new OrderRepository();
        var output = new TestOutput();
        var service = new OrderService(new ProtectedOrders(real, new Staff("Maria", false)),
            new PriceCalculator(), new ReceiptPrinter(output));

        var order = new Order("Victor");
        order.AddItem(new GiftBox(Make.Breakfast()));

        bool ok = service.ProcessOrder(order, new NoDiscount(), new CashPayment(20, output));

        Assert.False(ok);
        Assert.IsType<Cancelled>(order.Status);
        Assert.Empty(real.GetAll());
    }
}
