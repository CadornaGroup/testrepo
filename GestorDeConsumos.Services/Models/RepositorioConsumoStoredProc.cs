using GestorDeConsumos.Services.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GestorDeConsumos.Services.Models
{
    public class RepositorioConsumoStoredProc : IRepositorioConsumo
    {
        public ResultadoRegistracionConsumo Registrar(Consumo consumo)
        {
            String desc = "";
            String user = "";
            using (var conexion = CrearConexion())
            {
                using (var command = CrearComandoRegistroConsumo(conexion, consumo))
                {
                    conexion.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            desc = GetFieldValue<String>(reader, "Mensaje");
                            user = GetFieldValue<String>(reader, "NombreUsuario");
                            if (GetFieldValue<String>(reader, "Resultado") != Resources.Resultado_OK)
                            {
                                throw new ExcepcionConsumo(desc, user);
                            }
                        }
                    }
                }
            }

            return new ResultadoRegistracionConsumo(user, desc); ;
        }

        private T GetFieldValue<T>(SqlDataReader reader, string fieldName)
        {
            return reader.GetFieldValue<T>(reader.GetOrdinal(fieldName));
        }

        private static SqlCommand CrearComandoRegistroConsumo(SqlConnection conexion, Consumo consumo)
        {
            var comando = new SqlCommand("[dbo].[RegistrarConsumo]", conexion);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("IMEI", SqlDbType.VarChar, 15).Value = consumo.IMEI;
            comando.Parameters.Add("Tarjeta", SqlDbType.VarChar, 14).Value = consumo.IdTarjetaAltaFrecuencia;
            comando.Parameters.Add("TipoDeConsumo", SqlDbType.VarChar, 2).Value = consumo.TipoConsumo;
            var importe = new SqlParameter("ImporteCuentaCorriente", SqlDbType.Decimal);
            importe.Scale = 2;
            importe.Precision = 10;
            importe.Value = consumo.Importe;
            comando.Parameters.Add(importe);

            return comando;
        }

        private SqlConnection CrearConexion()
        {
            return new SqlConnection(
                ConfigurationManager.ConnectionStrings["GestionDeConsumos"].ConnectionString);
        }
    }
}