using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF
{
    //HES olusturdu
    //Sorular soruların iceriği
    //Soruların Nesne hali, önerisi olan yazsın
    public class Soru
    {
        public enum Turler
        {
            islem,
            oranli,
            problem
        }

        Turler soruTur;

        string Metin;
        //Metinstream byte dizisi için
        Stream MetinStream;
        
        //Soru Seçenekleri başka bir nesne olan Secenek'le yapılacaktır farklı sayılarda olabilmesi için list yapıldı
        List<Secenek>Secenekler;
        int Seviye;
        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir
        public Soru(Turler tur, int soruseceneksayisi=3 ,int kod=0)
        {
            //Aşağıdaki farklı tür soruları nasıl aynı nesneyle yapabileceğimizin örneğidir.
            soruTur = tur;
            Seviye = kod;
            switch (soruTur)
            {
                case Turler.islem:
                    SoruUretici.IslemSorusuOlustur(this, soruseceneksayisi);
                    break;
                case Turler.oranli:
                    break;
            }
        }

        public int GetSeviye()
        {
            return Seviye;
        }
        public string GetMetin()
        {
            return Metin;
        }
    }
}
