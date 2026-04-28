Bu proje, Yazılım Test ve Kalitesi dersi ödevi için hazırladığım, bir e-ticaret sisteminin temel işlevlerini C# ile kodlayıp NUnit kullanarak test ettiğim bir uygulamadır.

Projenin amacı, gerçek hayattaki bir e-ticaret akışını (ürün seçme, sepete ekleme, sipariş verme, ödeme yapma) kodlamak ve bu sisteme kasıtlı olarak yerleştirilen hataları farklı test yöntemleriyle tespit etmektir.

Proje yapısı şöyledir:
Core klasörü altında Product.cs, Cart.cs ve OrderService.cs dosyaları bulunmaktadır. Product.cs ürün modelini tanımlar. Cart.cs sepete ürün ekleme, çıkarma ve toplam fiyat hesaplama işlemlerini yapar. OrderService.cs ise sipariş oluşturma, ödeme alma ve sipariş iptal etme işlemlerini yönetir.

Tests.UnitTests klasörü altında CartTests.cs ve OrderServiceTests.cs dosyaları bulunmaktadır. Bu dosyalarda White Box, Black Box ve Gray Box test yöntemleriyle yazılmış toplam 12 test yer almaktadır.

Tests.IntegrationTests klasörü altında ECommerceFlowTests.cs dosyası bulunmaktadır. Bu dosyada ürün seçiminden ödemeye kadar tüm akışı test eden 2 entegrasyon testi yer almaktadır.

Kullandığım test türleri şunlardır:
White Box Test: Kodun iç yapısını bilerek yazdığım testlerdir. Hangi satırın ne yaptığını bildiğim için o satırı doğrudan hedef aldım.
Black Box Test: Kodun içine bakmadan sadece giriş ve çıkış davranışını test ettim. Yani metoda bir şey verdim, doğru sonucu döndürüyor mu diye baktım.
Gray Box Test: Kısmen iç yapıyı bilerek yazdığım testlerdir. Örneğin sepete aynı ürünü iki kez ekleyince miktarların toplanması gerektiğini bildiğim için bunu test ettim.
Integration Test: Birden fazla sınıfın birlikte çalışmasını test ettim. Örneğin Cart ve OrderService'in birlikte doğru çalışıp çalışmadığını kontrol ettim.

Tespit edilen bug'lar şunlardır:
BUG-1: Cart.cs içindeki TotalPrice() metodunda fiyat hesaplanırken sonuç 2 ile çarpılıyor. Bu yüzden toplam fiyat her zaman 2 katı çıkıyor. Beklenen 20000, gerçek çıktı 40000 oldu.
BUG-2: OrderService.cs içindeki ProcessPayment() metodunda tam ödeme tutarı kabul edilmiyor. Kontrol ifadesinde >= yerine > kullanıldığı için tam ödeme yapıldığında sistem ödemeyi reddediyor.
BUG-3: OrderService.cs içindeki PlaceOrder() metodunda sipariş verildikten sonra ürünün stok miktarı düşülmüyor. Bu yüzden stokta 5 adet olan bir üründen 2 adet sipariş verilse bile stok hâlâ 5 olarak kalıyor.

Test sonuçları: 14 testin 11'i başarılı, 3'ü başarısız oldu. Başarısız olan 3 test yukarıda açıklanan bug'ları yakalamak için kasıtlı olarak yazılmıştır. Detaylı açıklama TEST_REPORT.md dosyasında yer almaktadır.

Projeyi çalıştırmak için Visual Studio 2022 ile ECommerceApp.sln dosyasını açıp dotnet test komutunu çalıştırmanız yeterlidir. Ekstra bir ayar yapmaya gerek yoktur.

Hazırlayan: Sümeyye Çekiç - 20230108003 - BIP2
