using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UpdObjects;

namespace Launcher
{
    class Program
    {
        public const string Server = "http://localhost:32625";
        public const string AppName = "Прога № 1";
        public const int AppId = 1;
        private static WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };
        static List<string> Runable = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Я загрузчик вашего приложения");
            Console.WriteLine(AppName);
            Console.WriteLine("Подготавливаю платформу");
            var a = wc.DownloadString(Server + "/AppGet/info/" + AppId);
            Console.WriteLine("Получаю информацию по актуальным версиям файлов");
            var ll = JsonUtil<AppData>.ObjFromStr(a);
            Directory.CreateDirectory(ll.AppName);
            var di = new DirectoryInfo(ll.AppName);
            Console.WriteLine("Выясняю, что с вашими файлами");
            foreach (var fileData in ll.Files)
            {
                var fn = Path.Combine(di.FullName, fileData.Nm);
                if (fileData.Run) Runable.Add(fn);
                Console.Write(fileData.Nm);
                if (File.Exists(fn))
                {
                    var tmpMd5 = Utils.ComputeMD5Checksum(File.ReadAllBytes(fn));
                    if (tmpMd5 == fileData.Md5) { Console.WriteLine(); continue; }
                    Console.Write("+");
                }

                var bb = wc.DownloadData(Server + $"/AppGet/get/{fileData.Id}/{fileData.Rev}");
                File.WriteAllBytes(fn, bb);

                Console.WriteLine("~");
            }

            Console.WriteLine("Запускаю требуемые приложения ");
            foreach (string o in Runable)
            {
                var startInfo = new ProcessStartInfo(o);
                startInfo.WorkingDirectory = di.FullName;
                Process.Start(startInfo);
            }
            Console.WriteLine("Прощаюсь");
        }
    }
}
