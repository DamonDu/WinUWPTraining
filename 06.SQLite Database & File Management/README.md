# README

### 实验目标

实现todo表项的增、删、改、查。

利用数据库保存及恢复应用状态。

学习使用第三方工具查看数据库内容。

### 效果展示

![](http://ompnv884d.bkt.clouddn.com/uwphw6_1.JPG-1)

![](http://ompnv884d.bkt.clouddn.com/uwphw6_2.JPG-1)

### 实验思路

SQLite 是一个开源的无服务器嵌入式数据库。有三种方法将 SQLite 添加到 UWP 项目：1.使用 SDK SQLite
2.将 SQLite 包含在应用包内 3. 从 Visual Studio 中的源生成 SQLite。可以使用 SQLite C API 创建、更新和删除 SQLite 数据库。有六个接口来执行对这些对象的数据库操作：sqlite3_open()；sqlite3_prepare()；sqlite3_step()；sqlite3_column()；sqlite3_finalize()；sqlite3_close()。

### 实验过程

#### (一)配置`SQLite`环境

​	添加SQLite的UWP项目的方法在[微软开发文档](https://docs.microsoft.com/zh-cn/windows/uwp/data-access/sqlite-databases)有详细的介绍，这里不做赘述。

#### (二)创建`DBService.cs`

​	`DBService.cs`包含了数据库基本功能的实现，包括创建并初始化数据库，数据库的增、删、改、查功能。下面是具体代码：

​	创建数据库：

```c#
private void LoadDatabase()
{
    conn = new SQLiteConnection("Todo.db");
    string sql = @"CREATE TABLE IF NOT EXISTS "
                    + "Todo (TableId    INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
                        + "ItemId      VARCHAR(140),"
                        + "Title       VARCHAR(140),"
                        + "Description VARCHAR(140),"
                        + "Date        VARCHAR(140)"
                        + ");";
    using (var statement = conn.Prepare(sql))
    {
        statement.Step();
    }
}

private SQLiteConnection conn { get; set; }

public DBService()
{
    LoadDatabase();
}
```

​	增添功能：

```c#
public void CreateItem(Models.TodoItem TodoToCreate)
{
    try
    {
        string sql = "INSERT INTO Todo (ItemId, Title, Description, Date) VALUES (?, ?, ?, ?)";
        using (var statement = conn.Prepare(sql))
        {
            statement.Bind(1, TodoToCreate.Id);
            statement.Bind(2, TodoToCreate.Title);
            statement.Bind(3, TodoToCreate.Discription);
            statement.Bind(4, TodoToCreate.DueDate.ToString());
            statement.Step();
        }
    }
    catch (Exception ex)
    {
        var i = new MessageDialog("Error in creating item" + ex).ShowAsync();
    }
}
```

​	删除功能：

```c#
public void DeleteById(string itemId)
{
    string sql = "DELETE FROM Todo WHERE ItemId = ?";
    using (var statement = conn.Prepare(sql))
    {
        statement.Bind(1, itemId);
        statement.Step();
    }
}
```

​	修改功能：

```c#
public void UpdateItem(Models.TodoItem TodoToUpdate)
{
    string sql = "UPDATE Todo SET Title = ?, Description = ?, Date = ? WHERE ItemId = ?";
    using (var custstmt = conn.Prepare(sql))
    {
        custstmt.Bind(1, TodoToUpdate.Title);
        custstmt.Bind(2, TodoToUpdate.Discription);
        custstmt.Bind(3, TodoToUpdate.DueDate.ToString());
        custstmt.Bind(4, TodoToUpdate.Id);
        custstmt.Step();
    }
}
```

​	查询功能：

```c#
public List<Models.DisplayItem> GetItemsByStr(string str)
{
    string sql = "SELECT TableId, Title, Description, Date FROM Todo "
        + "WHERE Title LIKE ? OR Description LIKE ? OR Date LIKE ?";
    int i, searchKeyNum = 3;
    using (var statement = conn.Prepare(sql))
    {
        for (i = 1; i <= searchKeyNum; ++i)
            statement.Bind(i, "%" + str + "%");

        List<Models.DisplayItem> res = new List<Models.DisplayItem>();
        Models.DisplayItem tmp;
        while (statement.Step() == SQLiteResult.ROW)
        { // get result of one row
            tmp = new Models.DisplayItem()
            {
                Id = (Int64)statement[0],
                Title = (string)statement[1],
                Description = (string)statement[2],
                Date = (string)statement[3]
            };
            res.Add(tmp);
        }

        return res;
    }
}
```

#### (三)修改`MainPage`和`NewPage`中对应的函数

​	在`MainPage.xaml.cs`中添加查询函数：

```c#
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
```

​	`NewPage`中同理，只要将信息同步在数据库中修改就可以。

#### (四)使用第三方工具查看数据库内容

​	下载安装，找到程序数据库目录，打开后效果如下：

![](http://ompnv884d.bkt.clouddn.com/uwphw6_3.JPG)

