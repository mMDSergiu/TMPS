namespace CoffeeShop.Actions;

public interface IOrderAction
{
    string Name { get; }

    void Execute();
    void Undo();
}
