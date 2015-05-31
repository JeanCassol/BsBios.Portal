using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaDeRelatorioDeUsuario : IConsultaDeRelatorioDeUsuario
    {

        private readonly IUsuarios _usuarios;

        public ConsultaDeRelatorioDeUsuario(IUsuarios usuarios)
        {
            _usuarios = usuarios;
        }

        public IEnumerable<RelatorioDeUsuarioListagemVm> Listar(RelatorioDeUsuarioFiltroVm filtro)
        {
            _usuarios
                .LoginContendo(filtro.Login)
                .NomeContendo(filtro.Nome)
                .EmailContendo(filtro.Email);

            if (filtro.Status > 0)
            {
                var filtroDeStatus = (Enumeradores.StatusUsuario) Enum.Parse(typeof (Enumeradores.StatusUsuario), Convert.ToString(filtro.Status));

                _usuarios.ComStatus(filtroDeStatus);
            }

            IList<Usuario> usuarios = _usuarios
                .IncluirPerfis()
                .List();

            return usuarios.Select(usuario => new RelatorioDeUsuarioListagemVm
            {
                Login = usuario.Login,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Status = usuario.Status.Descricao(),
                Perfis = string.Join(" - ", usuario.Perfis.Select(perfil => perfil.Descricao())) 
            });

        }
    }
}