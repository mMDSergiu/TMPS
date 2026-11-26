using CoffeeShop.Actions;
using CoffeeShop.Listeners;
using CoffeeShop.Statuses;

namespace CoffeeShop.Tests;

public class TillTests
{
    [Fact]
    public void Undo_brings_back_the_status_from_before()
    {
        var order = Make.PaidOrder();
        var till = new Till(new TestOutput());

        till.Run(new StartPreparing(order));
        till.Run(new MarkReady(order));
        Assert.IsType<Ready>(order.Status);

        till.Undo();
        Assert.IsType<Preparing>(order.Status);

        till.Undo();
        Assert.IsType<Paid>(order.Status);
    }

    [Fact]
    public void Refused_action_is_not_saved_so_there_is_nothing_to_undo()
    {
        var order = Make.PaidOrder();
        var till = new Till(new TestOutput());

        bool ok = till.Run(new MarkReady(order));

        Assert.False(ok);
        Assert.Empty(till.History);
        Assert.False(till.Undo());
        Assert.IsType<Paid>(order.Status);
    }

    [Fact]
    public void Action_on_the_till_changes_the_status_and_the_listeners_hear_it()
    {
        var order = Make.PaidOrder("Ana");
        var output = new TestOutput();
        var notifier = new TestNotifier();
        order.Subscribe(new PickupScreen(output));
        order.Subscribe(new SmsAlert(notifier));
        var till = new Till(output);

        till.Run(new StartPreparing(order));
        till.Run(new MarkReady(order));

        Assert.IsType<Ready>(order.Status);
        Assert.Contains(output.Lines, l => l.Contains("Ana: READY"));
        Assert.Equal(new[] { $"Ana: your order #{order.Id} is ready :)" }, notifier.Sent);
    }
}
