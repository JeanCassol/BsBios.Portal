using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroItinerario:ICadastroItinerario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItinerarios _itinerarios;
        public CadastroItinerario(IUnitOfWork unitOfWork, IItinerarios itinerarios)
        {
            _unitOfWork = unitOfWork;
            _itinerarios = itinerarios;
        }

        private void AtualizarItinerario(ItinerarioCadastroVm itinerarioCadastroVm)
        {
            Itinerario itinerario = _itinerarios.BuscaPeloCodigo(itinerarioCadastroVm.Codigo).Single();
            if (itinerario != null)
            {
                itinerario.AtualizaDescricao(itinerarioCadastroVm.Descricao);
            }
            else
            {
                itinerario = new Itinerario(itinerarioCadastroVm.Codigo, itinerarioCadastroVm.Descricao);
            }
            _itinerarios.Save(itinerario);
        }

        public  void AtualizarItinerarios(IList<ItinerarioCadastroVm> itinerarios)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                foreach (var itinerarioCadastroVm in itinerarios)
                {
                    AtualizarItinerario(itinerarioCadastroVm);
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
