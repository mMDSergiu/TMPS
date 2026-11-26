using CoffeeShop.Listeners;
using CoffeeShop.Models;

namespace CoffeeShop.Tests;

public class ListenerTests
{
    [Fact]
    public void Subscribed_listeners_are_notified_and_unsubscribed_ones_are_not()
    {
        var order = new Order("Ana");
        var first = new TestListener();
        var second = new TestListener();
        order.Subscribe(first);
        order.Subscribe(second);

        order.Pay();
        order.Unsubscribe(second);
        order.StartPreparing();

        Assert.Equal(new[] { "paid", "preparing" }, first.Seen);
        Assert.Equal(new[] { "paid" }, second.Seen);
    }

    [Fact]
    public void Sms_is_sent_only_when_the_order_is_ready()
    {
        var notifier = new TestNotifier();
        var order = new Order("Ana");
        order.Subscribe(new SmsAlert(notifier));

        order.Pay();
        order.StartPreparing();
        Assert.Empty(notifier.Sent);

        order.MarkReady();

        Assert.Single(notifier.Sent);
        Assert.Contains("ready", notifier.Sent[0]);
    }

    [Fact]
    public void Subscribing_twice_still_gives_one_message()
    {
        var order = new Order("Ion");
        var listener = new TestListener();

        order.Subscribe(listener);
        order.Subscribe(listener);
        order.Pay();

        Assert.Single(listener.Seen);
    }
}
