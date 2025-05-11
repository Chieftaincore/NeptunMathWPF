using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{

    public class ZorlukModel
    {
        public SoruTerimleri.soruTuru soruTuru { get; set; }
        public int KullId { get => aktifKullanici.kullnId; }

        /// <summary>
        /// Model oluşturulurken seviyeye göre dictionary yapmak için
        /// </summary>
        public Dictionary<int, Func<Soru>> AlgoDictionary;

        public Func<Soru> Algorithma;

        public int seviye;
     

        public ZorlukModel()
        {

        }

        public Soru SonrakiAlgorithma(int _seviye)
        {
            List<Ifade> ifadeler = SoruAjani.CokluIfadeListesiOlustur(new List<SoruTerimleri.ifadeTurleri> { SoruTerimleri.ifadeTurleri.sayi, SoruTerimleri.ifadeTurleri.sayi });

            return SoruAjani.YerelSoruBirlestir(ifadeler, 5);
        }

        public Soru SonrakiAlgorithma(Func<Soru> func)
        {
            return func.Invoke();
        }

        public override string ToString()
        {
            return $"{soruTuru} {seviye}";
        }
    }

    public class IslemSoruZorlukModel : ZorlukModel
    {

    }

}
