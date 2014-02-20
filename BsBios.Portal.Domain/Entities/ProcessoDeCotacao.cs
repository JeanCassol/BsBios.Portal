using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class ProcessoDeCotacao : IAggregateRoot
    {

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

        public virtual int Id { get; protected set; }
        public virtual Enumeradores.StatusProcessoCotacao Status { get; protected set; }
        public virtual Produto Produto { get; protected set; }
        public virtual decimal Quantidade { get; protected set; }
        public virtual UnidadeDeMedida UnidadeDeMedida { get; protected set; }
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual DateTime DataDeFechamento { get; protected set; }
        public virtual string Requisitos { get; protected set; }
        public virtual IList<FornecedorParticipante> FornecedoresParticipantes { get; protected set; }

        public virtual IList<FornecedorParticipante> FornecedoresSelecionados
        {
            get
            {
                return FornecedoresParticipantes
                    .Where(fp => fp.Cotacao != null && fp.Cotacao.Selecionada)
                    .ToList();
            }
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
            if (Status == Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new AbrirProcessoDeCotacaoAbertoException();
            }
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

        protected virtual void InformarCotacao()
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoAtualizacaoCotacaoException();
            }
            if (DateTime.Today > DataLimiteDeRetorno)
            {
                throw new ProcessoDeCotacaoDataLimiteExpiradaException(DataLimiteDeRetorno.Value);
            }
        }

        protected Cotacao BuscarPodId(int idCotacao)
        {
            return FornecedoresParticipantes.First(x => x.Cotacao != null && x.Cotacao.Id == idCotacao).Cotacao;
        }

        public virtual void Fechar()
        {
            if (Status == Enumeradores.StatusProcessoCotacao.Fechado)
            {
                throw new FecharProcessoDeCotacaoFechadoException();
            }
            if (FornecedoresSelecionados.Count == 0)
            {
                throw new ProcessoDeCotacaoFecharSemCotacaoSelecionadaException();
            }
            Status = Enumeradores.StatusProcessoCotacao.Fechado;
            DataDeFechamento = DateTime.Now;
        }

        public virtual void Cancelar()
        {
            if (Status == Enumeradores.StatusProcessoCotacao.Fechado)
            {
                throw new CancelarProcessoDeCotacaoFechadoException();
            }
            Status = Enumeradores.StatusProcessoCotacao.Cancelado;
        }


        protected void SelecionarCotacao()
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
            
        }

        protected void RemoverSelecaoDaCotacao()
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
        }

        public virtual bool SuperouQuantidadeSolicitada(decimal quantidadeTotalAdquirida)
        {
            return quantidadeTotalAdquirida > Quantidade;
        }
    }
}
