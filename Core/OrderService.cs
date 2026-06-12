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
        private const decimal MinimumOrderAmount = 100.0m; // YENİ: Hoca 100 TL min limit istedi

        public Order PlaceOrder(Cart cart)
        {
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cart is empty.");

            // YENİ: Minimum sipariş tutarı kontrolü
            // 🐛 YENİ BUG: 100 TL limitini yanlışlıkla 50 TL olarak kontrol ediyor! (Boundary testinde patlayacak)
            if (cart.GetFinalPrice() < MinimumOrderAmount - 50m)
                throw new InvalidOperationException("Minimum siparis tutari karsilanmadi.");

            // 🐛 ESKİ BUG: Stok düşülmüyor (Aynen koruduk)
            var order = new Order
            {
                OrderId = _orderCounter++,
                Items = new List<(Product, int)>(cart.Items),
                TotalAmount = cart.GetFinalPrice(), // YENİ: İndirimli fiyatı siparişe yansıtıyoruz
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

            // 🐛 ESKİ BUG: >= yerine > kullanılmış (Aynen koruduk)
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