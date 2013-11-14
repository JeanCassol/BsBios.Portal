using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
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

            if (filtro.NumeroDaOrdemDeTransporte.HasValue)
            {
                _ordensDeTransporte.BuscaPorId(filtro.NumeroDaOrdemDeTransporte.Value);    
            }
            

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
                    let municipioDeOrigem = processoDeCotacao.MunicipioDeOrigem
                    let municipioDeDestino = processoDeCotacao.MunicipioDeDestino
                select new OrdemDeTransporteCadastroVm
                {
                    Id = ordemDeTransporte.Id,
                    QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada ,
                    QuantidadeColetada = ordemDeTransporte.QuantidadeColetada ,
                    PrecoUnitario = ordemDeTransporte.PrecoUnitario,
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
                        CnpjDoFornecedor = fornecedor != null ? fornecedor.Cnpj : "Não informado",
                        EnderecoDoFornecedor = fornecedor != null ? fornecedor.Endereco : "Não informado",
                        NomeDoDeposito = deposito != null ? deposito.Nome : "Não informado",
                        EnderecoDoDeposito = deposito != null ? deposito.Endereco : "Não informado",
                        DataDeValidadeInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString(),
                        DataDeValidadeFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString(),
                        MunicipioDeOrigem = municipioDeOrigem != null ? municipioDeOrigem.Nome + "/" + municipioDeOrigem.UF: "",
                        MunicipioDeDestino = municipioDeDestino != null ? municipioDeDestino.Nome + "/" + processoDeCotacao.MunicipioDeDestino.UF: "",
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

        public KendoGridVm ListarColetas(PaginacaoVm paginacao, int idDaOrdemDeTransporte)
        {
            _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte);

            var query = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                from coleta in ordemDeTransporte.Coletas
                select new ColetaListagemVm
                {
                    IdDaOrdemDeTransporte = idDaOrdemDeTransporte,
                    IdColeta = coleta.Id,
                    DataDePrevisaoDeChegada = coleta.DataDePrevisaoDeChegada.ToShortDateString(),
                    Placa = coleta.Placa,
                    Peso = coleta.Peso,
                    Motorista = coleta.Motorista,
                    Realizado = coleta.Realizado ? "Sim" : "Não"
                });

            return new KendoGridVm
            {
                QuantidadeDeRegistros = query.Count(),
                Registros = query.Skip(paginacao.Skip).Take(paginacao.Take).Cast<ListagemVm>().ToList()
            };
        }

        public ColetaVm ConsultaColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte);

            var coletaVm = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                from coleta in ordemDeTransporte.Coletas
                where coleta.Id == idDaColeta
                let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                select new ColetaVm
                {
                    IdDaOrdemDeTransporte = idDaOrdemDeTransporte,
                    IdColeta = coleta.Id,
                    Placa = coleta.Placa,
                    Motorista = coleta.Motorista,
                    Peso = coleta.Peso,
                    PrecoUnitario = ordemDeTransporte.PrecoUnitario,
                    ValorDoFrete = coleta.ValorDoFrete,
                    UnidadeDeMedida = processoDeCotacao.UnidadeDeMedida.Descricao,
                    DataDePrevisaoDeChegada = coleta.DataDePrevisaoDeChegada.ToShortDateString(),
                    PermiteEditar = true,

                }).Single();

            return coletaVm;
        }

        public IList<NotaFiscalDeColetaVm> NotasFiscaisDaColeta(int iddDaOrdemDeTransporte, int idColeta)
        {
            _ordensDeTransporte.BuscaPorId(iddDaOrdemDeTransporte);

            var notasFiscais = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                         from coleta in ordemDeTransporte.Coletas
                         from notaFiscal in coleta.NotasFiscais
                         let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                         let fornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria
                         select new NotaFiscalDeColetaVm
                         {
                             Peso = notaFiscal.Peso,
                             Id = notaFiscal.Id,
                             DataDeEmissao = notaFiscal.DataDeEmissao.ToShortDateString(),
                             Numero = notaFiscal.Numero,
                             Serie = notaFiscal.Serie,
                             Valor = notaFiscal.Valor,
                             NumeroDoContrato = processoDeCotacao.NumeroDoContrato,
                             CnpjDoEmitente = fornecedorDaMercadoria.Cnpj,
                             NomeDoEmitente = fornecedorDaMercadoria.Nome
                         }).ToList();

            return notasFiscais;

        }
    }
}