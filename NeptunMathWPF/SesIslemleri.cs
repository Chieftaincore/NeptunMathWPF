using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using NeptunMathWPF;
//Enes
//Genel ses islemleri için oluşturduğum class buradan metodları çağırıp kullanıcaz mesela soru yanlışsa if komuduyla yanlış
//sesi oynatıcaz bunu da sesler klasörüne ses ekleriz yanlış sesini koyup oradan dosya yoluyla çağırırız SesIslemleri.sesOynat()
//ses dosyaları .wav türünde olmalı dosya formatından değiştiririz
//örnek dosya yolu  C:/Users/Sounds/yanlisSesi.wav
public static class SesIslemleri
{
    private static SoundPlayer _player = new SoundPlayer(); // SoundPlayer nesnesini sınıf düzeyinde tanımlayalım

    public static void sesOynat(string DosyaYolu)
    {
        Genel.Handle(()=> { 
            _player.SoundLocation = @"" + DosyaYolu;
            _player.Play();
        });
    }
    //örnek kullanım SesIslemleri.sesDurdur();
    public static void sesDurdur()
    {
        _player.Stop();
    }
    public static void sesDonguyeAl(string DosyaYolu)
    {
        Genel.Handle(() =>
        {
            _player.SoundLocation = @"" + DosyaYolu;
            _player.PlayLooping();
        });
    }
    //Ses dosyası olup olmadığını kontrol edip çaldırmak daha mantıklı önce dosyanın varlığını kontrol eder sonra çaldırabilir
    public static bool sesDosyasiVarMi(string DosyaYolu)
    {
        return File.Exists(DosyaYolu);
    }
    //Örnek Kullanım
    /*string dosyaYolu = "C:/Users/Sounds/dogruSesi.wav";

        if (SesIslemleri.sesDosyasiVarMi(dosyaYolu))
        {
          Console.WriteLine($"'{dosyaYolu}' dosyası mevcut.");
          SesIslemleri.sesOynat(dosyaYolu); // Dosya varsa çal
            }
        else
        {
        Console.WriteLine($"'{dosyaYolu}' dosyası bulunamadı!");
        }*/

}
