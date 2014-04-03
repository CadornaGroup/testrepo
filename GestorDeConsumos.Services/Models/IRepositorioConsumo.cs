using GestorDeConsumos.Services.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDeConsumos.Services.Models
{
    public interface IRepositorioConsumo
    {
        ResultadoRegistracionConsumo Registrar(Consumo consumo);
    }
}