using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
//Enes
//Genel ses islemleri için oluşturduğum class buradan metodları çağırıp kullanıcaz mesela soru yanlışsa if komuduyla yanlış
//sesi oynatıcaz bunu da sesler klasörüne ses ekleriz yanlış sesini koyup oradan dosya yoluyla çağırırız SesIslemleri.sesOynat()
//ses dosyaları .wav türünde olmalı dosya formatından değiştiririz
//örnek dosya yolu  C:/Users/Sounds/yanlisSesi.wav
public static class SesIslemleri
{
    public static void sesOynat(string DosyaYolu)
    {
        SoundPlayer player = new SoundPlayer(@"" + DosyaYolu);
        player.Play();
        
    }
}
