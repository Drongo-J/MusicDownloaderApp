using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MusicDownloaderApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DefaultPath { get; internal set; }
        public static Grid MyGrid { get; internal set; }

        public static string SaveFolderPathFile = @"~/../../../Files\SaveFolderPath.txt";

        public static void SetDefaultPath()
        {
            var path = File.ReadAllText(SaveFolderPathFile);
            if (path == String.Empty || !File.Exists(path))
            {
                DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
            }
            else
            {
                DefaultPath = path;
            }
        }
    }
}
