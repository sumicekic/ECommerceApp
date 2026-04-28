using NUnit.Framework;
using Core;

namespace Tests.UnitTests
{
    [TestFixture]
    public class CartTests
    {
        private Product _product;
        private Cart _cart;

        [SetUp]
        public void Setup()
        {
            _product = new Product(1, "Laptop", 10000m, 5);
            _cart = new Cart();
        }

        // TC-01: WHITE BOX — AddItem geçerli ürün ekleme
        [Test]
        public void AddItem_ValidProduct_ShouldAddToCart()
        {
            _cart.AddItem(_product, 2);
            Assert.That(_cart.ItemCount(), Is.EqualTo(2));
        }

        // TC-02: WHITE BOX — Sıfır adet ekleme exception fırlatmalı
        [Test]
        public void AddItem_ZeroQuantity_ShouldThrowArgumentException()
        {
            Assert.Throws<System.ArgumentException>(() => _cart.AddItem(_product, 0));
        }

        // TC-03: WHITE BOX — Stok yetersizse exception fırlatmalı
        [Test]
        public void AddItem_ExceedStock_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<System.InvalidOperationException>(() => _cart.AddItem(_product, 10));
        }

        // TC-04: WHITE BOX — TotalPrice BUG testi (FAIL bekleniyor)
        [Test]
        public void TotalPrice_ShouldReturnCorrectValue()
        {
            _cart.AddItem(_product, 2);
            Assert.That(_cart.TotalPrice(), Is.EqualTo(20000m),
                "BUG: TotalPrice fiyati 2 ile carpiyor, sonuc yanlis!");
        }

        // TC-05: BLACK BOX — Ürün sepete eklenince ItemCount artar
        [Test]
        public void AddItem_ShouldIncreaseItemCount()
        {
            _cart.AddItem(_product, 1);
            Assert.That(_cart.ItemCount(), Is.EqualTo(1));
        }

        // TC-06: BLACK BOX — RemoveItem sonrası sepet boş olmalı
        [Test]
        public void RemoveItem_ShouldEmptyCart()
        {
            _cart.AddItem(_product, 1);
            _cart.RemoveItem(_product.Id);
            Assert.That(_cart.ItemCount(), Is.EqualTo(0));
        }

        // TC-07: BLACK BOX — Clear sonrası sepet tamamen boşalmalı
        [Test]
        public void Clear_ShouldRemoveAllItems()
        {
            _cart.AddItem(_product, 2);
            _cart.Clear();
            Assert.That(_cart.ItemCount(), Is.EqualTo(0));
        }

        // TC-08: GRAY BOX — Aynı ürün iki kez eklenince quantity toplanmalı
        [Test]
        public void AddItem_SameProductTwice_ShouldAccumulateQuantity()
        {
            _cart.AddItem(_product, 1);
            _cart.AddItem(_product, 2);
            Assert.That(_cart.ItemCount(), Is.EqualTo(3));
        }
    }
}