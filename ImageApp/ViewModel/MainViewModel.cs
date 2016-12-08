using GalaSoft.MvvmLight;
using ImageApp.Helper;
using ImageApp.Interfaces;
using ImageApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        List<Image> ListOfImages;
        MainWindow Window;


        public MainViewModel(MainWindow window)
        {
            ListOfImages = new List<Image>();
            ConvertImages = new List<Image>();
            ResultImage = new List<Image>();
            Window = window;
        }

        public List<IObject> AllObjects = new List<IObject>();
        public Double AverageWidth;
        public Double AverageHeight;

        public ICommand DownloadImagesCommand
        {
            get { return new DelegateCommand(DownloadImages); }
        }

        private void DownloadImages()
        {
            var openImage = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                FileName = "image",
                Multiselect=true
            };

            var resultDialog = openImage.ShowDialog();

            if (resultDialog == true)
            {
               LoadAllImages(openImage.FileNames.ToList());
            }

            GetTestData();
        }

        private void GetTestData()
        {
            var r1 = new Row();
            var c1 = new Model.Column();
            var r2 = new Row();
            var c2 = new Model.Column();
            var r3 = new Row();

            r1.Add(new Cartoon() { Link = ListOfImages[0],ParentType = r1.Type });
            r1.Add(c1);
            r1.Add(new Cartoon() { Link = ListOfImages[1],ParentType = r1.Type });
            c1.Add(r2);
            c1.Add(new Cartoon() { Link = ListOfImages[2], ParentType = c1.Type });
            r2.Add(new Cartoon() { Link = ListOfImages[3], ParentType = r2.Type });
            r2.Add(c2);
            c2.Add(r3);
            c2.Add(new Cartoon() { Link = ListOfImages[4], ParentType = c2.Type });
            r3.Add(new Cartoon() { Link = ListOfImages[5], ParentType = r3.Type });
            r3.Add(new Cartoon() { Link = ListOfImages[6], ParentType = r3.Type });

            //Window.StackP.Children.Add(ListOfImages[0]);
            //SourceImage = ListOfImages[0];

            GetResultImage(r1, 650);
            
        }

        private void GetResultImage(Interfaces.IContainer container, double countOfPixel)
        {
            if (container is Row)
            {
                GetAverageValue();

                foreach (var item in ListOfImages)
                {
                    ConvertImages.Add(GetImageWithAverageVolume(item, maxHeight: AverageHeight));
                }

                var proprotional = AllWidth / countOfPixel;

                foreach (var item in ConvertImages)
                {
                    ResultImage.Add(ResizeImageAfterAverageTransformation(item, proprotional));
                }

                foreach (var item in ResultImage)
                {
                    Window.StackP.Children.Add(item);
                }
            }
        }

        public void GetAverageValue()
        {
            var countImages = ListOfImages.Count;

            AverageWidth = ListOfImages.Select(x => x.Source.Width).ToList().Sum() / countImages;
            AverageHeight = ListOfImages.Select(x => x.Source.Height).ToList().Sum() / countImages;
        }

        public List<Image> ResultImage;

        private void LoadAllImages(List<string> listOfImagesPath)
        {
            foreach (var imagePath in listOfImagesPath)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imagePath));
                ListOfImages.Add(image);
            }
        }


        public double AllWidth = 0;
        public List<Image> ConvertImages;

        public Image ResizeImageAfterAverageTransformation(Image image, double proportional)
        {
            var height = image.Source.Height / proportional;
            var width = image.Source.Width / proportional;

            Image newImage = new Image();
            newImage.Source = CreateResizedImage(image.Source, Convert.ToInt32(width), Convert.ToInt32(height));

            return newImage;

        }
        public Image GetImageWithAverageVolume(Image image, double maxWidth = 0, double maxHeight = 0)
        {
            var height = image.Source.Height;
            var width = image.Source.Width;

            double percentOfResize = 0;

            if (maxWidth != 0)
            {
               percentOfResize = (((100 * maxWidth) / width)/100);  
            }

            if (maxHeight != 0)
            {
                percentOfResize = (((100 * maxHeight) / height)/100);
            }

            int toWidth = Convert.ToInt32(image.Source.Width * percentOfResize);
            int toHeight = Convert.ToInt32(image.Source.Height * percentOfResize);

            Image newImage = new Image();

            if (maxHeight != 0)
            {
                newImage.Source = CreateResizedImage(image.Source, toWidth, Convert.ToInt32(maxHeight));
            }

            if (maxWidth != 0)
            {
                newImage.Source = CreateResizedImage(image.Source, Convert.ToInt32(maxWidth), toHeight);
            }

            AllWidth += newImage.Source.Width;

            //Window.StackP.Children.Add(newImage);
            return newImage;


            //var ratioX = (double)maxWidth / image.Width;
            //var ratioY = (double)maxHeight / image.Height;
            //var ratio = Math.Min(ratioX, ratioY);

            //var newWidth = (int)(image.Width * ratio);
            //var newHeight = (int)(image.Height * ratio);

            //Image newImage = new Image();
            //newImage = image;
            //newImage.Width = newWidth;
            //newImage.Height = newHeight;

            //return newImage;
        }


        ImageSource CreateResizedImage(ImageSource source, int width, int height)
        {
            // Target Rect for the resize operation
            Rect rect = new Rect(0, 0, width, height);

            // Create a DrawingVisual/Context to render with
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(source, rect);
            }

            // Use RenderTargetBitmap to resize the original image
            RenderTargetBitmap resizedImage = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height,  // Resized dimensions
                96, 96,                             // Default DPI values
                PixelFormats.Default);              // Default pixel format
            resizedImage.Render(drawingVisual);

            // Return the resized image
            return resizedImage;
        }

    }


}