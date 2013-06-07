using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacao:ICompleteRepository<ProcessoDeCotacao>
    {
        IProcessosDeCotacao BuscaPorId(int id);
        IProcessosDeCotacao FiltraPorFornecedor(string codigoFornecedor);
        IProcessosDeCotacao DesconsideraNaoIniciados();
        IProcessosDeCotacao FiltraPorTipo(Enumeradores.TipoDeCotacao tipoDeCotacao);
        IProcessosDeCotacao CodigoDoProdutoContendo(string codigo);
        IProcessosDeCotacao DescricaoDoProdutoContendo(string descricao);
        IProcessosDeCotacao FiltraPorStatus(Enumeradores.StatusProcessoCotacao status);
        IProcessosDeCotacao Fechado();
        IProcessosDeCotacao EfetuadosPeloComprador(string loginComprador);
        IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(string numeroDaRequisicao, string numeroDoItem);
    }
}
