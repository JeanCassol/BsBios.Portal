using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using JetBrains.Annotations;
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
                             QuantidadeColetada = ordemDeTransporte.QuantidadeColetada,
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
            bool permiteAdicionarColeta = usuarioConectado.PermiteAlterarColeta();
            _ordensDeTransporte.BuscaPorId(id);

            var ordemDeTransporteCadastroVm =  (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                    let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                    let fornecedor = processoDeCotacao.FornecedorDaMercadoria
                    let deposito = processoDeCotacao.Deposito
                select new OrdemDeTransporteCadastroVm
                {
                    Id = ordemDeTransporte.Id,
                    QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada ,
                    QuantidadeColetada = ordemDeTransporte.QuantidadeColetada ,
                    PermiteAlterar = permiteAlterar,
                    PermiteAdicionarColeta = permiteAdicionarColeta,
                    Transportadora = ordemDeTransporte.Fornecedor.Nome,
                    Cabecalho = new ProcessoDeCotacaoDeFreteCabecalhoVm
                    {
                        Material = processoDeCotacao.Produto.Descricao,
                        UnidadeDeMedida = processoDeCotacao.UnidadeDeMedida.Descricao,
                        Cadencia = processoDeCotacao.Cadencia,
                        Classificacao = processoDeCotacao.Classificacao ? "Sim" : "Não",
                        NumeroDoContrato = processoDeCotacao.NumeroDoContrato,
                        NomeDoFornecedor = fornecedor != null ? fornecedor.Nome : "Não informado",
                        EnderecoDoFornecedor = fornecedor != null ? fornecedor.Endereco : "Não informado",
                        NomeDoDeposito = deposito != null ? deposito.Nome : "Não informado",
                        EnderecoDoDeposito = deposito != null ? deposito.Endereco : "Não informado",
                        DataDeValidadeInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString(),
                        DataDeValidadeFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString(),
                        MunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem.Nome + "/" + processoDeCotacao.MunicipioDeOrigem.UF,
                        MunicipioDeDestino = processoDeCotacao.MunicipioDeDestino.Nome + "/" + processoDeCotacao.MunicipioDeDestino.UF,
                        Requisitos = processoDeCotacao.Requisitos,
                        Quantidade = processoDeCotacao.Quantidade,
                        DataLimiteDeRetorno = processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString(),
                        Itinerario = processoDeCotacao.Itinerario.Descricao,
                        Status = processoDeCotacao.Status.ToString()
                    }

                }).Single();

            var status = (Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao),ordemDeTransporteCadastroVm.Cabecalho.Status);

            ordemDeTransporteCadastroVm.Cabecalho.Status = status.Descricao();

            return ordemDeTransporteCadastroVm;
        }
    }
}