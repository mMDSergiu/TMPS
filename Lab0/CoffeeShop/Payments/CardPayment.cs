namespace CoffeeShop.Payments;

public class CardPayment : IPaymentMethod
{
    private string cardNumber;

    public CardPayment(string cardNumber)
    {
        this.cardNumber = cardNumber;
    }

    public bool Pay(decimal amount)
    {
        string last4 = cardNumber.Substring(cardNumber.Length - 4);

        Console.WriteLine($"Paid {amount} lei with card ****{last4}");
        return true;
    }
}
