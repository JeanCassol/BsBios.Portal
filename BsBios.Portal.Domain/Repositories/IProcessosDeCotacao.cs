using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IProcessosDeCotacao:ICompleteRepository<ProcessoDeCotacao>
    {
        IProcessosDeCotacao BuscaPorId(int id);
        IProcessosDeCotacao FiltraPorFornecedor(string codigoFornecedor);
        IProcessosDeCotacao NomeDoFornecedorContendo(string nomeDoFornecedor);
        IProcessosDeCotacao DesconsideraNaoIniciados();
        IProcessosDeCotacao FiltraPorTipo(Enumeradores.TipoDeCotacao tipoDeCotacao);
        IProcessosDeCotacao DoProduto(string codigoDoProduto);
        IProcessosDeCotacao CodigoDoProdutoContendo(string codigo);
        IProcessosDeCotacao DescricaoDoProdutoContendo(string descricao);
        IProcessosDeCotacao FiltraPorStatus(Enumeradores.StatusProcessoCotacao status);
        IProcessosDeCotacao DesconsideraCancelados();
        IProcessosDeCotacao SomenteComFornecedoresSelecionados();
        IProcessosDeCotacao SomenteComFornecedoresNaoSelecionados();
        IProcessosDeCotacao Fechado();
        IProcessosDeCotacao EfetuadosPeloComprador(string loginComprador);
        IProcessosDeCotacao FechadosAPartirDe(DateTime data);
        IProcessosDeCotacao FechadosAte(DateTime data);



    }
}
