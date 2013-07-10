using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;

namespace BsBios.Portal.Domain.Entities
{
    public class Usuario : IAggregateRoot
    {
        public virtual string Nome { get; protected set; }
        public virtual string Login { get; protected set; }
        public virtual string Senha { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual Enumeradores.StatusUsuario Status { get; protected set; }
        public virtual IList< Enumeradores.Perfil> Perfis { get; set; }
        public virtual Permissao Permissao { get; protected internal set; }

        public Usuario(string nome, string login, string email):this()
        {
            Nome = nome;
            Login = login;
            Email = email;
            Status = Enumeradores.StatusUsuario.Ativo;
        }

        protected Usuario()
        {
            Perfis = new List<Enumeradores.Perfil>();
            Permissao = new Permissao(this);
        }


        public virtual void Alterar(string nome, string email)
        {
            Nome = nome;
            Email = email;
        }

        public virtual void CriarSenha(string senhaCriptografada)
        {
            Senha = senhaCriptografada;
        }

        public virtual void AdicionarPerfil(Enumeradores.Perfil perfil)
        {
            Perfis.Add(perfil);
            
        }
        public virtual void RemoverPerfil(Enumeradores.Perfil perfil)
        {
            Perfis.Remove(perfil);

        }

        public virtual void Bloquear()
        {
            Status = Enumeradores.StatusUsuario.Bloqueado;
        }

        public virtual void Ativar()
        {
            Status = Enumeradores.StatusUsuario.Ativo;
        }

        public virtual void AlterarSenha(string senhaAtualCriptografada, string senhaNovaCriptografada)
        {
            if (senhaAtualCriptografada != Senha)
            {
                throw new SenhaIncorretaException("A senha atual informada está incorreta");
            }
            Senha = senhaNovaCriptografada;
        }
    }

    public class Permissao
    {
        private readonly Usuario _usuario;
        internal Permissao(Usuario usuario)
        {
            _usuario = usuario;
        }
        public bool PermiteVisualizarCustos
        {
            get
            {
                return _usuario.Perfis.Contains(Enumeradores.Perfil.CompradorSuprimentos);
            }
        }
    }
}
