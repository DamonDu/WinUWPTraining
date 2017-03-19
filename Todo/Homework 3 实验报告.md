# Homework 3 实验报告

​				`中山大学数据科学与计算机学院   软件工程教务一班   15331069   杜一柯`



## 实验目标

​	实现Todo小程序的Adaptive UI和Data Binding

## 效果展示

![](http://ompnv884d.bkt.clouddn.com/uwp_1.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_2.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_3.JPG-default)

![](http://ompnv884d.bkt.clouddn.com/uwp_4.JPG-default)

## 实验思路

1. 文件结构主要分为两个层次，View层由Mainpage 和 Newpage两个Page构成；Model层由TodoListItem.cs和TodoItemViewModel.cs构成

2. 使用`VisualStateGroup`实现Adaptive UI

3. 使用`x:Bind`和`Binding`实现Data Binding

4. 使用`ObservableCollection`实现数据的实时更新

   ## 注意事项&解决方法

   ​

#### （一）`VisualStateGroup`的使用

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



#### (二)`x:Bind`和`Binding`的使用

```xml
<TextBlock Grid.Column="2" Text="{x:Bind title}" />
<Line Name="Line" Visibility="{Binding ElementName=CheckBox, Path=IsChecked,Mode=OneWay}"/>
```

#### (三)`ObservableCollection`的使用



```c#
private ObservableCollection<TodoListItem> allItems = new ObservableCollection<TodoListItem>();
```



```c#
public class TodoListItem : INotifyPropertyChanged
    {
       //field
       //method
    }
```

