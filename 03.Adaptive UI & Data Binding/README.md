# README

### 实验目标

​	实现Todo小程序的Adaptive UI和Data Binding

### 效果展示

![](http://ompnv884d.bkt.clouddn.com/uwp_1.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_2.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_3.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_4.JPG-default)

### 实验思路

1. 文件结构主要分为两个层次，View层由`Mainpage`和 `Newpage`两个Page构成；Model层由`TodoListItem.cs`和`TodoItemViewModel.cs`构成

2. 使用`VisualStateGroup`实现Adaptive UI

3. 使用`x:Bind`和`Binding`实现Data Binding

4. 使用`ObservableCollection`实现数据的实时更新

### 注意事项&解决方法

#### （一）`VisualStateGroup`的使用

​	在`MainPage.xaml`中添加如下代码：

```xml
<VisualStateManager.VisualStateGroups>
            <VisualStateGroup x:Name="VisualStateGroup">
                <VisualState x:Name="VisualStateMin0">
                    <VisualState.Setters>
                        <Setter Target="ToDoListDetailView.(Visibility)" Value="Collapsed"/>
                        <Setter Target="TodoGrid.(Grid.ColumnSpan)" Value="2"/>
                    </VisualState.Setters>
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="1"/>
                    </VisualState.StateTriggers>
                </VisualState>

                <VisualState x:Name="VisualStateMin800">
                    <VisualState.Setters>
                        <Setter Target="AddAppBarButton.(Control.IsEnabled)" Value="False"/>
                    </VisualState.Setters>
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="800"/>
                    </VisualState.StateTriggers>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>
```

​	`VisualStateGroups`通过判断当前设备窗口的大小，来设定不同的界面。`VisualStateGroups`的用法是：通过使用标签`VisualState`来定义不同的状态，使用其`Setters`来设定具体不同状态下特定标签的不同属性，`AdaptiveTrigger`则设置了不同的设备窗口判断条件。使用`VisualStateGroups`的好处是：不需要对整个界面进行重新设定，只要设置需要改变的标签的需要改变的属性，减少了代码量，增强了可读性。

#### (二)`x:Bind`和`Binding`的使用

​	在`MainPage.xaml`修改如下代码：

```xml
<TextBlock Grid.Column="2" Text="{x:Bind title}" />
<Line Name="Line" Visibility="{Binding ElementName=CheckBox, Path=IsChecked,Mode=OneWay}"/>
```

​	UWP开发框架提供数据绑定，具体是`x:Bind`和`Binding`，利用数据绑定，可以设置标签属性值与工程本地数据或其他标签属性值的绑定。设置绑定的`Mode`为`OneTime`、`OneWay`或`TwoWay`可以设置对应的绑定模式分别为一次绑定、单向绑定和双向绑定。设置数据绑定的好处是简化代码，开发者不需要在通过复杂的事件监听函数来实现数据的同步。

#### (三)`ObservableCollection`的使用

​	在`TodoItemViewModel.cs`添加如下代码：

```c#
private ObservableCollection<TodoListItem> allItems = new ObservableCollection<TodoListItem>();
```

​	上述代码实例化了一个`ObservableCollection`对象，该对象可以实现当数据成员发生添加、修改、删除时自动刷新，需要对数据成员类实现`INotifyPropertyChanged`接口，如下：

```c#
public class TodoListItem : INotifyPropertyChanged
    {
       //field
       //method
    }
```

​	通过实现上述接口，当`allItems`中的数据成员发生改变时，集合能自动刷新，这样就实现了当用户编辑Todo项目时Todo列表的自动刷新。