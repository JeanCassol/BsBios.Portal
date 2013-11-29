using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate.Linq;
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

            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            bool usuarioLogadoEUmaTransportadora = usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor);

            if (usuarioLogadoEUmaTransportadora)
            {
                filtro.CodigoDaTransportadora = usuarioConectado.Login;
            }


            _ordensDeTransporte.CodigoDaTransportadoraContendo(filtro.CodigoDaTransportadora);

            _ordensDeTransporte.NomeDaTransportadoraContendo(filtro.NomeDaTransportadora);

            if (filtro.NumeroDaOrdemDeTransporte.HasValue)
            {
                _ordensDeTransporte.BuscaPorId(filtro.NumeroDaOrdemDeTransporte.Value);    
            }

            if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
            {
                _ordensDeTransporte.NomeDoFornecedorDaMercadoriaContendo(filtro.NomeDoFornecedorDaMercadoria);
            }

            if (!string.IsNullOrEmpty(filtro.NumeroDoContrato))
            {
                _ordensDeTransporte.ComNumeroDeContrato(filtro.NumeroDoContrato);
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoMunicipioDeOrigem))
            {
                _ordensDeTransporte.ComOrigemNoMunicipio(filtro.CodigoDoMunicipioDeOrigem);
            }
            
            var query = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                         select new OrdemDeTransporteListagemVm
                         {
                             Id = ordemDeTransporte.Id,
                             CodigoDoFornecedor = usuarioLogadoEUmaTransportadora ? ordemDeTransporte.ProcessoDeCotacaoDeFrete.FornecedorDaMercadoria.Codigo : ordemDeTransporte.Fornecedor.Codigo,
                             NomeDoFornecedor = usuarioLogadoEUmaTransportadora ? ordemDeTransporte.ProcessoDeCotacaoDeFrete.FornecedorDaMercadoria.Nome : ordemDeTransporte.Fornecedor.Nome,
                             Material = ordemDeTransporte.ProcessoDeCotacaoDeFrete.Produto.Descricao,
                             QuantidadeColetada = ordemDeTransporte.QuantidadeColetada,
                             QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada,
                             QuantidadeRealizada = ordemDeTransporte.QuantidadeRealizada
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
                    QuantidadeRealizada = ordemDeTransporte.QuantidadeRealizada,
                    PrecoUnitario = ordemDeTransporte.PrecoUnitario,
                    PermiteAlterar = permiteAlterar,
                    PermiteAdicionarColeta = permiteAdicionarColeta,
                    Transportadora = ordemDeTransporte.Fornecedor.Nome,
                    Cabecalho = new ProcessoDeCotacaoDeFreteCabecalhoVm
                    {
                        Material = processoDeCotacao.Produto.Descricao,
                        UnidadeDeMedida = processoDeCotacao.UnidadeDeMedida.Descricao,
                        Cadencia = ordemDeTransporte.Cadencia,
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
                orderby coleta.DataDaColeta descending
                orderby coleta.Placa.ToLower()
                select new ColetaListagemVm
                {
                    IdDaOrdemDeTransporte = idDaOrdemDeTransporte,
                    IdColeta = coleta.Id,
                    DataDaColeta = coleta.DataDaColeta.ToShortDateString(),
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

        public ColetaVm ConsultaColeta(int idDaOrdemDeTransporte, int idDaColeta, UsuarioConectado usuarioConectado)
        {
            _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte);

            bool permiteAlterarColeta = usuarioConectado.PermiteAlterarColeta();

            var coletaVm = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                from coleta in ordemDeTransporte.Coletas
                where coleta.Id == idDaColeta
                let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                let fornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria
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
                    DataDaColeta = coleta.DataDaColeta.ToShortDateString(),
                    DataDePrevisaoDeChegada = coleta.DataDePrevisaoDeChegada.ToShortDateString(),
                    CnpjDoEmitente = fornecedorDaMercadoria.Cnpj,
                    NomeDoEmitente = fornecedorDaMercadoria.Nome ,
                    NumeroDoContrato = processoDeCotacao.NumeroDoContrato,
                    NomeDaTransportadora = ordemDeTransporte.Fornecedor.Nome,
                    PermiteEditar = permiteAlterarColeta && !coleta.Realizado,

                }).Single();

            return coletaVm;
        }

        public IList<NotaFiscalDeColetaVm> NotasFiscaisDaColeta(int iddDaOrdemDeTransporte, int idColeta)
        {
            _ordensDeTransporte.BuscaPorId(iddDaOrdemDeTransporte);

            var notasFiscais = (from ordemDeTransporte in _ordensDeTransporte.GetQuery()
                         from coleta in ordemDeTransporte.Coletas
                         from notaFiscal in coleta.NotasFiscais
                         where coleta.Id == idColeta
                         let processoDeCotacao = ordemDeTransporte.ProcessoDeCotacaoDeFrete
                         let fornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria
                         select new NotaFiscalDeColetaVm
                         {
                             Peso = notaFiscal.Peso,
                             Id = notaFiscal.Id,
                             DataDeEmissao = notaFiscal.DataDeEmissao.ToShortDateString(),
                             Numero = notaFiscal.Numero,
                             NumeroDoConhecimento = notaFiscal.NumeroDoConhecimento,
                             Serie = notaFiscal.Serie,
                             Valor = notaFiscal.Valor
                         }).ToList();

            return notasFiscais;

        }


        public decimal CalcularQuantidadeLiberadaPeloProcessoDeCotacao(int idDoProcessoDeCotacaoDeFrete)
        {
            var unitOfWork = ObjectFactory.GetInstance<IUnitOfWorkNh>();

            var quantidade = unitOfWork.Session.Query<OrdemDeTransporte>()
                .Where(ot => ot.ProcessoDeCotacaoDeFrete.Id == idDoProcessoDeCotacaoDeFrete)
                .Sum(ot => ot.QuantidadeLiberada);

            return quantidade;

        }
    }
}