using CoffeeShop.Models;

namespace CoffeeShop.Services;

public interface IReceiptPrinter
{
    void Print(Order order, decimal subtotal, decimal total, string discountName);
}
