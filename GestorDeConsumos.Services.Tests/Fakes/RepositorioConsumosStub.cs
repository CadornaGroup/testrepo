using GestorDeConsumos.Services.Models;
using GestorDeConsumos.Services.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeConsumos.Services.Tests.Fakes
{
    class RepositorioConsumosStub : IRepositorioConsumo
    {
        private List<Consumo> consumosRegistrados = new List<Consumo>();
        private String causa;
        private String userName;

        public ResultadoRegistracionConsumo Registrar(Consumo consumo)
        {
            return Exec(consumo);
        }

        public ResultadoRegistracionConsumo VerificarDisponibilidadConsumo(Consumo consumo)
        {
            return Exec(consumo);
        }

        private ResultadoRegistracionConsumo Exec(Consumo consumo)
        {
            if (!String.IsNullOrWhiteSpace(causa))
            {
                throw new ExcepcionConsumo(causa, userName);
            }

            var nombreYApellidoUsuario = Datos.Usuarios[consumo.IdTarjetaAltaFrecuencia];
            if (nombreYApellidoUsuario != null)
            {
                consumosRegistrados.Add(consumo);

                return new ResultadoRegistracionConsumo(nombreYApellidoUsuario, "Consumo registrado con éxito");
            }

            throw new Exception("Tarjeta no valida");
        }


        internal void RetornarErrorConCausa(string causa, string userName)
        {
            this.causa = causa;
            this.userName = userName;
        }
    }
}
