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
        public virtual bool Classificacao { get; protected set; }
        public virtual Municipio MunicipioDeOrigem { get; protected set; }
        public virtual Municipio MunicipioDeDestino { get; protected set; }
        public virtual Fornecedor Deposito { get; protected set; }
        public virtual Terminal Terminal { get; protected set; }

        public virtual FornecedorParticipante InformarCotacao(string codigoFornecedor, decimal valorComImpostos, decimal quantidadeDisponivel, string observacoesDoFornecedor)
        {
            base.InformarCotacao();
            var fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);
            if (fornecedorParticipante.Cotacao != null)
            {
                //cotacao = (CotacaoDeFrete)fornecedorParticipante.Cotacao.CastEntity();
                throw new AlterarCotacaoDeFreteException();
            }

            var cotacao = new CotacaoDeFrete();
            fornecedorParticipante.InformarCotacao(cotacao);

            var processoDeCotacaoItem = this.Itens.Single();
            cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, valorComImpostos, quantidadeDisponivel,
                observacoesDoFornecedor);

            return fornecedorParticipante;
        }

        protected ProcessoDeCotacaoDeFrete() { }
        public ProcessoDeCotacaoDeFrete(
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
            DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedorDaMercadoria, bool classificacao,
            Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal)
        {
            NumeroDoContrato = numeroDoContrato;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
            Requisitos = requisitos;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            FornecedorDaMercadoria = fornecedorDaMercadoria;
            Classificacao = classificacao;
            MunicipioDeOrigem = municipioDeOrigem;
            MunicipioDeDestino = municipioDeDestino;
            Deposito = deposito;
            Terminal = terminal;
        }

        public virtual ProcessoDeCotacaoItem AdicionarItem(Produto material, decimal quantidade, UnidadeDeMedida unidadeDeMedida, decimal cadencia, decimal? valorPrevisto)
        {
            AdicionarItem();
            var item = new ProcessoDeCotacaoDeFreteItem(this, material, quantidade, unidadeDeMedida, cadencia, valorPrevisto);
            Itens.Add(item);
            return item;
        }

        public virtual void Atualizar(string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial, DateTime dataDeValidadeFinal, 
            Itinerario itinerario, Fornecedor fornecedor, bool classificacao, Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAtualizacaoDadosException(Status.Descricao());
            }

            Requisitos = requisitos;
            NumeroDoContrato = numeroDoContrato;
            DataLimiteDeRetorno = dataLimiteDeRetorno;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
            FornecedorDaMercadoria = fornecedor;
            Classificacao = classificacao;
            MunicipioDeOrigem = municipioDeOrigem;
            MunicipioDeDestino = municipioDeDestino;
            Deposito = deposito;
            Terminal = terminal;
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
            var cotacao = (CotacaoDeFrete) BuscarPodId(idCotacao).CastEntity();
            cotacao.Selecionar(quantidadeAdquirida, cadencia);
            
        }

        public virtual void RemoverSelecaoDaCotacao(int idCotacao, int idProcessoCotacaoItem)
        {
            var cotacao = (CotacaoDeFrete) BuscarPodId(idCotacao).CastEntity();
            cotacao.RemoverSelecao();

        }

        public virtual void AtualizarItem(Produto produto, decimal quantidadeMaterial, UnidadeDeMedida unidadeDeMedida, decimal cadencia, decimal? valorPrevisto)
        {
            var item = (ProcessoDeCotacaoDeFreteItem)Itens.Single();
            item.Atualizar(produto, quantidadeMaterial, unidadeDeMedida, cadencia, valorPrevisto);
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
                let cotacaoFreteItem = (CotacaoFreteItem) cotacaoItem
                select new OrdemDeTransporte(this, fornecedorSelecionado.Fornecedor,
                    cotacaoFreteItem.QuantidadeAdquirida.Value, cotacaoFreteItem.ValorComImpostos, cotacaoFreteItem.Cadencia.Value)).ToList();

            return ordensDeTransporte;
        }

        public virtual void AbrirPreco()
        {
            var item = (ProcessoDeCotacaoDeFreteItem)Itens.Single();
            item.AbrirPreco();
        }

        public virtual void FecharPreco(decimal preco)
        {
            var item = (ProcessoDeCotacaoDeFreteItem)Itens.Single();
            item.FecharPreco(preco);
        }

        public virtual void EstabelecerPrecoMaximo(decimal preco)
        {
            var item = (ProcessoDeCotacaoDeFreteItem)Itens.Single();
            item.EstabelecerPrecoMaximo(preco);
        }

        public virtual ProcessoDeCotacaoDeFreteItem ObterItem()
        {
            return (ProcessoDeCotacaoDeFreteItem)Itens.Single();
        }


    }

}