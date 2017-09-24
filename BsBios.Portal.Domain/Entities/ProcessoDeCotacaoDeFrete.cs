using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoDeCotacaoDeFrete : ProcessoDeCotacao
    {
        public virtual string NumeroDoContrato { get; protected set; }
        public virtual DateTime DataDeValidadeInicial { get; protected set; }
        public virtual DateTime DataDeValidadeFinal { get; protected set; }
        public virtual Itinerario Itinerario { get; protected set; }
        public virtual Fornecedor FornecedorDaMercadoria { get; protected set; }
        public virtual decimal Cadencia { get; protected set; }
        public virtual bool Classificacao { get; protected set; }
        public virtual Municipio MunicipioDeOrigem { get; protected set; }
        public virtual Municipio MunicipioDeDestino { get; protected set; }
        public virtual Fornecedor Deposito { get; protected set; }
        public virtual Terminal Terminal { get; protected set; }
        public virtual decimal? ValorPrevisto { get; protected set; }

        public virtual void InformarCotacao(string codigoFornecedor, decimal valorComImpostos, decimal quantidadeDisponivel, string observacoesDoFornecedor)
        {
            base.InformarCotacao();
            var fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);
            var cotacao = (CotacaoDeFrete) fornecedorParticipante.Cotacao;
            if (cotacao == null)
            {
                cotacao = new CotacaoDeFrete();
                fornecedorParticipante.InformarCotacao(cotacao);
            }
            var processoDeCotacaoItem = this.Itens.Single();
            cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, valorComImpostos, quantidadeDisponivel,
                observacoesDoFornecedor);
        }

        public virtual Enumeradores.TipoDePrecoDoProcessoDeCotacao TipoDePreco { get; protected set; }
        public virtual decimal? ValorFechado { get; protected set; }
        public virtual decimal? ValorMaximo { get; protected set; }

        protected ProcessoDeCotacaoDeFrete() { }
        public ProcessoDeCotacaoDeFrete(/*Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, */
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
            DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedorDaMercadoria, decimal cadencia, bool classificacao,
            Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal, decimal valorPrevisto)//:base(produto, quantidade, unidadeDeMedida,requisitos, dataLimiteDeRetorno)
        {
            NumeroDoContrato = numeroDoContrato;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
            Requisitos = requisitos;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            FornecedorDaMercadoria = fornecedorDaMercadoria;
            Cadencia = cadencia;
            Classificacao = classificacao;
            MunicipioDeOrigem = municipioDeOrigem;
            MunicipioDeDestino = municipioDeDestino;
            Deposito = deposito;
            Terminal = terminal;
            ValorPrevisto = valorPrevisto;
        }

        public virtual ProcessoDeCotacaoItem AdicionarItem(Produto material, decimal quantidade, UnidadeDeMedida unidadeDeMedida)
        {
            AdicionarItem();
            var item = new ProcessoDeCotacaoDeFreteItem(this, material, quantidade, unidadeDeMedida);
            Itens.Add(item);
            return item;
        }

        public virtual void Atualizar(/*Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida,*/
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
            DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedor,
            decimal cadencia, bool classificacao, Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal,
            decimal valorPrevisto)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAtualizacaoDadosException(Status.Descricao());
            }

            //Produto = produto;
            //Quantidade = quantidade;
            //UnidadeDeMedida = unidadeDeMedida;
            Requisitos = requisitos;
            NumeroDoContrato = numeroDoContrato;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
            FornecedorDaMercadoria = fornecedor;
            Cadencia = cadencia;
            Classificacao = classificacao;
            MunicipioDeOrigem = municipioDeOrigem;
            MunicipioDeDestino = municipioDeDestino;
            Deposito = deposito;
            Terminal = terminal;
            ValorPrevisto = valorPrevisto;
        }

        //public virtual void DesativarParticipante(string codigoDoFornecedor)
        //{
        //    //var fornecedorParticipante = this.FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoDoFornecedor);
        //    //var cotacao = (CotacaoDeFrete) fornecedorParticipante.Cotacao.CastEntity();

        //    //if (cotacao == null)
        //    //{
        //    //    cotacao = new CotacaoDeFrete();
        //    //    fornecedorParticipante.InformarCotacao(cotacao);
        //    //}
        //    //ProcessoDeCotacaoItem processoDeCotacaoItem = Itens.First();
        //    //cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, valorTotalComImpostos, quantidadeDisponivel, observacoes);
        //}

        public virtual void SelecionarCotacao(int idCotacao, decimal quantidadeAdquirida, decimal cadencia)
        {
            this.Cadencia = cadencia;
            var cotacao = BuscarPodId(idCotacao).CastEntity();
            var itemDaCotacao = cotacao.Itens.First();

            itemDaCotacao.Selecionar(quantidadeAdquirida);
        }

        public virtual void RemoverSelecaoDaCotacao(int idCotacao, int idProcessoCotacaoItem)
        {
            var cotacao = BuscarPodId(idCotacao).CastEntity();
            var itemDaCotacao = cotacao.Itens.First(item => item.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
            itemDaCotacao.RemoverSelecao();
        }

        public virtual void AtualizarItem(Produto produto, decimal quantidadeMaterial, UnidadeDeMedida unidadeDeMedida)
        {
            var item = (ProcessoDeCotacaoDeFreteItem)Itens.First();
            item.Atualizar(produto, quantidadeMaterial, unidadeDeMedida);
        }

        public virtual IList<OrdemDeTransporte> FecharProcesso(IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento)
        {
            Fechar();

            foreach (var condicaoDoFechamentoNoSap in condicoesDeFechamento)
            {
                FornecedorParticipante fornecedorParticipante = this.FornecedoresSelecionados.Single(s => s.Fornecedor.Codigo == condicaoDoFechamentoNoSap.CodigoDoFornecedor);
                var cotacaoDeFrete = (CotacaoDeFrete)fornecedorParticipante.Cotacao.CastEntity();
                cotacaoDeFrete.InformarNumeroDaCondicao(condicaoDoFechamentoNoSap.NumeroGeradoNoSap);
            }

            if (FornecedoresSelecionados.Any(fs => string.IsNullOrEmpty(((CotacaoDeFrete)fs.Cotacao.CastEntity()).NumeroDaCondicaoGeradaNoSap)))
            {
                throw new FornecedorSemCondicaoGeradaNoSapException();

            }

            var ordensDeTransporte = (from fornecedorSelecionado in FornecedoresSelecionados
                let cotacao = (CotacaoDeFrete)fornecedorSelecionado.Cotacao.CastEntity()
                from cotacaoItem in cotacao.Itens
                select new OrdemDeTransporte(this, fornecedorSelecionado.Fornecedor,
                    cotacaoItem.QuantidadeAdquirida.Value, cotacaoItem.ValorComImpostos, cotacao.Cadencia.Value)).ToList();

            return ordensDeTransporte;
        }

        public virtual void AbrirPreco()
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorAberto;
            ValorFechado = null;
            ValorMaximo = null;
        }

        public virtual void FecharPreco(decimal preco)
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorFechado;
            ValorFechado = preco;
            ValorMaximo = null;
        }

        public virtual void EstabelecerPrecoMaximo(decimal preco)
        {
            TipoDePreco = Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorMaximo;
            ValorFechado = null;
            ValorMaximo = preco;
        }
    }

}