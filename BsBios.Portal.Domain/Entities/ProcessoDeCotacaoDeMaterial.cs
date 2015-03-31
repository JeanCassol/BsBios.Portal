using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoDeCotacaoDeMaterial: ProcessoDeCotacao
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }

        protected ProcessoDeCotacaoDeMaterial()
        {}
        public ProcessoDeCotacaoDeMaterial(RequisicaoDeCompra requisicaoDeCompra)
            :base(requisicaoDeCompra.Material, requisicaoDeCompra.Quantidade, requisicaoDeCompra.UnidadeMedida)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
        }

        public virtual void Atualizar(DateTime dataLimiteDeRetorno, string requisitos)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAbertoAtualizacaoDadosException(Status.Descricao());
            }
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            Requisitos = requisitos;
        }

        public virtual CotacaoMaterial InformarCotacao(string codigoFornecedor, CondicaoDePagamento condicaoDePagamento,
            Incoterm incoterm, string descricaoDoIncoterm, decimal valorTotalComImpostos,
            decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            base.ValidarCotacao();
            //busca a cotação do fornecedor
            FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);

            var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();

            if (cotacao == null)
            {
                cotacao = new CotacaoMaterial(condicaoDePagamento, incoterm, descricaoDoIncoterm, valorTotalComImpostos, mva, quantidadeDisponivel, prazoDeEntrega, observacoes);
                fornecedorParticipante.InformarCotacao(cotacao);
            }
            else
            {
                cotacao.Atualizar(valorTotalComImpostos,condicaoDePagamento, incoterm, descricaoDoIncoterm, mva, quantidadeDisponivel, prazoDeEntrega, observacoes );
            }

            //return fornecedorParticipante.InformarCotacao(condicaoDePagamento, incoterm, descricaoDoIncoterm,
            //    valorTotalComImpostos, mva, quantidadeDisponivel, prazoDeEntrega, observacoes);
            return cotacao;
        }


        public virtual void SelecionarCotacao(int idCotacao, decimal quantidadeAdquirida, Iva iva)
        {
            ValidarSelecaoDeCotacao(idCotacao);
            var cotacao = (CotacaoMaterial) BuscarPodId(idCotacao).CastEntity();
            cotacao.Selecionar(quantidadeAdquirida, iva);
        }

        public virtual void RemoverSelecaoDaCotacao(int idCotacao, Iva iva)
        {
            ValidarRemocaoDeSelecaoDaCotacao();
            var cotacao = (CotacaoMaterial) BuscarPodId(idCotacao).CastEntity();
            cotacao.RemoverSelecao(iva);
        }

        public override void FecharProcesso()
        {
            Fechar();
        }
    }
}