using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenWebApp.Models
{
    /// <summary>
    /// 顶部信息栏数据
    /// </summary>
    public class DashboardInfo
    {
        public int Saturation { get; set; }      // 急诊饱和度百分比
        public string SystemStatus { get; set; } // 系统状态：正常/警告/异常
        public int TodayVisits { get; set; }     // 今日就诊人数
        public int RetentionCount { get; set; }  // 留抢人数
        public int GreenPassCount { get; set; }  // 绿通人数
    }

    /// <summary>
    /// 趋势图数据
    /// </summary>
    public class TrendData
    {
        public List<string> Categories { get; set; } // X轴标签
        public List<decimal> Values { get; set; }    // Y轴数值
    }

    /// <summary>
    /// 床位信息
    /// </summary>
    public class BedInfo
    {
        public string Name { get; set; }
        public int Level { get; set; } // 1-5级
    }

    /// <summary>
    /// 床位状态集合
    /// </summary>
    public class BedStatusData
    {
        public List<BedInfo> RescueBeds { get; set; }   // 抢救室床位
        public List<BedInfo> ObservationBeds { get; set; } // 留观室床位
    }

    /// <summary>
    /// 温馨提示文本
    /// </summary>
    public class NoticeData
    {
        public string Message { get; set; }
    }
}