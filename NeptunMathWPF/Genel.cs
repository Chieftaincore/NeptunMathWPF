﻿using System;
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
            uslu
        }

        public enum soruTuru
        {
            islem,
            fonksiyon,
            problem,
            limit,
            polinom,
            turev
        }
    }

    //Ortak ve çok yönlu kullanımı olan fonksiyonlar bu sınıfa yazılacaktır
    //bu fonksiyonlara yerel loglama, try catch, hata dialogbox vs örnek gösterilebilir
    static class Genel
    {

        static readonly Random random = new Random();
        static internal NEPTUN_DBEntities dbEntities = new NEPTUN_DBEntities();

        public static string geminiFileName = "GEMINI.config";
        public static string geminiFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, geminiFileName);

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
                LogToDatabase(LogLevel.ERROR, $"{sqlEx.Message}\n{sqlEx.InnerException}\n{sqlEx.StackTrace}");
                MessageBox.Show("Veritabanı bağlantısında bir sorun oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (FileNotFoundException fileEx)
            {
                LogToDatabase(LogLevel.ERROR, $"{fileEx.Message}\n{fileEx.InnerException}\n{fileEx.StackTrace}");
                MessageBox.Show("Gerekli dosya bulunamadı!", "Dosya Hatası", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (UnauthorizedAccessException authEx)
            {
                LogToDatabase(LogLevel.WARNING, $"{authEx.Message}\n{authEx.InnerException}\n{authEx.StackTrace}");
                MessageBox.Show("Bu işlem için yetkiniz yok!", "Yetki Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                LogToDatabase(LogLevel.ERROR, $"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}");
                MessageBox.Show("Bir hata oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static async Task HandleAsync(Action action)
        {
            try
            { action(); }
            catch (Exception ex)
            {
                LogToDatabase(LogLevel.ERROR, $"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}");
                MessageBox.Show("Problem yüklenirken hata oluştu!\nBağlantınızı kontrol edin veya bir yetkiliyle iletişime geçin.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static void ReloadEntity() //Bazı durumlarda hatalarla karşılaşmamak için dbcontext'i new'lemek gerekiyor
        {
            dbEntities = new NEPTUN_DBEntities();
        }
        internal static void LogToDatabase(Enum enumLevel, string message)
        {
            var kullnId = aktifKullanici.kullnId;
            ReloadEntity();

            string level = enumLevel.ToString();
            try
            {

                dbEntities.LOGS.Add(new LOGS
                {
                    LOG_DATE = DateTime.Now,
                    USERID = kullnId,
                    MESSAGE = message,
                    LOG_LEVEL = level
                });
                dbEntities.SaveChanges();
            }
            catch
            {
                //Veritabanına loglanamıyorsa txt dosyasına logla
                try
                {

                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                        writer.WriteLine(logMessage);
                    }
                }
                catch
                {
                    MessageBox.Show("Loglama sırasında bir hata oluştu");
                }
            }

        }

        public static void UygulamaKapat()
        {
            System.Environment.Exit(110);
        }


        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
