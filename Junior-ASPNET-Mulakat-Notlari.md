# Junior ASP.NET Core mülakat notları

**Project2EmailNight · 10 Kasım 2026**

## Değerlendirme

Bu proje üzerinden yapılan ilk teknik görüşmede, junior pozisyon için seni bir sonraki aşamaya alırdım. GET ve POST akışını doğru anlattın, `await` için kendi örneğini kurdun ve Razor’daki tip uyuşmazlığını yakaladın. Takip sorularında, özellikle formun tekrar gönderilmesi ve şifre onayının neden saklanmadığı konusunda doğru sonuca ulaştın.

İşe alım kararı için uygulamalı bir görevde kodu nasıl takip ettiğini ve bir hatayı nasıl teşhis ettiğini de görmek isterdim. Bu turda en çok gelişim alanı, doğru sezgiyi teknik mekanizmayla eşleştirmekti. Örneğin “hata verir” dedikten sonra hatanın controller oluşturulurken mi, sorgu çalışırken mi, view oluşturulurken mi ortaya çıktığını söyleyebilirsin.

## Cevaplar ve düzeltmeler

| Konu | Cevabında öne çıkan nokta | Mülakatta nasıl netleştirebilirsin? |
| --- | --- | --- |
| GET, POST ve model doğrulama | Formu açarken GET, gönderirken POST çalıştığını doğru söyledin. İlk cevabında model doğrulamayı veritabanındaki şifre kontrolüyle karıştırdın. | `ModelState.IsValid`, form verisi modele aktarılırken veya doğrulanırken hata oluşup oluşmadığını gösterir. Giriş DTO’sundaki null kabul etmeyen kullanıcı adı ve şifre alanları MVC tarafından zorunlu sayılır. Kimlik bilgileri daha sonra kontrol edilir. |
| E-posta onayı | E-posta onayı kontrolünden sonra `SignInManager` ile giriş yapıldığını doğru anlattın. | Sıra model doğrulama, kullanıcıyı bulma, şifreyi doğrulama, e-posta onayını kontrol etme ve oturum açma şeklinde. Onay yoksa kullanıcı giriş ekranında uyarı görür. |
| Bağımlılık enjeksiyonu | `UserManager` ve `SignInManager` görevlerini doğru ayırdın. Servis kaydı kaldırılırsa hata olacağını bildin. | Controller servisleri constructor üzerinden alır. Bu, bağımlılık enjeksiyonudur. Servisler kayıtlı değilse ASP.NET Core controller’ı oluşturamaz ve action çalışmaz. |
| `async` ve `await` | Garson benzetmen, bekleme sırasında sunucunun başka istekleri işleyebilmesini iyi anlattı. `ToListAsync` için tek sorgunun hızlanacağını düşündün. | Bekleyen action, sonuç gelince devam eder. Bu sırada sunucu iş parçacığı başka isteklere hizmet edebilir. Sorgunun kendi süresi ayrıca değerlendirilir. |
| View ve yönlendirme | Hata sonrası giriş ekranında yenilemenin formu yeniden gönderebileceğini doğru söyledin. | Hata durumunda view mevcut POST isteğinin yanıtıdır. Başarılı girişte tarayıcı yönlendirmeyi takip edip profil için yeni bir GET gönderir. Yenilemenin davranışı, sayfayı getiren son isteğe bağlıdır. |
| Kimlik doğrulama ve yetkilendirme | Sıralamayı ve rol kontrolünü doğru anlattın. Panel yönlendirmesini yetkilendirmenin göreviyle bir tuttun. | Kimlik doğrulama isteği kimin yaptığını belirler. Yetkilendirme, o kişinin istediği kaynağa erişip erişemeyeceğine karar verir. Giriş sonrası hangi panele gidileceği uygulama akışının ayrı bir kararıdır. |
| `Authorize` ve `AllowAnonymous` | Oturum açmamış kullanıcının profile giremeyeceğini ve giriş ekranının herkese açık olması gerektiğini bildin. | Bu projede oturum açmamış biri profili isterse profil action’ı çalışmadan giriş ekranına yönlendirilir. Proje genelinde oturum şartı varsa giriş formunun GET ve POST action’larına `[AllowAnonymous]` eklenebilir. |
| EF Core ve LINQ | Alıcı e-postasının hangi değerle eşleştiğini doğru anlattın. | `Where` filtreyi tanımlar. `ToList` sorguyu çalıştırıp sonuçları listeye alır. Eşleşme yoksa boş liste gelir. `ToListAsync` aynı sorguyu asenkron çalıştırır. |
| Razor `@model` ve `Model` | Mesajların döngüyle gösterildiğini ve tek nesne bekleyen view’a liste verilince hata oluşacağını doğru söyledin. | `@model` view’ın beklediği tipi belirtir. `Model` controller’ın gönderdiği değerdir. Bir entity, DTO veya bunların listesi olabilir. Bu örnekte veriyi controller çeker, view hazır listeyi gösterir. |
| Kayıt DTO’su | İkinci şifrenin yalnızca eşleşme kontrolü için gerektiğini ve hesapta saklanmadığını doğru söyledin. | `UserRegisterDto` form alanlarını taşır. `[Compare]` iki şifrenin eşleşmesini denetler. Controller, hesap için gerekli alanlarla ayrı bir `AppUser` oluşturur. |
| Şifre hash’i ve salt | Identity’nin hash sakladığını ve aynı şifrenin farklı hash’ler üretebileceğini bildin. | Aynı yöntem, her şifre kaydında üretilen rastgele bir salt ile çalışır. Girişte Identity kayıtlı salt ile girilen şifreden yeniden hash hesaplar ve kayıtlı değerle karşılaştırır. |

## Projede tekrar bakılacak yerler

- [Giriş ve çıkış akışı](Project2EmailNight/Controllers/LoginController.cs)
- [Identity ayarları ve middleware sırası](Project2EmailNight/Program.cs)
- [Profil erişimi](Project2EmailNight/Controllers/ProfileController.cs)
- [Gelen kutusu sorgusu](Project2EmailNight/Controllers/MessageController.cs)
- [Gelen kutusu view’ı](Project2EmailNight/Views/Message/Inbox.cshtml)
- [Kayıt formunun doğrulama kuralları](Project2EmailNight/Dtos/UserRegisterDto.cs)
- [Kayıt akışı](Project2EmailNight/Controllers/RegisterController.cs)

## İkinci turda ele alınacak konular

- Kayıt ve profil formlarındaki doğrulama kurallarını kod üzerinden incelemek. Kuralların ne zaman çalıştığını ve hata mesajlarının ekrana nasıl ulaştığını açıklamak.
- View Component’lerin hangi verileri hazırladığını, kullandıkları sorguları ve sonuçları view’a nasıl aktardıklarını kod üzerinden anlatmak.
- Bu turda ele alınmayan diğer konuları proje dosyalarından seçmek.
