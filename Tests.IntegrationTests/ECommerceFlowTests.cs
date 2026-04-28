using NUnit.Framework;
using Core;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ECommerceFlowTests
    {
        // TC-13: INTEGRATION — Tam akış testi
        [Test]
        public void FullFlow_ProductToPayment_ShouldSucceed()
        {
            var product = new Product(1, "Klavye", 500m, 3);
            var cart = new Cart();
            var orderService = new OrderService();

            cart.AddItem(product, 2);
            var order = orderService.PlaceOrder(cart);
            bool paid = orderService.ProcessPayment(order, order.TotalAmount + 1);

            Assert.That(paid, Is.True);
            Assert.That(order.Status, Is.EqualTo(OrderStatus.Paid));
        }

        // TC-14: INTEGRATION — Stok düşme BUG testi (FAIL bekleniyor)
        [Test]
        public void PlaceOrder_ShouldDecreaseStock()
        {
            var product = new Product(2, "Mouse", 300m, 5);
            var cart = new Cart();
            var orderService = new OrderService();

            cart.AddItem(product, 2);
            orderService.PlaceOrder(cart);

            Assert.That(product.Stock, Is.EqualTo(3),
                "BUG: Siparis sonrasi stok düsülmüyor!");
        }
    }
}