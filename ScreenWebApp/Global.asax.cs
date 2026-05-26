using LicenseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ScreenWebApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 可选：启动时验证一次，无效则阻止应用启动
            if (!LicenseValidator_BS.IsValid())
            {
                throw new ApplicationException("软件授权无效，请联系供应商。");
            }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // 每个请求都验证（有缓存，性能影响很小）
            if (!LicenseValidator_BS.IsValid())
            {
                var response = HttpContext.Current.Response;
                response.StatusCode = 403;
                response.ContentType = "text/plain";
                response.Write("授权验证失败：许可证无效、过期或与本服务器不匹配。");
                response.End();
            }
        }
    }
}
