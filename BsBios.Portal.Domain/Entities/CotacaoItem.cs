using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Services;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class CotacaoItem
    {
        public virtual int Id { get; protected set; }
        public virtual Cotacao Cotacao { get; protected set; }
        public virtual ProcessoDeCotacaoItem ProcessoDeCotacaoItem { get; set; }
        public virtual bool Selecionada { get; protected set; }
        public virtual decimal Preco { get; protected set; }
        public virtual decimal PrecoInicial { get; protected set; }
        public virtual decimal ValorComImpostos { get; protected set; }
        public virtual decimal Custo { get; protected set; }
        public virtual decimal? QuantidadeAdquirida { get; protected set; }
        public virtual decimal QuantidadeDisponivel { get; protected set; }
        public virtual string Observacoes { get; protected set; }
        public virtual IList<Imposto> Impostos { get; protected set; }
        public virtual IList<HistoricoDePreco> HistoricosDePreco { get; protected set; }
        private readonly CalculadorDeBaseDeCalculoFactory _calculadorDeBaseDeCalculoFactory;

        protected CotacaoItem()
        {
            Impostos = new List<Imposto>();
            HistoricosDePreco = new List<HistoricoDePreco>();
            _calculadorDeBaseDeCalculoFactory = new CalculadorDeBaseDeCalculoFactory();
        }
        protected CotacaoItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem,decimal preco, 
            decimal quantidadeDisponivel,string observacoes):this()
        {
            Selecionada = false;
            Cotacao = cotacao;
            ProcessoDeCotacaoItem = processoDeCotacaoItem;
            Preco = preco;
            PrecoInicial = preco;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorComImpostos();
            CalculaCusto();
        }

        public virtual decimal ValorDoImposto(Enumeradores.TipoDeImposto tipoDeImposto)
        {
            var imposto = Imposto(tipoDeImposto);
            return imposto?.Valor ?? 0;
        }

        private void CalculaValorComImpostos()
        {
            ValorComImpostos = Preco + ValorDoImposto(Enumeradores.TipoDeImposto.Ipi);
        }

        private void CalculaCusto()
        {
            Produto produto = ProcessoDeCotacaoItem.Produto;
            if (produto.MaterialPrima)
            {
                Custo = Preco - ValorDoImposto(Enumeradores.TipoDeImposto.Icms) -
                        ValorDoImposto(Enumeradores.TipoDeImposto.PisCofins) - ValorDoImposto(Enumeradores.TipoDeImposto.Ipi);
            }
            else
            {
                Custo = ValorComImpostos;
            }
        }

        public virtual void Atualizar(decimal preco, decimal quantidadeDisponivel, string observacoes)
        {
            Preco = preco;
            QuantidadeDisponivel = quantidadeDisponivel;
            Observacoes = observacoes;
            CalculaValorComImpostos();
            CalculaCusto();
        }

        private void AtualizarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota)
        {
            ICalculadorDeBaseDeCalculo calculadorDeBaseDeCalculo = _calculadorDeBaseDeCalculoFactory.Construir(tipoDeImposto, ProcessoDeCotacaoItem.Produto);
            decimal baseDeCalculo = calculadorDeBaseDeCalculo.Calcular(this);
            Imposto imposto = Impostos.FirstOrDefault(x => x.Tipo == tipoDeImposto);
            if (imposto != null)
            {
                imposto.Atualizar(aliquota, baseDeCalculo);
            }
            else
            {
                imposto = new Imposto(this, tipoDeImposto, aliquota, baseDeCalculo);
                Impostos.Add(imposto);
            }

        }

        public virtual void InformarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota)
        {
            AtualizarImposto(tipoDeImposto, aliquota);

            CalculaValorComImpostos();
            CalculaCusto();
        }


        public virtual Imposto Imposto(Enumeradores.TipoDeImposto tipo)
        {
            return Impostos.SingleOrDefault(x => x.Tipo == tipo);
        }

        public virtual void Selecionar(decimal quantidadeAdquirida)
        {
            Selecionada = true;
            QuantidadeAdquirida = quantidadeAdquirida;
        }

        public virtual void RemoverSelecao()
        {
            Selecionada = false;
            QuantidadeAdquirida = null;

        }

        #region override members

        protected bool Equals(CotacaoItem other)
        {
            return Equals(Cotacao, other.Cotacao) && Equals(ProcessoDeCotacaoItem, other.ProcessoDeCotacaoItem);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CotacaoItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Cotacao != null ? Cotacao.GetHashCode() : 0)*397) ^ (ProcessoDeCotacaoItem != null ? ProcessoDeCotacaoItem.GetHashCode() : 0);
            }
        }

        #endregion


        public virtual void RemoverValores()
        {
            this.QuantidadeDisponivel = 0;
            this.ValorComImpostos = 0;
        }
    }

    public class CotacaoMaterialItem: CotacaoItem
    {
        public virtual decimal? Mva { get; protected set; }
        public virtual Iva Iva { get; protected set; }
        public virtual DateTime PrazoDeEntrega { get; protected set; }

        protected CotacaoMaterialItem(){}

        internal CotacaoMaterialItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem, decimal? mva, DateTime prazoDeEntrega,
            decimal preco, decimal quantidadeDisponivel, string observacoes) 
            : base(cotacao, processoDeCotacaoItem,preco, quantidadeDisponivel, observacoes)
        {
            Mva = mva;
            PrazoDeEntrega = prazoDeEntrega;
            AdicionarHistoricoDePreco(preco);
        }

        private void AdicionarHistoricoDePreco(decimal preco)
        {
            HistoricosDePreco.Add(new HistoricoDePreco(preco));
        }

        public virtual void Atualizar(decimal preco,decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
        {
            if (preco != Preco)
            {
                AdicionarHistoricoDePreco(preco);
            }
            base.Atualizar(preco, quantidadeDisponivel, observacoes);
            Mva = mva;
            PrazoDeEntrega = prazoDeEntrega;
        }
        public virtual void Selecionar(decimal quantidadeAdquirida, Iva iva)
        {
            base.Selecionar(quantidadeAdquirida);
            Iva = iva;
        }

        public virtual void RemoverSelecao(Iva iva)
        {
            base.RemoverSelecao();
            Iva = iva;
        }

        public override void InformarImposto(Enumeradores.TipoDeImposto tipoDeImposto, decimal aliquota)
        {
            base.InformarImposto(tipoDeImposto, aliquota);

            if (tipoDeImposto == Enumeradores.TipoDeImposto.IcmsSubstituicao
            && ValorDoImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao) > 0 && (!Mva.HasValue || Mva.Value == 0))
            {
                throw new MvaNaoInformadoException();
            }

        }

    }

    public class CotacaoFreteItem: CotacaoItem
    {
        public virtual decimal? Cadencia { get; protected set; }
        protected CotacaoFreteItem(){}
        internal CotacaoFreteItem(Cotacao cotacao, ProcessoDeCotacaoItem processoDeCotacaoItem,decimal valorTotalComImpostos, 
            decimal quantidadeDisponivel, string observacoes) : base(cotacao, processoDeCotacaoItem,valorTotalComImpostos, quantidadeDisponivel, observacoes)
        {
        }

        public override void RemoverSelecao()
        {
            base.RemoverSelecao();
            Cadencia = null;
        }

        protected internal virtual void Selecionar(decimal quantidadeAdquirida, decimal cadencia)
        {
            base.Selecionar(quantidadeAdquirida);
            this.Cadencia = cadencia;
        }
    }
}
