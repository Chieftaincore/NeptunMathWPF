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

        public ZorlukRepository(List<soruTur> turler)
        {
            if(turler.Count > 0)
            {
                Zorluklar = new Dictionary<soruTur, ZorlukModel>();

                if (turler.Contains(soruTur.islem))
                    Zorluklar.Add(soruTur.islem, new IslemSoruZorlukModel());

                if (turler.Contains(soruTur.fonksiyon) || turler.Contains(soruTur.limit))
                {
                    FonksiyonelSoruZorlukModel fonksmodel = new FonksiyonelSoruZorlukModel();

                    if(turler.Contains(soruTur.fonksiyon))
                         Zorluklar.Add(soruTur.fonksiyon, fonksmodel);

                    //limitin şu anlık kendi oluşturucusu yok
                    if (turler.Contains(soruTur.limit))
                    {
                        Zorluklar.Add(soruTur.limit, fonksmodel);
                        fonksmodel.Limit = true;
                    }
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

    }
}
