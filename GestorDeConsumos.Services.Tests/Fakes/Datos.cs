using GestorDeConsumos.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorDeConsumos.Services.Tests.Fakes
{
    static class Datos
    {
        public static readonly String[] IMEIs = new String[] { 
            "352001234567890"
        };

        public static readonly Dictionary<string, string> Usuarios = new Dictionary<string, string>()
        {
            { "7c7e5007",  "Gentili, Eduardo" }
        };
    }

    class Consumos
    {
        public static Consumo Almuerzo()
        {
            return new Consumo()
            {
                TipoConsumo = "AL",
                IMEI = Datos.IMEIs.First(),
                IdTarjetaAltaFrecuencia = Datos.Usuarios.First().Key
            };
        }

        public static Consumo Desayuno()
        {
            return new Consumo()
            {
                TipoConsumo = "DS",
                IMEI = Datos.IMEIs.First(),
                IdTarjetaAltaFrecuencia = Datos.Usuarios.First().Key
            };
        }

        public static Consumo TipoInvalido()
        {
            return new Consumo()
            {
                TipoConsumo = "tipoInvalido",
                IMEI = Datos.IMEIs.First(),
                IdTarjetaAltaFrecuencia = Datos.Usuarios.First().Key
            };
        }

        public static Consumo CuentaCorriente()
        {
            return new Consumo()
            {
                TipoConsumo = "CC",
                IMEI = Datos.IMEIs.First(),
                IdTarjetaAltaFrecuencia = Datos.Usuarios.First().Key
            }.ConImporte(1);
        }
    }

    static class ExtensionesConsumos
    {
        public static Consumo ConIMEI(this Consumo consumo, String imei)
        {
            consumo.IMEI = imei;
            return consumo;
        }

        public static Consumo ConIdTarjeta(this Consumo consumo, String idTarjeta)
        {
            consumo.IdTarjetaAltaFrecuencia = idTarjeta;
            return consumo;
        }

        public static Consumo ConImporte(this Consumo consumo, double importe)
        {
            consumo.Importe = importe;
            return consumo;
        }

        public static Consumo ConTipoConsumo(this Consumo consumo, String tipoConsumo)
        {
            consumo.TipoConsumo = tipoConsumo;
            return consumo;
        }
    }

}
