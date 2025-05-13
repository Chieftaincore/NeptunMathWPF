using System;
using System.Collections.Generic;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{

    using ifadeTuru = SoruTerimleri.ifadeTurleri;

    /// <summary>
    /// Ana Zorluk Model nesnesi diğer Zorluk Model Nesneleri için ortaklık içerir
    /// </summary>
    /// 
    public abstract class ZorlukModel
    {
        public SoruTerimleri.soruTuru soruTuru { get; set; }
        public int KullId { get => aktifKullanici.kullnId; }

        /// <summary>
        /// Model oluşturulurken seviyeye göre dictionary yapmak için
        /// </summary>
        public Dictionary<int, Func<Soru>> AlgoDictionary;

        internal Func<Soru> Algorithma;

        public int seviye;

        public virtual Soru SonrakiAlgorithma(int _seviye)
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
        internal Dictionary<string, int[]> DefaultAraliklar { get => ZorlukRepository.Araliklar; }

        public IslemSoruZorlukModel()
        {
            soruTuru = SoruTerimleri.soruTuru.islem;
        }

        public override Soru SonrakiAlgorithma(int _seviye)
        {
            List<ifadeTuru> _liste;
            int _seceneksayisi;

            if(_seviye < 4)
            {
                _seceneksayisi = 4;
            }
            else
            {
                _seceneksayisi = 5;
            }

            switch (_seviye)
            {

                case 1:
                    _liste = new List<ifadeTuru>()
                    {
                        ifadeTuru.sayi,
                        ifadeTuru.sayi,
                    };
                    break;
                case 2:

                    _liste = new List<ifadeTuru>()
                    {
                        ifadeTuru.sayi,
                        ifadeTuru.sayi,
                        ifadeTuru.sayi,
                    };

                    break;
                case 3:

                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.kesir}),
                        ifadeTuru.sayi,
                        ifadeTuru.sayi
                    };
                    break;
                case 4:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu}),
                        ifadeTuru.sayi,
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.kesir}),
                    };
                    break;

                case 5:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu, ifadeTuru.faktoriyel}),
                        ifadeTuru.sayi,
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.kesir}),
                    };
                    break;

                case 6:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu}),
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu}),
                        ifadeTuru.sayi
                    };
                    break;
                case 7:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu, ifadeTuru.faktoriyel}),
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu}),
                        ifadeTuru.sayi
                    };
                    break;
                case 8:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu, ifadeTuru.faktoriyel}),
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu, ifadeTuru.kesir}),
                        ifadeTuru.sayi,
                        ifadeTuru.sayi
                    };
                    break;
                case 9:
                    _liste = new List<ifadeTuru>()
                    {
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.faktoriyel}),
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.sayi, ifadeTuru.uslu, ifadeTuru.kesir}),
                        IfadeRNG(new ifadeTuru[] {ifadeTuru.uslu, ifadeTuru.sayi, ifadeTuru.faktoriyel}),
                        ifadeTuru.sayi
                    };
                    break;
                default:
                    return base.SonrakiAlgorithma(_seviye);
            }

            List<Ifade> Ifadeler = SoruAjani.CokluIfadeListesiOlustur(_liste);

            return SoruAjani.YerelSoruBirlestir(Ifadeler, _seceneksayisi);
        }


        private ifadeTuru IfadeRNG(ifadeTuru[] _ifadeler)
        {
            return _ifadeler[(new Random()).Next(_ifadeler.Length)];
        }
    }

    public class FonksiyonelSoruZorlukModel : ZorlukModel
    {
   


    }

    public class ProblemSoruZorlukModel : ZorlukModel
    {

    }

}
