Bu proje, Yazılım Test ve Kalitesi dersi final ödevi için hazırladığım, bir e-ticaret sisteminin temel işlevlerini (ürün seçme, sepete ekleme, sipariş verme, ödeme yapma) indirim uygulaması, stok kontrolü ve minimum sipariş tutarı limitleriyle genişletip C# ve NUnit kullanarak test ettiğim bir uygulamadır.

Projenin amacı, gerçek hayattaki bir e-ticaret akışını kodlamak ve bu sisteme kasıtlı olarak yerleştirilen hataları STLC sürecine uygun olarak, Equivalence Partitioning (EP) ve Boundary Value Analysis (BVA) test dizayn teknikleriyle tespit etmektir.

Proje yapısı şöyledir: Core klasörü altında Product.cs, Cart.cs ve OrderService.cs dosyaları bulunmaktadır. Product.cs ürün modelini tanımlar. Cart.cs sepete ürün ekleme, çıkarma, indirim uygulama ve toplam fiyat hesaplama işlemlerini yapar. OrderService.cs ise sipariş oluşturma (100 TL minimum limit kontrolüyle), ödeme alma ve sipariş iptal etme işlemlerini yönetir.

Tests.UnitTests klasörü altında CartTests.cs dosyası bulunmaktadır. Bu dosyada White Box, Black Box ve Gray Box test yöntemleriyle yazılmış birim testler yer almaktadır.

Tests.IntegrationTests klasörü altında OrderServiceTests.cs dosyası bulunmaktadır. Bu dosyada ürün seçiminden ödemeye kadar tüm akışı test eden entegrasyon testleri yer almaktadır.

Kullandığım test türleri şunlardır: White Box Test: Kodun iç yapısını bilerek yazdığım testlerdir. Hangi satırın ne yaptığını bildiğim için o satırı doğrudan hedef aldım. Black Box Test: Kodun içine bakmadan sadece giriş ve çıkış davranışını test ettim. Yani metoda bir şey verdim, doğru sonucu döndürüyor mu diye baktım. Gray Box Test: Kısmen iç yapıyı bilerek yazdığım testlerdir. Örneğin sepete aynı ürünü iki kez ekleyince miktarların toplanması gerektiğini bildiğim için bunu test ettim. Integration Test: Birden fazla sınıfın birlikte çalışmasını test ettim. Örneğin Cart ve OrderService'in birlikte doğru çalışıp çalışmadığını kontrol ettim. Ayrıca test senaryolarını yazarken sınır değerlerini test etmek için Boundary Value Analysis (BVA) ve geçerli/geçersiz girdi grupları oluşturmak için Equivalence Partitioning (EP) tekniklerini kullandım. Strateji olarak projedeki geliştirici hatalarına (Error) ve bu hataların kodda oluşturduğu kusurlara (Bug/Fault) odaklandım.

Tespit edilen bug'lar şunlardır: BUG-1: Cart.cs içindeki TotalPrice() metodunda fiyat hesaplanırken sonuç 2 ile çarpılıyor. Bu yüzden toplam fiyat her zaman 2 katı çıkıyor. BUG-2: OrderService.cs içindeki ProcessPayment() metodunda tam ödeme tutarı kabul edilmiyor. Kontrol ifadesinde >= yerine > kullanıldığı için tam ödeme yapıldığında sistem ödemeyi reddediyor. BUG-3: OrderService.cs içindeki PlaceOrder() metodunda sipariş verildikten sonra ürünün stok miktarı düşülmüyor. Bu yüzden stokta 5 adet olan bir üründen 2 adet sipariş verilse bile stok hâlâ 5 olarak kalıyor. BUG-4: Cart.cs içindeki indirim hesaplaması hatalı çalışıyor. Girilen indirimin 2 katını uyguluyor, örneğin %20 indirim girildiğinde fiyattan %40 düşüyor. BUG-5: İndirim uygulanırken eksi değerler (-5) veya %100'den büyük geçersiz oranlar sistem tarafından engellenmiyor. BUG-6: OrderService.cs içinde minimum sipariş tutarı gereksinimi 100 TL olarak belirlenmesine rağmen, kodda kontrol işlemi yanlışlıkla 50 TL üzerinden yapılıyor.

Test sonuçları: 20 testin 13'ü başarılı, 7'si başarısız oldu. Başarısız olan 7 test yukarıda açıklanan bug'ları yakalamak için kasıtlı olarak yazılmıştır.

Projeyi çalıştırmak için Visual Studio 2022 ile ECommerceApp.sln dosyasını açıp dotnet test komutunu çalıştırmanız yeterlidir. Ekstra bir ayar yapmaya gerek yoktur.

Hazırlayan: Sümeyye Çekiç - 20230108003 - BIP2
