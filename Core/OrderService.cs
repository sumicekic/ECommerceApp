using System;
using System.Collections.Generic;

namespace Core
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Cancelled
    }

    public class Order
    {
        public int OrderId { get; set; }
        public List<(Product product, int quantity)> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderService
    {
        private static int _orderCounter = 1;
        private List<Order> _orders = new();

        public Order PlaceOrder(Cart cart)
        {
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty.");

            // 🐛 BUG: Stok düşülmüyor
            var order = new Order
            {
                OrderId = _orderCounter++,
                Items = new List<(Product, int)>(cart.Items),
                TotalAmount = cart.TotalPrice(),
                Status = OrderStatus.Pending
            };

            _orders.Add(order);
            cart.Clear();
            return order;
        }

        public bool ProcessPayment(Order order, decimal amountPaid)
        {
            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Order is not in pending state.");

            // 🐛 BUG: >= yerine > kullanılmış
            if (amountPaid > order.TotalAmount)
            {
                order.Status = OrderStatus.Paid;
                return true;
            }

            return false;
        }

        public void CancelOrder(Order order)
        {
            if (order.Status == OrderStatus.Paid)
                throw new InvalidOperationException("Paid orders cannot be cancelled.");

            order.Status = OrderStatus.Cancelled;
        }

        public List<Order> GetAllOrders() => _orders;
    }
}