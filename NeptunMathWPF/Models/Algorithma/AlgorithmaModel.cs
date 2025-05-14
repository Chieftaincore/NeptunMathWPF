using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{

    using soruTur = SoruTerimleri.soruTuru;

    /// <summary>
    /// etkileşim sayfası için algorithma nesnesi
    /// kullanıcıya göre veya özel algorithma oluşturulması
    /// 
    /// </summary>
    class AlgorithmaModel
    {
        /// <summary>
        /// Seçili soru türlerindendi tanımlanmazsa random bütün soru türlerinden seçer
        /// </summary>
        List<soruTur> SoruTurleri;
        public ZorlukRepository repo { get; set; }

        public List<Soru> Bekleyen = new List<Soru>();
        public Func<Soru> SonrakiAlgorithma { get; set; }

        public AlgorithmaModel(List<soruTur> turler)
        {
            SoruTurleri = turler;

            repo = new ZorlukRepository(turler);
        }
        public AlgorithmaModel(soruTur[] turler)
        {
            SoruTurleri = turler.ToList();

            repo = new ZorlukRepository(turler.ToList());
        }
        public AlgorithmaModel()
        {

        }

        public Soru Sonraki()
        {
            if (Bekleyen.Count == 0)
            {
                Random rng = new Random();

                soruTur _tur = SoruTurleri[rng.Next(SoruTurleri.Count)];
                int _seviye = repo.Zorluklar[_tur].seviye;

                if (_tur == soruTur.problem)
                {
                    ProblemArkaSoruAsync();

                    List<soruTur> _Yedek = new List<soruTur>(SoruTurleri);
                    _Yedek.Remove(soruTur.problem);

                    _tur = _Yedek[rng.Next(_Yedek.Count)];
                }

                return repo.Zorluklar[_tur].SonrakiAlgorithma(_seviye);
            }
            else
            {
                MessageBox.Show("Geri Döndürdü","ASYNC");

                Soru soru = Bekleyen.Last();

                Bekleyen.Remove(Bekleyen.Last());

                return soru;
            }
        }

        public async Task ProblemArkaSoruAsync()
        {
            MessageBox.Show("ASYNC Tetiklendi"," ASYNC tetiklendi", MessageBoxButton.OK, MessageBoxImage.Asterisk);

            Bekleyen.Add(await ((ProblemSoruZorlukModel)repo.Zorluklar[soruTur.problem]).SoruHazirla(repo.Zorluklar[soruTur.problem].seviye));

            MessageBox.Show("ASYNC Problem Eklendi", " ASYNC eklendi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public Soru Sonraki(soruTur tur)
        {
            int _seviye = repo.Zorluklar[tur].seviye;

            return repo.Zorluklar[tur].SonrakiAlgorithma(_seviye);
        }

        public void ModelSeviyeArtır(soruTur tur)
        {
            repo.Zorluklar[tur].seviyeArttır();
        }

        public void ModelSeviyeAzalt(soruTur tur)
        {
            repo.Zorluklar[tur].seviyeAzalt();
        }

        public bool RepoDenetim(soruTur tur)
        {
            if (repo.Zorluklar.ContainsKey(tur))
            {
                return true;
            }

            return false;
        }
    }
}
