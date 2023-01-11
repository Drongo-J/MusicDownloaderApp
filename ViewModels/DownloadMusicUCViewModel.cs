using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MusicDownloaderApp.Commands;
using MusicDownloaderApp.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MusicDownloaderApp.ViewModels
{
    public class DownloadMusicUCViewModel : BaseViewModel
    {
        private static int StaticID = 0;

        public RelayCommand DownloadCommand { get; set; }
        public RelayCommand SelectSaveFolderCommand { get; set; }
        public RelayCommand YouTubeCommand { get; set; }
        private string downloadURL = String.Empty;

        public string DownloadURL
        {
            get { return downloadURL; }
            set { downloadURL = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UIElement> fileDetails = new ObservableCollection<UIElement>();

        public ObservableCollection<UIElement> FileDetails
        {
            get { return fileDetails; }
            set { fileDetails = value; }
        }

        public FileDetailUCViewModel CurrentDownload { get; set; }


        public DownloadMusicUCViewModel()
        {
            YouTubeCommand = new RelayCommand((y) =>
            {
                Process.Start("https://www.youtube.com");
            });

            DownloadCommand = new RelayCommand(async (d) =>
            {
                string url = DownloadURL.Trim();
                if (url != string.Empty)
                {
                    var fileDetailUC = new FileDetailUC();
                    var fileDetailUCVM = new FileDetailUCViewModel(url);
                    fileDetailUC.DataContext = fileDetailUCVM;
                    fileDetailUCVM.Id = ++StaticID;
                    FileDetails.Add(fileDetailUC);
                    bool delete = false;
                    try
                    {
                        await Task.Run(async () =>
                        {
                            var result1 = await fileDetailUCVM.SetImage();
                            if (result1 == false)
                            {
                                delete = true;
                                return;
                            }
                            var result2 = await fileDetailUCVM.StartDownloading();
                        });

                        if (delete)
                        {
                            MessageBox.Show("Please, enter valid YouTube video URL", "Incorrect Input", MessageBoxButton.OK, MessageBoxImage.Error);
                            if (FileDetails.Count > 0)
                            {
                                FileDetails.RemoveAt(FileDetails.Count - 1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                }
            });

            SelectSaveFolderCommand = new RelayCommand((s) =>
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var path = dialog.FileName + @"\";
                    App.DefaultPath = path;
                    File.WriteAllText(App.SaveFolderPathFile, path);
                    MessageBox.Show("Save Folder was successfully changed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });
        }
    }
}
