# README

### 效果展示

​							![0](http://ompnv884d.bkt.clouddn.com/0.JPG)

​							![1](http://ompnv884d.bkt.clouddn.com/1.JPG)

​							![2](http://ompnv884d.bkt.clouddn.com/2.JPG)

​							![3](http://ompnv884d.bkt.clouddn.com/3.JPG)									

### 实验思路

1. 文件结构主要分为两个页面：Mainpage 和 Newpage，主页面为 Mainpage 

2. 使用`Grid`来实现响应式UI

3. 两个页面通过 `Navigate`来实现导航，并通过向`GetForCurrentView().BackRequested`这个预定义的委托挂载返回请求来实现返回功能

4. 使用`datePicker`来实现日历选取功能

### 注意事项&解决方法

#### （一）`Grid` 的使用

​	在使用 `Grid` 之前，一直对 UWP 的响应式 UI 的实现方式很感兴趣。其实该实现方式并不复杂，UWP为开发者很好的封装了这个元素：

> `Grid` 定义由行和列组成的灵活的网格区域。`Grid` 的子元素根据其行/列分配（使用 [**Grid.Row**](https://msdn.microsoft.com/zh-cn/library/windows/apps/windows.ui.xaml.controls.grid.row.aspx)  和 [**Grid.Column**](https://msdn.microsoft.com/zh-cn/library/windows/apps/windows.ui.xaml.controls.grid.column.aspx) 附加属性来设置）和其他逻辑进行测量和排列。

​	通过设置`Grid`的 `RowDefinition` 和 `ColumnDefinition` 的 `Height` 或 `Width` 的值( `Auto` 、*或固定值)，就可以快速地制作一个响应式布局的 UWP 应用。

​	`Grid` 使用如下：

```xml
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
        
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
  		
  		<Grid Grid.Row="1">
          ...  
        </Grid>
  
</Grid>
```

​	当我们设置 `Height` 的值为 * 时，该行将自动填充剩余长度。

#### （二）导航

1.   页面导航

​	导航的实现同样很简洁，UWP为我们封装了`Navigate`函数，比如导航到目标页面TargetPage.xaml：

```c#
Frame.Navigate(typeof(TargetPage), ParametersObject);
```

​	这个方法是可以传递参数 `ParametersObject` 的，可以在目标页面接受此参数：

```c#
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    //参数处理
}
```

1.   页面回退

​	UWP中，如果不进行后退处理，页面导航完了默认是不能后退。页面回退的实现方式相对比较复杂一点，较为简单的方法是在每个导航到达的页面都进行如下代码编辑：

```c#
//后退
if (Frame.CanGoBack)
{
    Frame.GoBack();
}
```

​	这种方法逻辑比较简单，但是缺点也明显：需要重复地在每个页面设置，太过冗余。一般我们通过修改App.xaml.cs 来实现自动处理后退的逻辑：

```c#
//修改该函数
protected override void OnLaunched(LaunchActivatedEventArgs e)
{

    Frame rootFrame = Window.Current.Content as Frame;
    if (rootFrame == null)
    {
        rootFrame = new Frame();
        rootFrame.NavigationFailed += OnNavigationFailed;
        rootFrame.Navigated += OnNavigated; // 添加此句
        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
           
        }
        Window.Current.Content = rootFrame;
		//添加下面这句
        SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            rootFrame.CanGoBack ?
            AppViewBackButtonVisibility.Visible :
            AppViewBackButtonVisibility.Collapsed;
    }

    if (rootFrame.Content == null)
    {
        rootFrame.Navigate(typeof(MainPage), e.Arguments);
    }
    Window.Current.Activate();
}

//添加此函数
private void OnNavigated(object sender, NavigationEventArgs e)
{
    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
        ((Frame)sender).CanGoBack ?
        AppViewBackButtonVisibility.Visible :
        AppViewBackButtonVisibility.Collapsed;
}

//添加此函数
private void OnBackRequested(object sender, BackRequestedEventArgs e)
{
    Frame rootFrame = Window.Current.Content as Frame;

    if (rootFrame != null && rootFrame.CanGoBack)
    {
        e.Handled = true;
        rootFrame.GoBack();
    }
}
```

​	上述代码主要是通过向`rootFrame.Navigated` 和`GetForCurrentView().BackRequested` 这两个委托注册回退事件来实现自动处理回退。

#### （三）选择本地图片并显示

​	由于之前没有接触过 Win API 所以低估了这部分的实现难度，一开始的想法是只要拿到图片的URI然后赋值给Image元素就行，后面实现过程才发现没这么简单。先附上最后的实现代码：

```c#
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
```

​	一开始遇到的问题是，得到了图片的路径，但是字符串赋值给Image的Source时出现了格式无法转化的问题，百思不得其解，最后是在API手册中看到这段话才明白：

> 若要显示单个图像，请使用枚举库时提供 [**StorageFile**](https://msdn.microsoft.com/zh-cn/library/windows/apps/xaml/windows.storage.storagefile.aspx) 对象，然后调 [**OpenAsync**](https://msdn.microsoft.com/zh-cn/library/windows/apps/xaml/br227221.aspx) 以获取流。使用此流来设置图像源，方法是创建新 [**BitmapSource**](https://msdn.microsoft.com/zh-cn/library/windows/apps/xaml/windows.ui.xaml.media.imaging.bitmapsource.aspx)，并使用流来调用 [**SetSourceAsync**](https://msdn.microsoft.com/zh-cn/library/windows/apps/xaml/windows.ui.xaml.media.imaging.bitmapsource.setsourceasync.aspx)。注意，你需要指定以编程方式访问图片库的功能

​	其次，由于不熟悉C#的异步过程，所以也遇到了许多麻烦，最后通过`await`来实现异步过程。需要注意的是要先把该方法声明为异步方法，才能顺利使用`await`关键字。

​	在文件选择器里设置过滤是必要的通常的做法，通过`openPicker.FileTypeFilter.Add(".jpg")`实现

### 经验总结


​	这次project还是学到比较多东西的，对UWP的平台特质又有了更进一步的了解。例如响应式的UI设计、导航和回退等等。

​	XAML的高拓展性令我印象很深刻，通过依赖属性、静态绑定这些功能，开发者可以快速的解决一些样式问题，甚至可以完成一些简单的逻辑实现。

​	Win API 很是庞大，善于使用API 手册很重要。



