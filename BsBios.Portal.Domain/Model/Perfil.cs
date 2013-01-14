﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public abstract class Perfil
    {
        public IList<Menu> Menus { get; private set; }
        public bool PermiteLogin { get; set; }

        protected Perfil(bool permiteLogin)
        {
            Menus = new List<Menu>();
            PermiteLogin = permiteLogin;
        }
        protected void AdicionarMenu(Menu menu)
        {
            Menus.Add(menu);
        }
    }

    public class PerfilComprador: Perfil
    {
        public PerfilComprador():base(true)
        {
            var menuCadastro = new Menu("Cadastros");
            menuCadastro.AdicionarItem("Produtos","Produto","Index");
            menuCadastro.AdicionarItem("Fornecedores", "Fornecedor", "Index");
            menuCadastro.AdicionarItem("Centros", "Centro", "Index");
            menuCadastro.AdicionarItem("Itinerários", "Itinerario", "Index");
            Menus.Add(menuCadastro);

            var menuCotacao = new Menu("Cotações");
            menuCotacao.AdicionarItem("Cotações de Frete", "CotacaFrete","Index");
            menuCotacao.AdicionarItem("Adicionar", "CotacaoFrete", "Adicionar");
            Menus.Add(menuCotacao);
        }
    }

    public class PerfilFornecedor: Perfil
    {
        public PerfilFornecedor(): base(true)
        {
            var menuCotacao = new Menu("Cotações");
            menuCotacao.AdicionarItem("Minhas Cotações", "CotacaoFrete", "Index");
        }
    }

    public class PerfilNaoAutorizado: Perfil
    {
        public PerfilNaoAutorizado():base(false)
        {
            PermiteLogin = false;
        }
    }


}
