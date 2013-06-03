using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeFrete : IConsultaProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IProcessoCotacaoIteracoesUsuario _iteracoesUsuario;


        public ConsultaProcessoDeCotacaoDeFrete(IProcessosDeCotacao processosDeCotacao, IProcessoCotacaoIteracoesUsuario iteracoesUsuario)
        {
            _processosDeCotacao = processosDeCotacao;
            _iteracoesUsuario = iteracoesUsuario;
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

        public IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao)
        {
            //var retorno = new List<CotacaoSelecionarVm>();
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);

            var retorno = new List<CotacaoSelecionarVm>();

            var fornecedoresParticipantes = (from pc in _processosDeCotacao.GetQuery()
                                             from fp in pc.FornecedoresParticipantes
                                            select fp).ToList();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoSelecionarVm
                    {
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                        Fornecedor = fornecedorParticipante.Fornecedor.Nome,
                        Cnpj = fornecedorParticipante.Fornecedor.Cnpj
                    };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = fornecedorParticipante.Cotacao;
                cotacaoSelecionarVm.IdCotacao = cotacao.Id;

                var cotacaoItem = cotacao.Itens.SingleOrDefault();

                if (cotacaoItem == null)
                {
                    continue;
                }

                cotacaoSelecionarVm.QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida;
                cotacaoSelecionarVm.QuantidadeDisponivel = cotacaoItem.QuantidadeDisponivel;
                cotacaoSelecionarVm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacaoItem.Selecionada;
                cotacaoSelecionarVm.ObservacaoDoFornecedor = cotacaoItem.Observacoes;
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

        public KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao)
        {
            List<FornecedorParticipante> fornecedoresParticipantes = (from p in
                                                                          _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                                      from fp in p.FornecedoresParticipantes
                                                                      select fp).ToList();


            var registros = new List<ProcessoCotacaoFornecedorVm>();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                ProcessoCotacaoIteracaoUsuario iteracaoUsuario = _iteracoesUsuario.BuscaPorIdParticipante(fornecedorParticipante.Id);
                var vm = new ProcessoCotacaoFornecedorVm
                    {
                        IdFornecedorParticipante = fornecedorParticipante.Id,
                        Codigo = fornecedorParticipante.Fornecedor.Codigo,
                        Nome = fornecedorParticipante.Fornecedor.Nome,
                        VisualizadoPeloFornecedor = iteracaoUsuario != null && iteracaoUsuario.VisualizadoPeloFornecedor ? "Sim" : "Não"
                    };

                if (fornecedorParticipante.Cotacao != null)
                {
                    var cotacaoItem = fornecedorParticipante.Cotacao.Itens.SingleOrDefault();
                    if (cotacaoItem != null)
                    {
                        vm.Selecionado = (cotacaoItem.Selecionada ? "Sim" : "Não");
                        vm.ValorLiquido = cotacaoItem.Preco;
                        vm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                        vm.QuantidadeDisponivel = cotacaoItem.QuantidadeDisponivel;
                    }

                }

                registros.Add(vm);
            }

            var kendoGridVm = new KendoGridVm()
            {
                QuantidadeDeRegistros = registros.Count,
                Registros = registros.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;
        }
    }
}