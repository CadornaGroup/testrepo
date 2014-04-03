using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GestorDeConsumos.Services.Models
{
    public class ResultadoRegistracionConsumo
    {
        public String NombreUsuario { get; set; }
        public String Mensaje { get; set; }

        public ResultadoRegistracionConsumo(String nombreUsuario, String mensaje)
        {
            this.NombreUsuario = nombreUsuario;
            this.Mensaje = mensaje;
        }
    }
}
