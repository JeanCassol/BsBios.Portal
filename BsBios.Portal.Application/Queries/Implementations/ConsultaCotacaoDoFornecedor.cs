using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaCotacaoDoFornecedor : IConsultaCotacaoDoFornecedor
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ConsultaCotacaoDoFornecedor(IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
        }

        public CotacaoCadastroVm ConsultarCotacao(int idProcessoCotacao, string codigoFornecedor)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao)
                               .FiltraPorFornecedor(codigoFornecedor);

            var vm = (from p in _processosDeCotacao.GetQuery()
                    from pf in p.FornecedoresParticipantes
                    select new 
                        {
                            IdProcessoCotacao = p.Id,
                            CodigoFornecedor = pf.Fornecedor.Codigo,
                            Status = p.Status,
                            DescricaoDoProcessoDeCotacao = "", //falta implementar. Acho que a descrição tem que estar no processo e não na requisição de compra 
                            DataLimiteDeRetorno = p.DataLimiteDeRetorno.ToString(),
                            Material = p.Produto.Descricao,
                            Quantidade = p.Quantidade,
                            UnidadeDeMedida = "UND",
                            CodigoCondicaoPagamento = pf.Cotacao.CondicaoDePagamento.Codigo,
                            CodigoIncoterm =  pf.Cotacao.Incoterm.Codigo,
                            DescricaoIncoterm = pf.Cotacao.DescricaoIncoterm,
                            Mva = pf.Cotacao.Mva,
                            ValorTotalSemImpostos = pf.Cotacao.ValorTotalSemImpostos,
                            ValorTotalComImpostos = pf.Cotacao.ValorTotalComImpostos
                        }).Single();

            return new CotacaoCadastroVm()
                {
                    IdProcessoCotacao = vm.IdProcessoCotacao,
                    CodigoFornecedor = vm.CodigoFornecedor,
                    Status = vm.Status.Descricao(),
                    DescricaoDoProcessoDeCotacao = "", //falta implementar. Acho que a descrição tem que estar no processo e não na requisição de compra 
                    DataLimiteDeRetorno = vm.DataLimiteDeRetorno,
                    Material = vm.Material,
                    Quantidade = vm.Quantidade,
                    UnidadeDeMedida = vm.UnidadeDeMedida,
                    CodigoCondicaoPagamento = vm.CodigoCondicaoPagamento,
                    CodigoIncoterm = vm.CodigoIncoterm,
                    DescricaoIncoterm = vm.DescricaoIncoterm,
                    Mva = vm.Mva,
                    ValorTotalSemImpostos = vm.ValorTotalSemImpostos,
                    ValorTotalComImpostos = vm.ValorTotalComImpostos

                };
        }
    }
}