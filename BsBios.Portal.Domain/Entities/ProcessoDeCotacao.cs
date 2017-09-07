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
        public virtual DateTime? DataLimiteDeRetorno { get; protected set; }
        public virtual DateTime? DataDeFechamento { get; protected set; }
        public virtual string Requisitos { get; protected set; }
        public virtual IList<ProcessoDeCotacaoItem> Itens { get; protected set; }
        public virtual IList<FornecedorParticipante> FornecedoresParticipantes { get; protected set; }
        public virtual Usuario Comprador { get; protected set; }

        protected ProcessoDeCotacao()
        {
            FornecedoresParticipantes = new List<FornecedorParticipante>();
            Itens = new List<ProcessoDeCotacaoItem>();
            Status = Enumeradores.StatusProcessoCotacao.NaoIniciado;
        }

        protected ProcessoDeCotacao(string requisitos, DateTime dataLimiteRetorno)
        {
            Requisitos = requisitos;
            DataLimiteDeRetorno = dataLimiteRetorno;

        }

        protected void AdicionarItem()
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAlterarItensException(Status.Descricao());
            }
        }

        public virtual void RemoverItem(ProcessoDeCotacaoItem item)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
            {
                throw new ProcessoDeCotacaoAlterarItensException(Status.Descricao());
            }
            Itens.Remove(item);
        }

        //public virtual Produto Produto { get; protected set; }
        //public virtual decimal Quantidade { get; protected set; }
        //public virtual UnidadeDeMedida UnidadeDeMedida { get; protected set; }

        public virtual IList<FornecedorParticipante> FornecedoresSelecionados
        {
            get
            {
                return FornecedoresParticipantes
                    .Where(fp => fp.Cotacao != null && fp.Cotacao.Itens.Any(i => i.Selecionada))
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

        public virtual void Abrir(Usuario comprador)
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
            if (Itens.Count == 0)
            {
                throw new ProcessoDeCotacaoSemItemException();
            }

            Status = Enumeradores.StatusProcessoCotacao.Aberto;
            Comprador = comprador;

        }

        protected virtual void ValidarCotacao()
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
            if (FornecedoresParticipantes.Count(x => x.Cotacao  != null && x.Cotacao.Itens.Any(c => c.Selecionada)) == 0)
            {
                throw new ProcessoDeCotacaoFecharSemCotacaoSelecionadaException();
            }

            Status = Enumeradores.StatusProcessoCotacao.Fechado;
            DataDeFechamento = DateTime.Now;
        }

        //public abstract void FecharProcesso();


        public virtual void Cancelar()
        {
            if (Status == Enumeradores.StatusProcessoCotacao.Fechado)
            {
                throw new CancelarProcessoDeCotacaoFechadoException();
            }
            Status = Enumeradores.StatusProcessoCotacao.Cancelado;
        }


        protected void ValidarSelecaoDeCotacao(int idDaCotacao)
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }

            FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.Single(x => x.Cotacao != null && x.Cotacao.Id == idDaCotacao);

            if (fornecedorParticipante.Resposta != Enumeradores.RespostaDaCotacao.Aceito)
            {
                throw new Exception("Não é possível selecionar um fornecedor que recusou o processo de cotação.");
            }

        }

        protected void ValidarRemocaoDeSelecaoDaCotacao()
        {
            if (Status != Enumeradores.StatusProcessoCotacao.Aberto)
            {
                throw new ProcessoDeCotacaoFechadoSelecaoCotacaoException();
            }
        }

        //public virtual bool SuperouQuantidadeSolicitada(decimal quantidadeTotalAdquirida)
        //{
        //    return quantidadeTotalAdquirida > Quantidade;
        //}

        public virtual void DesativarParticipante(string codigoDoFornecedor)
        {

            if (Status != Enumeradores.StatusProcessoCotacao.Aberto )
            {
                throw new Exception("O Processo de Cotação não está mais aberto. Não foi possível sair do Processo de Cotação.");
            }

            FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.Single(f => f.Fornecedor.Codigo == codigoDoFornecedor);

            fornecedorParticipante.Recusar();

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
    }

    //public class ProcessoDeCotacaoDeMaterial: ProcessoDeCotacao
    //{
       
    //    public virtual ProcessoDeCotacaoItem AdicionarItem(RequisicaoDeCompra requisicaoDeCompra)
    //    {
    //        AdicionarItem();
    //        if (requisicaoDeCompra.GerouProcessoDeCotacao)
    //        {
    //            throw new RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException(requisicaoDeCompra.Numero,requisicaoDeCompra.NumeroItem);
    //        }

    //        if (requisicaoDeCompra.Status == Enumeradores.StatusRequisicaoCompra.Bloqueado)
    //        {
    //            throw new SelecionarRequisicaoDeCompraBloqueadaException(requisicaoDeCompra.Numero, requisicaoDeCompra.NumeroItem);
    //        }

    //        var item = new ProcessoDeCotacaoDeMaterialItem(this, requisicaoDeCompra);
    //        Itens.Add(item);
    //        return item;
    //    }

    //    public new virtual void RemoverItem(ProcessoDeCotacaoItem item)
    //    {
    //        var itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item;
    //        itemMaterial.RequisicaoDeCompra.DesvincularDeProcessoDeCotacao();
    //        base.RemoverItem(item);
    //    }

    //    public virtual void Atualizar(DateTime dataLimiteDeRetorno, string requisitos)
    //    {
    //        if (Status == Enumeradores.StatusProcessoCotacao.Fechado)
    //        {
    //            throw new ProcessoDeCotacaoAtualizacaoDadosException(Status.Descricao());
    //        }

    //    }

    //    public virtual CotacaoMaterial InformarCotacao(string codigoFornecedor, CondicaoDePagamento condicaoDePagamento,
    //        Incoterm incoterm, string descricaoDoIncoterm)
    //    {
    //        base.InformarCotacao();
    //        //busca a cotação do fornecedor
    //        FornecedorParticipante fornecedorParticipante = FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == codigoFornecedor);

    //        var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();

    //        if (cotacao == null)
    //        {
    //            cotacao = new CotacaoMaterial(condicaoDePagamento, incoterm, descricaoDoIncoterm);
    //            fornecedorParticipante.InformarCotacao(cotacao);
    //        }
    //        else
    //        {
    //            cotacao.Atualizar(condicaoDePagamento, incoterm, descricaoDoIncoterm);
    //        }

    //        return cotacao;
    //    }

    //    public virtual CotacaoItem InformarCotacaoDeItem(int idProcessoDeCotacaoItem, int idCotacao, decimal preco, 
    //        decimal? mva, decimal quantidadeDisponivel, DateTime prazoDeEntrega, string observacoes)
    //    {
    //        base.InformarCotacao();

    //        var cotacao = (CotacaoMaterial)  FornecedoresParticipantes.Where(fp => fp.Cotacao != null && fp.Cotacao.Id == idCotacao)
    //                                     .Select(fp => fp.Cotacao).Single().CastEntity();

    //        ProcessoDeCotacaoItem processoDeCotacaoItem = Itens.Single(item => item.Id == idProcessoDeCotacaoItem);

    //        return cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, preco, mva, quantidadeDisponivel,prazoDeEntrega, observacoes);

    //    }



    //    public virtual void SelecionarCotacao(int idCotacao,int idProcessoCotacaoItem, decimal quantidadeAdquirida, Iva iva)
    //    {
    //        SelecionarCotacao();
    //        var cotacao = (CotacaoMaterial) BuscarPodId(idCotacao).CastEntity();
    //        var itemDaCotacao = (CotacaoMaterialItem)  cotacao.Itens.First(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
    //        itemDaCotacao.Selecionar(quantidadeAdquirida, iva);
    //    }

    //    public virtual void RemoverSelecaoDaCotacao(int idCotacao, int idProcessoCotacaoItem, Iva iva)
    //    {
    //        RemoverSelecaoDaCotacao();
    //        var cotacao = (CotacaoMaterial) BuscarPodId(idCotacao).CastEntity();
    //        var itemDaCotacao = (CotacaoMaterialItem)cotacao.Itens.First(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
    //        itemDaCotacao.RemoverSelecao(iva);
    //    }
    //}

    //public class ProcessoDeCotacaoDeFrete: ProcessoDeCotacao
    //{
    //    public virtual string NumeroDoContrato{ get; protected set; }
    //    public virtual DateTime DataDeValidadeInicial { get; protected set; }
    //    public virtual DateTime DataDeValidadeFinal { get; protected set; }
    //    public virtual Itinerario Itinerario { get; protected set; }

    //    protected ProcessoDeCotacaoDeFrete(){}
    //    public ProcessoDeCotacaoDeFrete(/*Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida, */
    //        string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial, 
    //        DateTime dataDeValidadeFinal, Itinerario itinerario)//:base(produto, quantidade, unidadeDeMedida,requisitos, dataLimiteDeRetorno)
    //    {
    //        NumeroDoContrato = numeroDoContrato;
    //        DataDeValidadeInicial = dataDeValidadeInicial;
    //        DataDeValidadeFinal = dataDeValidadeFinal;
    //        Itinerario = itinerario;
    //        Requisitos = requisitos;
    //        DataLimiteDeRetorno = dataLimiteDeRetorno;
    //    }

    //    public virtual ProcessoDeCotacaoItem AdicionarItem(Produto material, decimal quantidade, UnidadeDeMedida unidadeDeMedida)
    //    {
    //        AdicionarItem();
    //        var item = new ProcessoDeCotacaoDeFreteItem(this, material, quantidade, unidadeDeMedida);
    //        Itens.Add(item);
    //        return item;
    //    }

    //    public virtual void Atualizar(/*Produto produto, decimal quantidade, UnidadeDeMedida unidadeDeMedida,*/
    //        string requisitos, string numeroDoContrato, DateTime dataLimiteDeRetorno, DateTime dataDeValidadeInicial,
    //        DateTime dataDeValidadeFinal, Itinerario itinerario)
    //    {
    //        if (Status != Enumeradores.StatusProcessoCotacao.NaoIniciado)
    //        {
    //            throw new ProcessoDeCotacaoAtualizacaoDadosException(Status.Descricao());
    //        }

    //        //Produto = produto;
    //        //Quantidade = quantidade;
    //        //UnidadeDeMedida = unidadeDeMedida;
    //        Requisitos = requisitos;
    //        NumeroDoContrato = numeroDoContrato;
    //        DataLimiteDeRetorno = dataLimiteDeRetorno;
    //        DataDeValidadeInicial = dataDeValidadeInicial;
    //        DataDeValidadeFinal = dataDeValidadeFinal;
    //        Itinerario = itinerario;

    //    }

    //    public virtual void DesativarParticipante(string codigoDoFornecedor)
    //    {

    //        var cotacao = (CotacaoFrete)fornecedorParticipante.Cotacao.CastEntity();

    //        if (cotacao == null)
    //        {
    //            cotacao = new CotacaoFrete();
    //            fornecedorParticipante.InformarCotacao(cotacao);
    //        }
    //        ProcessoDeCotacaoItem processoDeCotacaoItem = Itens.First();
    //        cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, valorTotalComImpostos, quantidadeDisponivel, observacoes);
    //        return cotacao;
    //    }

    //    public virtual void SelecionarCotacao(int idCotacao,  decimal quantidadeAdquirida)
    //    {
    //        SelecionarCotacao();
    //        var cotacao = (CotacaoFrete)BuscarPodId(idCotacao).CastEntity();
    //        var itemDaCotacao = cotacao.Itens.First();

    //        itemDaCotacao.Selecionar(quantidadeAdquirida);
    //    }

    //    public virtual void RemoverSelecaoDaCotacao(int idCotacao, int idProcessoCotacaoItem)
    //    {
    //        RemoverSelecaoDaCotacao();
    //        var cotacao = (CotacaoFrete)BuscarPodId(idCotacao).CastEntity();
    //        var itemDaCotacao = cotacao.Itens.First(item => item.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);
    //        itemDaCotacao.RemoverSelecao();
    //    }

    //    public virtual void AtualizarItem(Produto produto, decimal quantidadeMaterial, UnidadeDeMedida unidadeDeMedida)
    //    {
    //        var item = (ProcessoDeCotacaoDeFreteItem) Itens.First();
    //        item.Atualizar(produto, quantidadeMaterial, unidadeDeMedida);
    //    }
    //}

}
