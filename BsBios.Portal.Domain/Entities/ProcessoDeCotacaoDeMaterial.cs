using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoDeCotacaoDeMaterial : ProcessoDeCotacao
    {

        public virtual ProcessoDeCotacaoItem AdicionarItem(RequisicaoDeCompra requisicaoDeCompra)
        {
            AdicionarItem();
            if (requisicaoDeCompra.GerouProcessoDeCotacao)
            {
                throw new RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException(requisicaoDeCompra.Numero,
                    requisicaoDeCompra.NumeroItem);
            }

            if (requisicaoDeCompra.Status == Enumeradores.StatusRequisicaoCompra.Bloqueado)
            {
                throw new SelecionarRequisicaoDeCompraBloqueadaException(requisicaoDeCompra.Numero,
                    requisicaoDeCompra.NumeroItem);
            }

            var item = new ProcessoDeCotacaoDeMaterialItem(this, requisicaoDeCompra);
            Itens.Add(item);
            return item;
        }

        public new virtual void RemoverItem(ProcessoDeCotacaoItem item)
        {
            var itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item;
            itemMaterial.RequisicaoDeCompra.DesvincularDeProcessoDeCotacao();
            base.RemoverItem(item);
        }

        public virtual void Atualizar(DateTime dataLimiteDeRetorno, string requisitos)
        {
            if (Status == Enumeradores.StatusProcessoCotacao.Fechado)
            {
                throw new ProcessoDeCotacaoAtualizacaoDadosException(Status.Descricao());
            }

        }

        public virtual CotacaoMaterial InformarCotacao(string codigoFornecedor, CondicaoDePagamento condicaoDePagamento,
            Incoterm incoterm, string descricaoDoIncoterm)
        {
            base.InformarCotacao();
            //busca a cotação do fornecedor
            FornecedorParticipante fornecedorParticipante =
                FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);

            var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();

            if (cotacao == null)
            {
                cotacao = new CotacaoMaterial(condicaoDePagamento, incoterm, descricaoDoIncoterm);
                fornecedorParticipante.InformarCotacao(cotacao);
            }
            else
            {
                cotacao.Atualizar(condicaoDePagamento, incoterm, descricaoDoIncoterm);
            }

            return cotacao;
        }

        public virtual CotacaoItem InformarCotacaoDeItem(int idProcessoDeCotacaoItem, int idCotacao, decimal preco,
            decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            base.InformarCotacao();

            var cotacao = (CotacaoMaterial) FornecedoresParticipantes
                .Where(fp => fp.Cotacao != null && fp.Cotacao.Id == idCotacao)
                .Select(fp => fp.Cotacao).Single().CastEntity();

            ProcessoDeCotacaoItem processoDeCotacaoItem = Itens.Single(item => item.Id == idProcessoDeCotacaoItem);

            return cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, preco, mva, quantidadeDisponivel,
                prazoDeEntrega, observacoes);

        }

        public virtual void SelecionarCotacao(int idCotacao, int idProcessoCotacaoItem, decimal quantidadeAdquirida, Iva iva)
        {
            SelecionarCotacao();
            var cotacao = (CotacaoMaterial)BuscarPodId(idCotacao).CastEntity();
            var itemDaCotacao = (CotacaoMaterialItem)cotacao.Itens.First(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
            itemDaCotacao.Selecionar(quantidadeAdquirida, iva);
        }

        public virtual void RemoverSelecaoDaCotacao(int idCotacao, int idProcessoCotacaoItem, Iva iva)
        {
            RemoverSelecaoDaCotacao();
            var cotacao = (CotacaoMaterial)BuscarPodId(idCotacao).CastEntity();
            var itemDaCotacao = (CotacaoMaterialItem)cotacao.Itens.First(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
            itemDaCotacao.RemoverSelecao(iva);
        }

    }
}