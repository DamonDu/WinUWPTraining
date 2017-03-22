using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("LastWork");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("LastWork"))
                {
                    var compositeValue = ApplicationData.Current.LocalSettings.Values["LastWork"] as ApplicationDataCompositeValue;
                    CheckBox.IsChecked = (bool?)compositeValue["checkBoxValue"];
                    CheckBox1.IsChecked = (bool?)compositeValue["checkBoxValue1"];
                    ApplicationData.Current.LocalSettings.Values.Remove("LastWork");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (((App)App.Current).IsSuspending)
            {
                var compositeValue = new ApplicationDataCompositeValue();
                compositeValue["checkBoxValue"] = CheckBox.IsChecked;
                compositeValue["checkBoxValue1"] = CheckBox1.IsChecked;
                ApplicationData.Current.LocalSettings.Values["LastWork"] = compositeValue;
            }
        }
    }
}
