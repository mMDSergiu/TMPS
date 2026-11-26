using CoffeeShop.Output;

namespace CoffeeShop.Actions;

public class Till
{
    private readonly Stack<IOrderAction> done = new();
    private readonly List<string> history = new();
    private readonly IOutput output;

    public Till(IOutput output)
    {
        this.output = output;
    }

    public IReadOnlyList<string> History => history;

    public bool Run(IOrderAction action)
    {
        output.WriteLine("till: " + action.Name);
        try
        {
            action.Execute();
        }
        catch (InvalidOperationException e)
        {
            output.WriteLine("  not allowed, " + e.Message);
            return false;
        }

        done.Push(action);
        history.Add(action.Name);
        return true;
    }

    public bool Undo()
    {
        if (done.Count == 0)
        {
            output.WriteLine("till: nothing to undo");
            return false;
        }

        var last = done.Pop();
        output.WriteLine("till: undo " + last.Name);
        last.Undo();

        history.Add("undo " + last.Name);
        return true;
    }
}
