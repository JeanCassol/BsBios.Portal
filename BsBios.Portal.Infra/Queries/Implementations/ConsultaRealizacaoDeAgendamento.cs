using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaRealizacaoDeAgendamento : IConsultaRealizacaoDeAgendamento
    {
        private readonly IBuilder<Enumeradores.RealizacaoDeAgendamento, RealizacaoDeAgendamentoVm> _builder;

        public ConsultaRealizacaoDeAgendamento(IBuilder<Enumeradores.RealizacaoDeAgendamento, RealizacaoDeAgendamentoVm> builder)
        {
            _builder = builder;
        }

        public IList<RealizacaoDeAgendamentoVm> Listar()
        {
            var realizacoesDeAgendamento = Enum.GetValues(typeof(Enumeradores.RealizacaoDeAgendamento)).Cast<Enumeradores.RealizacaoDeAgendamento>().ToList();
            
            return _builder.BuildList(realizacoesDeAgendamento);

        }
    }
}