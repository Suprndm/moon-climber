using MoonClimber.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MoonClimber.Data
{
    public class FileService<T> where T : class, new()
    {
        // TODO protected string _fileName = AppSettings.PlayerPersonalDataFileName + nameof(T);
        protected string _fileName = string.Empty;
        protected T _fileData;

        public FileService()
        {
            if (DependencyService.Get<IFileHandler>().CheckExistenceOf(_fileName))
            {
                Load();
            }
            else
            {
                Initialize();
            }
        }

        public T GetData()
        {
            return _fileData;
        }

        public void SaveChanges()
        {
            string playerDataSerialized = JsonConvert.SerializeObject(_fileData);
            var fileWriter = DependencyService.Get<IFileHandler>();
            fileWriter.SaveText(_fileName, playerDataSerialized);
        }

        public void SaveBackup()
        {
            var fileHandler = DependencyService.Get<IFileHandler>();
            string currentSave = fileHandler.LoadText(_fileName);
            fileHandler.SaveText("backup_" + _fileName, currentSave);
        }

        public void Load()
        {
            var fileReader = DependencyService.Get<IFileHandler>();
            var result = fileReader.LoadText(_fileName);
            _fileData = JsonConvert.DeserializeObject<T>(result);
        }

        protected void Initialize()
        {
            _fileData = new T();
            SaveChanges();
        }

    }
}
