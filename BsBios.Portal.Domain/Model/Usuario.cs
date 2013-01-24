using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public class Usuario : IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual string Nome { get; protected set; }
        public virtual string Login { get; protected set; }
        public virtual string Senha { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual Enumeradores.Perfil Perfil { get; set; }

        public Usuario(string nome, string login, string senha, string email, Enumeradores.Perfil perfil)
        {
            Nome = nome;
            Login = login;
            Senha = senha;
            Email = email;
            Perfil = perfil;
        }

        protected Usuario(){}


    }
}
