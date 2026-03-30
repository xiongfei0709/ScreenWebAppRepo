using ScreenWebApp.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using ScreenWebApp.Models;

namespace ScreenWebApp.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        /// <summary>
        /// 获取顶部信息栏数据
        /// GET api/dashboard/info
        /// </summary>
        [HttpGet]
        [Route("info")]
        public IHttpActionResult GetInfo()
        {
            // 模拟数据，实际应从数据库查询
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
            var beds = new BedStatusData
            {
                RescueBeds = GetMockRescueBeds(),
                ObservationBeds = GetMockObservationBeds()
            };
            return Ok(beds);
        }

        #region 模拟数据生成（实际应替换为真实业务逻辑）

        private TrendData GetMockTrendData(string type, string period)
        {
            // 这里用随机生成示例，实际可返回固定模拟数据
            var rand = new Random();
            var trend = new TrendData();

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
            return trend;
        }

        private List<BedInfo> GetMockRescueBeds()
        {
            // 抢救室床位名称列表（可从前端原有列表复制）
            var names = new List<string>
            {
                "复苏1", "复苏2", "复苏1+", "复苏2+", "抢1床", "抢2床", "1-1", "1-2", "1-3",
                "2-4", "2-5", "2-6", "抢3床", "抢4床", "抢5床", "抢6床", "抢7床", "抢8床",
                "3-7", "3-8", "3-9", "4-10", "4-11", "4-12", "抢9床", "抢10床", "抢11床", "抢12床",
                "抢13床", "抢14床", "5-13", "5-14", "5-15"
            };
            var rand = new Random();
            var beds = new List<BedInfo>();
            foreach (var name in names)
            {
                beds.Add(new BedInfo { Name = name, Level = rand.Next(1, 6) });
            }
            return beds;
        }

        private List<BedInfo> GetMockObservationBeds()
        {
            // 留观室床位名称列表
            var names = new List<string>
            {
                "留观1", "留观2", "留观3", "留观4", "留观5", "留观6", "留观7", "留观8",
                "留观9", "留观10", "留观11", "留观12", "留观13", "留观14", "留观15", "留观16"
            };
            var rand = new Random();
            var beds = new List<BedInfo>();
            foreach (var name in names)
            {
                beds.Add(new BedInfo { Name = name, Level = rand.Next(1, 6) });
            }
            return beds;
        }

        #endregion

        /// <summary>
        /// 获取底部温馨提示
        /// GET api/dashboard/notice
        /// </summary>
        [HttpGet]
        [Route("notice")]
        public IHttpActionResult GetNotice()
        {
            // 模拟数据，实际应从数据库或配置中获取
            var notice = new NoticeData
            {
                Message = "🏥 温馨提示：请保持急救通道畅通，危重患者优先就诊。如需帮助请联系急诊分诊台。感谢您的配合！"
            };
            return Ok(notice);
        }
    }
}