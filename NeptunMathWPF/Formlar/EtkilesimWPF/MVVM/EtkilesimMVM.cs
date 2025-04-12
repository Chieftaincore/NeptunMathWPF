using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM
{
    class EtkilesimMVM
    {
   
        public ObservableCollection<SoruCardModel> Sorular { get; set; }
      
        List<SoruTerimleri.ifadeTurleri> turleris = new List<SoruTerimleri.ifadeTurleri>{
                SoruTerimleri.ifadeTurleri.sayi,
                SoruTerimleri.ifadeTurleri.sayi,
                SoruTerimleri.ifadeTurleri.faktoriyel,
            };


        public EtkilesimMVM()
        {
            Sorular = new ObservableCollection<SoruCardModel>();

            List<Ifade> Liste = SoruAjani.CokluIfadeListesiOlustur(turleris);
            Soru soru = SoruAjani.YerelSoruBirlestir(Liste, 5);

            Sorular.Add(new SoruCardModel
            {
                secilisoru = soru,
                kaynak = "Yerel",
                zaman = DateTime.Now,
                Aktif = true
            });

        }
        
        
        public void Ekle()
        {
            List<Ifade> Liste = SoruAjani.CokluIfadeListesiOlustur(turleris);
            Soru soru = SoruAjani.YerelSoruBirlestir(Liste, 5);

            Sorular.Add(new SoruCardModel
            {
                secilisoru = soru,
                kaynak = "Yerel",
                zaman = DateTime.Now,
                Aktif = true
            });
        }
    }
}
