using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacao : IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual Enumeradores.StatusProcessoCotacao Status { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual UnidadeDeMedida UnidadeDeMedida { get; protected set; }
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual string Requisitos { get; protected set; }
        public virtual IList<FornecedorParticipante> FornecedoresParticipantes { get; protected set; }
        //public virtual IList<Cotacao> Cotacoes { get; protected set; }

        protected ProcessoDeCotacao()
        {
            FornecedoresParticipantes = new List<FornecedorParticipante>();
            //Cotacoes = new List<Cotacao>();
            Status = Enumeradores.StatusProcessoCotacao.NaoIniciado;
        }

        protected ProcessoDeCotacao(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida):this()
        {
            Produto = produto;
            Quantidade = quantidade;
            UnidadeDeMedida = unidadeDeMedida;
        }

        protected ProcessoDeCotacao(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, 
            string requisitos, DateTime dataLimiteRetorno):this(produto, quantidade, unidadeDeMedida)
        {
            Requisitos = requisitos;
            DataLimiteDeRetorno = dataLimiteRetorno;

        }

        public virtual FornecedorParticipante AdicionarFornecedor(Fornecedor fornecedor)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw  new ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException(Status.Descricao());
            }
            var fornecedorConsulta = FornecedoresParticipantes.SingleOrDefault(x => x.Fornecedor.Codigo == fornecedor.Codigo);
            if (fornecedorConsulta != null)
            {
                return fornecedorConsulta;
            }
            var fornecedorParticipante = new FornecedorParticipante(this, fornecedor);
            FornecedoresParticipantes.Add(fornecedorParticipante);
            return fornecedorParticipante;
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

            Status = Enumeradores.StatusProcessoCotacao.Aberto;
        }

        public virtual Cotacao InformarCotacao(string codigoFornecedor, CondicaoDePagamento condicaoDePagamento, 
            Incoterm incoterm, string descricaoDoIncoterm, decimal valorTotalSemImpostos, decimal? valorTotalComImpostos, decimal? mva)
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
            FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);

            return fornecedorParticipante.InformarCotacao(condicaoDePagamento,  incoterm, descricaoDoIncoterm,valorTotalSemImpostos, valorTotalComImpostos, mva);
        }

        private Cotacao BuscarPodId(int idCotacao)
        {
            return FornecedoresParticipantes.First(x => x.Cotacao != null && x.Cotacao.Id == idCotacao).Cotacao;
        }

        public virtual void SelecionarCotacao(int idCotacao, decimal quantidadeAdquirida, Iva iva)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
            Cotacao cotacao = BuscarPodId(idCotacao);
            cotacao.Selecionar(quantidadeAdquirida, iva);
        }
        
        public virtual void RemoverSelecaoDaCotacao(int idCotacao, Iva iva)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
            Cotacao cotacao = BuscarPodId(idCotacao);
            cotacao.RemoverSelecao(iva);

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


    }

    public class ProcessoDeCotacaoDeFrete: ProcessoDeCotacao
    {
        public virtual string NumeroDoContrato{ get; protected set; }
        public virtual DateTime DataDeValidadeInicial { get; protected set; }
        public virtual DateTime DataDeValidadeFinal { get; protected set; }
        public virtual Itinerario Itinerario { get; protected set; }

        protected ProcessoDeCotacaoDeFrete(){}
        public ProcessoDeCotacaoDeFrete(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, 
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial, 
            DateTime dataDeValidadeFinal, Itinerario itinerario):base(produto, quantidade, unidadeDeMedida,requisitos, dataLimiteDeRetorno)
        {
            NumeroDoContrato = numeroDoContrato;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
        }

        public virtual void Atualizar(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida,
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
            DateTime dataDeValidadeFinal, Itinerario itinerario)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAbertoAtualizacaoDadosException(Status.Descricao());
            }

            Produto = produto;
            Quantidade = quantidade;
            UnidadeDeMedida = unidadeDeMedida;
            Requisitos = requisitos;
            NumeroDoContrato = numeroDoContrato;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;

        }
    }
}
