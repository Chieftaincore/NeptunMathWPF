using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static AngouriMath.Entity;

namespace NeptunMathWPF
{
    //Herhangi bir sınıfdan çekilebilecek terimler -Hüseyin
    public struct SoruTerimleri
    {
        public enum ifadeTurleri
        {
            sayi,
            faktoriyel,
            kesir,
        }
    }

    // deneme yazısı
    //Ortak ve çok yönlu kullanımı olan fonksiyonlar bu sınıfa yazılacaktır
    //bu fonksiyonlara yerel loglama, try catch, hata dialogbox vs örnek gösterilebilir
    static class Genel
    {
        static internal NeptunDB dbEntities = new NeptunDB();

        private static readonly string logFilePath = "app_log.txt"; // Hata loglaması için path

        internal static void Handle(Action action) //Hata yönetimi için hazır try-catch blokları
        /***********************
        Kullanım: Handle(() => {
        *KODLAR*
        }); 
        ***********************/
        {
            try { action(); }
            catch (SqlException sqlEx)
            {
                LogToDatabase($"{sqlEx.Message}\n{sqlEx.InnerException}\n{sqlEx.StackTrace}", sqlEx.HResult.ToString());
                MessageBox.Show("Veritabanı bağlantısında bir sorun oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (FileNotFoundException fileEx)
            {
                LogToDatabase($"{fileEx.Message}\n{fileEx.InnerException}\n{fileEx.StackTrace}", fileEx.HResult.ToString());
                MessageBox.Show("Gerekli dosya bulunamadı!", "Dosya Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (UnauthorizedAccessException authEx)
            {
                LogToDatabase($"{authEx.Message}\n{authEx.InnerException}\n{authEx.StackTrace}", authEx.HResult.ToString());
                MessageBox.Show("Bu işlem için yetkiniz yok!", "Yetki Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                LogToDatabase($"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}",ex.HResult.ToString());
                MessageBox.Show("Bir hata oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        internal static void ReloadEntity()
        {
            dbEntities = new NeptunDB();
        }
        internal static void LogToDatabase(string message, string hatakod)
        {
            var kullnId = aktifKullanici.kullnId;
            ReloadEntity();

                dbEntities.SistemikHataLog.Add(new SistemikHataLog
                {
                    zaman = DateTime.Now,
                    kullnID = kullnId,
                    hatakod = hatakod,
                    hatamesaj = message
                });
                dbEntities.SaveChanges();

            
        }

    }
}
