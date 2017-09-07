using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroUnidadeDeMedida:ICadastroUnidadeDeMedida
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnidadesDeMedida _unidadesDeMedida;
        private IList<UnidadeDeMedida> _unidadesDeMedidaConsultadas;
        public CadastroUnidadeDeMedida(IUnitOfWork unitOfWork, IUnidadesDeMedida unidadesDeMedida)
        {
            _unitOfWork = unitOfWork;
            _unidadesDeMedida = unidadesDeMedida;
        }

        private void AtualizarUnidadeDeMedida(UnidadeDeMedidaCadastroVm unidadeDeMedidaCadastroVm)
        {
            UnidadeDeMedida unidadeDeMedida =
                _unidadesDeMedidaConsultadas.SingleOrDefault(
                    x => x.CodigoInterno == unidadeDeMedidaCadastroVm.CodigoInterno);
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
                foreach (var unidadeDeMedidaCadastroVm in unidadesDeMedida.Where(unidadeDeMedidaCadastroVm => 
                    string.IsNullOrEmpty(unidadeDeMedidaCadastroVm.Descricao)))
                {
                    unidadeDeMedidaCadastroVm.Descricao = unidadeDeMedidaCadastroVm.CodigoExterno;
                }
                _unitOfWork.BeginTransaction();
                _unidadesDeMedidaConsultadas =
                    _unidadesDeMedida.FiltraPorListaDeCodigosInternos(
                        unidadesDeMedida.Select(x => x.CodigoInterno).ToArray()).List();
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
