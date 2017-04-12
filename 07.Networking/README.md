# README

### 实验目标

使用`HttpWebRequest`或`HttpClient`访问网络

查手机号码归属地，输入城市查天气，快递查询等生活实用功能至少完成一种

### 效果展示



![](http://ompnv884d.bkt.clouddn.com/uwp7_1.JPG-1)

### 实验思路

HttpClient是.NET4.5引入的一个HTTP客户端库，其命名空间为System.Net.Http。它可以实现：

- 可以使用单个HttpClient实例发任意数目的请求
- 一个HttpClient实例不会跟某个HTTP服务器或主机绑定，也就是说我们可以用一个实例同时给www.a.com和www.b.com发请求、
- 可以继承HttpClient达到定制目的
- HttpClient利用了最新的面向任务模式，使得处理异步请求非常容易

HttpRequest类的对象用于服务器端，获取客户端传来的请求的信息，包括HTTP报文传送过来的所有信息。HttpWebRequest这个类非常强大，强大的地方在于它封装了几乎HTTP请求报文里需要用到的东西，以致于能够能够发送任意的HTTP请求并获得服务器响应(Response)信息。

### 实验过程

#### (一)安装`Newtonsoft.Json`

​	在VS中，点击工具>>NuGet包管理器>>管理解决方案的NuGet的程序包，搜索`Newtonsoft.Json`并下载安装。

​	![](http://ompnv884d.bkt.clouddn.com/uwp7_2.JPG-1)

#### (二)使用`JSON`格式实现查询手机归属地

​	简单构建完MainPage的布局后，就可以开始实现查询手机归属地功能，这里我选择获取`JSON`格式来处理，实现代码如下：

```c#
private async void GetAttribution_json(string tel)
{
    try
    {
      	//创建HttpClient对象并完成基本配置
        HttpClient httpClient = new HttpClient();
        var headers = httpClient.DefaultRequestHeaders;
        string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
        if (!headers.UserAgent.TryParseAdd(header))
        {
            throw new Exception("Invalid header value: " + header);
        }
        
        //调用API
        headers.Add("apikey", "82b464f3e6e940432862aaabeac7e8c9");
        string getPhoneNumCode = "http://sj.apidata.cn/" + "?mobile=" + tel;
        string result = await httpClient.GetStringAsync(getPhoneNumCode);
          
        //处理json数据
        JObject res = (JObject)JsonConvert.DeserializeObject(result);
        if (res["status"].ToString() == "0")
            throw (new Exception("手机号码有误"));

        if (res["data"] != null)
        {
            ProvinceName.Text = res["data"]["province"].ToString();
            CATName.Text = res["data"]["types"].ToString();
        }
    }
    catch (Exception e)
    {
        infor.Text = e.ToString();
    }
}
```

​	这里我使用了http://sj.apidata.cn/的API，通过URL设置需要查询的手机号码，使用GET的方法就可以获取结果。结果的处理我先使用`GetStringAsync()`储存到字符串变量中，然后再通过使用`JsonConvert.DeserializeObject()`方法获取对应的`JObject`对象。在第一步导入了`Newtonsoft.Json`后，我们就可以直接使用`Newtonsoft.Json`类中的方法来处理`JObject`对象了。

#### (三)使用`XML`格式实现查询天气

​	相似的，这次换使用XML格式文件来获取数据，实现查询天气功能：

```c#
private async void GetAttribution_xml(string city)
{
    try
    {
      	//创建HttpClient对象并完成基本配置
        HttpClient httpClient = new HttpClient();
        var headers = httpClient.DefaultRequestHeaders;
        string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
        if (!headers.UserAgent.TryParseAdd(header))
        {
            throw new Exception("Invalid header value: " + header);
        }
      
		//调用API
        string getPhoneNumCode_xml = "http://v.juhe.cn/weather/index?format=2&dtype=xml&key=c4c61e395b3dda219fcb12b78165230a&cityname=" + city;
        string result = await httpClient.GetStringAsync(getPhoneNumCode_xml);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(result);

        //处理xml数据
        XmlNodeList list = doc.GetElementsByTagName("resultcode");
        IXmlNode node = list.Item(0);
        if (node.InnerText != "200")
            throw (new Exception("输入城市错误")); 

        list = doc.GetElementsByTagName("temperature");
        node = list.Item(0);
        weather.Text = node.InnerText;
    }
    catch (Exception e)
    {
        infor.Text = e.ToString();
    }
}
```

​	实现方法类似，主要是xml数据的处理不尽相同，直接调用系统类中的`Windows.Data.Xml.Dom`来获取信息。

