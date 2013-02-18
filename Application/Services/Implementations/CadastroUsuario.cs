using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Implementation
{
    public class CadastroUsuario: ICadastroUsuario
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarios _usuarios;
        private readonly IProvedorDeCriptografia _provedorDeCriptografia;

        public CadastroUsuario(IUnitOfWork unitOfWork, IUsuarios usuarios, IProvedorDeCriptografia provedorDeCriptografia)
        {
            _unitOfWork = unitOfWork;
            _usuarios = usuarios;
            _provedorDeCriptografia = provedorDeCriptografia;
        }

        public void Novo(UsuarioVm usuarioVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                string senhaCriptografada = _provedorDeCriptografia.Criptografar(usuarioVm.Senha);
                var novoUsuario = new Usuario(usuarioVm.Nome, usuarioVm.Login, senhaCriptografada
                    , usuarioVm.Email, (Enumeradores.Perfil)Enum.Parse(typeof(Enumeradores.Perfil), Convert.ToString(usuarioVm.CodigoPerfil)));
                _usuarios.Save(novoUsuario);
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
