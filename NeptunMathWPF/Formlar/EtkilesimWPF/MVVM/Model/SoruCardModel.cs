using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    //Model WPF sayfası için bir öğedir

    using SoruTuru = SoruTerimleri.soruTuru;
    class SoruCardModel : ObservableObject
    {
        public Soru soru { get; set; }
        public SoruTuru Tur { get => soru.SoruTuru; }
        public SeceneklerModel NesneSecenekler { get; set; }

        public string LaTeX { get; set; }
        public string kaynak { get; set; }
        public string ekYazi { get; set; }
        public DateTime zaman { get; set; }
        public bool Aktif { get; set; }

        public SoruCardModel(Soru s)
        {
            soru = s;
            LaTeX = soru.GetLaTex();

            Genel.Handle(() =>
            {
                Random random = new Random();
                int inject = random.Next(0, soru.GetDigerSecenekler().Length);

                var _secenekler = new ObservableCollection<string>();

                for (int i=0; i<soru.GetDigerSecenekler().Length; i++)
                {
                   if(i == inject)
                       _secenekler.Add(soru.GetSonucSecenek());

                    _secenekler.Add(soru.GetDigerSecenekler()[i]);
                }

                NesneSecenekler = new SeceneklerModel(_secenekler, soru.GetSonucSecenek());

            });
        }

        public void EkYaziGuncelle(string y)
        {
            ekYazi = y;

            OnPropertyChanged(nameof(ekYazi));
        }
    }
}
