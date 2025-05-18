using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{
    using soruTur = SoruTerimleri.soruTuru;
    public class ZorlukRepository
    {
        
        public static Dictionary<string, int[]> Araliklar = new Dictionary<string, int[]>
        {
            {"TAMSAYINORMAL", new int[] {2,50} }, {"TAMSAYIBOLME", new int[] {2,5} }, {"TAMSAYIYANILMA", new int[] {-30,30} },
            {"FAKTORIYELNORMAL",new int[]{2,6}}, {"KESIRTAMCARPAN",new int[] {2,15} }, {"FAKTORIYELSAYI", new int[] { 2, 6 } },
            {"USLUNORMAL",new int[]{1,11}}, {"USTAMCARPAN",new int[] {0,4} }, {"USBOLMECARPAN", new int[]{2,5}}
        };

        public Dictionary<soruTur,ZorlukModel> Zorluklar { get; set; }

        /// <summary>
        /// Parametreler ile gerekli ZorlukModelleri oluşturur, her soruTuru'ne göre ayrı bir
        /// Zorluk Modeli oluşturur.
        /// </summary>
        /// <param name="turler"></param>
        public ZorlukRepository(List<soruTur> turler)
        {
            if(turler.Count > 0)
            {
                Zorluklar = new Dictionary<soruTur, ZorlukModel>();

                if (turler.Contains(soruTur.islem))
                    Zorluklar.Add(soruTur.islem, new IslemSoruZorlukModel());

                if (turler.Contains(soruTur.fonksiyon))
                {
                    Zorluklar.Add(soruTur.fonksiyon, new FonksiyonelSoruZorlukModel());
                }

                if (turler.Contains(soruTur.limit))
                {
                    Zorluklar.Add(soruTur.limit, new LimitSoruZorlukModel());
                }

                if (turler.Contains(soruTur.problem))
                {
                    Zorluklar.Add(soruTur.problem, new ProblemSoruZorlukModel());
                }
            }
            else
            {
                MessageBox.Show("Algorithma modeli yetersiz sayıda Parametre aldı tek tip soruya ayarlandı", "Algorithma model bildiri", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                if (turler.Contains(soruTur.islem))
                    Zorluklar.Add(soruTur.islem, new IslemSoruZorlukModel());
            }
        }

        public ZorlukRepository(HashSet<ZorlukModel> _modellHash)
        {
            Genel.Handle(() =>
            {
                foreach (ZorlukModel zorlukModel in _modellHash)
                {
                    if (!Zorluklar.ContainsKey(zorlukModel.soruTuru))
                    {
                        Zorluklar.Add(zorlukModel.soruTuru, zorlukModel);
                    }
                    else
                    {
                        MessageBox.Show("Aynı Soru Türünde birden fazla Zorluk Modeli oluşturuldu fazladan olan model Algorithmaya eklenmedi", "Zorluk Model ve Repo Uyumzusluğu",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            });
        }
    }
}
