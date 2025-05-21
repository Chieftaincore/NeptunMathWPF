using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
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

        public static string geminiFileName = "GEMINI.config";
        public static string geminiFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, geminiFileName);

        private static readonly string logFilePath = "app_log.txt"; // Hata loglaması için path

        public static string dbDirectory = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "NeptunMath"
);
        public static string dbPath = Path.Combine(dbDirectory, "NEPTUN_DB.mdf");

        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;MultipleActiveResultSets=True;";

        private static EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder
        {
            Provider = "System.Data.SqlClient",
            ProviderConnectionString = connectionString,
            Metadata = @"res://*/NeptunDB.csdl|res://*/NeptunDB.ssdl|res://*/NeptunDB.msl"
        };

        public static NeptunDB dbEntities = new NeptunDB(builder.ToString());

        internal static void Handle(Action action) //Hata yönetimi için hazır try-catch blokları
        /***********************
        Kullanım: Handle(() => {
        *KODLAR*
        }); 
        ***********************/
        {
            try { action(); }
            catch (Exception sqlEx) when
            (sqlEx is SqlException || sqlEx is System.Data.Entity.Core.EntityException || sqlEx is DbUpdateException || sqlEx is EntityException)
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

        internal static void ReloadEntity() //Bazı durumlarda hatalarla karşılaşmamak için dbcontext'i new'lemek gerekiyor
        {
            dbEntities = new NeptunDB(builder.ToString());
            using (var context = new NeptunDB(builder.ToString()))
            {
                // Veritabanı işlemleri
            }
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
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
