using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Application.Queries.Builders;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaCotacaoDoFornecedor : IConsultaCotacaoDoFornecedor
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IBuilder<CotacaoItem, CotacaoImpostosVm> _builderImpostos;
        private readonly IUsuarios _usuarios;
        private const string ValorNaoInformado = "Não informado";


        public ConsultaCotacaoDoFornecedor(IProcessosDeCotacao processosDeCotacao, IBuilder<CotacaoItem, CotacaoImpostosVm> builderImpostos, IUsuarios usuarios)
        {
            _processosDeCotacao = processosDeCotacao;
            _builderImpostos = builderImpostos;
            _usuarios = usuarios;
        }

        //public CotacaoCadastroVm ConsultarCotacao(int idProcessoCotacao, string codigoFornecedor)
        //{
        //    _processosDeCotacao.BuscaPorId(idProcessoCotacao)
        //                       .FiltraPorFornecedor(codigoFornecedor);

        //    var vm = (from p in _processosDeCotacao.GetQuery()
        //              from fp in p.FornecedoresParticipantes
        //            let pcm = (ProcessoDeCotacaoDeMaterial ) p
        //            select new 
        //                {
        //                    IdProcessoCotacao = pcm.Id,
        //                    CodigoFornecedor = fp.Fornecedor.Codigo,
        //                    Status = pcm.Status,
        //                    DescricaoDoProcessoDeCotacao = pcm.RequisicaoDeCompra.Descricao,  
        //                    DataLimiteDeRetorno = pcm.DataLimiteDeRetorno.Value.ToShortDateString(),
        //                    Material = pcm.Produto.Descricao,
        //                    Quantidade = pcm.Quantidade,
        //                    UnidadeDeMedida = pcm.RequisicaoDeCompra.UnidadeMedida,
        //                    //CodigoCondicaoPagamento = fp.Cotacao != null ?  fp.Cotacao.CondicaoDePagamento.Codigo : null,
        //                    //CodigoIncoterm =  fp.Cotacao != null ?  fp.Cotacao.Incoterm.Codigo : null,
        //                    //DescricaoIncoterm = fp.Cotacao != null ?  fp.Cotacao.DescricaoIncoterm : null,
        //                    //Mva = fp.Cotacao != null ?  fp.Cotacao.Mva : null,
        //                    //ValorTotalSemImpostos = fp.Cotacao != null ?  fp.Cotacao.ValorTotalSemImpostos : (decimal?) null,
        //                    //ValorTotalComImpostos = fp.Cotacao != null ? fp.Cotacao.ValorTotalComImpostos : null
        //                    //CodigoCondicaoPagamento = fp.Cotacao != null ?  fp.Cotacao.CondicaoDePagamento.Codigo : null,

        //                    CodigoCondicaoPagamento = fp.Cotacao.CondicaoDePagamento.Codigo,
        //                    CodigoIncoterm =  fp.Cotacao.Incoterm.Codigo,
        //                    DescricaoIncoterm = fp.Cotacao.DescricaoIncoterm,
        //                    Mva = fp.Cotacao.Mva,
        //                    ValorTotalSemImpostos = fp.Cotacao.ValorTotalSemImpostos,
        //                    ValorTotalComImpostos = fp.Cotacao.ValorTotalComImpostos
        //                }).Single();



        //    return new CotacaoCadastroVm()
        //        {
        //            IdProcessoCotacao = vm.IdProcessoCotacao,
        //            CodigoFornecedor = vm.CodigoFornecedor,
        //            Status = vm.Status.Descricao(),
        //            DescricaoDoProcessoDeCotacao = vm.DescricaoDoProcessoDeCotacao, //falta implementar. Acho que a descrição tem que estar no processo e não na requisição de compra 
        //            DataLimiteDeRetorno = vm.DataLimiteDeRetorno,
        //            Material = vm.Material,
        //            Quantidade = vm.Quantidade,
        //            UnidadeDeMedida = vm.UnidadeDeMedida,
        //            CodigoCondicaoPagamento = vm.CodigoCondicaoPagamento,
        //            CodigoIncoterm = vm.CodigoIncoterm,
        //            DescricaoIncoterm = vm.DescricaoIncoterm,
        //            Mva = vm.Mva,
        //            ValorTotalSemImpostos = vm.ValorTotalSemImpostos,
        //            ValorTotalComImpostos = vm.ValorTotalComImpostos

        //        };
        //}

        public CotacaoMaterialCadastroVm ConsultarCotacaoDeMaterial(int idProcessoCotacao, string codigoFornecedor)
        {
            var processo = (ProcessoDeCotacaoDeMaterial) _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var fp = processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == codigoFornecedor);

            var vm = new CotacaoMaterialCadastroVm
                {
                    PermiteEditar = processo.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                    IdProcessoCotacao = processo.Id,
                    CodigoFornecedor = fp.Fornecedor.Codigo,
                    Status = processo.Status.Descricao(),
                    Requisitos = processo.Requisitos,
                    //DescricaoDoProcessoDeCotacao = processo.RequisicaoDeCompra.Descricao,
                    DataLimiteDeRetorno = processo.DataLimiteDeRetorno.Value.ToShortDateString(),
                    IdFornecedorParticipante = fp.Id
                };


            if (fp.Cotacao != null)
            {

                var cotacao = (CotacaoMaterial)  fp.Cotacao.CastEntity();
                vm.IdCotacao = cotacao.Id;
                vm.CodigoCondicaoPagamento = cotacao.CondicaoDePagamento.Codigo;
                vm.CodigoIncoterm = cotacao.Incoterm.Codigo;
                vm.DescricaoIncoterm = cotacao.DescricaoIncoterm;

            }
            return vm;

        }

        public CotacaoMaterialConsultarCadastroVm ConsultarCotacaoDeMaterialParaVisualizacao(int idProcessoCotacao, string codigoFornecedor)
        {
            var processo = (ProcessoDeCotacaoDeMaterial)_processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var fp = processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == codigoFornecedor);

            var vm = new CotacaoMaterialConsultarCadastroVm
            {
                IdProcessoCotacao = processo.Id,
                CodigoFornecedor = fp.Fornecedor.Codigo,
            };


            if (fp.Cotacao != null)
            {
                var cotacao = (CotacaoMaterial)fp.Cotacao.CastEntity();
                vm.IdCotacao = cotacao.Id;
                vm.CondicaoPagamento = cotacao.CondicaoDePagamento.Descricao;
                vm.Incoterm = cotacao.Incoterm.Descricao;
                vm.DescricaoIncoterm = cotacao.DescricaoIncoterm;
            }
            else
            {
                vm.CondicaoPagamento = ValorNaoInformado;
                vm.Incoterm = ValorNaoInformado;
                vm.DescricaoIncoterm = ValorNaoInformado;
            }
            return vm;
        }

        public CotacaoMaterialItemCadastroVm ConsultarCotacaoDeItemDeMaterial(int idProcessoCotacao, string codigoFornecedor,
            string numeroDaRequisicao, string numeroDoItemDaRequisicao)
        {
            var processoDeCotacao = (ProcessoDeCotacaoDeMaterial)  _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var itemDoProcessoDeCotacao = processoDeCotacao.Itens.Single(item =>
                {
                    var itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item;
                    return itemMaterial.RequisicaoDeCompra.Numero == numeroDaRequisicao
                           && itemMaterial.RequisicaoDeCompra.NumeroItem == numeroDoItemDaRequisicao;
                });
            
            var vm = new CotacaoMaterialItemCadastroVm
                {
                    IdProcessoCotacao = idProcessoCotacao,
                    IdProcessoCotacaoItem = itemDoProcessoDeCotacao.Id,
                    Material = itemDoProcessoDeCotacao.Produto.Descricao,
                    Quantidade = itemDoProcessoDeCotacao.Quantidade,
                    UnidadeDeMedida = itemDoProcessoDeCotacao.UnidadeDeMedida.Descricao
                };

            var fp = processoDeCotacao.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == codigoFornecedor);

            CotacaoItem itemCotacao = null;
            if (fp.Cotacao != null)
            {
                vm.IdCotacao = fp.Cotacao.Id;
                itemCotacao = fp.Cotacao.Itens.SingleOrDefault(x => x.ProcessoDeCotacaoItem.Id == itemDoProcessoDeCotacao.Id);    
            }
            
            if (itemCotacao != null)
            {
                var itemCotacaoMaterial = (CotacaoMaterialItem) itemCotacao;
                vm.IdCotacao = fp.Cotacao.Id;
                vm.IdCotacaoItem = itemCotacaoMaterial.Id;
                vm.Mva = itemCotacaoMaterial.Mva;
                vm.Preco = itemCotacaoMaterial.Preco;
                vm.ValorComImpostos = itemCotacaoMaterial.ValorComImpostos;
                vm.Custo = itemCotacaoMaterial.Custo;
                vm.ObservacoesDoFornecedor = itemCotacaoMaterial.Observacoes;
                vm.PrazoDeEntrega = itemCotacaoMaterial.PrazoDeEntrega.ToShortDateString();
                vm.QuantidadeDisponivel = itemCotacao.QuantidadeDisponivel;

                vm.Impostos = _builderImpostos.BuildSingle(itemCotacaoMaterial);

            }
            else
            {
                vm.Impostos = new CotacaoImpostosVm();
            }
            Usuario usuarioConectado = _usuarios.UsuarioConectado();
            vm.PermiteVisualizarCustos = usuarioConectado.Permissao.PermiteVisualizarCustos;

            return vm;


            //esta foi uma tentativa de fazer toda a consulta em uma única query, mas não chegou a ser testada
            //var query = _processosDeCotacao.GetQuery();

            //var vm = (from pc in query
            //         from fp in pc.FornecedoresParticipantes
            //         where fp.Fornecedor.Codigo == codigoFornecedor
            //         from itemProcesso in pc.Itens
            //         let itemProcessoMaterial = (ProcessoDeCotacaoDeMaterialItem) itemProcesso
            //         where itemProcessoMaterial.RequisicaoDeCompra.Numero == numeroDaRequisicao
            //               && itemProcessoMaterial.RequisicaoDeCompra.NumeroItem == numeroDoItemDaRequisicao
            //         from cotacaoItem in fp.Cotacao.Itens
            //         where itemProcessoMaterial.Id == cotacaoItem.ProcessoDeCotacaoItem.Id
            //         select new CotacaoMaterialItemCadastroVm
            //             {
            //                 IdProcessoCotacao = idProcessoCotacao,
            //                 IdCotacao = fp.Cotacao.Id,
            //                 IdCotacaoItem = cotacaoItem.Id,
            //                 IdProcessoCotacaoItem = itemProcessoMaterial.Id,
            //                 Material = itemProcessoMaterial.Produto.Descricao
            //             }
            //        ).SingleOrDefault();

        }

        public CotacaoFreteCadastroVm ConsultarCotacaoDeFrete(int idProcessoCotacao, string codigoFornecedor)
        {
            var processo = (ProcessoDeCotacaoDeFrete)_processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var fp = processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == codigoFornecedor);

            var item = processo.Itens.First();

            var vm = new CotacaoFreteCadastroVm
            {
                PermiteEditar = processo.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                IdProcessoCotacao = processo.Id,
                CodigoFornecedor = fp.Fornecedor.Codigo,
                Status = processo.Status.Descricao(),
                Requisitos = processo.Requisitos,
                DataLimiteDeRetorno = processo.DataLimiteDeRetorno.Value.ToShortDateString(),
                //Material = processo.Produto.Descricao,
                //Quantidade = processo.Quantidade,
                //UnidadeDeMedida = processo.UnidadeDeMedida.Descricao,
                Material = item.Produto.Descricao,
                Quantidade = item.Quantidade,
                UnidadeDeMedida = item.UnidadeDeMedida.Descricao,
                DataDeValidadeInicial = processo.DataDeValidadeInicial.ToShortDateString(),
                DataDeValidadeFinal = processo.DataDeValidadeFinal.ToShortDateString(),
                Itinerario = processo.Itinerario.Descricao,
                IdFornecedorParticipante = fp.Id
            };

            if (fp.Cotacao != null)
            {

                var cotacao = fp.Cotacao.CastEntity();
                var itemDaCotacao = cotacao.Itens.First();

                //vm.ValorComImpostos = cotacao.ValorComImpostos;
                //vm.ObservacoesDoFornecedor = cotacao.Observacoes;
                //vm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;
                vm.ValorComImpostos = itemDaCotacao.ValorComImpostos;
                vm.ObservacoesDoFornecedor = itemDaCotacao.Observacoes;
                vm.QuantidadeDisponivel = itemDaCotacao.QuantidadeDisponivel;

            }

            return vm;
        }
    }
}