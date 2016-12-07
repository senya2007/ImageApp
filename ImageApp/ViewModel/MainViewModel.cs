using GalaSoft.MvvmLight;
using ImageApp.Helper;
using ImageApp.Interfaces;
using ImageApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
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

        public MainViewModel()
        {
            ListOfImages = new List<Image>();

        }

        public List<IObject> AllObjects = new List<IObject>();

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

            r1.Add(new Cartoon() { Link = ListOfImages[0] });
            r1.Add(c1);
            r1.Add(new Cartoon() { Link = ListOfImages[1] });
            c1.Add(r2);
            c1.Add(new Cartoon() { Link = ListOfImages[2] });
            r2.Add(new Cartoon() { Link = ListOfImages[3] });
            r2.Add(c2);
            c2.Add(r3);
            c2.Add(new Cartoon() { Link = ListOfImages[4] });
            r3.Add(new Cartoon() { Link = ListOfImages[5] });
            r3.Add(new Cartoon() { Link = ListOfImages[6] });
        }

        private void LoadAllImages(List<string> listOfImagesPath)
        {
            foreach (var imagePath in listOfImagesPath)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(imagePath));
                ListOfImages.Add(image);
            }
        }

        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            Image newImage = new Image();
            newImage = image;
            newImage.Width = newWidth;
            newImage.Height = newHeight;

            return newImage;
        }

    }
}