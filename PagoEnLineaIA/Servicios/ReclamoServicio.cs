using Dapper;
using PagoEnLineaIA.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace PagoEnLineaIA.Servicios
{
    public class ReclamoServicio : IReclamoServicio
    {
        private readonly IConfiguration config;
        public ReclamoServicio(IConfiguration config)
        {
            this.config = config;
        }

        async Task<bool> IReclamoServicio.InsertReclamos(Reclamos reclamos)
        {
            using var cnx = new SqlConnection(config["ConnectionStrings:SQLEegsa"]);
            cnx.Open();
            var sql = @"INSERT INTO [botEmpresaElectrica].[dbo].[reclamo]([idCliente], [contador], [correlativo], [comentario], [estadoReclamo])
                        VALUES(@idCliente, @contador, @correlativo, @comentario, @estadoReclamo)";

            var result = await cnx.ExecuteAsync(sql.ToString(),
                new { reclamos.idCliente, reclamos.contador, reclamos.correlativo,
                reclamos.comentario, reclamos.estadoReclamo});
            return result > 0;
        }
    }
}
