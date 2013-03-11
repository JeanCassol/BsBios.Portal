using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Builders
{
    public class MenuUsuarioBuilder
    {
        //private readonly Enumeradores.Perfil _perfil;
        //private readonly MenuBuilder _builder;
        private readonly IList<Enumeradores.Perfil>  _perfis;

        public MenuUsuarioBuilder(IList<Enumeradores.Perfil> perfis)
        {
            _perfis = perfis;
        }

        public IList<Menu> Construct()
        {
            var menus = new List<Menu>();
            if (_perfis.Contains(Enumeradores.Perfil.CompradorLogistica) ||
                _perfis.Contains(Enumeradores.Perfil.CompradorSuprimentos))
            {
                menus.Add(new MenuCadastro());
            }
            if (_perfis.Contains(Enumeradores.Perfil.CompradorLogistica))
            {
                menus.Add(new MenuLogistica());
            }
            if (_perfis.Contains(Enumeradores.Perfil.CompradorSuprimentos))
            {
                menus.Add(new MenuSuprimentos());
            }

            if (_perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                menus.Add(new MenuFornecedor());
            }

            if (_perfis.Contains(Enumeradores.Perfil.Administrador))
            {
                menus.Add(new MenuAdministrativo());
            }
            return menus;
        }

    }

    internal class MenuCadastro: Menu
    {
        public MenuCadastro() : base("Cadastros")
        {
            AdicionarItem("Produtos", "Produto", "Index");
            AdicionarItem("Fornecedores", "Fornecedor", "Index");
        }
    }

    internal class MenuLogistica: Menu
    {
        public MenuLogistica() : base("Cotações de Frete")
        {
            AdicionarItem("Listar", "ProcessoCotacaoFrete", "Index");
            AdicionarItem("Adicionar", "ProcessoCotacaoFrete", "NovoCadastro");
        }
    }

    internal class MenuSuprimentos : Menu
    {
        public MenuSuprimentos()
            : base("Cotações de Materiais")
        {
            AdicionarItem("Listar", "ProcessoCotacaoMaterial", "Index");
        }
    }

    internal class MenuFornecedor : Menu
    {
        public MenuFornecedor()
            : base("Minhas Cotações")
        {
            AdicionarItem("Minhas Cotações", "ProcessoCotacaoMaterial", "Index");
        }
    }

    internal class MenuAdministrativo : Menu
    {
        public MenuAdministrativo()
            : base("Administrativo")
        {
            AdicionarItem("Usuários", "Usuario", "Index");
        }
    }


    //internal abstract class MenuBuilder
    //{
    //    public abstract IList<Menu> BuildMenu();
    //}

    //internal class MenuCompradorLogisticaBuilder : MenuBuilder
    //{
    //    public override IList<Menu> BuildMenu()
    //    {
    //        var menus = new List<Menu>();
    //        menus.Add(new MenuCadastro());
    //        menus.Add(new MenuLogistica());

    //        return menus;
    //    }
    //}

    //internal class MenuCompradorSuprimentosBuilder : MenuBuilder
    //{
    //    public override IList<Menu> BuildMenu()
    //    {
    //        var menus = new List<Menu>();
    //        menus.Add(new MenuCadastro());
    //        menus.Add(new MenuLogistica());

    //        return menus;
    //    }
    //}

    //internal class MenuFornecedorBuider: MenuBuilder
    //{
    //    public override IList<Menu> BuildMenu()
    //    {
    //        var menus = new List<Menu>();
    //        var menuCotacao = new Menu("Cotações");
    //        menuCotacao.AdicionarItem("Minhas Cotações", "ProcessoCotacaoMaterial", "Index");
    //        menus.Add(menuCotacao);

    //        return menus;
    //    }
    //}
}
