using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    //Model WPF sayfası için bir öğedir
    using SoruTuru = SoruTerimleri.soruTuru;

    internal class SoruCardModel : ObservableObject, IStyleAnahtar
    {
        //önemli Nesneler
        public Soru soru { get; set; }
        public SoruTuru Tur { get => soru.SoruTuru; }
        public Enum SoruAltTuru { get; set; }
        public string soruStyle { get; set; }
        public SeceneklerModel NesneSecenekler { get; set; }

        //Yazilar
        public string LaTeX { get; set; }
        public string kaynak { get; set; }
        public string ekYazi { get; set; }
        public DateTime zaman { get; set; }
        public bool kilitlendi { get => NesneSecenekler.kilitli; }

        //Renk ile ilgili
        private Color _tabRenk;
        public Color TabRenk
        {
            get => _tabRenk; set
            {

                if (_tabRenk != value)
                {
                    _tabRenk = value;
                    TabBrush = new SolidColorBrush(TabRenk);

                    OnPropertyChanged(nameof(TabBrush));
                }
            }
        }

        public SolidColorBrush TabBrush { get; set; }

        public SoruCardModel(Soru sorunesne)
        {
            soru = sorunesne;
            LaTeX = soru.GetLaTex();
            TabRenk = Colors.LightSteelBlue;

            Genel.Handle(() =>
            {
                Random random = new Random();
                int inject = random.Next(0, soru.GetDigerSecenekler().Length);

                var _secenekler = new ObservableCollection<string>();

                for (int i = 0; i < soru.GetDigerSecenekler().Length; i++)
                {
                    if (i == inject)
                        _secenekler.Add(soru.GetSonucSecenek());

                    _secenekler.Add(soru.GetDigerSecenekler()[i]);
                }

                NesneSecenekler = new SeceneklerModel(_secenekler, soru.GetSonucSecenek());
            });

            soruStyle = SoruTuruStyleTemplate();
            OnPropertyChanged(nameof(soruStyle));
        }

        public void EkYaziGuncelle(string y, int durum = 0)
        {
            ekYazi = y;

            OnPropertyChanged(nameof(ekYazi));
            RenkDegis(durum);
        }

        public string SoruTuruStyleTemplate()
        {
            if (kilitlendi)
                return "SoruModuKilitli";

            switch (Tur)
            {
                case SoruTuru.islem:
                    return "SoruModuNormal";
                    
                case SoruTuru.fonksiyon:
                    return "SoruModuNormal";

                case SoruTuru.problem:
                    return "SoruModuMetin";

                default:
                    return "SoruModuNormal";
            }
        }

        public void RenkDegis(int durum)
        {
            SolidColorBrush solidColor = new SolidColorBrush();
            Color color;

            switch (durum)
            {
                case 1:
                    color = Colors.LightSteelBlue;
                    break;
                case 2:
                    color = Colors.IndianRed;
                    break;
                case 3:
                    color = Colors.LightSkyBlue;
                    break;
                default:
                    color = Colors.LightSteelBlue;
                    break;
            }

            TabRenk = color;
        }
    }
}
