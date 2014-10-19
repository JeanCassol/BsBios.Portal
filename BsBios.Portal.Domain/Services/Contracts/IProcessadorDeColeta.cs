using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessadorDeColeta
    {
        OrdemDeTransporte Processar(ConhecimentoDeTransporte conhecimentoDeTransporte);
    }
}