using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class ProcessoCotacaoFornecedorBuilder : Builder<FornecedorParticipante, ProcessoCotacaoFornecedorVm>
    {
        public override ProcessoCotacaoFornecedorVm BuildSingle(FornecedorParticipante model)
        {
            return new ProcessoCotacaoFornecedorVm()
            {
                Codigo = model.Fornecedor.Codigo,
                Nome = model.Fornecedor.Nome,
                ValorLiquido = model.Cotacao != null ? model.Cotacao.ValorLiquido : (decimal?) null,
                ValorComImpostos = model.Cotacao != null ? model.Cotacao.ValorComImpostos : (decimal?)null,
                Selecionado = (model.Cotacao != null && model.Cotacao.Selecionada ? "Sim" : "Não")
            };
        }

    }

}
