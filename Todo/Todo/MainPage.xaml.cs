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
using Todo.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Todo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TodoItemViewModel ViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.ForestGreen;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.ForestGreen;
            ViewModel = new ViewModel.TodoItemViewModel();
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1), ViewModel);
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
                ViewModel.AddTodoItem(title.Text, details.Text, datePicker.Date.DateTime);
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

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (DataModel.TodoListItem)(e.ClickedItem);
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.ActualWidth >= 800)
            {
                title.Text = ViewModel.SelectedItem.title;
                details.Text = ViewModel.SelectedItem.description;
                datePicker.Date = ViewModel.SelectedItem.date;
                createButton.Visibility = Visibility.Collapsed;
                updateButton.Visibility = Visibility.Visible;
            }
        }

        
    }
}
