using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
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

        public IList<MonitorDeOrdemDeTransporteVm> ListagemDoMonitor(MonitorDeOrdemDeTransporteFiltroVm filtro)
        {
            var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            var session = unitOfWorkNh.Session;
            var itens = session.GetNamedQuery("MONITORDEORDEMDETRANSPORTE")
                .SetParameter("p_dataInicial", filtro.DataInicial)
                .SetParameter("p_dataFinal", filtro.DataFinal)
                
                .SetResultTransformer(Transformers.AliasToBean<MonitorDeOrdemDeTransporteVm>())
                .List<MonitorDeOrdemDeTransporteVm>();

            return itens;
        }

        public IList<MonitorDeOrdemDeTransporteVm> ListagemDoMonitor2(MonitorDeOrdemDeTransporteFiltroVm filtro)
        {
            var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            ISession session = unitOfWorkNh.Session;

            //OrdemDeTransporte ordemDeTransporte = null;
            Fornecedor fornecedor = null;
            ProcessoDeCotacaoDeFrete processoDeCotacao = null;
            Produto produto = null;
            //Coleta coleta = null;

            //var subQueryChegadaNoDia = QueryOver.Of<Coleta>()
            //    .Where(c => c.DataDePrevisaoDeChegada == DateTime.Today)
            //    .SelectList(list => list.SelectSubQuery())

            Expression<Func<OrdemDeTransporte, bool>> whereDoPeriodo =
                ordem => processoDeCotacao.DataDeValidadeFinal >= filtro.DataInicial
                         && processoDeCotacao.DataDeValidadeInicial <= filtro.DataFinal;

            MonitorDeOrdemDeTransporteVm registro = null;

            //parâmetro columnAliases não funciona. Tenho que colocar o AS "" junto com a expressão SQL.
            var projecaoDeQuantidadeEmTransito = Projections.SqlProjection("SUM(QuantidadeColetada - QuantidadeRealizada) AS \"QuantidadeEmTransito\" ", 
                new[] { "QuantidadeEmTransito" }, new IType[] { NHibernateUtil.Decimal });

            var projecaoDeQuantidadePendente = Projections.SqlProjection("SUM(QuantidadeLiberada - QuantidadeColetada) AS \"QuantidadePendente\" ", 
                new[] { "QuantidadePendente" }, new IType[] { NHibernateUtil.Decimal });

            var queryQuantidadeLiberada = session.QueryOver<OrdemDeTransporte>(/*() => ordemDeTransporte*/)
                .Where(whereDoPeriodo)
                .JoinAlias(ordem => ordem.Fornecedor, () => fornecedor)
                .JoinAlias(ordem => ordem.ProcessoDeCotacaoDeFrete, () => processoDeCotacao)
                .JoinAlias(ordem => processoDeCotacao.Produto, () => produto)
                //.JoinAlias(ordem => ordem.Coletas, () => coleta)
                .SelectList(list => list
                    .SelectGroup(g => fornecedor.Nome).WithAlias(() => registro.Fornecedor)
                    .SelectGroup(g => produto.Descricao).WithAlias(() => registro.Material)
                    .SelectSum(g => g.QuantidadeLiberada).WithAlias(() => registro.QuantidadeLiberada)
                    .Select(projecaoDeQuantidadeEmTransito).WithAlias(() => registro.QuantidadeEmTransito)
                    .Select(projecaoDeQuantidadePendente).WithAlias(() => registro.QuantidadePendente)
                    .SelectSubQuery(
                        QueryOver.Of<Coleta>()
                        .Where(c => c.DataDePrevisaoDeChegada == DateTime.Today /*&& c.OrdemDeTransporte.Id == ordemDeTransporte.Id*/)
                    //select subquery tem problema porque não consigo ligar o id da ordem de transporte da query principal com o id da ordem de transporte da coleta,
                    //já que não uso relacionamento bidirecional.
                    //.And(Restrictions.EqProperty("OrdemDeTransporte.Id","Coleta.IdOrdemTransporte"))
                        .SelectList(lista => lista.SelectSum(m => m.Peso))
                    ).WithAlias(() => registro.PrevisaoDeChegadaNoDia)
                )
                
                    .TransformUsing(Transformers.AliasToBean<MonitorDeOrdemDeTransporteVm>())
                    //.TransformUsing(Transformers.DistinctRootEntity)
                    .List<MonitorDeOrdemDeTransporteVm>();



            return queryQuantidadeLiberada;

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