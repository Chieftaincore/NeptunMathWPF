using NeptunMathWPF.Fonksiyonlar;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Linq;

// İlk deneme1'den farklı olarak soru havuzu yerine soru üret kısmı var çünkü biz soruları önceden yazıp soru havuzuna atmıyoruz onun yerine
// kullanıcı istediğinde generator ile anında üretiyorduk o yüzden soru havuzu gereksiz oluyor.İlk denemede de dediğim gibi
// burada kullanici classı zorunlu değil zorluk seviyesini sadece bir değişkende de tutulabilir
public class Kullanici
{
    public int ZorlukSeviyesi { get; set; } = 5; //zorluk seviyesi default 5 seviye
    public List<Soru> CozulenSorular { get; set; } = new List<Soru>();
}

public class Soru
{
    public string SoruMetni { get; set; }//sorunın kendisi
    public string Cevap { get; set; }// sorunun cevabı
    public int ZorlukSeviyesi { get; set; }// sorunun zorluk seviyesi
    //public SoruTuru SoruTuru {get; set;} //soru türünü tutmak istersek işe yarar
}

public static class SoruYoneticisi
{
    private static Random rnd = new Random();
    //private static List<Soru> soruHavuzu = new List<Soru>(); //soru havuzunu kaldırdım çünkü önceden soru tanımlamıyoruz
    //onun yerine kullanıcı istediğinde soru oluşturuyoruz ve bu random sayılarla oluşturuluyordu

    //soru üretme işini yapan metot(SoruAjani sınıfından uyarlanacak)
    public static Soru UretSoru(int zorlukSeviyesi)
    {
        // Burada SoruAjani sınıfını kullanarak soru üretiliyor
        // Örnek soru üretimi : 
        // 1- Soru tipini belirle (fonksiyon, işlem, vb.)
        // 2- Zorluk seviyesine göre sayıların aralıkları ayarla
        // 3- SoruAjani'ndaki metotları kullanarak soruyu oluşturulur
        // 4- Oluşturulan sorunun zorluk seviyesini belirle
        Soru uretilenSoru;
        switch (zorlukSeviyesi)
        {
            //soru ajani classındaki soru generator isimlerini buraya aktaramadığım için altını çizip hata veriyor
            // buradaki soru ajani classindaki soru generator isimleri örnektir o yüzden hata veriyor
            case 1:
                // seviye 1 
                uretilenSoru = SoruAjani.IslemSorusuUret(1, 10, 1, 5); //örnek parametreler : ilk sayı için 1 ila 10 ikinci sayı için 1 ila 5 arasında sayı üret
                uretilenSoru.ZorlukSeviyesi = 1;
                break;
            case 2:
                //seviye 2 ...
                uretilenSoru = SoruAjani.IslemSorusuUret(5, 15, 2, 8); //ilk sayı için 5 ila 15 ikinci sayı için 2 ila 8 arasında
                uretilenSoru.ZorlukSeviyesi = 2;
                break;
            case 3:
                uretilenSoru = SoruAjani.IslemSorusuUret(10, 20, 5, 10);
                uretilenSoru.ZorlukSeviyesi = 3;
                break;
            case 4:
                uretilenSoru = SoruAjani.FonksiyonSorusuUret(FunctionType.Linear, 1, 5); //Lineer fonksiyon için x in katsayısı 1 sabit değer 5 sabit değeri random da atanabilir
                uretilenSoru.ZorlukSeviyesi = 4;
                break;
            case 5:
                uretilenSoru = SoruAjani.FonksiyonSorusuUret(FunctionType.Quadratic, 2, 7);//karesel fonksiyon x^2 gibi
                uretilenSoru.ZorlukSeviyesi = 5;
                break;
            case 6:
                uretilenSoru = SoruAjani.FonksiyonSorusuUret(FunctionType.Root, 3, 9);//köklü fonksiyon
                uretilenSoru.ZorlukSeviyesi = 6;
                break;
            case 7:
                uretilenSoru = SoruAjani.LimitSorusuUret(7);// 7 zorluk seviyesine sahip Limit sorusu
                uretilenSoru.ZorlukSeviyesi = 7;
                break;
            case 8:
                uretilenSoru = SoruAjani.LimitSorusuUret(9); // 9 zorluk seviyesine sahip Limit sorusu örnek lim (x->2) (x^2 - 4) / (x - 2)
                uretilenSoru.ZorlukSeviyesi = 8;
                break;
            case 9:
                uretilenSoru = SoruAjani.TurevSorusuUret(9);
                uretilenSoru.ZorlukSeviyesi = 9;
                break;
            case 10:
                uretilenSoru = SoruAjani.TurevSorusuUret(10);
                uretilenSoru.ZorlukSeviyesi = 10;
                break;
            default:
                uretilenSoru = SoruAjani.IslemSorusuUret(1, 10, 1, 5);
                uretilenSoru.ZorlukSeviyesi = 1;
                break;

        }

        return uretilenSoru;
    }

    public static Soru GetirSoru(Kullanici kullanici)
    {
        // Kullanıcının seviyesine uygun soru üret
        int zorlukSeviyesi = kullanici.ZorlukSeviyesi;
        Soru yeniSoru = UretSoru(zorlukSeviyesi);

        // Daha önce çözülen sorulardan farklı bir soru üretmek için (gerekirse)
        while (kullanici.CozulenSorular.Contains(yeniSoru))
        {
            yeniSoru = UretSoru(zorlukSeviyesi);
        }
        return yeniSoru;
    }

    public static void CevapKontrolEt(Kullanici kullanici, Soru soru, string kullaniciCevabi)
    {
        if (soru.Cevap == kullaniciCevabi)
        {   //doğruysa 1 arttır
            kullanici.ZorlukSeviyesi = Math.Min(10, kullanici.ZorlukSeviyesi + 1);
        }
        else
        {   //değilse azalt
            kullanici.ZorlukSeviyesi = Math.Max(1, kullanici.ZorlukSeviyesi - 1);
        }

        kullanici.CozulenSorular.Add(soru);
    }
}

//main metodu ile konsol örneği yorum satırını aldım hepsini int main birden fazla olursa diye
public class Program
{
    public static void Main(string[] args)
    {
        Kullanici aktifKullanici = new Kullanici();
        Soru mevcutSoru;

        for (int i = 0; i < 5; i++) // 5 soru sorulacak örnek
        {
            mevcutSoru = SoruYoneticisi.GetirSoru(aktifKullanici);
            Console.WriteLine("Soru: " + mevcutSoru.SoruMetni);
            Console.Write("Cevabınız: ");
            string kullaniciCevabi = Console.ReadLine();

            SoruYoneticisi.CevapKontrolEt(aktifKullanici, mevcutSoru, kullaniciCevabi);
            Console.WriteLine("Yeni Zorluk Seviyesi: " + aktifKullanici.ZorlukSeviyesi);
        }
    }
}

/*
SoruAjani sınıfından uyarlayanabilecek metotlar:
- IslemSorusuUret()
- FonksiyonSorusuUret() (tüm fonksiyon tipleri için ayrı ayrı metotlar olabilir)
- LimitSorusuUret()
- TurevSorusuUret()
*/
