using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoDeCotacaoDeFrete: ProcessoDeCotacao
    {

        private void ValidarDataDeValidade(DateTime dataDeValidadeInicial, DateTime dataDeValidadeFinal)
        {
            if (dataDeValidadeInicial > dataDeValidadeFinal)
            {
                throw new DataDeValidadeInicialMaiorQueDataDeValidadeFinalException();
            }
            
        }


        protected ProcessoDeCotacaoDeFrete(){}
        public ProcessoDeCotacaoDeFrete(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, 
            string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial, 
            DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedorDaMercadoria, decimal cadencia, bool classificacao,
            Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal, decimal valorPrevisto)
            :base(produto, quantidade, unidadeDeMedida,requisitos, dataLimiteDeRetorno)
        {

            ValidarDataDeValidade(dataDeValidadeInicial, dataDeValidadeFinal);

            NumeroDoContrato = numeroDoContrato;
            DataDeValidadeInicial = dataDeValidadeInicial;
            DataDeValidadeFinal = dataDeValidadeFinal;
            Itinerario = itinerario;
            FornecedorDaMercadoria = fornecedorDaMercadoria;
            Cadencia = cadencia;
            Classificacao = classificacao;
            MunicipioDeOrigem = municipioDeOrigem;
            MunicipioDeDestino = municipioDeDestino;
            Deposito = deposito;
            Terminal = terminal;
            ValorPrevisto = valorPrevisto;
        }

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
        public virtual decimal? ValorFechado { get; protected set; }


        public virtual void Atualizar(Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, string requisitos, string numeroDoContrato, 
            DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial, DateTime dataDeValidadeFinal, Itinerario itinerario, Fornecedor fornecedor, 
            decimal cadencia, bool classificacao, Municipio municipioDeOrigem, Municipio municipioDeDestino, Fornecedor deposito, Terminal terminal,
            decimal valorPrevisto)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAbertoAtualizacaoDadosException(Status.Descricao());
            }

            ValidarDataDeValidade(dataDeValidadeInicial, dataDeValidadeFinal);

            Produto = produto;
            Quantidade = quantidade;
            UnidadeDeMedida = unidadeDeMedida;
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

        public virtual CotacaoDeFrete InformarCotacao(string codigoFornecedor, decimal valorTotalComImpostos,
            decimal quantidadeDisponivel, string observacoes)
        {
            base.InformarCotacao();
            //busca a cotação do fornecedor
            FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);

            var cotacao = (CotacaoDeFrete)fornecedorParticipante.Cotacao.CastEntity();

            if (cotacao == null)
            {
                cotacao = new CotacaoDeFrete(valorTotalComImpostos, quantidadeDisponivel, observacoes);
                fornecedorParticipante.InformarCotacao(cotacao);
            }
            else
            {
                fornecedorParticipante.AceitarCotacao();
                cotacao.Atualizar(valorTotalComImpostos, quantidadeDisponivel, observacoes);
            }

            return cotacao;
        }


        public virtual void SelecionarCotacao(int idCotacao, decimal quantidadeAdquirida, decimal cadencia)
        {
            ValidarSelecaoDeCotacao(idCotacao);
            //FornecedoresParticipantes.First(x => x.Cotacao != null && x.Cotacao.Id == idCotacao)
            var cotacao = (CotacaoDeFrete)BuscarPodId(idCotacao).CastEntity();

            cotacao.Selecionar(quantidadeAdquirida,cadencia);
        }

        public virtual void RemoverSelecaoDaCotacao(int idCotacao)
        {
            ValidarRemocaoDeSelecaoDaCotacao();

            var cotacao = (CotacaoDeFrete)BuscarPodId(idCotacao).CastEntity();
            cotacao.RemoverSelecao();
        }

        //não posso usar este
        public override void FecharProcesso()
        {
            throw new NotImplementedException("Deve ser utilizado o método que fecha o processo de cotação passando a lista de condições de fechamento");
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

            if (FornecedoresSelecionados.Any(fs => string.IsNullOrEmpty(((CotacaoDeFrete) fs.Cotacao.CastEntity()).NumeroDaCondicaoGeradaNoSap)))
            {
                throw new FornecedorSemCondicaoGeradaNoSapException();

            }

            var ordensDeTransporte = (from fornecedorSelecionado in FornecedoresSelecionados
                let cotacao = (CotacaoDeFrete) fornecedorSelecionado.Cotacao.CastEntity()
                select new OrdemDeTransporte(this, fornecedorSelecionado.Fornecedor,
                    cotacao.QuantidadeAdquirida.Value, cotacao.ValorComImpostos, cotacao.Cadencia.Value)).ToList();

            return ordensDeTransporte;
        }

        public virtual void AbrirPreco()
        {
            ValorFechado = null;
        }

        public virtual void FecharPreco(decimal valor)
        {
            ValorFechado = valor;
        }

    }
}