using System;
using System.Collections.Generic;
using System.IO;
using UploadFile.Proxy;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Runtime.InteropServices.WindowsRuntime;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UploadFile
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void OpenFileClick(object sender, RoutedEventArgs e)
        {
            PictureServiceClient oPicture = new PictureServiceClient();
            PictureFile pictureFile = new PictureFile();

            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".gif");

            

            if (chkMultipleFiles.IsChecked.Value)
            {
                IAsyncOperation<IReadOnlyList<StorageFile>> asyncOp = filePicker.PickMultipleFilesAsync();
                IReadOnlyList<StorageFile> fileList = await asyncOp;

                if (fileList != null)
                {
                    var myPictures = new List<MyPicture>();

                    foreach (var file in fileList)
                    {
                        using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read)) 
                        {
                            var image = new BitmapImage();
                            image.SetSource(stream);
                            myPictures.Add(new MyPicture { PictureImage = image });
                        };                        
                    }

                    gvPictures.ItemsSource = myPictures;
                }
            }
            else
            {
                IAsyncOperation<StorageFile> asyncOp = filePicker.PickSingleFileAsync();
                StorageFile file = await asyncOp;

                if (file != null)
                {
                    using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        BitmapImage image = new BitmapImage();
                        image.SetSource(stream);

                        IBuffer buffer = await FileIO.ReadBufferAsync(file);

                        byte[] pixeBuffer = buffer.ToArray();

                        pictureFile.PictureStream = pixeBuffer;
                        pictureFile.PictureName = file.Name;

                        bool rpta = await oPicture.UploadAsync(pictureFile);

                        var myPictures = new MyPicture[] { new MyPicture { PictureImage = image } };
                        gvPictures.ItemsSource = myPictures;

                    }; 
                    
                }
            }

        }

        
    }

    public class MyPicture
    {
        public ImageSource PictureImage { get; set; }
    }
}
