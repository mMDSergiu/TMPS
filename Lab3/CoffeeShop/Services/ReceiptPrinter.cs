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
        foreach (var item in order.Items)
        {
            output.WriteLine($"  {item.Description,-40}{item.Price,6} lei");
            PrintParts(item, "    ");
        }

        output.WriteLine($"Subtotal: {subtotal} lei");
        if (subtotal != total)
            output.WriteLine($"Discount: {discountName}");
        output.WriteLine($"Total: {total} lei");
    }

    private void PrintParts(IOrderItem item, string indent)
    {
        foreach (var part in item.Parts)
        {
            output.WriteLine(indent + "- " + part.Description);
            PrintParts(part, indent + "  ");
        }
    }
}
