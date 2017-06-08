using System;
using System.Web.Http.Filters;

namespace RxTest.Infrastructure.Cors
{
    public class AllowCors : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null)
            {
                throw new ArgumentNullException("actionExecutedContext");
            }
            else
            {
                actionExecutedContext.Response.Headers.Remove("Access-Control-Allow-Origin");

                if (actionExecutedContext.Request.Headers.Referrer != null)
                {
                    actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", actionExecutedContext.Request.Headers.Referrer.Scheme + "://" + actionExecutedContext.Request.Headers.Referrer.Authority);
                }
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}