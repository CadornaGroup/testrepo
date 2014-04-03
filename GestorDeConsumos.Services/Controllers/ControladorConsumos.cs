using GestorDeConsumos.Services.Models;
using GestorDeConsumos.Services.Properties;
using GestorDeConsumos.Services.Views;
using NLog;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestorDeConsumos.Services.Controllers
{
    public class ConsumosController : ApiController
    {
        private IRepositorioConsumo repositorioConsumos;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Validador validador;

        #region Constructores

        static ConsumosController()
        {
            validador = new Validador(LogManager.GetCurrentClassLogger());
            validador.Validar(consumo => consumo != null, Resources.InvalidParam_ConsumoNullOrEmpty)
                     .Validar(consumo => !string.IsNullOrWhiteSpace(consumo.IMEI), Resources.InvalidParam_ImeiNoEspecificado)
                     .Validar(consumo => !string.IsNullOrWhiteSpace(consumo.IdTarjetaAltaFrecuencia), Resources.InvalidParam_IdTarjetaNoIdentificado)
                     .Validar(consumo => !string.IsNullOrWhiteSpace(consumo.TipoConsumo), Resources.InvalidParam_TipoConsumoNoEspecificado)
                     .Validar(consumo => TipoConsumoValido(consumo), Resources.InvalidParam_TipoConsumoInvalido);
        }

        #endregion

        public ConsumosController()
        {
            this.repositorioConsumos = new RepositorioConsumoStoredProc();
        }

        public ConsumosController(IRepositorioConsumo repositorioConsumos)
        {
            this.repositorioConsumos = repositorioConsumos;
        }

        [HttpGet]
        public HttpResponseMessage Ping()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public ResultadoConsumo RegistrarConsumo([FromBody]Consumo consumo)
        {
            logger.Debug(Resources.Log_InicioRegistroConsumo, consumo);

            if (!validador.Validar(consumo))
            {
                ThrowBadRequestException(Resources.Error_ParametrosInvalidos);
            }

            ResultadoRegistracionConsumo resultado = null;
            try
            {
                resultado = this.repositorioConsumos.Registrar(consumo);
                logger.Debug(Resources.Log_ConsumoRegistrado, consumo);
            }
            catch (ExcepcionConsumo exRegistraction)
            {
                logger.Info(Resources.Log_BadRequest, consumo, exRegistraction.Message);
                ThrowBadRequestException(exRegistraction.Message, exRegistraction.UserName);
            }
            catch (Exception ex)
            {
                logger.Error(Resources.Log_ErrorDesconocido, consumo, ex);
                ThrowInternalServerException(Resources.Error_ErrorDesconocido);
            }

            return new ResultadoConsumo()
            {
                Usuario = resultado.NombreUsuario,
                Mensaje = resultado.Mensaje
            };
        }

        private static bool TipoConsumoValido(Consumo consumo)
        {
            if (consumo.TipoConsumo.Equals(Resources.TipoConsumo_Desayuno) ||
                consumo.TipoConsumo.Equals(Resources.TipoConsumo_Almuerzo))
            {
                return consumo.Importe == 0;
            }
            else if (consumo.TipoConsumo.Equals(Resources.TipoConsumo_CuentaCorriente))
            {
                return consumo.Importe > 0;
            }
            else
            {
                return false;
            }
        }

        private void ThrowBadRequestException(String causa)
        {
            ThrowBadRequestException(causa, null);
        }

        private void ThrowBadRequestException(String causa, String nombreUsuario)
        {
            var resp = Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorRegistracion(causa, nombreUsuario));
            resp.ReasonPhrase = causa;

            throw new HttpResponseException(resp);
        }

        private void ThrowInternalServerException(String causa)
        {
            var resp = Request.CreateResponse(HttpStatusCode.InternalServerError, new ErrorRegistracion(causa));
            resp.ReasonPhrase = causa;

            throw new HttpResponseException(resp);
        }

        #region Clases utiles

        class Validacion
        {
            private Func<Consumo, bool> funcionValidacion;

            public Validacion(Func<Consumo, bool> funcionValidacion, String message)
            {
                this.funcionValidacion = funcionValidacion;
                this.Message = message;
            }

            public bool Validar(Consumo consumo)
            {
                return this.funcionValidacion(consumo);
            }

            public String Message { get; private set; }
        }

        class Validador
        {
            private ICollection<Validacion> validadores;
            private Logger logger;

            public Validador(Logger logger)
            {
                this.logger = logger;
                validadores = new List<Validacion>();
            }

            public Validador Validar(Func<Consumo, Boolean> funcionValidacion, String mensajeValidacionFallida)
            {
                validadores.Add(new Validacion(funcionValidacion, mensajeValidacionFallida));
                return this;
            }

            public bool Validar(Consumo consumo)
            {
                foreach (var validador in validadores)
                {
                    if (!validador.Validar(consumo))
                    {
                        logger.Info(Resources.Log_ParametrosInvalidos, validador.Message);
                        return false;
                    }
                }

                return true;
            }
        }

        #endregion
    }
}