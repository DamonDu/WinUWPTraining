using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using SQLitePCL;
using System.Collections.Generic;


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
            this.ViewModel = new ViewModels.TodoItemViewModel();
            db = new Services.DBService();
        }

        public ViewModels.TodoItemViewModel ViewModel { get; set; }
        private string currentId { get; set; }
        private Services.DBService db { get; set; }


        private void AddTodoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1), ViewModel);
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (e.ClickedItem as Models.TodoItem);
            Frame.Navigate(typeof(BlankPage1), ViewModel);
        }

        private void UpdateTileButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.NewestItem == null)
            {
                var i = new MessageDialog("There is no Todo Item!").ShowAsync();
                return;
            }

            string from = ViewModel.NewestItem.Title;
            string body = ViewModel.NewestItem.Discription;

            // Construct the tile content
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,
                    DisplayName = "Todos",

                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                new TileText()
                                {
                                    Text = from,
                                    Style = TileTextStyle.Subtitle
                                }
                            }
                        }
                    },

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                 new TileText()
                                 {
                                    Text = from,
                                    Style = TileTextStyle.Subtitle
                                 },
                                 new TileText()
                                 {
                                    Text = body,
                                    Style = TileTextStyle.CaptionSubtle
                                 }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                 new TileText()
                                 {
                                    Text = from,
                                    Style = TileTextStyle.Subtitle
                                 },
                                 new TileText()
                                 {
                                    Text = body,
                                    Style = TileTextStyle.CaptionSubtle
                                 }
                            }
                        }
                    }
                }
            };

            var notification = new TileNotification(content.GetXml());
            // send the notification
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("LastWork");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("LastWork"))
                {
                    var compositeValue = ApplicationData.Current.LocalSettings.Values["LastWork"] as ApplicationDataCompositeValue;
                    this.ViewModel = (ViewModels.TodoItemViewModel)compositeValue["viewModel"];
                    ApplicationData.Current.LocalSettings.Values.Remove("LastWork");
                }
            }
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = e.Parameter as ViewModels.TodoItemViewModel;
            }
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).IsSuspending)
            {
                var compositeValue = new ApplicationDataCompositeValue();
                compositeValue["viewModel"] = this.ViewModel;
                ApplicationData.Current.LocalSettings.Values["LastWork"] = compositeValue;
            }
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            // get the Id of current button
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            currentId = fe.DataContext.ToString();
            DataTransferManager.ShowShareUI();
        }

        // Handle DataRequested event and provide DataPackage
        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            Models.TodoItem sharedItem = ViewModel.GetItemById(currentId);
            var data = args.Request.Data;
            DataRequestDeferral GetFiles = args.Request.GetDeferral();

            try
            {
                data.Properties.Title = sharedItem.Title;
                data.Properties.Description = sharedItem.Discription;
                var imageFile = sharedItem.ShareFile;
                if (imageFile != null)
                {
                    data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(imageFile);
                    data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
                }
                else
                {
                    data.SetText(sharedItem.Discription);  // if no image, share text
                }
            }
            finally
            {
                GetFiles.Complete();
            }
        }

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            List<Models.DisplayItem> QueryResult = db.GetItemsByStr(Query.Text);
            string ItemInfo = "";
            for (int i = 0; i < QueryResult.Count; ++i)
            {
                ItemInfo += QueryResult[i].ToString() + "\n";
            }
            if (ItemInfo == "")
            {
                ItemInfo = "There is no item in database which is matched to the query!";
            }
            var message = new MessageDialog(ItemInfo).ShowAsync();
        }
    }
}
