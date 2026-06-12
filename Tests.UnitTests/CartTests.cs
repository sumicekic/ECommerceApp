using NUnit.Framework;
using Core;
using System;

namespace Tests.UnitTests
{
    [TestFixture]
    public class CartTests
    {
        private Cart _cart;
        private Product _testProduct;

        [SetUp]
        public void Setup()
        {
            _cart = new Cart();
            _testProduct = new Product(1, "Laptop", 1000m, 5);
        }

        [Test]
        public void TC01_AddItem_ValidQuantity_IncreasesItemCount() // EP - Geçerli Değer / Black Box
        {
            _cart.AddItem(_testProduct, 1);
            Assert.AreEqual(1, _cart.ItemCount());
        }

        [Test]
        public void TC02_AddItem_ZeroQuantity_ThrowsException() // BVA - Sınır Değer
        {
            Assert.Throws<ArgumentException>(() => _cart.AddItem(_testProduct, 0));
        }

        [Test]
        public void TC03_AddItem_MoreThanStock_ThrowsException() // BVA - Geçersiz Üst Sınır
        {
            Assert.Throws<InvalidOperationException>(() => _cart.AddItem(_testProduct, 6));
        }

        [Test]
        public void TC04_TotalPrice_CalculatesCorrectBasePrice() // Unit Test
        {
            _cart.AddItem(_testProduct, 2);
            // Beklenen 2000, ama vizeden kalan x2 bug'ı yüzünden 4000 dönecek (TEST FAIL OLACAK)
            Assert.AreEqual(2000m, _cart.TotalPrice());
        }

        [Test]
        public void TC05_ApplyDiscount_ValidPercentage_SetsCorrectly() // EP - Geçerli İndirim
        {
            _cart.ApplyDiscount(20);
            Assert.AreEqual(20, _cart.DiscountPercentage);
        }

        [Test]
        public void TC06_GetFinalPrice_WithDiscount_CalculatesCorrectly() // Unit Test
        {
            _cart.AddItem(_testProduct, 1); // 1000 TL
            _cart.ApplyDiscount(10); // %10 indirim
            // Yeni eklediğimiz indirim bug'ı var, %20 uygulayacak. (TEST FAIL OLACAK)
            Assert.AreEqual(900m, _cart.GetFinalPrice());
        }

        [Test]
        public void TC07_ApplyDiscount_NegativePercentage_ThrowsException() // BVA - Negatif Değer
        {
            // İndirim eksi olamaz ama kontol koymadık (Bug). (TEST FAIL OLACAK)
            Assert.Throws<ArgumentException>(() => _cart.ApplyDiscount(-5));
        }

        [Test]
        public void TC08_ApplyDiscount_Over100Percentage_ThrowsException() // BVA - Üst Sınır Aşımı
        {
            // %150 indirim olamaz. Kontrol yok (Bug). (TEST FAIL OLACAK)
            Assert.Throws<ArgumentException>(() => _cart.ApplyDiscount(150));
        }

        [Test]
        public void TC09_Clear_EmptiesCartAndResetsDiscount() // Gray Box
        {
            _cart.AddItem(_testProduct, 1);
            _cart.ApplyDiscount(15);
            _cart.Clear();
            Assert.AreEqual(0, _cart.ItemCount());
            Assert.AreEqual(0, _cart.DiscountPercentage);
        }

        [Test]
        public void TC10_RemoveItem_RemovesCorrectProduct() // Unit Test
        {
            _cart.AddItem(_testProduct, 1);
            _cart.RemoveItem(1);
            Assert.AreEqual(0, _cart.ItemCount());
        }
    }
}