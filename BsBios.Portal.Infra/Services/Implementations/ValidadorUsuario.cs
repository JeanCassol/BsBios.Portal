using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ValidadorUsuario: IValidadorUsuario
    {
        private readonly IUsuarios _usuarios;
        private readonly IProvedorDeCriptografia _provedorDeCriptografia;

        public ValidadorUsuario(IUsuarios usuarios, IProvedorDeCriptografia provedorDeCriptografia)
        {
            _usuarios = usuarios;
            _provedorDeCriptografia = provedorDeCriptografia;
        }

        public UsuarioConectado Validar(string login, string senha)
        {
            login = login.ToLower();
            Usuario usuario = _usuarios.BuscaPorLogin(login);
            if (usuario == null)
            {
                throw new UsuarioNaoCadastradoException(login);
            }
            string senhaCriptografada = _provedorDeCriptografia.Criptografar(senha);
            if (usuario.Senha == senhaCriptografada)
            {
                return new UsuarioConectado(usuario.Login, usuario.Nome,(int) usuario.Perfil);
                    
            }
            throw new SenhaIncorretaException();
        }
    }
}
