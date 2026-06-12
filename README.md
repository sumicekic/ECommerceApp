# 🛒 ECommerceApp - Yazılım Test ve Kalitesi Final Projesi

Bu proje, Yazılım Test ve Kalitesi dersi final ödevi için hazırladığım, bir e-ticaret sisteminin temel işlevlerini (ürün seçme, sepete ekleme, sipariş verme) stok kontrolü, indirim uygulaması ve minimum sipariş tutarı özellikleriyle genişletip C# ve NUnit kullanarak test ettiğim bir uygulamadır.

Projenin amacı, genişletilmiş sisteme kasıtlı olarak yerleştirilen hataları (bug) STLC sürecine uygun olarak, Equivalence Partitioning (EP) ve Boundary Value Analysis (BVA) test dizayn teknikleriyle tespit etmektir.

## 🏗️ Proje Yapısı
* **Core Klasörü:** `Product.cs`, `Cart.cs` ve `OrderService.cs` dosyaları bulunmaktadır. Ürün modeli, sepete ürün ekleme/çıkarma, toplam fiyat hesaplama, sipariş ve ödeme işlemleri burada yönetilir.
* **Tests.UnitTests Klasörü:** `CartTests.cs` dosyası bulunmaktadır.
* **Tests.IntegrationTests Klasörü:** `OrderServiceTests.cs` dosyası bulunmaktadır. 

## 🔁 STLC (Test Süreci)
1. **Requirement:** İndirim, stok ve minimum sipariş (100 TL) kurallarının analizi yapıldı.
2. **Test Plan:** NUnit altyapısı ve test kapsamı belirlendi.
3. **Test Design:** BVA ve EP teknikleriyle 20 test senaryosu tasarlandı.
4. **Test Execution:** Yazılan testler Visual Studio'da çalıştırıldı.
5. **Test Result & Reporting:** Çıkan hatalar ve test başarı oranları raporlandı.

## 🧪 Test Türleri ve Teknikleri
* **White Box Test:** Kodun iç yapısını bilerek yazdığım testlerdir. Hangi satırın ne yaptığını bildiğim için o satırı doğrudan hedef aldım.
* **Black Box Test:** Kodun içine bakmadan sadece giriş ve çıkış davranışını test ettim. Metoda veriyi verip doğru sonucu döndürüyor mu diye baktım.
* **Gray Box Test:** Kısmen iç yapıyı bilerek yazdığım testlerdir. Nesnelerin durum değişimlerini kontrol ettim.
* **Integration Test:** Birden fazla sınıfın (örn: Cart ve OrderService) birlikte doğru çalışıp çalışmadığını kontrol ettim.
* **Test Case Dizaynı:** Sınırları test etmek için **Boundary Value Analysis (BVA)** (örn: %100 üstü indirim, 0 stok) ve geçerli/geçersiz girdi grupları için **Equivalence Partitioning (EP)** tekniklerini kullandım.

## ⚠️ Hata Kavramları ve Stratejiler
* **Error:** Geliştiricinin kod yazarken yaptığı insan kaynaklı yanlışlık.
* **Fault:** Error sonucunda kodun içine yerleşen kusur (Bug).
* **Failure:** Yazılım çalışırken Fault'un tetiklenip sistemin yanlış sonuç vermesi.
* **Defect / Bug:** Test sürecinde benim tarafımdan tespit edilen ve raporlanan hatalar.
* **Test Stratejileri:** Çevik geliştirme süreçlerine uygun **Agile Testing**, sistemin en kritik yerlerine (ödeme ve stok) odaklanan **Risk-Based Testing** ve yeni eklenen indirim özelliklerinin eski sistemi bozmadığını teyit eden **Regression Testing** uygulandı.

## 🐞 Tespit Edilen Bug Listesi
* **BUG-1:** `Cart.cs` içindeki `TotalPrice()` metodunda fiyat hesaplanırken sonuç 2 ile çarpılıyor.
* **BUG-2:** `OrderService.cs` içindeki `ProcessPayment()` metodunda tam ödeme tutarı kabul edilmiyor. Kontrol ifadesinde >= yerine > kullanılmış.
* **BUG-3:** `OrderService.cs` içindeki `PlaceOrder()` metodunda sipariş verildikten sonra ürünün stok miktarı düşülmüyor.
* **BUG-4 (Yeni):** `Cart.cs` içindeki indirim hesaplaması hatalı; girilen indirimin 2 katını uyguluyor (%20 girilirse %40 düşüyor).
* **BUG-5 (Yeni):** Negatif (örn: -5) veya %100'den büyük geçersiz indirim oranları sistem tarafından engellenmiyor.
* **BUG-6 (Yeni):** `OrderService.cs` içindeki minimum sipariş tutarı gereksinimi 100 TL olmasına rağmen, kodda limit 50 TL olarak baz alınıp onay veriliyor.

## 📊 Test Sonuçları
Toplam 20 testin 13'ü başarılı (Passed), 7'si başarısız (Failed) olmuştur. Başarısız olan 7 test, yukarıda açıklanan 6 adet bug'ı yakalamak için kasıtlı olarak sınır değer (BVA) ve denklik (EP) teknikleriyle yazılmıştır. Amacına tamamen ulaşmıştır.

## 🚀 Kurulum ve Çalıştırma
Projeyi çalıştırmak için Visual Studio 2022 ile `ECommerceApp.sln` dosyasını açıp "Test Gezgini" üzerinden testleri çalıştırmanız veya terminalde `dotnet test` komutunu girmeniz yeterlidir. Ekstra bir ayar yapmaya gerek yoktur.

Hazırlayan: Sümeyye Çekiç - 20230108003 - BIP2
