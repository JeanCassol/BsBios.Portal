using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class VerificaPermissaoAgendamento : IVerificaPermissaoAgendamento
    {
        public bool PermiteEditar(AgendamentoDeCarga agendamentoDeCarga, Usuario usuario)
        {
            return
                (!agendamentoDeCarga.Realizado && agendamentoDeCarga.Quota.Data > DateTime.Today 
                && usuario.Perfis.Contains(Enumeradores.Perfil.AgendadorDeCargas));
        }

        public bool PermiteRealizar(AgendamentoDeCarga agendamentoDeCarga, Usuario usuario)
        {
            return !agendamentoDeCarga.Realizado && usuario.Perfis.Contains(Enumeradores.Perfil.ConferidorDeCargas);
        }

        public bool PermiteAdicionar(Quota quota, Usuario usuario)
        {
            return quota.Data >= DateTime.Today && usuario.Perfis.Contains(Enumeradores.Perfil.AgendadorDeCargas);
        }
    }
}
