using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{
    using soruTur = SoruTerimleri.soruTuru;

    /// <summary>
    /// etkileşim sayfası için algorithma nesnesi
    /// kullanıcıya göre veya parametreli algorithma oluşturulmasını sağlar
    /// </summary>
    public class AlgorithmaModel
    {   
        /// <summary>
        /// Seçili soru türlerindendi tanımlanmazsa random bütün soru türlerinden seçer
        /// </summary>
        List<soruTur> SoruTurleri;
        public ZorlukRepository repo { get; set; }

        public List<Soru> Bekleyen = new List<Soru>();
        public Func<Soru> SonrakiAlgorithma { get; set; }

        /// <summary>
        /// Algorithma model oluşturucu soru turlerini liste olarak gönderin
        /// </summary>
        /// <param name="turler"></param>
        public AlgorithmaModel(List<soruTur> turler)
        {
            SoruTurleri = turler;

            repo = new ZorlukRepository(turler);
        }

        /// <summary>
        /// Algorithma model oluşturucu soru turlerini dizi olarak gönderin
        /// </summary>
        /// <param name="turler"></param>
        public AlgorithmaModel(soruTur[] turler)
        {
            SoruTurleri = turler.ToList();

            repo = new ZorlukRepository(turler.ToList());
        }

        /// <summary>
        /// önceden oluşturulmuş, varsayılan dışı Zorluk modelleri için Algorithma model oluşturucu
        /// </summary>
        /// <param name="_modeller">Önceden oluşturulmuş ZorlukModelleri eklemek için</param>
        public AlgorithmaModel(List<ZorlukModel> _modeller)
        {
            repo = new ZorlukRepository(_modeller);
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
                Soru soru = Bekleyen.Last();

                Bekleyen.Remove(Bekleyen.Last());

                return soru;
            }
        }

        public async Task ProblemArkaSoruAsync()
        {
            Bekleyen.Add(await ((ProblemSoruZorlukModel)repo.Zorluklar[soruTur.problem]).SoruHazirla(repo.Zorluklar[soruTur.problem].seviye));

            //MessageBox.Show("ASYNC Problem Sorusu Bekleyenlere Eklendi", "ASYNC eklendi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
