namespace CoffeeShop.Output;

public interface IOutput
{
    void WriteLine(string text);
}

public class ConsoleOutput : IOutput
{
    public void WriteLine(string text) => Console.WriteLine(text);
}
