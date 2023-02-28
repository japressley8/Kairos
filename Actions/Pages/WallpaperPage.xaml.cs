using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Kairos.Actions.Pages
{
    /// <summary>
    /// Interaction logic for WallpaperPage.xaml
    /// </summary>
    public partial class WallpaperPage : System.Windows.Controls.Page
    {
        Wallpaper wallpaper;
        private static string _orientationQuery = "System.Photo.Orientation";
        public WallpaperPage(Wallpaper wall)
        {
            InitializeComponent();
            wallpaper = wall;
            if (wallpaper.Path != "")
            {
                try
                {
                Uri imageUri = new Uri(wallpaper.Path, UriKind.Absolute);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                imageFrame.Source = LoadImageFile(wallpaper.Path);
                imageFrame.Height = 200;
                imageFrame.Visibility = Visibility.Visible;
                }
                catch
                {
                    MessageBox.Show("Image file missing");
                }
            }
        }
        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "Select a picture";
            dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + 
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + 
                "Portable Network Graphic (*.png)|*.png"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                wallpaper.Path = dialog.FileName;
                Uri imageUri = new Uri(wallpaper.Path, UriKind.Absolute);
                BitmapImage imageBitmap = new BitmapImage(imageUri);
                imageFrame.Source = LoadImageFile(wallpaper.Path);
            }
        }
        public static BitmapSource LoadImageFile(String path)
        {
            Rotation rotation = Rotation.Rotate0;
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BitmapFrame bitmapFrame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                BitmapMetadata bitmapMetadata = bitmapFrame.Metadata as BitmapMetadata;

                if ((bitmapMetadata != null) && (bitmapMetadata.ContainsQuery(_orientationQuery)))
                {
                    object o = bitmapMetadata.GetQuery(_orientationQuery);

                    if (o != null)
                    {
                        switch ((ushort)o)
                        {
                            case 6:
                                {
                                    rotation = Rotation.Rotate90;
                                }
                                break;
                            case 3:
                                {
                                    rotation = Rotation.Rotate180;
                                }
                                break;
                            case 8:
                                {
                                    rotation = Rotation.Rotate270;
                                }
                                break;
                        }
                    }
                }
            }

            BitmapImage _image = new BitmapImage();
            _image.BeginInit();
            _image.UriSource = new Uri(path);
            _image.Rotation = rotation;
            _image.EndInit();
            _image.Freeze();

            return _image;
        }
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            wallpaper.Do();
        }
        private void showbutton_Click(object sender, RoutedEventArgs e)
        {
            wallpaper.Undo();
        }
    }
}
