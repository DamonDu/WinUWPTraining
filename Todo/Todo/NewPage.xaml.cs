using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Todo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }
        private ViewModel.TodoItemViewModel ViewModel;

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            ViewModel = ((ViewModel.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createButton.Visibility = Visibility.Visible;
                updateButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                createButton.Visibility = Visibility.Collapsed;
                updateButton.Visibility = Visibility.Visible;
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                datePicker.Date = ViewModel.SelectedItem.date;
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
                ViewModel.AddTodoItem(title.Text, details.Text, datePicker.Date.DateTime);
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
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
                ViewModel.UpdateTodoItem(title.Text, details.Text, datePicker.Date.DateTime);
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

        private void delete_button_click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.RemoveTodoItem();
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }
    }
}
