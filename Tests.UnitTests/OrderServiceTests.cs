using NUnit.Framework;
using Core;
using System;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Cart _cart;
        private Product _cheapProduct;
        private Product _expensiveProduct;

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService();
            _cart = new Cart();
            _cheapProduct = new Product(1, "Mouse", 40m, 10);
            _expensiveProduct = new Product(2, "Monitor", 200m, 10);
        }

        [Test]
        public void TC11_PlaceOrder_EmptyCart_ThrowsException() // BVA - Alt Sınır 0 Eleman
        {
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart));
        }

        [Test]
        public void TC12_PlaceOrder_AboveMinimumAmount_CreatesOrder() // EP - Geçerli Tutar / Integration
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.Pending, order.Status);
        }

        [Test]
        public void TC13_PlaceOrder_BelowMinimumAmount_ThrowsException() // BVA - Sınır Altı
        {
            _cart.AddItem(_cheapProduct, 1); // 40 TL (100 TL limitinin altında)
            // Hoca limiti 100 istedi ama kod 50'ye bakıyor. 40 için patlaması lazım ama patlamayacak. (TEST FAIL OLACAK)
            Assert.Throws<InvalidOperationException>(() => _orderService.PlaceOrder(_cart));
        }

        [Test]
        public void TC14_PlaceOrder_ExactlyMinimumAmount_CreatesOrder() // BVA - Tam Sınır Değeri
        {
            _cart.AddItem(new Product(3, "Keyboard", 100m, 5), 1);
            Assert.DoesNotThrow(() => _orderService.PlaceOrder(_cart));
        }

        [Test]
        public void TC15_PlaceOrder_ReducesProductStock() // Integration Test
        {
            _cart.AddItem(_expensiveProduct, 1); // Başlangıç stok 10
            _orderService.PlaceOrder(_cart);
            // Vizeden kalma BUG var, stok düşmüyor. Beklenen 9 ama 10 kalacak. (TEST FAIL OLACAK)
            Assert.AreEqual(9, _expensiveProduct.Stock);
        }

        [Test]
        public void TC16_ProcessPayment_ExactAmount_SetsStatusToPaid() // EP - Tam Ödeme
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);

            bool result = _orderService.ProcessPayment(order, order.TotalAmount);
            // Vize BUG'ı (>= yerine >). Tam ödeme kabul edilmeyecek. (TEST FAIL OLACAK)
            Assert.IsTrue(result);
            Assert.AreEqual(OrderStatus.Paid, order.Status);
        }

        [Test]
        public void TC17_ProcessPayment_OverPayment_SetsStatusToPaid() // BVA - Fazla Ödeme
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);

            bool result = _orderService.ProcessPayment(order, order.TotalAmount + 10m);
            Assert.IsTrue(result);
        }

        [Test]
        public void TC18_ProcessPayment_UnderPayment_ReturnsFalse() // EP - Eksik Ödeme
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);

            bool result = _orderService.ProcessPayment(order, order.TotalAmount - 10m);
            Assert.IsFalse(result);
        }

        [Test]
        public void TC19_CancelOrder_PendingOrder_SetsStatusToCancelled() // Black Box
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);

            _orderService.CancelOrder(order);
            Assert.AreEqual(OrderStatus.Cancelled, order.Status);
        }

        [Test]
        public void TC20_CancelOrder_PaidOrder_ThrowsException() // State Based Test
        {
            _cart.AddItem(_expensiveProduct, 1);
            var order = _orderService.PlaceOrder(_cart);
            _orderService.ProcessPayment(order, order.TotalAmount + 1m); // Ödendi statüsüne çek

            Assert.Throws<InvalidOperationException>(() => _orderService.CancelOrder(order));
        }
    }
}