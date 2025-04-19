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
    class SoruCardModel
    {
        public Soru soru { get; set; }
       
        public ObservableCollection<string> NesneSecenekler { get; set; }

        public string LaTeX { get; set; }
        public string kaynak { get; set; }
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

                NesneSecenekler = new ObservableCollection<string>();
                for (int i=0; i<soru.GetDigerSecenekler().Length; i++)
                {
                   if(i == inject)
                       NesneSecenekler.Add(soru.GetSonucSecenek());

                    NesneSecenekler.Add(soru.GetDigerSecenekler()[i]);
                }
            });
        }
    }
}
