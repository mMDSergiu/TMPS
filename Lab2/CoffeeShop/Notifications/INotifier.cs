namespace CoffeeShop.Notifications;

public interface INotifier
{
    void Send(string to, string message);
}

public class ConsoleNotifier : INotifier
{
    public void Send(string to, string message)
    {
        Console.WriteLine($">> {to}: {message}");
    }
}


public class SmsNotifier : INotifier
{
    public void Send(string to, string message) => Console.WriteLine($"[SMS to {to}] {message}");
}
