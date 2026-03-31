using ScreenWebApp.Conmmon;
using ScreenWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Xml.Linq;

namespace ScreenWebApp.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        DbContext _dbContext = null;
        //当前时间
        private DateTime _currentDt;

        public DateTime CurrentDt
        {
            get { return _currentDt; }
            set { _currentDt = value; }
        }

        //运行环境 Demo或Normal
        private string _mode;
        EnumMode _enumMode;

        /// <summary>
        /// init
        /// </summary>
        private int Init()
        {
            _mode = Function.Setting["AppSettings:Mode"];
            if (_mode.Equals("Product"))
            {
                _enumMode = EnumMode.Product;
            }
            else if (_mode.Equals("Demo"))
            {
                _enumMode = EnumMode.Demo;
            }

            if (_dbContext == null)
            {
                //_dbContext = new DbContext();
                //var obj = DbContext.GetSingle("select sysdate from dual");
                //_currentDt = (DateTime)obj;
            }

            return 0;
        }


        /// <summary>
        /// 获取顶部信息栏数据
        /// GET api/dashboard/info
        /// </summary>
        [HttpGet]
        [Route("info")]
        public IHttpActionResult GetInfo()
        {
            Init();
            if (_enumMode == EnumMode.Demo)
            {
                var info = new DashboardInfo
                {
                    Saturation = 75,
                    SystemStatus = "正常",
                    TodayVisits = 120,
                    RetentionCount = 15,
                    GreenPassCount = 5
                };
                return Ok(info);
            }
            else {
                string systemStatus = "正常";
                int saturation = 0;
                int todayVisits = 0;
                int retentionCount = 0;
                int greenPassCount = 0;

                try
                {
                    if (_dbContext == null)
                    {
                        //_dbContext = new DbContext();
                    }
                    saturation = Convert.ToInt32(DbContext.GetSingle(Function.Setting["AppSettings:头部信息:急诊饱和度"]));                    
                    todayVisits = Convert.ToInt32(DbContext.GetSingle(Function.Setting["AppSettings:头部信息:今日就诊人数"]));
                    retentionCount = Convert.ToInt32(DbContext.GetSingle(Function.Setting["AppSettings:头部信息:留抢人数"]));
                    greenPassCount = Convert.ToInt32(DbContext.GetSingle(Function.Setting["AppSettings:头部信息:绿通人数"]));
                }
                catch (Exception e)
                {
                    systemStatus = "警告";
                    Function.WriteLog(e);
                }
                var info = new DashboardInfo
                {
                    Saturation = saturation,
                    SystemStatus = systemStatus,
                    TodayVisits = todayVisits,
                    RetentionCount = retentionCount,
                    GreenPassCount = greenPassCount
                };
                return Ok(info);

            }
        }

        /// <summary>
        /// 获取趋势图数据
        /// GET api/dashboard/trend?type=triage&period=day
        /// </summary>
        /// <param name="type">triage|registration|visit|rescue</param>
        /// <param name="period">day|week|month|year</param>
        [HttpGet]
        [Route("trend")]
        public IHttpActionResult GetTrend(string type, string period)
        {
            Init();
            // 模拟数据，实际应根据 type 和 period 从数据库或业务逻辑获取
            var trend = GetMockTrendData(type, period);
            if (trend == null)
                return BadRequest("无效的类型或周期");

            return Ok(trend);
        }

        /// <summary>
        /// 获取床位状态数据
        /// GET api/dashboard/beds
        /// </summary>
        [HttpGet]
        [Route("beds")]
        public IHttpActionResult GetBeds()
        {
            Init();
            var beds = new BedStatusData
            {
                RescueBeds = GetMockRescueBeds(),
                ObservationBeds = GetMockObservationBeds()
            };
            return Ok(beds);
        }

        /// <summary>
        /// 获取底部温馨提示
        /// GET api/dashboard/notice
        /// </summary>
        [HttpGet]
        [Route("notice")]
        public IHttpActionResult GetNotice()
        {
            Init();
            if (_enumMode == EnumMode.Demo)
            {
                // 模拟数据，实际应从数据库或配置中获取
                var notice = new NoticeData
                {
                    Message = "🏥 温馨提示：请保持急救通道畅通，危重患者优先就诊。如需帮助请联系急诊分诊台。感谢您的配合！"
                };
                return Ok(notice);
            }
            else
            {
                // 模拟数据，实际应从数据库或配置中获取
                var notice = new NoticeData
                {
                    Message = Function.Setting["AppSettings:温馨提示:内容"]
                };
                return Ok(notice);
            }
        }

        #region 模拟数据生成（实际应替换为真实业务逻辑）

        private TrendData GetMockTrendData(string type, string period)
        {
            // 这里用随机生成示例，实际可返回固定模拟数据
            var rand = new Random();
            var trend = new TrendData();
            if (_enumMode == EnumMode.Demo)
            {
                switch (period)
                {
                    case "day":
                        trend.Categories = new List<string> { "00", "02", "04", "06", "08", "10", "12", "14", "16", "18", "20", "22" };
                        trend.Values = new List<decimal>();
                        for (int i = 0; i < 12; i++) trend.Values.Add(rand.Next(10, 50));
                        break;
                    case "week":
                        trend.Categories = new List<string> { "周一", "周二", "周三", "周四", "周五", "周六", "周日" };
                        trend.Values = new List<decimal>();
                        for (int i = 0; i < 7; i++) trend.Values.Add(rand.Next(100, 200));
                        break;
                    case "month":
                        trend.Categories = new List<string> { "第1周", "第2周", "第3周", "第4周" };
                        trend.Values = new List<decimal>();
                        for (int i = 0; i < 4; i++) trend.Values.Add(rand.Next(3000, 4500));
                        break;
                    case "year":
                        trend.Categories = new List<string> { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
                        trend.Values = new List<decimal>();
                        for (int i = 0; i < 12; i++) trend.Values.Add(rand.Next(40000, 55000));
                        break;
                    default:
                        return null;
                }
            }
            else
            {
                string sql = "";
                try
                {
                    switch (type)
                    {
                        case "triage":
                            // 根据 period 从数据库查询分诊趋势数据
                            switch (period)
                            {
                                case "day":
                                    sql = Function.Setting["AppSettings:急诊分诊人次:日"];                                   
                                    break;
                                case "week":
                                    sql = Function.Setting["AppSettings:急诊分诊人次:周"];
                                    break;
                                case "month":
                                    sql = Function.Setting["AppSettings:急诊分诊人次:月"];
                                    break;
                                case "year":
                                    sql = Function.Setting["AppSettings:急诊分诊人次:年"];
                                    break;
                                default:
                                    return null;
                            }                            
                            break;
                        case  "registration":
                            switch (period)
                            {
                                case "day":
                                    sql = Function.Setting["AppSettings:急诊挂号人次:日"];
                                    break;
                                case "week":
                                    sql = Function.Setting["AppSettings:急诊挂号人次:周"];
                                    break;
                                case "month":
                                    sql = Function.Setting["AppSettings:急诊挂号人次:月"];
                                    break;
                                case "year":
                                    sql = Function.Setting["AppSettings:急诊挂号人次:年"];
                                    break;
                                default:
                                    return null;
                            }
                            break;
                        case "visit":
                            switch (period)
                            {
                                case "day":
                                    sql = Function.Setting["AppSettings:急诊就诊人次:日"];
                                    break;
                                case "week":
                                    sql = Function.Setting["AppSettings:急诊就诊人次:周"];
                                    break;
                                case "month":
                                    sql = Function.Setting["AppSettings:急诊就诊人次:月"];
                                    break;
                                case "year":
                                    sql = Function.Setting["AppSettings:急诊就诊人次:年"];
                                    break;
                                default:
                                    return null;
                            }
                            break;
                        case "rescue":
                            switch (period)
                            {
                                case "day":
                                    sql = Function.Setting["AppSettings:急诊绿通三无人次:日"];
                                    break;
                                case "week":
                                    sql = Function.Setting["AppSettings:急诊绿通三无人次:周"];
                                    break;
                                case "month":
                                    sql = Function.Setting["AppSettings:急诊绿通三无人次:月"];
                                    break;
                                case "year":
                                    sql = Function.Setting["AppSettings:急诊绿通三无人次:年"];
                                    break;
                                default:
                                    return null;
                            }
                            break;
                        default:
                            return null;
                    }

                    List<string> x = new List<string>();
                    List<decimal> y = new List<decimal>();
                    DataSet ds = DbContext.Query(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            x.Add(ds.Tables[0].Rows[i][0].ToString());
                            y.Add(Convert.ToDecimal(ds.Tables[0].Rows[i][1]));
                        }
                    }
                    trend.Categories = x;
                    trend.Values = y;
                }
                catch (Exception e)
                {
                    //systemStatus = "警告";
                    Function.WriteLog(e);
                }
            }

            return trend;
            
        }

        private List<BedInfo> GetMockRescueBeds()
        {
            var rand = new Random();
            var beds = new List<BedInfo>();
            if (_enumMode == EnumMode.Demo)
            {
                // 抢救室床位名称列表（可从前端原有列表复制）
                var names = new List<string>
                {
                    "复苏1", "复苏2", "复苏1+", "复苏2+", "抢1床", "抢2床", "1-1", "1-2", "1-3",
                    "2-4", "2-5", "2-6", "抢3床", "抢4床", "抢5床", "抢6床", "抢7床", "抢8床",
                    "3-7", "3-8", "3-9", "4-10", "4-11", "4-12", "抢9床", "抢10床", "抢11床", "抢12床",
                    "抢13床", "抢14床", "5-13", "5-14", "5-15"
                };
                
                foreach (var name in names)
                {
                    beds.Add(new BedInfo { Name = name, Level = rand.Next(1, 6) });
                }
                
            }
            else
            {
                try
                {
                    string sql = Function.Setting["AppSettings:急诊抢救室床位状态:数据源"];
                    DataSet ds = DbContext.Query(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string name = ds.Tables[0].Rows[i][0].ToString();
                            int level = Convert.ToInt32(ds.Tables[0].Rows[i][1]);
                            if (level < 1 || level > 5)
                            {
                                //systemStatus = "警告";
                                level = 5;
                                Function.WriteLog($"床位：‘{name}’存在不可识别的状态‘{level}’");
                            }
                            beds.Add(new BedInfo { Name = name, Level = level });
                        }
                    }
                }
                catch (Exception e)
                {
                    //systemStatus = "警告";
                    Function.WriteLog(e);
                }
            }
            return beds;
        }

        private List<BedInfo> GetMockObservationBeds()
        {
            var rand = new Random();
            var beds = new List<BedInfo>();
            if (_enumMode == EnumMode.Demo)
            {
                // 留观室床位名称列表
                    var names = new List<string>
                {
                    "留观1", "留观2", "留观3", "留观4", "留观5", "留观6", "留观7", "留观8",
                    "留观9", "留观10", "留观11", "留观12", "留观13", "留观14", "留观15", "留观16"
                };
                
                foreach (var name in names)
                {
                    //beds.Add(new BedInfo { Name = name, Level = rand.Next(1, 6) });
                    int level = rand.Next(1, 6);
                    if (level < 1 || level > 5)
                    {
                        //systemStatus = "警告";
                        level = 5;
                        Function.WriteLog($"床位：‘{name}’存在不可识别的状态‘{level}’");
                    }
                    beds.Add(new BedInfo { Name = name, Level = level });
                }                
            }
            else
            {
                try
                {
                    string sql = Function.Setting["AppSettings:急诊留观室床位状态:数据源"];
                    DataSet ds = DbContext.Query(sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string name = ds.Tables[0].Rows[i][0].ToString();
                            int level = Convert.ToInt32(ds.Tables[0].Rows[i][1]);
                            if (level < 1 || level > 5)
                            {
                                //systemStatus = "警告";
                                //level = 5;
                                Function.WriteLog($"床位：‘{name}’存在不可识别的状态‘{level}’");
                            }
                            beds.Add(new BedInfo { Name = name, Level = level });
                        }
                    }
                }
                catch (Exception e)
                {
                    //systemStatus = "警告";
                    Function.WriteLog(e);
                }
            }
            return beds;
        }

        #endregion

        
    }
}