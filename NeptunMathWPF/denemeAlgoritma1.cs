//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//// Tüm satırları yorum satırına aldım denemeAlgoritma2 ile çakışıyor diye bu önceki deneme Okumak için hepsini seçip üssten yorum satırından kaldırılır
//// Burada kullanıcı için zorluk seviyesi belirlenebilir örnek olsun diye kullanıcı classı oluşturdum
////mesela illa da kullanıcı olmak zorunda değil zorluk seviyesini sadece bir değişkende de tutabiliriz fakat
//// her kullanıcının kendi seviyesinde zorluk seviyesi olması olacaksa böyle olması daha kullanışlı olabilir
//public class Kullanici
//{
//    public int ZorlukSeviyesi { get; set; } = 5; // Başlangıç seviyesi 5 verdim
//    public List<Soru> CozulenSorular { get; set; } = new List<Soru>(); // aynı soruyu sormasın diye
//}

//public class Soru
//{
//    public string SoruMetni { get; set; } //sorunın kendisi
//    public string Cevap { get; set; } // sorunun cevabı
//    public int ZorlukSeviyesi { get; set; } // sorunun zorluk seviyesi
//    // Diğer soru özellikleri eklenebilir
//}

//public static class SoruYoneticisi
//{
//    //Buradaki sorun bizim soru metnini random olarak oluşturmamız yani biz random fonksiyonuyla soruların
//    //sayılarını değiştiriyoruz fakat random sayılarla soru ürettiğimiz için soruya zorluk seviyesi belirlememiz
//    //imkansızlaşıyor. bizim projede bu şekilde tanımlamamız mümkün değil bu da bir sorun oluşturuyor
//    private static List<Soru> soruHavuzu = new List<Soru>()
//    {
//        // Örnek sorular
//        new Soru { SoruMetni = "2 + 2 = ?", Cevap = "4", ZorlukSeviyesi = 1 },
//        new Soru { SoruMetni = "5 * 3 = ?", Cevap = "15", ZorlukSeviyesi = 3 },
//        new Soru { SoruMetni = "10 - 7 = ?", Cevap = "3", ZorlukSeviyesi = 2 },
//        new Soru { SoruMetni = "x^2 - 4 = 0, x = ?", Cevap = "2", ZorlukSeviyesi = 6 },
//        // ... Daha fazla soru
//    };

//    private static Random rnd = new Random();

//    public static Soru GetirSoru(Kullanici kullanici)
//    {
//        // Kullanıcının seviyesine uygun soruları filtrele
//        var uygunSorular = soruHavuzu.Where(s =>
//                        s.ZorlukSeviyesi >= kullanici.ZorlukSeviyesi - 1 &&
//                        s.ZorlukSeviyesi <= kullanici.ZorlukSeviyesi + 1 &&
//                        !kullanici.CozulenSorular.Contains(s) // Daha önce çözülenlerden olmasın
//                        ).ToList();

//        if (uygunSorular.Count == 0)
//        {
//            // Uygun soru yoksa en yakın seviyedeki rastgele bir soru seç 5 yoksa 6 veya 4 seç
//            uygunSorular = soruHavuzu.OrderBy(s => Math.Abs(s.ZorlukSeviyesi - kullanici.ZorlukSeviyesi)).ToList();
//            if (uygunSorular.Count > 0)
//            {
//                int r = rnd.Next(uygunSorular.Count);
//                return uygunSorular[r];
//            }
//            else
//            {
//                return null; // Soru kalmadı null döndür
//            }

//        }

//        int index = rnd.Next(uygunSorular.Count);
//        return uygunSorular[index];
//    }

//    public static void CevapKontrolEt(Kullanici kullanici, Soru soru, string kullaniciCevabi)
//    {
//        if (soru.Cevap == kullaniciCevabi)
//        {
//            kullanici.ZorlukSeviyesi = Math.Min(10, kullanici.ZorlukSeviyesi + 1); // Seviye artır
//        }
//        else
//        {
//            kullanici.ZorlukSeviyesi = Math.Max(1, kullanici.ZorlukSeviyesi - 1); // Seviye azalt
//        }

//        kullanici.CozulenSorular.Add(soru); // Çözülenlere ekle
//    }
//}

//// en basitinden kullanıcı için konsol örneği
//// yorum satırına aldım birden fazla int main olmasın diye
////public class Program
////{
////    public static void Main(string[] args)
////    {
////        Kullanici aktifKullanici = new Kullanici();
////        Soru mevcutSoru = SoruYoneticisi.GetirSoru(aktifKullanici);

////        if (mevcutSoru != null)
////        {
////            Console.WriteLine("Soru: " + mevcutSoru.SoruMetni);
////            Console.Write("Cevabınız: ");
////            string kullaniciCevabi = Console.ReadLine();

////            SoruYoneticisi.CevapKontrolEt(aktifKullanici, mevcutSoru, kullaniciCevabi);

////            Console.WriteLine("Yeni Zorluk Seviyesi: " + aktifKullanici.ZorlukSeviyesi);
////        }
////        else
////        {
////            Console.WriteLine("Çözülecek soru kalmadı.");
////        }
////    }
////}
