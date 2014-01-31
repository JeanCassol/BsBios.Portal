using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroTerminal : ICadastroTerminal
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITerminais _terminais;
        private IList<Terminal> _terminaisConsultados;

        public CadastroTerminal(IUnitOfWork unitOfWork, ITerminais terminais)
        {
            _unitOfWork = unitOfWork;
            _terminais = terminais;
        }

        private void AtualizarTerminal(TerminalCadastroVm terminalCadastroVm)
        {
            Terminal terminal = _terminaisConsultados.SingleOrDefault(x => x.Codigo == terminalCadastroVm.Codigo);
            if (terminal != null)
            {
                terminal.Atualizar(terminalCadastroVm.Nome, terminalCadastroVm.Cidade);
            }
            else
            {
                terminal = new Terminal(terminalCadastroVm.Codigo, terminalCadastroVm.Nome, terminalCadastroVm.Cidade);
            }
            _terminais.Save(terminal);
        }

        public void AtualizarTerminais(ListaTerminal terminais)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _terminaisConsultados = _terminais.BuscaListaPorCodigo(terminais.Select(x => x.Codigo).ToArray()).List();
                foreach (var terminalCadastroVm in terminais)
                {
                    AtualizarTerminal(terminalCadastroVm);
                }

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}