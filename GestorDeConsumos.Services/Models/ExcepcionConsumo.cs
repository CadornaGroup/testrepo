using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorDeConsumos.Services.Models
{
    public class ExcepcionConsumo : Exception
    {
        public String UserName { get; set; }

        public ExcepcionConsumo(string message, string userName)
            : base(message)
        {
            this.UserName = userName;
        }
    }
}