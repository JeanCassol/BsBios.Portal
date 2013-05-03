using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessoDeCotacaoFactory
    {
        void AdicionarItem(Produto material, UnidadeDeMedida unidadeDeMedida, decimal quantidade);
        ProcessoDeCotacaoDeMaterial CriarProcesso(RequisicaoDeCompra requisicaoDeCompra);
        ProcessoDeCotacaoDeFrete CriarProcesso();
    }
}