using HonkSharp.Fluency;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM
{
    class EtkilesimMVM : ObservableObject
    {
        internal enum Durum
        {
            SoruModu,
            Proompter,
            DialogModu,
        }

        List<SoruTerimleri.ifadeTurleri> turleris = new List<SoruTerimleri.ifadeTurleri>{
                SoruTerimleri.ifadeTurleri.faktoriyel,
                SoruTerimleri.ifadeTurleri.sayi,
                SoruTerimleri.ifadeTurleri.sayi,
               
        };

        public ObservableCollection<SoruCardModel> Sorular  { get; set; }

        public ICommand DenemeEkleKomut { get; set; }

        public SoruCardModel secilisoru { get; set; }

        public ObservableCollection<string> secenekler { get; set; }

        private Object _seciliTur;
        public Object seciliTur{ get => _seciliTur ; set
            {
                if (_seciliTur != value)
                {
                    _seciliTur = value;
                    OnPropertyChanged(nameof(seciliTur));
                }
            }
        }

        public EtkilesimMVM()
        {
            Sorular = new ObservableCollection<SoruCardModel>();

            //MVVM'de Komutları bu sınıfa yazım altta belirtmeniz gerek
            DenemeEkleKomut = new RelayCommand(o => Ekle());
            seciliTur = "SoruModu";
            Ekle();
            OnPropertyChanged();
        }

        public void Ekle()
        {
            List<Ifade> Liste = SoruAjani.CokluIfadeListesiOlustur(turleris);
            Soru soru = SoruAjani.YerelSoruBirlestir(Liste, 5);

            secilisoru = new SoruCardModel(soru)
            {
                zaman = DateTime.Now,
                kaynak = "Yerel"
            };

            secenekler = secilisoru.secenekler;
            Sorular.Add(secilisoru);

            OnPropertyChanged();
        }
        
        //StackOverflow'dan aldım
        internal class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                if (execute == null) throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter ?? "<N/A>");
            }
        }

       
    }
}
