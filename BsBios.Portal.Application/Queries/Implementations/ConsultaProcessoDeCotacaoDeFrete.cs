using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeFrete : IConsultaProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;


        public ConsultaProcessoDeCotacaoDeFrete(IProcessosDeCotacao processosDeCotacao)
        {
            _processosDeCotacao = processosDeCotacao;
        }


        public ProcessoCotacaoFreteCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial)
        {
             
            var processoDeCotacao = (ProcessoDeCotacaoDeFrete)  _processosDeCotacao.BuscaPorId(idProcessoCotacaoMaterial).Single();
            ProcessoDeCotacaoItem item = processoDeCotacao.Itens.First();

            return new ProcessoCotacaoFreteCadastroVm()
                {
                    Id = processoDeCotacao.Id,
                    DataLimiteRetorno = processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString(),
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    //CodigoMaterial = processoDeCotacao.Produto.Codigo,
                    //DescricaoMaterial = processoDeCotacao.Produto.Descricao,
                    //QuantidadeMaterial = processoDeCotacao.Quantidade ,
                    //CodigoUnidadeMedida = processoDeCotacao.UnidadeDeMedida.CodigoInterno ,
                    CodigoMaterial = item.Produto.Codigo,
                    DescricaoMaterial = item.Produto.Descricao,
                    QuantidadeMaterial = item.Quantidade,
                    CodigoUnidadeMedida = item.UnidadeDeMedida.CodigoInterno,
                    CodigoItinerario = processoDeCotacao.Itinerario.Codigo,
                    DescricaoItinerario = processoDeCotacao.Itinerario.Descricao ,
                    Requisitos = processoDeCotacao.Requisitos ,
                    NumeroDoContrato = processoDeCotacao.NumeroDoContrato ,
                    DataValidadeCotacaoInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString() ,
                    DataValidadeCotacaoFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString() ,
                };
        }

        public IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            var retorno = new List<CotacaoSelecionarVm>();
            ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var q = (from pc in _processosDeCotacao.GetQuery()
                         from fp in pc.FornecedoresParticipantes
                          from item in fp.Cotacao.Itens
                         where item.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem 

                         select new CotacaoSelecionarVm
                             {
                                 
                             }
                         )

            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoSelecionarVm { Fornecedor = fornecedorParticipante.Fornecedor.Nome };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = fornecedorParticipante.Cotacao;

                cotacaoSelecionarVm.IdCotacao = cotacao.Id;
                cotacaoSelecionarVm.QuantidadeAdquirida = cotacao.QuantidadeAdquirida;
                cotacaoSelecionarVm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;
                cotacaoSelecionarVm.ValorComImpostos = cotacao.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacao.Selecionada;
                cotacaoSelecionarVm.ObservacaoDoFornecedor = cotacao.Observacoes;
            }

            return retorno;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro)
        {
            _processosDeCotacao.FiltraPorTipo(
                (Enumeradores.TipoDeCotacao)
                Enum.Parse(typeof (Enumeradores.TipoDeCotacao), Convert.ToString(filtro.TipoDeCotacao)));
            if (filtro.CodigoFornecedor != null)
            {
                _processosDeCotacao
                    .DesconsideraNaoIniciados()
                    .FiltraPorFornecedor(filtro.CodigoFornecedor);

            }

            _processosDeCotacao.CodigoDoProdutoContendo(filtro.CodigoProduto)
                               .DescricaoDoProdutoContendo(filtro.DescricaoProduto);

            if (filtro.CodigoStatusProcessoCotacao.HasValue)
            {
                _processosDeCotacao.FiltraPorStatus(
                    (Enumeradores.StatusProcessoCotacao)
                    Enum.Parse(typeof (Enumeradores.StatusProcessoCotacao),
                               Convert.ToString(filtro.CodigoStatusProcessoCotacao.Value)));
            }

            var query = (from p in _processosDeCotacao.GetQuery()
                         from item in p.Itens
                         select new
                             {
                                 CodigoMaterial = item.Produto.Codigo,
                                 Material = item.Produto.Descricao,
                                 DataTermino = p.DataLimiteDeRetorno,
                                 Id = p.Id,
                                 Quantidade = item.Quantidade,
                                 Status = p.Status,
                                 UnidadeDeMedida = item.UnidadeDeMedida.Descricao
                             }
                        );

            var quantidadeDeRegistros = query.Count();

            var registros = query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList()
                                 .Select(x => new ProcessoCotacaoMaterialListagemVm()
                                     {
                                         Id = x.Id,
                                         CodigoMaterial = x.CodigoMaterial,
                                         Material = x.Material,
                                         DataTermino =
                                             x.DataTermino.HasValue ? x.DataTermino.Value.ToShortDateString() : "",
                                         Quantidade = x.Quantidade,
                                         Status = x.Status.Descricao(),
                                         UnidadeDeMedida = x.UnidadeDeMedida
                                     }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = quantidadeDeRegistros,
                    Registros = registros
                };

            return kendoGridVm;
        }
    }
}