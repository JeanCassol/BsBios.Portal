using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaUsuario : IConsultaUsuario
    {
        private readonly IUsuarios _usuarios;
        private readonly IBuilder<Usuario, UsuarioConsultaVm> _builderUsuario;
        private readonly IBuilder<Enumeradores.Perfil, PerfilVm> _builderPerfil;
        public ConsultaUsuario(IUsuarios usuarios, IBuilder<Usuario, UsuarioConsultaVm> builderUsuario, IBuilder<Enumeradores.Perfil, PerfilVm> builderPerfil)
        {
            _usuarios = usuarios;
            _builderUsuario = builderUsuario;
            _builderPerfil = builderPerfil;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, UsuarioFiltroVm usuarioFiltroVm)
        {
            _usuarios
                .LoginContendo(usuarioFiltroVm.Login)
                .NomeContendo(usuarioFiltroVm.Nome);
            
            var kendoGridVmn = new KendoGridVm()
            {
                QuantidadeDeRegistros = _usuarios.Count(),
                Registros =
                    _builderUsuario.BuildList(_usuarios.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List())
                            .Cast<ListagemVm>()
                            .ToList()

            };
            return kendoGridVmn;

        }

        public UsuarioConsultaVm ConsultaPorLogin(string login)
        {
            return _builderUsuario.BuildSingle(_usuarios.BuscaPorLogin(login));
        }

        public IList<PerfilVm> PerfisDoUsuario(string login)
        {
            return _builderPerfil.BuildList(_usuarios.BuscaPorLogin(login).Perfis);
        }
    }
}