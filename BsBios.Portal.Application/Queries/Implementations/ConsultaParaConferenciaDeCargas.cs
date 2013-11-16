using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Linq;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaParaConferenciaDeCargas : IConsultaParaConferenciaDeCargas
    {
        private readonly ISession _session;
        private readonly UsuarioConectado _usuarioConectado;

        public ConsultaParaConferenciaDeCargas(ISession session, UsuarioConectado usuarioConectado)
        {
            this._session = session;
            _usuarioConectado = usuarioConectado;
        }

        public KendoGridVm Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro)
        {

            if (_usuarioConectado.Perfis.Contains(Enumeradores.Perfil.ConferidorDeCargasEmDeposito))
            {
                filtro.CodigoDeposito = _usuarioConectado.Login;
            }

            var queryable = _session.Query<ConferenciaDeCargaPesquisaResultadoVm>();

            queryable = queryable.Where(x => x.CodigoDeposito == filtro.CodigoDeposito && x.CodigoTerminal == filtro.CodigoTerminal);

            if (filtro.RealizacaoDeAgendamento.HasValue)
            {
                var realizacaoDeAgendamento = (Enumeradores.RealizacaoDeAgendamento) Enum.Parse(typeof(Enumeradores.RealizacaoDeAgendamento),
                    Convert.ToString(filtro.RealizacaoDeAgendamento.Value));
                bool realizado = (realizacaoDeAgendamento == Enumeradores.RealizacaoDeAgendamento.Realizado);
                queryable = queryable.Where(x => x.Realizado == realizado);

            }
            if (!string.IsNullOrEmpty(filtro.Placa))
            {
                var placa = filtro.Placa.ToLower().Replace("-", "");
                queryable = queryable.Where(x => x.Placa.ToLower() == placa);
            }

            if (!string.IsNullOrEmpty(filtro.DataAgendamento))
            {
                queryable = queryable.Where(x => x.DataAgendamento == Convert.ToDateTime(filtro.DataAgendamento));
            }

            if (!string.IsNullOrEmpty(filtro.NomeDoFornecedor))
            {
                queryable = queryable.Where(x => x.NomeEmitente.ToLower().Contains(filtro.NomeDoFornecedor.ToLower()));
            }

            if (!string.IsNullOrEmpty(filtro.NumeroNf))
            {
                queryable = queryable.Where(x => x.NumeroNf == filtro.NumeroNf);
            }

            return new KendoGridVm()
            {
                QuantidadeDeRegistros = queryable.Count(),
                Registros = queryable.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).Cast<ListagemVm>().ToList()
            };
        }
    }
}