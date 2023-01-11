using MediaToolkit.Model;
using MediaToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Net;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using MusicDownloaderApp.Helpers;
using MusicDownloaderApp.Views;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using MusicDownloaderApp.Models;
using System.Drawing.Design;
using MusicDownloaderApp.Commands;
using Newtonsoft.Json.Linq;

namespace MusicDownloaderApp.ViewModels
{
    public class FileDetailUCViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public RelayCommand CancelCommand { get; set; }

        private DownloadDetailsModel downloadDetails;

        public DownloadDetailsModel DownloadDetails
        {
            get { return downloadDetails; }
            set { downloadDetails = value; OnPropertyChanged(); }
        }

        private Visibility cancelButtonVisibility;

        public Visibility CancelButtonVisibility
        {
            get { return cancelButtonVisibility; }
            set { cancelButtonVisibility = value; OnPropertyChanged(); }
        }

        public string DownloadURL { get; set; }

        public FileDetailUCViewModel(string downloadURL)
        {
            CancelButtonVisibility = Visibility.Visible;
            DownloadURL = downloadURL;
            DownloadDetails = new DownloadDetailsModel();
            DownloadDetails.ButtonToolTip = "Cancel";

            CancelCommand = new RelayCommand((c) =>
            {
                var cancelSource = new CancellationTokenSource();
                token = cancelSource.Token;
                cancelSource.Cancel();
                var items = ((App.MyGrid.Children[0] as DownloadMusicUC).DataContext as DownloadMusicUCViewModel).FileDetails;
                var item = items.Where(x => ((x as FileDetailUC).DataContext as FileDetailUCViewModel).Id == this.Id).First();
                items.Remove(item);

                Task.Run(() =>
                {
                    FileInfo file = new FileInfo(DownloadDetails.FilePath);
                    while (Converter.IsFileLocked(file))
                        Thread.Sleep(1000);
                    file.Delete();

                    var mp3File = DownloadDetails.FilePath.Replace(".mp4", ".mp3");
                    if (File.Exists(mp3File))
                    {
                        FileInfo file2 = new FileInfo(mp3File);
                        while (Converter.IsFileLocked(file2))
                            Thread.Sleep(1000);
                        file2.Delete();
                    }
                });
            });
        }

        private delegate void Func();

        private FileStream fs = null;
        private Stream streamweb = null;
        private WebResponse w_response = null;

        public bool SetVideo()
        {
            try
            {
                var youtube = YouTube.Default;
                DownloadDetails.YoutubeVideo = youtube.GetVideo(DownloadURL);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetImage()
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    return GetImage();
                });
                return result;
            }
            catch (Exception ex)
            {
                return false;
                //MessageBox.Show($"Image error \n + {ex}");
                //throw;
            }
        }

        public bool GetImage()
        {
            try
            {
                var result = GetVideoIdentifier(DownloadURL);
                if (result == string.Empty)
                    return false;

                string ImageUrl = "https://img.youtube.com/vi/" + result + "/maxresdefault.jpg";
                DownloadDetails.ImageSource = ImageUrl;
                return true;
            }
            catch
            {
                return false;
                //MessageBox.Show($"There was an error getting thumbnail of video", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetVideoIdentifier(string url)
        {
            try
            {
                string identifier = url.Split(new char[] { '=' })[1];

                try
                {
                    if (identifier.Contains("&"))
                    {
                        identifier = identifier.Split(new char[] { '&' })[0];
                    }
                }
                catch (Exception) { }

                return identifier;
            }
            catch (Exception)
            {
                //MessageBox.Show($"There was an error getting thumbnail of video", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return String.Empty;
            }
        }

        public async Task<bool> DownloadFileAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    SetVideo();
                    DownloadDetails.FilePath = App.DefaultPath + DownloadDetails.YoutubeVideo.FullName;
                    WebRequest w_request = WebRequest.Create(DownloadDetails.YoutubeVideo.Uri);
                    if (w_request != null)
                    {
                        w_response = w_request.GetResponse();
                        if (w_response != null)
                        {
                            fs = new FileStream(DownloadDetails.FilePath, FileMode.Create);

                            streamweb = w_response.GetResponseStream();
                        }
                    }
                });
                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show($"You are already downloading that music", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private CancellationToken token;

        public async Task<bool> StartDownloading()
        {
            DownloadDetails.ProgressInfo = "Calculating size of the file";
            var result = await DownloadFileAsync();
            try
            {
                if (result)
                {
                    int total = 0;
                    byte[] buffer = new byte[128 * 1024];
                    int bytesRead = 0;

                    do
                    {
                        if (token.IsCancellationRequested)
                        {
                            Console.WriteLine("Stopped from Foo2");
                            break;
                        }
                        var length = DownloadDetails.YoutubeVideo.ContentLength;
                        DownloadDetails.ContentLength = Converter.ConvertBytesToMegabytes(long.Parse(length.ToString())).ToString("f1") + " mb";
                        bytesRead = streamweb.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, bytesRead);
                        total += bytesRead;

                        var pr = Math.Round(((double)total / (int)length) * 100, 2);
                        Func del = delegate
                        {
                            DownloadDetails.ProgressPercentage = pr.ToString("f2") + " %";
                            DownloadDetails.Progress = pr;
                            DownloadDetails.ProgressInfo = $"{Converter.ConvertBytesToMegabytes(long.Parse(total.ToString())).ToString("f1") + " mb"} / {DownloadDetails.ContentLength}";
                        };
                        del.Invoke();
                    } while (bytesRead > 0);
                    Converter.Mp4ToMp3(DownloadDetails.FilePath);
                    DownloadDetails.ProgressPercentage = "Download completed!";
                }
                else
                {
                    MessageBox.Show($"You are already downloading \"{DownloadDetails.YoutubeVideo.FullName}\".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    DownloadDetails.ProgressPercentage = "Download aborted!";
                    DownloadDetails.ProgressInfo = String.Empty;
                    DownloadDetails.ButtonToolTip = "Remove";
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"There was an error downloading \"{DownloadDetails.YoutubeVideo.FullName}\"", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                if (w_response != null) w_response.Close();
                if (fs != null) fs.Close();
                if (streamweb != null) streamweb.Close();
            }
            return true;
        }
    }
}
