using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace yapv.Data
{
    public static class Gallery
    {
        //todo:
        public static List<String> FolderScan(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".png", StringComparison.OrdinalIgnoreCase));

            List<String> paths = new List<String>();
            foreach (var item in files)
            {
                paths.Add(item);
            }
            return paths;
        }


        [Serializable]
        public class SelectedImg : INotifyPropertyChanged
        {
            private BitmapImage img;
            public string Path { get; private set; }
            public bool FullQuality { get; private set; }
            public double Zoom { get; set; }
            public BitmapImage Img
            {
                get
                {
                    return img;
                }
                set
                {
                    img = value;
                    OnPropertyChanged("Img");
                }
            }
            
            public SelectedImg()
            {
                Path = null;
                FullQuality = false;
                Img = null;
                Zoom = 1.0;
            }
            
            public void LoadImg(string path, bool originalQuality = false)
            {
                Path = path;
                FullQuality = originalQuality;

                BitmapImage bi = new BitmapImage();
                //var stream = File.OpenRead(path);

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                if (!originalQuality)
                {
                    bi.DecodePixelWidth = 1280;
                    //bi.DecodePixelHeight = 360;
                }
                bi.UriSource = new Uri(path);
                //bi.StreamSource = stream;
                bi.EndInit();

                //stream.Close();
                //stream.Dispose();
                bi.Freeze(); // freeze the image source, used to move it across the thread
                //return bi;

                Img = bi;
            }

            private void OnPropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        [Serializable]
        public class ImgPreview : INotifyPropertyChanged
        {
            private BitmapImage img;
            public string Path { get; set; }
            public BitmapImage Img
            {
                get
                {
                    return img;
                }
                set
                {
                    img = value;
                    OnPropertyChanged("Img");
                }
            }
            public ImgPreview(string imgPath)
            {
                Path = imgPath;
                Img = null;//ToBitmapImage(Properties.Images.icons8_image_100);
                Img = LoadConvertImage(imgPath);

            }
            public BitmapImage LoadConvertImage(string fileName)
            {
                try
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    if (true)
                    {
                        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                            int width = bitmapFrame.PixelWidth;
                            int height = bitmapFrame.PixelHeight;
                            //int aspectRatio = width / height;

                            bi.DecodePixelHeight = 100;
                            //bi.DecodePixelHeight = bi.DecodePixelWidth / aspectRatio;
                        }
                        //downscale
                    }
                    bi.CacheOption = BitmapCacheOption.OnLoad; //allow deletion of file on disk.
                    bi.UriSource = new Uri(fileName);
                    bi.EndInit();
                    return bi;
                }
                catch
                {
                    return ToBitmapImage(Properties.Images.icons8_image_100);
                }
            }

            private void OnPropertyChanged(string property)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            try
            {
                using (var memory = new MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.DecodePixelWidth = 100;
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
