using MusicDownloaderApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;

namespace MusicDownloaderApp.Models
{
    public class DownloadDetailsModel : BaseViewModel
    {
        private string buttonToolTip;

        public string ButtonToolTip
        {
            get { return buttonToolTip; }
            set { buttonToolTip = value; OnPropertyChanged(); }
        }

        private string imageSource;

        public string ImageSource
        {
            get { return imageSource; }
            set { imageSource = value; OnPropertyChanged(); }
        }

        private string progressPercentage = "0 %";

        public string ProgressPercentage
        {
            get { return progressPercentage; }
            set { progressPercentage = value; OnPropertyChanged(); }
        }

        private string progressInfo = string.Empty;

        public string ProgressInfo
        {
            get { return progressInfo; }
            set { progressInfo = value; OnPropertyChanged(); }
        }

        private double progress = 0;

        public double Progress
        {
            get { return progress; }
            set { progress = value; OnPropertyChanged(); }
        }


        private string contentLength;

        public string ContentLength
        {
            get { return contentLength; }
            set { contentLength = value; OnPropertyChanged(); }
        }

        private YouTubeVideo youtubeVideo;

        public YouTubeVideo YoutubeVideo
        {
            get { return youtubeVideo; }
            set { youtubeVideo = value; OnPropertyChanged(); }
        }

        public string FilePath { get; set; }
    }
}
