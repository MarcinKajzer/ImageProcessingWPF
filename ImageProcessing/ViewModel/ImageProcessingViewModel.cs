using ImageProcessing.Command;
using ImageProcessing.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace ImageProcessing.ViewModel
{
    public class ImageProcessingViewModel : INotifyPropertyChanged
    {
        private long imageAsyncProcessingTime_InMiliseconds;
        private long imageProcessingTime_InMiliseconds;

        public ImageProcessingViewModel()
        {
            Model = new ImageProcessingModel();

            var stopWatch = new Stopwatch();

            ReadFileCommand = new RelayCommand(
                () =>
                {
                    Model.ReadFIle();
                    ImageAsyncProcessingTime_InMiliseconds = 0;
                    ImageProcessingTime_InMiliseconds = 0;

                    OnPropertyChanged(nameof(ImageProcessingTime_InMiliseconds));
                    OnPropertyChanged(nameof(ImageAsyncProcessingTime_InMiliseconds));
                    OnPropertyChanged(nameof(Model.ImageBeforeProcessing));
                    OnPropertyChanged(nameof(Model.ImageAfterProcessing));

                }, () => true);

            ToMainColorsCommand = new RelayCommand(
                () =>
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    Model.ToMainColors();

                    stopWatch.Stop();
                    ImageProcessingTime_InMiliseconds = stopWatch.ElapsedMilliseconds;

                    OnPropertyChanged(nameof(Model.ImageAfterProcessing));

                }, ToMainColors_CanExecute);

            ToMainColorsAsyncCommand = new RelayCommand(
                async () =>
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    await Model.ToMainColorsAsync();

                    stopWatch.Stop();
                    ImageAsyncProcessingTime_InMiliseconds = stopWatch.ElapsedMilliseconds;

                    OnPropertyChanged(nameof(Model.ImageAfterProcessing));

                }, ToMainColors_CanExecute);

            SaveFileCommand = new RelayCommand(
                () =>
                {
                    Model.SaveFIle();
                }, SaveCommand_CanExecute);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand ReadFileCommand { get; private set; }
        public RelayCommand ToMainColorsCommand { get; private set; }
        public RelayCommand ToMainColorsAsyncCommand { get; private set; }
        public RelayCommand SaveFileCommand { get; private set; }
        public ImageProcessingModel Model { get; set; }

        public long ImageProcessingTime_InMiliseconds
        {
            get
            {
                return imageProcessingTime_InMiliseconds;
            }
            set
            {
                imageProcessingTime_InMiliseconds = value;
                OnPropertyChanged(nameof(ImageProcessingTime_InMiliseconds));
            }
        }

        public long ImageAsyncProcessingTime_InMiliseconds
        {
            get
            {
                return imageAsyncProcessingTime_InMiliseconds;
            }
            set
            {
                imageAsyncProcessingTime_InMiliseconds = value;
                OnPropertyChanged(nameof(ImageAsyncProcessingTime_InMiliseconds));
            }
        }

        public BitmapImage ImageBeforeProcessing
        {
            get
            {
                return Model.ImageBeforeProcessing;
            }
        }

        public BitmapImage ImageAfterProcessing
        {
            get
            {
                return Model.ImageAfterProcessing;
            }
        }


        public bool ToMainColors_CanExecute()
        {
            if (Model.ImageBeforeProcessing != null)
                return true;
            
            return false;
        }

        public bool SaveCommand_CanExecute()
        {
            if (Model.ImageAfterProcessing != null)
                return true;

            return false;
        }


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
