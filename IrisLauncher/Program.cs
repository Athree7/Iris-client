using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;

class Program
{
    public static WebClient client = new WebClient();
    public static DirectoryInfo config;

    static void Main(string[] args)
    {
        client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.iris");

        config = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.iris");

        Console.WriteLine("Powered by kaffee injector (https://discord.gg/45yQ6KtVR7)");
        Console.WriteLine("Made by yeemi/Iris (https://discord.gg/3Da9HakJam)");

        while (true)
        {
            CheckDownload();

            try
            {
                FileInfo file = new FileInfo(config.FullName + "\\Iris.dll");

                InjectionHandler.InjectDLL(file.FullName);
            }
            catch { }

            Thread.Sleep(1000);
        }
    }

    static void CheckDownload() // this doesnt work properly
    {
        string downloadUri = client.DownloadString("https://raw.githubusercontent.com/Laamy/iris-public/main/latest.txt");

        if (!File.Exists(config.FullName + "\\version.txt"))
        {
            client.DownloadFile(downloadUri, config.FullName + "\\Iris.dll");
            File.WriteAllText(config.FullName + "\\version.txt", downloadUri);
            return;
        }

        string oldVersion = File.ReadAllText(config.FullName + "\\version.txt");

        if (oldVersion.Replace("\r\n", "") != downloadUri)
        {
            Console.WriteLine("out of date iris");

            File.Delete(config.FullName + "\\Iris.dll");
            File.Delete(config.FullName + "\\version.txt");

            client.DownloadFile(downloadUri, config.FullName + "\\Iris.dll");
            File.WriteAllText(config.FullName + "\\version.txt", downloadUri);
        }
    }
}