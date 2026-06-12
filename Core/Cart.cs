using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Cart
    {
        private List<(Product product, int quantity)> _items = new();

        public IReadOnlyList<(Product product, int quantity)> Items => _items;

        public decimal DiscountPercentage { get; private set; } = 0;

        // YENİ: İndirim Uygulama
        public void ApplyDiscount(decimal percentage)
        {
            // 🐛 YENİ BUG: İndirim oranı %100'den büyük mü veya eksi mi diye kontrol etmiyor! (Boundary testinde patlayacak)
            DiscountPercentage = percentage;
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new System.ArgumentException("Quantity must be greater than zero.");

            if (product.Stock < quantity)
                throw new System.InvalidOperationException("Insufficient stock.");

            var existing = _items.FindIndex(i => i.product.Id == product.Id);
            if (existing >= 0)
            {
                var old = _items[existing];
                _items[existing] = (old.product, old.quantity + quantity);
            }
            else
            {
                _items.Add((product, quantity));
            }
        }

        public void RemoveItem(int productId)
        {
            _items.RemoveAll(i => i.product.Id == productId);
        }

        public decimal TotalPrice()
        {
            // 🐛 ESKİ BUG: fiyat 2 ile çarpılıyor (Aynen koruduk)
            return _items.Sum(i => i.product.Price * i.quantity * 2);
        }

        // YENİ: İndirimli Son Fiyatı Hesaplama
        public decimal GetFinalPrice()
        {
            decimal baseTotal = TotalPrice();
            // 🐛 YENİ BUG: İndirimi 2 katı uyguluyor. %20 girilirse %40 düşüyor. (EP testinde patlayacak)
            return baseTotal - (baseTotal * (DiscountPercentage * 2) / 100);
        }

        public int ItemCount()
        {
            return _items.Sum(i => i.quantity);
        }

        public void Clear()
        {
            _items.Clear();
            DiscountPercentage = 0; // Sepet temizlenince indirim de sıfırlansın
        }
    }
}