using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.ViewModel;
using BsBios.Portal.Application.Queries.Builders;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaCotacaoDoFornecedor : IConsultaCotacaoDoFornecedor
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IBuilder<Cotacao, CotacaoImpostosVm> _builderImpostos;

        public ConsultaCotacaoDoFornecedor(IProcessosDeCotacao processosDeCotacao, IBuilder<Cotacao, CotacaoImpostosVm> builderImpostos)
        {
            _processosDeCotacao = processosDeCotacao;
            _builderImpostos = builderImpostos;
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
                    DescricaoDoProcessoDeCotacao = processo.RequisicaoDeCompra.Descricao,
                    DataLimiteDeRetorno = processo.DataLimiteDeRetorno.Value.ToShortDateString(),
                    Material = processo.Produto.Descricao,
                    Quantidade = processo.Quantidade,
                    UnidadeDeMedida = processo.UnidadeDeMedida.Descricao,
                    IdFornecedorParticipante = fp.Id
                };

            if (fp.Cotacao != null)
            {

                var cotacao = (CotacaoMaterial)  fp.Cotacao.CastEntity();

                vm.CodigoCondicaoPagamento = cotacao.CondicaoDePagamento.Codigo;
                vm.CodigoIncoterm = cotacao.Incoterm.Codigo;
                vm.DescricaoIncoterm = cotacao.DescricaoIncoterm;
                vm.Mva = cotacao.Mva;
                vm.ValorLiquido = cotacao.ValorLiquido;
                vm.ValorComImpostos = cotacao.ValorComImpostos;
                vm.ObservacoesDoFornecedor = cotacao.Observacoes;
                vm.PrazoDeEntrega = cotacao.PrazoDeEntrega.ToShortDateString();
                vm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;

                vm.Impostos = _builderImpostos.BuildSingle(cotacao);

            }
            else
            {
                vm.Impostos = new CotacaoImpostosVm();
            }

            return vm;

        }

        public CotacaoFreteCadastroVm ConsultarCotacaoDeFrete(int idProcessoCotacao, string codigoFornecedor)
        {
            var processo = (ProcessoDeCotacaoDeFrete)_processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

            var fp = processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == codigoFornecedor);

            Fornecedor fornecedor = processo.FornecedorDaMercadoria;
            Fornecedor deposito = processo.Deposito;
            Municipio municipioDeOrigem = processo.MunicipioDeOrigem;
            Municipio municipioDeDestino = processo.MunicipioDeDestino;

            var vm = new CotacaoFreteCadastroVm
            {
                PermiteEditar = processo.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                PermiteAlterarPreco = !processo.ValorFechado.HasValue,
                IdProcessoCotacao = processo.Id,
                CodigoFornecedor = fp.Fornecedor.Codigo,
                IdFornecedorParticipante = fp.Id,
                ValorComImpostos = processo.ValorFechado,
                Cabecalho = new ProcessoDeCotacaoDeFreteCabecalhoVm
                {
                    Status = processo.Status.Descricao(),
                    Requisitos = processo.Requisitos,
                    DataLimiteDeRetorno = processo.DataLimiteDeRetorno.Value.ToShortDateString(),
                    Material = processo.Produto.Descricao,
                    Quantidade = processo.Quantidade,
                    UnidadeDeMedida = processo.UnidadeDeMedida.Descricao,
                    DataDeValidadeInicial = processo.DataDeValidadeInicial.ToShortDateString(),
                    DataDeValidadeFinal = processo.DataDeValidadeFinal.ToShortDateString(),
                    Itinerario = processo.Itinerario.Descricao,
                    Classificacao = processo.Classificacao ? "Sim" : "Não",
                    NumeroDoContrato = processo.NumeroDoContrato,
                    NomeDoFornecedorDaMercadoria = fornecedor != null ? fornecedor.Nome : "Não informado",
                    EnderecoDoFornecedor = fornecedor != null ? fornecedor.Endereco : "Não informado",
                    NomeDoDeposito = deposito != null ? deposito.Nome : "Não informado",
                    EnderecoDoDeposito = deposito != null ? deposito.Endereco : "Não informado",
                    MunicipioDeOrigem = municipioDeOrigem != null ? municipioDeOrigem.Nome + "/" + municipioDeOrigem.UF : "Não informado" ,
                    MunicipioDeDestino = municipioDeDestino != null ? municipioDeDestino.Nome + "/" + municipioDeDestino.UF : "Não informado",
                    Resposta = fp.Resposta.Descricao()
                }
            };

            if (fp.Cotacao != null)
            {

                var cotacao = (CotacaoDeFrete) fp.Cotacao.CastEntity();

                vm.ValorComImpostos = cotacao.ValorComImpostos;
                vm.ObservacoesDoFornecedor = cotacao.Observacoes;
                vm.QuantidadeDisponivel = cotacao.QuantidadeDisponivel;

                vm.Cabecalho.Cadencia = cotacao.Cadencia == null ? processo.Cadencia : cotacao.Cadencia.Value;

            }

            return vm;
        }
    }
}