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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Windows.UI.Popups;
using Windows.Data.Xml.Dom;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MobileQuery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void GetAttribution_json(string tel)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var headers = httpClient.DefaultRequestHeaders;
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }
                
                headers.Add("apikey", "82b464f3e6e940432862aaabeac7e8c9");
                string getPhoneNumCode = "http://sj.apidata.cn/" + "?mobile=" + tel;
                string result = await httpClient.GetStringAsync(getPhoneNumCode);
            
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

        private async void GetAttribution_xml(string city)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var headers = httpClient.DefaultRequestHeaders;
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }

                string getPhoneNumCode_xml = "http://v.juhe.cn/weather/index?format=2&dtype=xml&key=c4c61e395b3dda219fcb12b78165230a&cityname=" + city;
                string result = await httpClient.GetStringAsync(getPhoneNumCode_xml);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);

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

        private void Submit_json(object sender, RoutedEventArgs e)
        {
            ProvinceName.Text = "";
            CATName.Text = "";
            infor.Text = ""; 
            GetAttribution_json(PhoneNum.Text);
        }

        private void Submit_xml(object sender, RoutedEventArgs e)
        {
            weather.Text = "";
            infor.Text = "";
            GetAttribution_xml(city.Text);
        }
    }
}
