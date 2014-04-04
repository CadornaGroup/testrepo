using GestorDeConsumos.SelfHost.Dispatching;
using GestorDeConsumos.SelfHost.Properties;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;

namespace GestorDeConsumos.SelfHost
{
/* My test class
second line
*/
    public static class Program
    {
        static void Main()
        {
            using (var config = CrearConfiguracion(GetBaseAddress(), new ServicesAssemblyResolver()))
            {
                using (var host = CreateSelfHost(config))
                {
                    host.OpenAsync().Wait();

                    Console.WriteLine(Resources.ConfirmarCerrarHost);
                    Console.ReadLine();
                }
            }
        }

        private static string GetBaseAddress()
        {
            return ConfigurationManager.AppSettings["baseUri"];
        }

        private static HttpSelfHostServer CreateSelfHost(HttpSelfHostConfiguration config)
        {
            return new HttpSelfHostServer(config);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed by caller")]
        private static HttpSelfHostConfiguration CrearConfiguracion(String baseAddress, IAssembliesResolver assemblyResovler)
        {
            var config = new HttpSelfHostConfiguration(baseAddress);
            config.Services.Replace(typeof(IAssembliesResolver), assemblyResovler);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: ConfigurationManager.AppSettings["routeTemplate"],
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }
    }
}
