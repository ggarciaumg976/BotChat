using Dapper;
using EnergyBotUmg.Entidades;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EnergyBotUmg.Servicio
{
    public class ServicioConsulta : IServicioConsulta
    {
        private readonly IConfiguration config;
        public ServicioConsulta(IConfiguration config)
        {
            this.config = config;
        }

        async Task<ClienteSaldo> IServicioConsulta.ClienteSaldoContador(string Contador)
        {
            using var cnx = new SqlConnection(config["ConnectionStrings:SQLEegsa"]);
            cnx.Open();
            return await cnx.QuerySingleOrDefaultAsync<ClienteSaldo>("dbo.SL_Consulta_Saldo_Contador", new { Contador }, commandType: CommandType.StoredProcedure);
        }

        async Task<ClienteSaldo> IServicioConsulta.ClienteSaldoCorrelativo(string Correlativo)
        {
            using var cnx = new SqlConnection(config["ConnectionStrings:SQLEegsa"]);
            cnx.Open();
            return await cnx.QuerySingleOrDefaultAsync<ClienteSaldo>("dbo.SL_Consulta_Saldo_Correlativo", new { Correlativo }, commandType: CommandType.StoredProcedure);
        }
        async Task<LogTransaccion> IServicioConsulta.inslogTransaccion(LogTransaccion logTransaccion)
        {
            using var cnx = new SqlConnection(config["ConnectionStrings:SQLEegsa"]);
            cnx.Open();
            return await cnx.QuerySingleOrDefaultAsync<LogTransaccion>("dbo.prc_transaccion_ins", new { logTransaccion.tipo, logTransaccion.idConsulta }, commandType: CommandType.StoredProcedure);
        }
    }
}
