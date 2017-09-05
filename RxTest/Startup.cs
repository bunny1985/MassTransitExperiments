using Autofac;
using Autofac.Integration.WebApi;
using BusDomain.CommandHalders;
using BusDomain.Commnads;
using BusDomain.Events;
using EventFlow;
using EventFlow.Autofac.Extensions;
using EventFlow.Configuration;
using EventFlow.EventStores.Files;
using EventFlow.Extensions;
using EventFlow.Logs;
using EventFlow.Owin.Extensions;
using EventFlow.Owin.Middlewares;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using Owin;
using RxTest.Infrastructure.Cors;
using Swashbuckle.Application;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using WebSocketHandling;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace RxTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            //AUTOFAC
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterWebApiFilterProvider(httpConfiguration);
            //containerBuilder.RegisterType<CommandPublishMiddleware>();//.InstancePerRequest();
            containerBuilder.RegisterWebApiModelBinderProvider();
            containerBuilder.RegisterType<WebSocketMessageDispatcher>().As<IMessageDispatcher>().SingleInstance();

            //END AUTOFAC

            //EVENT FLOW
            var uri = new Uri("amqp://admin:admin@localhost/mainVHost");
            var container = EventFlowOptions.New.
                UseAutofacContainerBuilder(containerBuilder)
                .PublishToRabbitMq(RabbitMqConfiguration.With(uri))
                    .AddCommands(typeof(PublishMessageCommand))
                    .AddEvents(typeof(MessagePublishedEvent))
                    .AddCommandHandlers(typeof(PublishMessageCommandHandler))
                    //.AddOwinMetadataProviders()
                    .CreateContainer();

            //END OF EVENT FLOW

            //appBuilder.Use<DummyAuthenticationMiddleware>(new DummmyAuthOptions("dummy"));
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
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseWebApi(httpConfiguration);
            httpConfiguration
                .EnableSwagger(c => c.SingleApiVersion("v1", "OSOM FUCKING APP "))
                .EnableSwaggerUi();
        }
    }
}