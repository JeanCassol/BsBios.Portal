using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacao : IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual Enumeradores.StatusProcessoCotacao Status { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual IList<FornecedorParticipante> FornecedoresParticipantes { get; protected set; }
        //public virtual IList<Cotacao> Cotacoes { get; protected set; }

        protected ProcessoDeCotacao()
        {
            FornecedoresParticipantes = new List<FornecedorParticipante>();
            //Cotacoes = new List<Cotacao>();
            Status = Enumeradores.StatusProcessoCotacao.NaoIniciado;
        }

        protected ProcessoDeCotacao(Produto produto, decimal quantidade):this()
        {
            Produto = produto;
            Quantidade = quantidade;
        }

        public virtual void Atualizar(DateTime dataLimiteDeRetorno)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoIniciadoAtualizacaoDadosException(Status.Descricao());
            }
            DataLimiteDeRetorno = dataLimiteDeRetorno;
        }


        public virtual void AdicionarFornecedor(Fornecedor fornecedor)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw  new ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException(Status.Descricao());
            }
            var fornecedorConsulta = FornecedoresParticipantes.SingleOrDefault(x => x.Fornecedor.Codigo == fornecedor.Codigo);
            if (fornecedorConsulta != null)
            {
                return;
            }
            var fornecedorParticipante = new FornecedorParticipante(this, fornecedor);
            FornecedoresParticipantes.Add(fornecedorParticipante);
        }
        public virtual void RemoverFornecedor(string codigoFornecedor)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException(Status.Descricao());
            }
            var fornecedorParticipante = FornecedoresParticipantes.SingleOrDefault(x => x.Fornecedor.Codigo == codigoFornecedor);
            if (fornecedorParticipante == null)
            {
                return;
            }
            FornecedoresParticipantes.Remove(fornecedorParticipante);
        }

        public virtual void Abrir()
        {
            if (!DataLimiteDeRetorno.HasValue)
            {
                throw new ProcessoDeCotacaoSemDataLimiteRetornoException();
            }
            if (FornecedoresParticipantes.Count == 0)
            {
                throw new ProcessoDeCotacaoSemFornecedoresException();
            }

            foreach (var fornecedorParticipante in FornecedoresParticipantes)
            {
                fornecedorParticipante.CriarCotacao();
            }

            Status = Enumeradores.StatusProcessoCotacao.Aberto;
        }

        public virtual void AtualizarCotacao(int idCotacao, decimal valorUnitario, CondicaoDePagamento condicaoDePagamento, Incoterm incoterm, string descricaoDoIncoterm)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoAtualizacaoCotacaoException();
            }
            if (DateTime.Today > DataLimiteDeRetorno)
            {
                throw new ProcessoDeCotacaoDataLimiteExpiradaException(DataLimiteDeRetorno.Value);
            }
            //busca a cotação do fornecedor
            Cotacao cotacao = FornecedoresParticipantes.First(x => x.Cotacao.Id == idCotacao).Cotacao;

            cotacao.Atualizar(valorUnitario, incoterm, descricaoDoIncoterm);
        }

        public virtual void SelecionarCotacao(string codigoFornecedor, decimal quantidadeAdquirida, Iva iva, CondicaoDePagamento condicaoDePagamento)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
            Cotacao cotacao = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor).Cotacao;
            cotacao.Selecionar(quantidadeAdquirida, iva,condicaoDePagamento);
        }

        public virtual void Fechar()
        {
            if (FornecedoresParticipantes.Count(x => x.Cotacao  != null && x.Cotacao.Selecionada) == 0)
            {
                throw new ProcessoDeCotacaoFecharSemCotacaoSelecionadaException();
            }
            Status = Enumeradores.StatusProcessoCotacao.Fechado;
        }
        
    }

    public class ProcessoDeCotacaoDeMaterial: ProcessoDeCotacao
    {
        public virtual RequisicaoDeCompra RequisicaoDeCompra { get; protected set; }

        protected ProcessoDeCotacaoDeMaterial(){}
        public ProcessoDeCotacaoDeMaterial(RequisicaoDeCompra requisicaoDeCompra, Produto produto, decimal quantidade):base(produto, quantidade)
        {
            RequisicaoDeCompra = requisicaoDeCompra;
            Produto = produto;
            Quantidade = quantidade;
        }

    }
}
