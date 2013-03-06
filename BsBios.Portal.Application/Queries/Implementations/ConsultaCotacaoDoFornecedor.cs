using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
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
                    let pcm = (ProcessoDeCotacaoDeMaterial ) p
                    select new 
                        {
                            IdProcessoCotacao = pcm.Id,
                            CodigoFornecedor = pf.Fornecedor.Codigo,
                            Status = pcm.Status,
                            DescricaoDoProcessoDeCotacao = pcm.RequisicaoDeCompra.Descricao,  
                            DataLimiteDeRetorno = pcm.DataLimiteDeRetorno.Value.ToShortDateString(),
                            Material = pcm.Produto.Descricao,
                            Quantidade = pcm.Quantidade,
                            UnidadeDeMedida = pcm.RequisicaoDeCompra.UnidadeMedida,
                            CodigoCondicaoPagamento = pf.Cotacao != null ?  pf.Cotacao.CondicaoDePagamento.Codigo : null,
                            CodigoIncoterm =  pf.Cotacao != null ?  pf.Cotacao.Incoterm.Codigo : null,
                            DescricaoIncoterm = pf.Cotacao != null ?  pf.Cotacao.DescricaoIncoterm : null,
                            Mva = pf.Cotacao != null ?  pf.Cotacao.Mva : null,
                            ValorTotalSemImpostos = pf.Cotacao != null ?  pf.Cotacao.ValorTotalSemImpostos : (decimal?) null,
                            ValorTotalComImpostos = pf.Cotacao != null ? pf.Cotacao.ValorTotalComImpostos : null
                        }).Single();

            return new CotacaoCadastroVm()
                {
                    IdProcessoCotacao = vm.IdProcessoCotacao,
                    CodigoFornecedor = vm.CodigoFornecedor,
                    Status = vm.Status.Descricao(),
                    DescricaoDoProcessoDeCotacao = vm.DescricaoDoProcessoDeCotacao, //falta implementar. Acho que a descrição tem que estar no processo e não na requisição de compra 
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