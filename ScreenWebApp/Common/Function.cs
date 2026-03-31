using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace ScreenWebApp.Conmmon
{
    public class Function
    {
        private static string Description = "App";

        /// <summary>
        /// 配置文件
        /// </summary>        

        private static IConfigurationRoot setting;

        public static IConfigurationRoot Setting
        {
            get {
                if (setting == null)
                {
                    IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                    return config;
                }
                else
                {
                    return setting;
                }
                    
            }
            set { setting = value; }
        }


        public static string accessToken { get; set; }

        private static string serviceUrl
        {
            get { return Setting["AppSettings:UrlA"]; }
        }

        private static string key
        {
            get { return Setting["AppSettings:SM4:Key"]; }
        }

        private static string algo
        {
            get { return Setting["AppSettings:SM4:Algo"]; }
        }

        public Function()
        {
            
        }

        public static string ReadConfig(string name)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string conString = config[name];
            return conString;
        }

        #region 利用反射来判断对象是否包含某个属性
        /// <summary>
        /// 利用反射来判断对象是否包含某个属性
        /// </summary>
        /// <param name="instance">object</param>
        /// <param name="propertyName">需要判断的属性</param>
        /// <returns>是否包含</returns>
        public static bool ContainProperty(object instance, string propertyName)
        {
            if (instance != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo _findedPropertyInfo = instance.GetType().GetProperty(propertyName);
                return _findedPropertyInfo != null;
            }
            return false;
        }

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        public static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null)
            {
                return string.Empty;
            }
            object o = property.GetValue(t, null);
            if (o == null)
            {
                return string.Empty;
            }
            return o.ToString();
        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void GetObjectProperty<T>()
        {
            Type t = typeof(T);
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo info in properties)
            {
                Console.Write("name=" + info.Name + ";" + "type=" + info.PropertyType.Name + ";value=" + GetObjectPropertyValue(new object(), info.Name) + "<br />");
            }
        }
        #endregion


        /// <summary>
        /// 用App.Config作为配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Config(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 使用appsettings.json作为配置文件
        /// 读取示例 GetSettings("RabbitMQ:host")
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSettings(string key)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration[key];
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="instr">内容</param>
        public static void WriteLog(object instr)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filedirectory = currentDirectory + "bin\\Logs";
            if (!Directory.Exists(filedirectory))
            {
                Directory.CreateDirectory(filedirectory);
            }
            string filename = filedirectory + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            ManLog manLog = new ManLog(filename);
            if (instr is string)
            {
                manLog.WriteLog(new List<string> { DateTime.Now.ToString(), (string)instr });
            }
            else if (instr is List<string>)
            {
                manLog.WriteLog((List<string>)instr);
            }
            else if (instr is Exception)
            {
                manLog.WriteLog(new List<string> { (instr as Exception).Message, (instr as Exception).StackTrace });
            }
        }
    }

    enum EnumMode
    {
        Debug = 0,
        Demo = 1,
        Product = 2
    }
}
