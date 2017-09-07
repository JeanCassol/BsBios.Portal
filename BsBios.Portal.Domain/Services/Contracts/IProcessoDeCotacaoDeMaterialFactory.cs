using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessoDeCotacaoDeMaterialFactory
    {
        void AdicionarRequisicaoDeCompra(RequisicaoDeCompra requisicaoDeCompra);
        ProcessoDeCotacaoDeMaterial CriarProcesso();
    }
}