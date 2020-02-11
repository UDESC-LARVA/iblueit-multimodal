using System.IO;
using System.Text;

namespace Ibit.Core.Util
{
    public class FileManager
    {
        public static void WriteAllText(string path, string contents)
        {
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(path, contents, Encoding.UTF8);

            UnityEngine.Debug.LogFormat("File saved: {0}", path);
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            File.WriteAllLines(path, contents);
        }

        public static string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public static string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        public static void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents, Encoding.UTF8);
        }

        public static string ReadCsv(string path)
        {
            var text = ReadAllText(path);

            if (text.Split('\t').Length > 0)
                text = text.Replace('\t', ';');

            return text;
        }
    }
}