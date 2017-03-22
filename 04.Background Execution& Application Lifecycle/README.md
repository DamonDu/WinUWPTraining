# README

### 实验目标

实现UWP应用挂起时保存进度，下次启动恢复现场的功能。

### 实验思路

1. `OnNavigatedTo`方法是在每次页面成为活动页面时调用的方法。在该方法中恢复上次应用进度。

2. `OnNavigatedFrom`方法时页面成为非活动页面时执行的最后操作。在该方法中保存应用进度。

3. 将`ApplicationData.LocalSettings`是`windows.storage`中提供的应用程序设置容器，每次挂起和重启时，将程序设置保存到该容器或者从该容器中调用即可实现保存和恢复应用进度。

4. 此外，如果用户挂起之前停留在`newpage`页面，还要实现下次启动程序自动跳转到`newpage`，这个功能通过修改`App.xaml.cs`的`OnLaunched`和`OnSuspending`实现。

### 实验过程

#### (一)修改部分代码

​	这次的实验代码，我是基于 week2 做的*XAML Controls & Page Navigation & Adaptive UI*的代码修改而来，由于当时对数据绑定不了解，所以在实现点击checkbox时使`Line`出现这个功能时用了比较复杂的方法，所以我先用数据绑定重写了这部分实现：

​	在`MainPage.xaml`中：

```xml
<Page.Resources>
        <md:BoolToVisibilityConverter x:Key="BoolToVisConverter" />
</Page.Resources>

<Line Name="Line" Grid.Column="2" Stretch="Fill" Stroke="Black" StrokeThickness="2" X1="1" VerticalAlignment="Center" HorizontalAlignment="Stretch"  Visibility="{Binding Path=IsChecked, ElementName=CheckBox, Converter={StaticResource BoolToVisConverter}, Mode=TwoWay}"/>
```

​	在`BoolToVisibilityConverter.cs`定义一个转换器：

```c#
namespace Todo.Models
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert (object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility _visibility = (Visibility)value;
            return _visibility == Visibility.Collapsed ? false : true;
        }
    }
}
```
#### （二）修改`App.xaml.cs`的`OnLaunched`和`OnSuspending`

​	修改`OnLaunched`以实现让程序启动时进入挂起前的状态。其中，`ApplicationExecutionState`是一个指定应用程式的执行状态的枚举量，当他的值为`Terminated`，程序会加载挂起前的程序状态：

```c#
protected override void OnLaunched(LaunchActivatedEventArgs e)
{
  //其它代码
  if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
  {
    //TODO: 从之前挂起的应用程序加载状态
    if(ApplicationData.Current.LocalSettings.Values.ContainsKey("NavigationState"))
    {                      rootFrame.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["NavigationState"]);
    }
  }
  //其它代码
}
```

​	修改`OnSuspending`以实现让程序挂起时保存状态。

```c#
private void OnSuspending(object sender, SuspendingEventArgs e)
{
	var deferral = e.SuspendingOperation.GetDeferral();
	//TODO: 保存应用程序状态并停止任何后台活动

	IsSuspending = true;
	Frame frame = Window.Current.Content as Frame;
	ApplicationData.Current.LocalSettings.Values["NavigationState"]=	frame.GetNavigationState();
	deferral.Complete();
}
```

#### （三）修改`OnNavigatedTo`和`OnNavigatedFrom`

​	正如上文提到：`ApplicationData.LocalSettings`是`windows.storage`中提供的应用程序设置容器，我们只需要将组件的状态值储存到容器中，然后再下次启动时调用并删除就可以了。下面是`Mainpage`的实现，`Newpage`同理：

```c#
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
			var compositeValue = ApplicationData.Current.LocalSettings.Values["LastWork"] 			as ApplicationDataCompositeValue;
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
```

