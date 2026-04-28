using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Cart
    {
        private List<(Product product, int quantity)> _items = new();

        public IReadOnlyList<(Product product, int quantity)> Items => _items;

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
            // 🐛 BUG: fiyat 2 ile çarpılıyor
            return _items.Sum(i => i.product.Price * i.quantity * 2);
        }

        public int ItemCount()
        {
            return _items.Sum(i => i.quantity);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}