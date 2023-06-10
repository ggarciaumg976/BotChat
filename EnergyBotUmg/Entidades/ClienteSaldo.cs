using Microsoft.VisualBasic;
using System;

namespace EnergyBotUmg.Entidades
{
    public class ClienteSaldo
    {
        public int idCliente { get; set; }
        public string nombreCliente { get; set; }
        public string contador { get; set; }
        public string correlativo { get; set; }
        public string fecha { get; set; }
        public string direccion { get; set; }
        public decimal Saldo { get; set; }  

    }
}
