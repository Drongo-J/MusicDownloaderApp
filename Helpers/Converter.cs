using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using MediaToolkit.Model;
using MediaToolkit;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Media.Animation;
using System.Threading;

namespace MusicDownloaderApp.Helpers
{
    public static class Converter
    {
        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }

        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public static bool Mp4ToMp3(string mp4Path)
        {
            try
            {
                var inputFile = new MediaFile { Filename = mp4Path };
                var outputFile = new MediaFile { Filename = $"{mp4Path.Replace(".mp4", "")}.mp3" };

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }

                Task.Run(() =>
                {
                    FileInfo file = new FileInfo(mp4Path);
                    while (IsFileLocked(file))
                        Thread.Sleep(1000);
                    file.Delete();
                });
                return true;
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show($"Error deleting \"{mp4Path.Replace(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\", "").Trim()}\" file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show($"Error converting \"{mp4Path.Replace(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\", "").Trim()}\" to MP3 file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
