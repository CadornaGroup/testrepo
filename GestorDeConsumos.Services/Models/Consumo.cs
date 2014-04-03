using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace GestorDeConsumos.Services.Models
{
    public class Consumo
    {
        [JsonProperty(PropertyName = "IMEI")]
        public String IMEI { get; set; }

        [JsonProperty(PropertyName = "Tarjeta")]
        public String IdTarjetaAltaFrecuencia { get; set; }

        [JsonProperty(PropertyName = "TipoDeConsumo")]
        public String TipoConsumo { get; set; }

        [JsonProperty(PropertyName = "ImporteCuentaCorriente")]
        public double Importe { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("IMEI: ");
            builder.Append(this.IMEI);
            builder.Append(", ID Tarjeta: ");
            builder.Append(this.IdTarjetaAltaFrecuencia);
            builder.Append(", Tipo: ");
            builder.Append(this.TipoConsumo);
            builder.Append(", Importe: ");
            builder.Append(this.Importe);

            return builder.ToString();
        }
    }
}