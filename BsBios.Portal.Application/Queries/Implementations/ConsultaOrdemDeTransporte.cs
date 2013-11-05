using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaOrdemDeTransporte : IConsultaOrdemDeTransporte
    {
        private readonly IOrdensDeTransporte _ordensDeTransporte;
        

        public ConsultaOrdemDeTransporte(IOrdensDeTransporte ordensDeTransporte)
        {
            _ordensDeTransporte = ordensDeTransporte;
        }

        public KendoGridVm Listar(PaginacaoVm paginacao, OrdemDeTransporteListagemFiltroVm filtro)
        {
            _ordensDeTransporte.CodigoDoFornecedorContendo(filtro.CodigoDoFornecedor);

            _ordensDeTransporte.NomeDoFornecedorContendo(filtro.NomeDoFornecedor);

            //var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

            //fornecedores
            //    .NomeContendo(filtro.NomeDoFornecedor)
            //    .CodigoContendo(filtro.CodigoDoFornecedor);

            //var query = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
            //    join fornecedor in fornecedores.GetQuery()
            //    on ordemDeTransporte.Fornecedor.Codigo equals fornecedor.Codigo
            //    select new OrdemDeTransporteListagemVm
            //    {
            //        Id = ordemDeTransporte.Id,
            //        CodigoDoFornecedor = fornecedor.Codigo,
            //        NomeDoFornecedor = fornecedor.Nome,

            //        Material = ordemDeTransporte.ProcessoDeCotacaoDeFrete.Produto.Descricao,
            //        QuantidadeAdquirida = ordemDeTransporte.QuantidadeAdquirida,
            //        QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada
            //    });

            var query = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                         select new OrdemDeTransporteListagemVm
                         {
                             Id = ordemDeTransporte.Id,
                             CodigoDoFornecedor = ordemDeTransporte.Fornecedor.Codigo,
                             NomeDoFornecedor = ordemDeTransporte.Fornecedor.Nome,
                             Material = ordemDeTransporte.ProcessoDeCotacaoDeFrete.Produto.Descricao,
                             QuantidadeAdquirida = ordemDeTransporte.QuantidadeAdquirida,
                             QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada
                         });

            return new KendoGridVm
            {
                QuantidadeDeRegistros = query.Count(),
                Registros = query.Skip(paginacao.Skip).Take(paginacao.Take).ToList().Cast<ListagemVm>().ToList()
            };

        }

        public OrdemDeTransporteCadastroVm ConsultarOrdem(int id)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            bool permiteAlterar = usuarioConectado.PermiteAlterarOrdemDeTransporte();
            _ordensDeTransporte.BuscaPorId(id);

            return (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                    let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                    let fornecedor = processoDeCotacao.Fornecedor
                select new OrdemDeTransporteCadastroVm
                {
                    Id = ordemDeTransporte.Id,
                    Material = processoDeCotacao.Produto.Descricao,
                    UnidadeDeMedida = processoDeCotacao.UnidadeDeMedida.Descricao,
                    QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada ,
                    QuantidadeColetada = ordemDeTransporte.QuantidadeColetada ,
                    Cadencia = processoDeCotacao.Cadencia ,
                    Classificacao = processoDeCotacao.Classificacao ? "Sim": "Não" ,
                    Transportadora = ordemDeTransporte.Fornecedor.Nome,
                    NumeroDoContrato = processoDeCotacao.NumeroDoContrato ,
                    NomeDoFornecedor =  fornecedor != null ? fornecedor.Nome: "Não informado",
                    EnderecoDoFornecedor = fornecedor != null ? fornecedor.Endereco: "Não informado",
                    DataDeValidadeInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString() ,
                    DataDeValidadeFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString(),
                    MunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem.ToString(),
                    MunicipioDeDestino = processoDeCotacao.MunicipioDeDestino.ToString() ,
                    Requisitos = processoDeCotacao.Requisitos ,
                    PermiteAlterar = permiteAlterar

                }).Single();
        }
    }
}