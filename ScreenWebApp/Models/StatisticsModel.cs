using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenWebApp.Models
{
    public class SalesData
    {
        public List<string> Categories { get; set; }   // 类别（如月份）
        public List<decimal> Amounts { get; set; }      // 销售额
    }

    public class PieData
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}