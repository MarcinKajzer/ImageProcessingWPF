using ImageProcessing.Utilities;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageProcessing.Model
{
    public class ImageProcessingModel
    {
        public BitmapImage ImageBeforeProcessing { get; private set; }
        public BitmapImage ImageAfterProcessing { get; private set; }
        private Bitmap Bitmap { get; set; }

        public void ReadFIle()
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
            openFileDlg.Title = "Select a picture.";

            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                ImageAfterProcessing = null;

                Uri fileUri = new Uri(openFileDlg.FileName);
                ImageBeforeProcessing = new BitmapImage(fileUri);
                Bitmap = new Bitmap(fileUri.LocalPath);
            }
        }


        public void ToMainColors()
        {
            for (int x = 0; x < Bitmap.Width; x++)
            {
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    Color pixel = Bitmap.GetPixel(x, y);
                    var maxValue = Math.Max(pixel.R, Math.Max(pixel.G, pixel.B));

                    if (maxValue == pixel.R)
                    {
                        pixel = Color.FromArgb(255, 0, 0);
                    }
                    else if (maxValue == pixel.G)
                    {
                        pixel = Color.FromArgb(0, 255, 0);
                    }
                    else
                    {
                        pixel = Color.FromArgb(0, 0, 255);
                    }

                    Bitmap.SetPixel(x, y, pixel);
                }
            }

            ImageAfterProcessing = Bitmap.ToBitmapImage();
        }

        public async Task ToMainColorsAsync()
        {
            await Task.Run(() => ToMainColors());
        }

        public void SaveFIle()
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
            saveFileDlg.Title = "Save a picture.";

            bool? result = saveFileDlg.ShowDialog();

            if (result == true)
            {
                Bitmap.Save(saveFileDlg.FileName);
            }
        }
    }
}
