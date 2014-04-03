using GestorDeConsumos.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GestorDeConsumos.Services.Views
{
    public class ResultadoConsumo
    {
        [JsonProperty(PropertyName = "Mensaje")]
        public String Mensaje { get; set; }

        [JsonProperty(PropertyName = "NombreUsuario")]
        public String Usuario { get; set; }
    }
}