using System.Collections.Generic;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Builders
{
    public class MenuUsuarioBuilder
    {
        //private readonly Enumeradores.Perfil _perfil;
        private readonly MenuBuilder _builder;

        public MenuUsuarioBuilder(int perfil)
        {
            var enumPerfil = (Enumeradores.Perfil) perfil;

            if (enumPerfil == Enumeradores.Perfil.Comprador)
            {
                _builder = new MenuCompradorBuilder();
            }
            if (enumPerfil == Enumeradores.Perfil.Fornecedor)
            {
                _builder = new MenuFornecedorBuider();
            }
        }

        public IList<Menu> Construct()
        {
            return _builder.BuildMenu();
        }

    }

    internal abstract class MenuBuilder
    {
        public abstract IList<Menu> BuildMenu();
    }

    internal class MenuCompradorBuilder : MenuBuilder
    {
        public override IList<Menu> BuildMenu()
        {
            var menus = new List<Menu>();
            var menuCadastro = new Menu("Cadastros");
            menuCadastro.AdicionarItem("Produtos", "Produto", "Index");
            menuCadastro.AdicionarItem("Fornecedores", "Fornecedor", "Index");
            menuCadastro.AdicionarItem("Centros", "Centro", "Index");
            menuCadastro.AdicionarItem("Itinerários", "Itinerario", "Index");
            menus.Add(menuCadastro);

            var menuCotacao = new Menu("Cotações");
            menuCotacao.AdicionarItem("Cotações de Material", "ProcessoCotacaoMaterial", "Index");
            menuCotacao.AdicionarItem("Adicionar", "CotacaoFrete", "NovoCadastro");
            menus.Add(menuCotacao);

            return menus;
        }
    }
    internal class MenuFornecedorBuider: MenuBuilder
    {
        public override IList<Menu> BuildMenu()
        {
            var menus = new List<Menu>();
            var menuCotacao = new Menu("Cotações");
            menuCotacao.AdicionarItem("Minhas Cotações", "CotacaoFrete", "Index");
            menus.Add(menuCotacao);

            return menus;
        }
    }
}
