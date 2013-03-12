using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroUnidadeDeMedida:ICadastroUnidadeDeMedida
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnidadesDeMedida _unidadesDeMedida;
        public CadastroUnidadeDeMedida(IUnitOfWork unitOfWork, IUnidadesDeMedida unidadesDeMedida)
        {
            _unitOfWork = unitOfWork;
            _unidadesDeMedida = unidadesDeMedida;
        }

        private void AtualizarUnidadeDeMedida(UnidadeDeMedidaCadastroVm unidadeDeMedidaCadastroVm)
        {
            UnidadeDeMedida unidadeDeMedida = _unidadesDeMedida.BuscaPeloCodigoInterno(unidadeDeMedidaCadastroVm.CodigoInterno).Single();
            if (unidadeDeMedida != null)
            {
                unidadeDeMedida.Atualizar(unidadeDeMedidaCadastroVm.CodigoExterno,unidadeDeMedidaCadastroVm.Descricao);
            }
            else
            {
                unidadeDeMedida = new UnidadeDeMedida(unidadeDeMedidaCadastroVm.CodigoInterno,unidadeDeMedidaCadastroVm.CodigoExterno, unidadeDeMedidaCadastroVm.Descricao);
            }
            _unidadesDeMedida.Save(unidadeDeMedida);
        }

        public  void AtualizarUnidadesDeMedida(IList<UnidadeDeMedidaCadastroVm> unidadesDeMedida)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                foreach (var unidadeDeMedidaCadastroVm in unidadesDeMedida)
                {
                    AtualizarUnidadeDeMedida(unidadeDeMedidaCadastroVm);
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
