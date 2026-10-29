namespace CoffeeShop.Payments;

public class CashPayment : IPaymentMethod
{

    private decimal cash;


    public CashPayment(decimal cash)
    {
        this.cash = cash;
    }

    public bool Pay(decimal amount)
    {
        if (cash < amount)
        {
            Console.WriteLine("Not enough money! need " + amount + " lei, got " + cash);
            return false;
        }
        Console.WriteLine($"Paid cash: {cash} lei, change: {cash - amount} lei");
        return true;
    }
}
