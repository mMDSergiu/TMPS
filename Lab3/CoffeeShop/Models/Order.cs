using CoffeeShop.Statuses;

namespace CoffeeShop.Models
{
    public class Order : ICopyable<Order>
    {
        static int counter = 1;

        private readonly List<IOrderItem> items = new();


        public int Id { get; private set; }


        public string CustomerName { get; set; }
        public IReadOnlyList<IOrderItem> Items => items;

        public OrderStatus Status { get; private set; } = new Placed();

        public Order(string customerName)
        {
            Id = counter++;
            CustomerName = customerName;
        }

        public void AddItem(IOrderItem item)
        {
            CheckItemsCanChange();
            items.Add(item);
        }

        public void RemoveItem(IOrderItem item)
        {
            CheckItemsCanChange();
            items.Remove(item);
        }

        private void CheckItemsCanChange()
        {
            if (!Status.CanChangeItems)
                throw new InvalidOperationException($"order #{Id} is {Status.Name}, the items cant be changed");
        }

        public void Pay() => Status.Pay(this);
        public void StartPreparing() => Status.StartPreparing(this);
        public void MarkReady() => Status.MarkReady(this);
        public void HandOver() => Status.HandOver(this);
        public void Cancel() => Status.Cancel(this);

        internal void SetStatus(OrderStatus status)
        {
            Status = status;
        }

        public Order Clone()
        {
            var copy = new Order(CustomerName);
            foreach (var item in items)
                copy.AddItem(item.Clone());

            return copy;
        }
    }
}
