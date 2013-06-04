using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaEficienciaNegociacao : IConsultaEficienciaNegociacao
    {
        private readonly IProcessosDeCotacaoDeMaterial _processosDeCotacaoDeMaterial;

        public ConsultaEficienciaNegociacao(IProcessosDeCotacaoDeMaterial processosDeCotacaoDeMaterial)
        {
            _processosDeCotacaoDeMaterial = processosDeCotacaoDeMaterial;
        }

        public IList<ProcessoDeCotacaoValoresVm> Consultar(RelatorioEficienciaNegociacaoFiltroVm filtro)
        {
            _processosDeCotacaoDeMaterial.Fechado();
            if (!string.IsNullOrEmpty(filtro.LoginComprador))
            {
                _processosDeCotacaoDeMaterial.EfetuadosPeloComprador(filtro.LoginComprador);
            }

            return (from pc in _processosDeCotacaoDeMaterial.GetQuery()
                     from processoItem in pc.Itens
                     from fp in pc.FornecedoresParticipantes
                     from cotacaoItem in fp.Cotacao.Itens
                     let processoItemMaterial = (ProcessoDeCotacaoDeMaterialItem) processoItem
                     where cotacaoItem.Selecionada
                     select new ProcessoDeCotacaoValoresVm
                         {
                             Login = pc.Comprador.Login ,
                             Nome = pc.Comprador.Nome,
                             CodigoDoProduto = cotacaoItem.ProcessoDeCotacaoItem.Produto.Codigo ,
                             DescricaoDoProduto = cotacaoItem.ProcessoDeCotacaoItem.Produto.Descricao,
                             NumeroDaRequisicao = processoItemMaterial.RequisicaoDeCompra.Numero,
                             NumeroDoItem = processoItemMaterial.RequisicaoDeCompra.NumeroItem,
                             PrecoInicial = cotacaoItem.PrecoInicial ,
                             PrecoDeFechamento = cotacaoItem.Preco   
                         }).ToList();


        }
    }
}