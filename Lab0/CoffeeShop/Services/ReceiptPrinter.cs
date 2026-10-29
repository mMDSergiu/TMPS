using CoffeeShop.Models;

namespace CoffeeShop.Services;

public class ReceiptPrinter
{
    public void Print(Order order, decimal subtotal, decimal total, string discountName)
    {
        Console.WriteLine("Order #" + order.Id);
        Console.WriteLine("Client: " + order.CustomerName);
        foreach (var drink in order.Drinks)
        {
            Console.WriteLine($"  {drink.Name,-12}{drink.Price,8} lei");
        }


        Console.WriteLine($"Subtotal: {subtotal} lei");
        if (subtotal != total)
            Console.WriteLine($"Discount: {discountName}");
        Console.WriteLine($"Total: {total} lei");
    }
}
