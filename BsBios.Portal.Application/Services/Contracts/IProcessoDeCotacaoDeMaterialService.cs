using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeMaterialService
    {
        void AtualizarProcesso(ProcessoDeCotacaoAtualizarVm atualizacaoDoProcessoDeCotacaoVm);
        VerificacaoDeQuantidadeAdquiridaVm VerificarQuantidadeAdquirida(int idProcessoCotacao, int idItem, decimal quantidadeTotalAdquirida);
    }

}