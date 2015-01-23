using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using Owin;

namespace Hello_SelfHost_WebAPI.Start
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var listener = (HttpListener) appBuilder.Properties["System.Net.HttpListener"];
            listener.AuthenticationSchemes = AuthenticationSchemes.Basic;

            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Filters.Add(new AuthorizeAttribute());
            appBuilder.UseWebApi(config);
        }
    }
}