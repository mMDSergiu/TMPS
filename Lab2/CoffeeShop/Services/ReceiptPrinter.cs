using CoffeeShop.Models;
using CoffeeShop.Output;

namespace CoffeeShop.Services;

public class ReceiptPrinter : IReceiptPrinter
{
    private readonly IOutput output;

    public ReceiptPrinter(IOutput output)
    {
        this.output = output;
    }

    public void Print(Order order, decimal subtotal, decimal total, string discountName)
    {
        output.WriteLine("Order #" + order.Id);
        output.WriteLine("Client: " + order.CustomerName);
        foreach (var drink in order.Drinks)
        {
            output.WriteLine($"  {drink,-40}{drink.Price,6} lei");
        }

        output.WriteLine($"Subtotal: {subtotal} lei");
        if (subtotal != total)
            output.WriteLine($"Discount: {discountName}");
        output.WriteLine($"Total: {total} lei");
    }
}
