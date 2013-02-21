using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Domain.Model
{
    public class Usuario : IAggregateRoot
    {
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


        public virtual void Alterar(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }
    }
}
