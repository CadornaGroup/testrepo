using FluentAssertions;
using GestorDeConsumos.Services.Controllers;
using GestorDeConsumos.Services.Models;
using GestorDeConsumos.Services.Tests.Fakes;
using GestorDeConsumos.Services.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace GestorDeConsumos.Services.Tests.Controllers
{
    [TestClass]
    public class ConsumosControllerTest
    {
        private ConsumosController controladorConsumos;

        [TestInitialize]
        public void Setup()
        {
            controladorConsumos = new ConsumosController(new RepositorioConsumosStub());
            controladorConsumos.Request = new HttpRequestMessage();
            controladorConsumos.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        #region Tests

        [TestMethod]
        public void DadoConsumoVlido_CuandoSeRegistraConsumo_RetornaNombreDeUsuario()
        {
            var consumo = Consumos.Almuerzo();
            var resultado = controladorConsumos.RegistrarConsumo(consumo);

            resultado.Usuario.Should().Be(Datos.Usuarios[consumo.IdTarjetaAltaFrecuencia]);
            resultado.Mensaje.Should().Be("Consumo registrado con éxito");
        }

        [TestMethod]
        public void DadoClieteConConsumoAutorizado_CuandoSeRegistraConsumo_RetornaDatosConsumoRegistrado()
        {
            RegistrarConsumo(null)
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));

        }

        [TestMethod]
        public void DadoIMEINoProvisto_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConIMEI(null))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoIMEIVacio_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConIMEI(""))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoIdTarjetaNoProvisto_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConIdTarjeta(null))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoIdTarjetaVacio_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConIdTarjeta(""))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoTipoConsumoNoValido_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConTipoConsumo(null))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoDesayunoConImporte_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Desayuno().ConImporte(1))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoAlmuerzoConImporte_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.Almuerzo().ConImporte(1))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoConsumoInvalido_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.TipoInvalido())
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoConsumoACuentaSinImporte_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.CuentaCorriente().ConImporte(0))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        [TestMethod]
        public void DadoConsumoACuentaConImporteNegativo_CuandoSeRegistraConsumo_RetornaBadRequest()
        {
            RegistrarConsumo(Consumos.CuentaCorriente().ConImporte(-1))
                .ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "Los parámetros provistos no son válidos."));
        }

        #endregion

        [TestMethod]
        public void DadoRegistracionFalla_RetornaBadRequestConLaCausaDeLaFalla()
        {
            RepositorioConsumosStub repositorioConsumos = new RepositorioConsumosStub();
            ConsumosController controladorConsumos = new ConsumosController(repositorioConsumos);
            controladorConsumos.Request = new HttpRequestMessage();
            controladorConsumos.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            repositorioConsumos.RetornarErrorConCausa("descripcion del error", "nombre del usuario");

            Action registrarConsumo = () => controladorConsumos.RegistrarConsumo(Consumos.Almuerzo());
            registrarConsumo.ShouldThrow<HttpResponseException>()
                .Where(ex => EsBadRequest(ex, "descripcion del error", "nombre del usuario"));
        }

        #region Metodos utiles

        private Action RegistrarConsumo(Consumo consumo)
        {
            return () => controladorConsumos.RegistrarConsumo(consumo);
        }

        private bool EsBadRequest(HttpResponseException ex, String causa)
        {
            return ex.Response.StatusCode.Equals(HttpStatusCode.BadRequest) &&
                   ex.Response.ReasonPhrase.Equals(causa);
        }

        private bool EsBadRequest(HttpResponseException ex, String causa, String userNameEsperado)
        {
            return ex.Response.StatusCode.Equals(HttpStatusCode.BadRequest) &&
                   ex.Response.ReasonPhrase.Equals(causa) &&
                   ex.Response.Content.As<ObjectContent<ErrorRegistracion>>().Value.As<ErrorRegistracion>().NombreUsuario.Equals(userNameEsperado);
        }

        #endregion

    }
}
