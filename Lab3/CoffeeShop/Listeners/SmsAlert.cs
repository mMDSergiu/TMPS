using CoffeeShop.Models;
using CoffeeShop.Notifications;
using CoffeeShop.Statuses;

namespace CoffeeShop.Listeners;

public class SmsAlert : IOrderListener
{
    private readonly INotifier sms;

    public SmsAlert(INotifier sms) => this.sms = sms;

    public void StatusChanged(Order order)
    {
        if (order.Status is Ready)
            sms.Send(order.CustomerName, $"your order #{order.Id} is ready :)");

        else if (order.Status is Cancelled)
            sms.Send(order.CustomerName, $"your order #{order.Id} was cancelled");
    }
}
