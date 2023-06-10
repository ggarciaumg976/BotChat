using PagoEnLineaIA.Entidades;

namespace PagoEnLineaIA.Servicios
{
    public interface IReclamoServicio
    {
        Task<bool> InsertReclamos(Reclamos reclamos);

    }
}
