using System;
using System.Collections.Generic;
using System.Linq;
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
        private IList<Itinerario> _itinerariosConsultados;
        public CadastroItinerario(IUnitOfWork unitOfWork, IItinerarios itinerarios)
        {
            _unitOfWork = unitOfWork;
            _itinerarios = itinerarios;
        }

        private void AtualizarItinerario(ItinerarioCadastroVm itinerarioCadastroVm)
        {
            Itinerario itinerario = _itinerariosConsultados.SingleOrDefault(x => x.Codigo == itinerarioCadastroVm.Codigo);
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
                _itinerariosConsultados =
                    _itinerarios.FiltraPorListaDeCodigos(itinerarios.Select(x => x.Codigo).ToArray()).List();

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
