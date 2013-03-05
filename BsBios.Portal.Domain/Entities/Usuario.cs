using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class Usuario : IAggregateRoot
    {
        public virtual string Nome { get; protected set; }
        public virtual string Login { get; protected set; }
        public virtual string Senha { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual Enumeradores.Perfil Perfil { get; set; }

        public Usuario(string nome, string login, string email, Enumeradores.Perfil perfil)
        {
            Nome = nome;
            Login = login;
            Email = email;
            Perfil = perfil;
        }

        protected Usuario(){}


        public virtual void Alterar(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public virtual void CriarSenha(string senhaCriptografada)
        {
            Senha = senhaCriptografada;
        }
    }
}
