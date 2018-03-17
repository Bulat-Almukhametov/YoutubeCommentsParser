using CsvHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    public static class AppDataRepository
    {
        #region Свойства

        private static ILog _Log = LogManager.GetLogger("FileLogger");
        public static List<WordsTonality> WordsTonalities { get; private set; }

        public static string YoutubeKey { get; set; }

        #endregion

        #region Открытые методы
        public static DataLoadResult LoadDataFiles()
        {
            if (!LoadWordsTonality())
            {
                return new DataLoadResult { Success = false, Message = "Не удалось загрузить тональности слов" };
            }

            return new DataLoadResult { Success = true };
        }

        public static void Save()
        {

        }
        #endregion

        #region Закрытые методы


        private static bool LoadWordsTonality()
        {
            StreamReader reader = null;
            try
            {
                string filename = Properties.Settings.Default["WordsTonalityFilename"].ToString();
                reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));

                var csv = new CsvReader(reader);

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<WordsTonalityMap>();

                WordsTonalities = csv.GetRecords<WordsTonality>().ToList<WordsTonality>();

                reader.Dispose();
            }
            catch (Exception ex)
            {
                reader?.Dispose();
                _Log.Error(ex.ToString());

                return false;
            }

            return true;
        }

        #endregion
    }
}
