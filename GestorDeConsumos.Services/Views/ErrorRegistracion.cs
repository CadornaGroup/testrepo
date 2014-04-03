using Newtonsoft.Json;
using System;

namespace GestorDeConsumos.Services.Views
{
    public class ErrorRegistracion
    {
        [JsonProperty(PropertyName = "Mensaje")]
        public String Causa { get; set; }

        [JsonProperty(PropertyName = "NombreUsuario", NullValueHandling = NullValueHandling.Ignore)]
        public String NombreUsuario { get; set; }

        public ErrorRegistracion(String causa)
            : this(causa, null)
        {
        }

        public ErrorRegistracion(String causa, String nombreUsario)
        {
            this.Causa = causa;
            this.NombreUsuario = nombreUsario;
        }
    }
}