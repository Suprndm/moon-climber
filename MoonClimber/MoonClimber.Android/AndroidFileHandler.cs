using System.IO;
using MoonClimber.Droid;
using Xamarin.Forms;
using MoonClimber.Services;
using Environment = System.Environment;

[assembly: Dependency(typeof(AndroidFileHandler))]
namespace MoonClimber.Droid
{
    public class AndroidFileHandler : IFileHandler
    {
        public string GetPath(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
           return Path.Combine(documentsPath, filename);
        }

        public void SaveText(string filename, string text)
        {
            var documentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }

        public string LoadText(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return System.IO.File.ReadAllText(filePath);
        }

        public bool CheckExistenceOf(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return File.Exists(filePath);
        }
    }
}
