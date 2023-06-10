using EnergyBotUmg.Entidades;
using System.Threading.Tasks;

namespace EnergyBotUmg.Servicio
{
    public interface IServicioConsulta
    {
        Task<ClienteSaldo> ClienteSaldoContador(string Contador);
        Task<ClienteSaldo> ClienteSaldoCorrelativo(string Correlativo);
        Task<LogTransaccion> inslogTransaccion(LogTransaccion logTransaccion);
    }
}
