using NUnit.Framework;
using Core;

namespace Tests.UnitTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Product _product;
        private Cart _cart;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _product = new Product(1, "Telefon", 5000m, 10);
            _cart = new Cart();
            _orderService = new OrderService();
        }

        // TC-09: GRAY BOX — Sipariş verilince cart temizlenmeli
        [Test]
        public void PlaceOrder_ShouldClearCartAfterOrder()
        {
            _cart.AddItem(_product, 1);
            _orderService.PlaceOrder(_cart);
            Assert.That(_cart.ItemCount(), Is.EqualTo(0));
        }

        // TC-10: GRAY BOX — ProcessPayment BUG testi (FAIL bekleniyor)
        [Test]
        public void ProcessPayment_ExactAmount_ShouldReturnTrue()
        {
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);
            bool result = _orderService.ProcessPayment(order, order.TotalAmount);
            Assert.That(result, Is.True,
                "BUG: Tam odeme >= ile kontrol edilmeli, > hatali!");
        }

        // TC-11: WHITE BOX — Boş sepete sipariş exception fırlatmalı
        [Test]
        public void PlaceOrder_EmptyCart_ShouldThrowException()
        {
            Assert.Throws<System.InvalidOperationException>(
                () => _orderService.PlaceOrder(_cart));
        }

        // TC-12: BLACK BOX — İptal edilen siparişe ödeme exception fırlatmalı
        [Test]
        public void ProcessPayment_CancelledOrder_ShouldThrowException()
        {
            _cart.AddItem(_product, 1);
            var order = _orderService.PlaceOrder(_cart);
            _orderService.CancelOrder(order);
            Assert.Throws<System.InvalidOperationException>(
                () => _orderService.ProcessPayment(order, 99999m));
        }
    }
}