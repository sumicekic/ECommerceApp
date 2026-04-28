# E-Commerce Test Raporu

## Proje Bilgileri
- Dil: C#
- Test Framework: NUnit
- Proje: ECommerceApp
- Tarih: 28.04.2026
- Hazırlayan: Sümeyye Çekiç - 20230108003 - BIP2

## Test Türleri

White Box Test: Kodun iç yapısı bilinerek yazılan testlerdir. Hangi satırın ne yaptığını bildiğim için o satırı doğrudan hedef aldım.

Black Box Test: Kodun içine bakmadan sadece giriş ve çıkış davranışını test ettim. Metoda bir şey verdim, doğru sonucu döndürüyor mu diye baktım.

Gray Box Test: Kısmen iç yapıyı bilerek yazdığım testlerdir. Örneğin sepete aynı ürünü iki kez ekleyince miktarların toplanması gerektiğini bildiğim için bunu test ettim.

Integration Test: Birden fazla sınıfın birlikte çalışmasını test ettim. Cart ve OrderService sınıflarının birlikte doğru çalışıp çalışmadığını kontrol ettim.

---

## Test Sonuçları

| TC    | Test Adı                                                | Tür         | Sonuç |
|-------|---------------------------------------------------------|-------------|-------|
| TC-01 | AddItem_ValidProduct_ShouldAddToCart                    | White Box   | PASS  |
| TC-02 | AddItem_ZeroQuantity_ShouldThrowArgumentException       | White Box   | PASS  |
| TC-03 | AddItem_ExceedStock_ShouldThrowInvalidOperationException | White Box   | PASS  |
| TC-04 | TotalPrice_ShouldReturnCorrectValue                     | White Box   | FAIL  |
| TC-05 | AddItem_ShouldIncreaseItemCount                         | Black Box   | PASS  |
| TC-06 | RemoveItem_ShouldEmptyCart                              | Black Box   | PASS  |
| TC-07 | Clear_ShouldRemoveAllItems                              | Black Box   | PASS  |
| TC-08 | AddItem_SameProductTwice_ShouldAccumulateQuantity       | Gray Box    | PASS  |
| TC-09 | PlaceOrder_ShouldClearCartAfterOrder                    | Gray Box    | PASS  |
| TC-10 | ProcessPayment_ExactAmount_ShouldReturnTrue             | Gray Box    | FAIL  |
| TC-11 | PlaceOrder_EmptyCart_ShouldThrowException               | White Box   | PASS  |
| TC-12 | ProcessPayment_CancelledOrder_ShouldThrowException      | Black Box   | PASS  |
| TC-13 | FullFlow_ProductToPayment_ShouldSucceed                 | Integration | PASS  |
| TC-14 | PlaceOrder_ShouldDecreaseStock                          | Integration | FAIL  |

Toplam: 14 test, 11 basarili, 3 basarisiz

## Hangi Testler Fail Oldu ve Neden?

TC-04 numarali test, Cart.cs dosyasinin icindeki TotalPrice() metodunu test etmektedir. Bu metotta fiyat hesaplanirken miktar ile carpildiktan sonra sonuc bir de 2 ile carpiliyor. Yani kod "Price x quantity x 2" seklinde calisiyor. Bu fazladan carpan yuzunden toplam fiyat her zaman olmasi gerekenin 2 kati cikiyor. Testde 2 adet 10000 TL'lik urun icin beklenen sonuc 20000 iken sistem 40000 donurdu. Dogru olmasi gereken ifade sadece "Price x quantity" seklinde olmalidir.

TC-10 numarali test, OrderService.cs dosyasindaki ProcessPayment() metodunu test etmektedir. Bu metodda odeme kontrolu yapilirken "buyuktur" operatoru kullanilmis, ancak olmasi gereken "buyuk esit" operatorudur. Bu kucuk hata yuzunden kullanici tam siparis tutarini odediginde sistem odemeyi gecersiz sayiyor ve False donuyor. Yani ornegin siparis tutari 10000 TL ise kullanici tam 10000 TL gonderdiginde odeme kabul edilmiyor, sadece 10001 TL ve uzeri kabul ediliyor. Test beklenen True degerini alamadigi icin basarisiz oldu.

TC-14 numarali test, siparis verildikten sonra urun stogundaki dusumu kontrol etmektedir. PlaceOrder() metodu siparis olusturuyor ancak urunun stok miktarini hic azaltmiyor. Yani stokta 5 adet olan bir urunden 2 adet siparis verilse bile stok sayisi hala 5 olarak kaliyor. Bu durum gercek bir e-ticaret sisteminde stok takibini tamamen islevsiz kilar. Testde siparis sonrasi stokun 3 olmasi beklenirken sistem 5 dondurdu. Cozum olarak PlaceOrder() metodu icerisine her siparis kalemi icin stok dusumu yapan bir satir eklenmesi gerekmektedir.
