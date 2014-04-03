using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace GestorDeConsumos.SelfHost.Dispatching
{
    internal class ServicesAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var servicesAssembly = typeof(GestorDeConsumos.Services.Controllers.ConsumosController).Assembly;
            return new List<Assembly>(new Assembly[] { servicesAssembly });
        }
    }
}
