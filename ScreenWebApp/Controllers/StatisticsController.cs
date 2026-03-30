using ScreenWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ScreenWebApp.Models;
//using System.Web.Mvc;

namespace ScreenWebApp.Controllers
{
    public class StatisticsController : ApiController
    {
        // GET: Statistics
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET api/statistics/sales
        [HttpGet]
        [Route("api/statistics/sales")]
        public IHttpActionResult GetSalesData()
        {
            // 模拟从数据库查询的数据
            var data = new SalesData
            {
                Categories = new List<string> { "1月", "2月", "3月", "4月", "5月", "6月" },
                Amounts = new List<decimal> { 1200, 1350, 1480, 1700, 2100, 2560 }
            };
            return Ok(data);
        }

        // GET api/statistics/pie
        [HttpGet]
        [Route("api/statistics/pie")]
        public IHttpActionResult GetPieData()
        {
            var data = new List<PieData>
        {
            new PieData { Name = "产品A", Value = 335 },
            new PieData { Name = "产品B", Value = 310 },
            new PieData { Name = "产品C", Value = 234 },
            new PieData { Name = "产品D", Value = 135 },
            new PieData { Name = "产品E", Value = 98 }
        };
            return Ok(data);
        }
    }
}