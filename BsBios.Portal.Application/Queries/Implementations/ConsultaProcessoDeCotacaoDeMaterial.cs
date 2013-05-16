using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeMaterial : IConsultaProcessoDeCotacaoDeMaterial
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IProcessoCotacaoIteracoesUsuario _iteracoesUsuario;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builderFornecedor;
        private readonly IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> _builderRequisicaoDeCompra;
        //private readonly IBuilder<FornecedorParticipante, ProcessoCotacaoFornecedorVm> _builderProcessoCotacao;  


        public ConsultaProcessoDeCotacaoDeMaterial(IProcessosDeCotacao processosDeCotacao, IBuilder<Fornecedor, FornecedorCadastroVm> builderFornecedor
            , IProcessoCotacaoIteracoesUsuario iteracoesUsuario, IBuilder<RequisicaoDeCompra, RequisicaoDeCompraVm> builderRequisicaoDeCompra /*, 
            IBuilder<FornecedorParticipante, ProcessoCotacaoFornecedorVm> builderProcessoCotacao*/)
        {
            _processosDeCotacao = processosDeCotacao;
            _builderFornecedor = builderFornecedor;
            _iteracoesUsuario = iteracoesUsuario;
            _builderRequisicaoDeCompra = builderRequisicaoDeCompra;
            //_builderProcessoCotacao = builderProcessoCotacao;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro)
        {
            _processosDeCotacao.FiltraPorTipo(
                (Enumeradores.TipoDeCotacao) Enum.Parse(typeof (Enumeradores.TipoDeCotacao), Convert.ToString(filtro.TipoDeCotacao)));
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
                _processosDeCotacao.FiltraPorStatus((Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof (Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.CodigoStatusProcessoCotacao.Value)));
            }

            //var query = (from p in _processosDeCotacao.GetQuery()
            //             select new 
            //             {
            //                 //CodigoMaterial = p.Produto.Codigo,
            //                 //Material = p.Produto.Descricao,
            //                 DataTermino = p.DataLimiteDeRetorno,
            //                 Id = p.Id,
            //                 //Quantidade = p.Quantidade,
            //                 Status = p.Status,
            //                 //UnidadeDeMedida = p.UnidadeDeMedida.Descricao
            //             }
            //            );

            var query = _processosDeCotacao.GetQuery();

            var quantidadeDeRegistros = query.Count();

            var processosListados = query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList();

            var registros = processosListados
                             .Select(x => new ProcessoCotacaoMaterialListagemVm()
                                 {
                                     Id = x.Id,
                                     //CodigoMaterial = x.CodigoMaterial,
                                     Material = String.Join(", ", x.Itens.Select(i => i.Produto.Descricao)),
                                     DataTermino = x.DataLimiteDeRetorno.HasValue ? x.DataLimiteDeRetorno.Value.ToShortDateString(): "",
                                     //Quantidade = x.Quantidade,
                                     Status = x.Status.Descricao(),
                                     //UnidadeDeMedida = x.UnidadeDeMedida
                                 }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = quantidadeDeRegistros,
                    Registros = registros
                };

            return kendoGridVm;

        }

        public ProcessoCotacaoMaterialCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial)
        {
             _processosDeCotacao.BuscaPorId(idProcessoCotacaoMaterial);
            var processoDeCotacao = (from p in _processosDeCotacao.GetQuery()
                                     let processo = (ProcessoDeCotacaoDeMaterial) p
                                     select new
                                         {
                                             processo.Id,
                                             DataTerminoLeilao = p.DataLimiteDeRetorno,
                                             processo.Status,
                                             processo.Requisitos,
                                             //processo.RequisicaoDeCompra.Numero,
                                             //processo.RequisicaoDeCompra.NumeroItem,
                                             //processo.RequisicaoDeCompra.Centro,
                                             //CodigoMaterial = processo.Produto.Codigo,
                                             //Material = processo.Produto.Descricao,
                                             //processo.RequisicaoDeCompra.Descricao,
                                             //processo.Quantidade,
                                             //processo.RequisicaoDeCompra.DataDeLiberacao,
                                             //processo.RequisicaoDeCompra.DataDeRemessa,
                                             //processo.RequisicaoDeCompra.DataDeSolicitacao,
                                             //FornecedorPretendido = processo.RequisicaoDeCompra.FornecedorPretendido.Nome,
                                             //Criador = processo.RequisicaoDeCompra.Criador.Nome, 
                                             //processo.RequisicaoDeCompra.Requisitante,
                                             //processo.RequisicaoDeCompra.UnidadeMedida
                                         }).Single();

            return new ProcessoCotacaoMaterialCadastroVm()
                {
                    Id = processoDeCotacao.Id,
                    DataLimiteRetorno = processoDeCotacao.DataTerminoLeilao.HasValue ? processoDeCotacao.DataTerminoLeilao.Value.ToShortDateString(): null,
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    //CodigoMaterial = processoDeCotacao.CodigoMaterial,
                    Requisitos =  processoDeCotacao.Requisitos,
                    //RequisicaoDeCompraVm = new RequisicaoDeCompraVm()
                    //    {
                    //        Centro = processoDeCotacao.Centro,
                    //        Criador = processoDeCotacao.Criador,
                    //        DataDeLiberacao = processoDeCotacao.DataDeLiberacao.ToShortDateString(),
                    //        DataDeRemessa = processoDeCotacao.DataDeRemessa.ToShortDateString(),
                    //        DataDeSolicitacao = processoDeCotacao.DataDeSolicitacao.ToShortDateString(),
                    //        Descricao = processoDeCotacao.Descricao,
                    //        FornecedorPretendido = processoDeCotacao.FornecedorPretendido,
                    //        Material = processoDeCotacao.Material,
                    //        NumeroItem = processoDeCotacao.NumeroItem,
                    //        NumeroRequisicao = processoDeCotacao.Numero,
                    //        Quantidade = processoDeCotacao.Quantidade,
                    //        Requisitante = processoDeCotacao.Requisitante,
                    //        UnidadeMedida = processoDeCotacao.UnidadeMedida.Descricao
                    //    }
                };
        }

        public KendoGridVm FornecedoresParticipantes(int idProcessoCotacao)
        {
            IList<Fornecedor> fornecedoresParticipantes = (from p in
                                                               _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                           from fp in p.FornecedoresParticipantes
                                                           select fp.Fornecedor).ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = fornecedoresParticipantes.Count,
                    Registros = _builderFornecedor.BuildList(fornecedoresParticipantes).Cast<ListagemVm>().ToList()
                };

            return kendoGridVm;

        }

        public IList<CotacaoMaterialSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            var retorno = new List<CotacaoMaterialSelecionarVm>();
            ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoMaterialSelecionarVm { Fornecedor = fornecedorParticipante.Fornecedor.Nome };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();

                cotacaoSelecionarVm.IdCotacao = cotacao.Id;
                cotacaoSelecionarVm.CondicaoDePagamento = cotacao.CondicaoDePagamento.Descricao;
                cotacaoSelecionarVm.Incoterm = cotacao.Incoterm.Descricao;

                var cotacaoItem = (CotacaoMaterialItem)cotacao.Itens.SingleOrDefault(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);

                if (cotacaoItem == null)
                {
                    continue;
                }

                cotacaoSelecionarVm.QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida;
                cotacaoSelecionarVm.CodigoIva = cotacaoItem.Iva != null ? cotacaoItem.Iva.Codigo : null;
                cotacaoSelecionarVm.ValorLiquido = cotacaoItem.ValorLiquido;
                cotacaoSelecionarVm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacaoItem.Selecionada;

                Imposto imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Icms);
                cotacaoSelecionarVm.ValorIcms = imposto != null ? imposto.Valor : (decimal?) null;

                imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.IcmsSubstituicao);
                cotacaoSelecionarVm.ValorIcmsSt = imposto != null ? imposto.Valor : (decimal?)null;

                imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Ipi);
                cotacaoSelecionarVm.ValorIpi = imposto != null ? imposto.Valor : (decimal?)null;

            }

            return retorno;
        }

        public KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao)
        {
            
            //var queryIteracoesUsuario = iteracoesUsuario.GetQuery();

            //var queryfp = (from p in
            //                   _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
            //               from fp in p.FornecedoresParticipantes
            //               select fp);

            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes =
            //    (from fp in queryfp
            //     join iu in queryIteracoesUsuario on fp.Id equals iu.IdFornecedorParticipante
            //         into joinedTable
            //     from iu in joinedTable.DefaultIfEmpty()
            //     select new ProcessoCotacaoFornecedorVm
            //         {
            //             IdFornecedorParticipante = fp.Id,
            //             Codigo = fp.Fornecedor.Codigo,
            //             Nome = fp.Fornecedor.Nome,
            //             Selecionado = (fp.Cotacao != null && fp.Cotacao.Selecionada ? "Sim" : "Não"),
            //             ValorLiquido = (fp.Cotacao != null ? fp.Cotacao.ValorLiquido : (decimal?)null),
            //             ValorComImpostos = (fp.Cotacao != null ? fp.Cotacao.ValorComImpostos : (decimal?)null),
            //             VisualizadoPeloFornecedor = iu != null && iu.VisualizadoPeloFornecedor ? "Sim" : "Não"
            //         }).ToList();

            List<FornecedorParticipante> fornecedoresParticipantes = (from p in
                                                                          _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                                      from fp in p.FornecedoresParticipantes
                                                                      select fp).ToList();


            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes = (from p in
            //                                                              _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
            //                                                          from fp in p.FornecedoresParticipantes
            //                                                          join it in queryIteracoesUsuario on fp.Id equals it.IdFornecedorParticipante
            //                                                          select new ProcessoCotacaoFornecedorVm
            //                                                              {
            //                                                                    IdFornecedorParticipante = fp.Id,
            //                                                                    Codigo = fp.Fornecedor.Codigo ,
            //                                                                    Nome =  fp.Fornecedor.Nome,
            //                                                                    Selecionado = (fp.Cotacao != null && fp.Cotacao.Selecionada ? "Sim" : "Não") ,
            //                                                                    ValorLiquido = (fp.Cotacao != null ? fp.Cotacao.ValorLiquido : (decimal?) null),
            //                                                                    ValorComImpostos = (fp.Cotacao != null ? fp.Cotacao.ValorComImpostos : (decimal?)null),
            //                                                                   // VisualizadoPeloFornecedor = it.VisualizadoPeloFornecedor ? "Sim": "Não"
            //                                                              }).ToList();

            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes = queryParticipantes.Join( queryIteracoesUsuario, p => p.FornecedoresParticipantes.Select(fp => fp.Id),iu => iu.IdFornecedorParticipante )

            var registros = new List<ProcessoCotacaoFornecedorVm>();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                ProcessoCotacaoIteracaoUsuario iteracaoUsuario = _iteracoesUsuario.BuscaPorIdParticipante(fornecedorParticipante.Id);
                registros.Add(new ProcessoCotacaoFornecedorVm
                    {
                        IdFornecedorParticipante = fornecedorParticipante.Id,
                        Codigo = fornecedorParticipante.Fornecedor.Codigo,
                        Nome = fornecedorParticipante.Fornecedor.Nome,
                        Selecionado =
                            (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Itens.Any(x => x.Selecionada)
                                 ? "Sim"
                                 : "Não"),
                        //ValorLiquido =
                        //    (fornecedorParticipante.Cotacao != null
                        //         ? fornecedorParticipante.Cotacao.ValorLiquido
                        //         : (decimal?) null),
                        //ValorComImpostos =
                        //    (fornecedorParticipante.Cotacao != null
                        //         ? fornecedorParticipante.Cotacao.ValorComImpostos
                        //         : (decimal?) null),
                        //QuantidadeDisponivel = fornecedorParticipante.Cotacao != null ? fornecedorParticipante.Cotacao.QuantidadeDisponivel : (decimal?) null,
                        VisualizadoPeloFornecedor = iteracaoUsuario != null && iteracaoUsuario.VisualizadoPeloFornecedor ? "Sim" : "Não"
                    });
            }


            var kendoGridVm = new KendoGridVm()
            {
                QuantidadeDeRegistros = registros.Count,
                //Registros = _builderProcessoCotacao.BuildList(fornecedoresParticipantes).Cast<ListagemVm>().ToList()
                Registros = registros.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;
        }

        public KendoGridVm ListarItens(int idProcessoCotacao)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            var requisicoes = (from pc in _processosDeCotacao.GetQuery()
                               from item in pc.Itens
                               let itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item
                               select itemMaterial.RequisicaoDeCompra).ToList();

            return new KendoGridVm
                {
                    QuantidadeDeRegistros = requisicoes.Count,
                    Registros = _builderRequisicaoDeCompra.BuildList(requisicoes).Cast<ListagemVm>().ToList()
                };
        }

        public string[] CodigoDosProdutos(int idProcessoCotacao)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            return (from pc in _processosDeCotacao.GetQuery()
             from item in pc.Itens
             select item.Produto.Codigo).Distinct().ToArray();
        }
    }
}