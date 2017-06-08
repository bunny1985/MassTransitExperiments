using Owin;
using RxTest.Infrastructure.Cors;
using Swashbuckle.Application;
using System;
using System.Web;
using System.Web.Http;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace RxTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            appBuilder.Use(new Func<AppFunc, AppFunc>(next => (async env =>
            {
                var context = HttpContext.Current;
                if (context.Request.Cookies["SessionId"] == null)
                {
                    context.Response.AppendCookie(new HttpCookie("SessionId", Guid.NewGuid().ToString()) { HttpOnly = false });
                    //context.Response.Cookies.Append("SessionId", Guid.NewGuid().ToString(), new CookieOptions() { HttpOnly = false });
                }
                await next.Invoke(env);
            })));
            var corsPolicyProvider = new CorsPolicyProvider();

            httpConfiguration.EnableCors(corsPolicyProvider);
            WebApiConfig.Register(httpConfiguration);

            appBuilder.UseWebApi(httpConfiguration);
            httpConfiguration
                .EnableSwagger(c => c.SingleApiVersion("v1", "OSOM FUCKING APP "))
                .EnableSwaggerUi();
        }
    }
}