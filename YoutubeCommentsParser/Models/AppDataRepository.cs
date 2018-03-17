using CsvHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static Dictionary<Guid, SearchProject> SearchProjects { get; private set; } = new Dictionary<Guid, SearchProject>();

        #endregion

        #region Открытые методы
        public static DataLoadResult LoadDataFiles()
        {
            if (!LoadWordsTonality())
            {
                return new DataLoadResult { Success = false, Message = "Не удалось загрузить тональности слов" };
            }

            Deserialize();

            return new DataLoadResult { Success = true };
        }

        public static void Save()
        {
            var memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = null;
            try
            {
                var data = new SerializeData
                {
                    YoutubeKey = YoutubeKey,
                    Projects = SearchProjects

                };
                formatter.Serialize(memoryStream, data);

                // если сериализация прошла без ошибок, то записываем данные в файл
                var path = (String)Properties.Settings.Default["AppDataFilePath"];
                fileStream = new FileStream(path, FileMode.Create);
                memoryStream.Position = 0;
                var byteArray = memoryStream.ToArray();
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                _Log.Error(ex.ToString());
            }
            finally
            {
                memoryStream.Close();
                fileStream?.Close();
            }
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

        private static void Deserialize()
        {
            FileStream fileStream = null;
            try
            {
                var path = (String)Properties.Settings.Default["AppDataFilePath"];
                fileStream = new FileStream(path, FileMode.Open);

                BinaryFormatter formatter = new BinaryFormatter();
                var data = (SerializeData)formatter.Deserialize(fileStream);
                YoutubeKey = data.YoutubeKey;
                SearchProjects = data.Projects;
            }
            catch(FileNotFoundException ex)
            {

            }
            catch(Exception ex)
            {
                _Log.Error(ex.ToString());
            }
            finally
            {
                fileStream?.Close();
            }
        }

        #endregion
    }
}
