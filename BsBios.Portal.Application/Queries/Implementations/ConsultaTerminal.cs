using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaTerminal : IConsultaTerminal
    {
        private readonly ITerminais _terminais;

        public ConsultaTerminal(ITerminais terminais)
        {
            _terminais = terminais;
        }

        public IList<TerminalCadastroVm> ListarTodos()
        {
            var consulta = (from terminal in _terminais.GetQuery()
                select new TerminalCadastroVm
                {
                    Codigo = terminal.Codigo,
                    Nome = terminal.Nome
                });

            return consulta.ToList();

        }
    }
}