using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenWebApp.Conmmon
{
    public class ManLog
    {
        #region 定义
        /// <summary>
        /// 抽象类
        /// </summary>
        private TextWriter output;
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public ManLog()
        {
            newLog();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strFileName">文件名</param>
        public ManLog(string strFileName)
        {
            this.strFileName = strFileName;
            newLog();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设置/读取文件名
        /// </summary>
        private string strFileName = ".\\log.txt";
        public string FileName
        {
            get
            {
                return strFileName;
            }
            set
            {
                strFileName = value;
            }
        }
        #endregion

        #region 自定义的方法
        /// <summary>
        /// 新建日志
        /// </summary>
        private void newLog()
        {
            //if (!System.IO.File.Exists(strFileName))
            //{
            //    System.IO.File.CreateText(strFileName);
            //}
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="str">内容</param>
        public void WriteLog(List<string> str)
        {
            try
            {
                //output = System.IO.File.AppendText(strFileName);
                //output.WriteLine(DateTime.Now + "\n" + str);
                //output.Flush();
                //output.Close();
                //System.IO.File.AppendAllLines(strFileName, str, Encoding.Default);
                File.AppendAllLines(strFileName, str);
                //FileStream _file = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite);
                //using (StreamWriter writer1 = new StreamWriter(_file))
                //{
                //    writer1.WriteLine(DateTime.Now + "\n" + str);
                //    writer1.Flush();
                //    writer1.Close();
                //    _file.Close();
                //}
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}
