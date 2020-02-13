using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using yapv.Data;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace yapv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //todo:
        List<String> imgPaths = null;
        private ObservableCollection<Gallery.ImgPreview> tileDataList = new ObservableCollection<Gallery.ImgPreview>();    
        private  DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private Gallery.SelectedImg selectedImg = new Gallery.SelectedImg();
        public MainWindow()
        {
            InitializeComponent();
            MaximiseWindow();

            //test only
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                imgPaths = Gallery.FolderScan(Path.GetDirectoryName( openFileDialog.FileName));
            else
                System.Windows.Application.Current.Shutdown();

            //imgData = Gallery.FolderScan(@"H:\2. Stock Wallpapers");
            imgLB.ItemsSource = tileDataList;
            imgElement.DataContext = selectedImg;

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

            this.KeyDown += MainWindow_KeyDown;
            this.MouseWheel += image_MouseWheel;
        }

        private volatile int cacheImg = 0;
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
           lock(this)
            {
                if (tileDataList.Count != imgPaths.Count)
                {
                    tileDataList.Add(new Gallery.ImgPreview(imgPaths[cacheImg++]));
                    //tileDataList.Add(new Gallery.ImgPreview(imgData[cacheImg++].path));
                }
                else
                {
                    dispatcherTimer.Stop();
                }
            }
        }

        //https://stackoverflow.com/questions/7435816/how-to-animate-image-zoom-change-in-wpf
        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            double zoom = e.Delta > 0 ? 0.1 : -0.1;
            ZoomImage(zoom);
        }

        private void ZoomImage(double zoom)
        {
            if (imgLB.SelectedIndex == -1)
                return;

            Storyboard storyboardh = new Storyboard();
            Storyboard storyboardv = new Storyboard();

            ScaleTransform scale = new ScaleTransform(selectedImg.Zoom, selectedImg.Zoom);
            imgElement.RenderTransformOrigin = new Point(0.5, 0.5);
            imgElement.RenderTransform = scale;

            double startNum = selectedImg.Zoom;
            selectedImg.Zoom += zoom;// * (ZoomLevel/1.5f);
            double endNum = selectedImg.Zoom;

            if (endNum > 5.0)
            {
                endNum = 5.0;
                selectedImg.Zoom = 5.0;
            }
            else if(endNum < 0.1)
            {
                endNum = 0.1;
                selectedImg.Zoom = 0.1;
            }

            if(endNum > 2)
            {
                //if zoomed in switch to full res: image.
                if(!selectedImg.FullQuality)
                {
                    selectedImg.LoadImg(selectedImg.Path, true);
                }
            }
            
            DoubleAnimation growAnimation = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(200),
                From = startNum,
                To = endNum,
               // EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            storyboardh.Children.Add(growAnimation);
            storyboardv.Children.Add(growAnimation);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, imgElement);
            storyboardh.Begin();

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(growAnimation, imgElement);
            storyboardv.Begin();

            /*
             DoubleAnimation animation = new DoubleAnimation
             {
                 From = startNum,
                 To = endNum,
                 Duration = TimeSpan.FromMilliseconds(50),
                 EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
             };

             imgElement.BeginAnimation(Canvas.LeftProperty, animation);
             */
        }


        private void MaximiseWindow()
        {
            this.Left = System.Windows.SystemParameters.WorkArea.X;
            this.Top = System.Windows.SystemParameters.WorkArea.Y;
            this.Width = System.Windows.SystemParameters.WorkArea.Width;
            this.Height = System.Windows.SystemParameters.WorkArea.Height;
            this.WindowState = WindowState.Normal;
        }

        private void BorderWindow()
        {
            return;

            MainWindow m = new MainWindow
            {
                AllowsTransparency = false,
                WindowStyle = WindowStyle.SingleBorderWindow
            };
            m.Show();
            this.Close();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Escape:
                    BorderWindow();
                    break;
                case Key.Up:
                    ZoomImage(0.1);
                    break;
                case Key.Down:
                    ZoomImage(-0.1);
                    break;
                case Key.Right:
                    if( (imgLB.SelectedIndex+1) < imgLB.Items.Count)
                    imgLB.SelectedIndex++;
                    break;
                case Key.Left:
                    if ((imgLB.SelectedIndex - 1) > -1 && imgLB.Items.Count != 0)
                        imgLB.SelectedIndex--;
                    break;
            }

        }

        private void wallpapersLV_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {

        }

        private void  wallpapersLV_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (imgLB.SelectedIndex == -1)
                return;

            selectedImg.LoadImg(tileDataList[imgLB.SelectedIndex].Path);

            try
            {
                ScaleTransform scale = new ScaleTransform(1, 1);
                imgElement.RenderTransformOrigin = new Point(0.5, 0.5);
                imgElement.RenderTransform = scale;
            }
            catch { }
        }

        private void imgLB_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var item = (ListBox)sender;
            ScrollViewer scrollViewer = GetDescendantByType(item, typeof(ScrollViewer)) as ScrollViewer;
            if (e.Delta < 0)
            {
                scrollViewer.LineRight();
            }
            else
            {
                scrollViewer.LineLeft();
            }
            e.Handled = true;
        }

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null)
            {
                return null;
            }
            if (element.GetType() == type)
            {
                return element;
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                {
                    break;
                }
            }
            return foundElement;
        }
    }

}

