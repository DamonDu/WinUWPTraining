using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Todo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.ForestGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.ForestGreen;
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(CheckBox.IsChecked == true)
            {
                Line.Visibility = Visibility.Visible;
            }
            else
            {
                Line.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (CheckBox1.IsChecked == true)
            {
                Line1.Visibility = Visibility.Visible;
            }
            else
            {
                Line1.Visibility = Visibility.Collapsed;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Now;

            if (title.Text == "" || details.Text == "")
            {
                displayTextErrorDialog();
            }
            else if (datePicker.Date < today)
            {
                displayDateErrorDialog();
            }
            else
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private async void displayTextErrorDialog()
        {
            ContentDialog textErrorDialog = new ContentDialog()
            {
                Title = "Error",
                Content = "Please fill all the blank!",
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await textErrorDialog.ShowAsync();
        }

        private async void displayDateErrorDialog()
        {
            ContentDialog dateErrorDialog = new ContentDialog()
            {
                Title = "Error",
                Content = "Please choose the right date!",
                PrimaryButtonText = "Ok"
            };

            ContentDialogResult result = await dateErrorDialog.ShowAsync();
        }

        async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap 
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    selectPicture.Source = bitmapImage;
                }
            }
        }
    }
}
