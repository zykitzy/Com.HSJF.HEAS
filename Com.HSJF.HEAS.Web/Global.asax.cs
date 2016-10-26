using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using Newtonsoft.Json;

namespace Com.HSJF.HEAS.Web
{
    public class Global : HttpApplication
    {
        public static Infrastructure.LogExtend.LogManagerExtend logger = new Infrastructure.LogExtend.LogManagerExtend();
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex.GetType().FullName != typeof(Com.HSJF.Infrastructure.LogExtend.LogException).FullName)
            {
                logger.WriteException("Application Error", ex);
                if (ex.GetBaseException() != null)
                {
                    logger.WriteException("Application Base Error", ex.GetBaseException());
                }
            }
        }
    }
}