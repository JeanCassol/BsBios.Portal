using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Builders
{
    public class MenuUsuarioBuilder
    {
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

            if (_perfis.Contains(Enumeradores.Perfil.GerenciadorDeQuotas))
            {
                menus.Add(new MenuQuotas());
                menus.Add(new MenuRelatorioDeAgendamentos());
            }

            if (_perfis.Contains(Enumeradores.Perfil.AgendadorDeCargas))
            {
                menus.Add(new MenuAgendamentoDeCarga());
            }

            if (_perfis.Contains(Enumeradores.Perfil.ConferidorDeCargas) 
                || _perfis.Contains(Enumeradores.Perfil.ConferidorDeCargasEmDeposito))
            {
                menus.Add(new MenuConferenciaDeCargas());
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
            AdicionarItem("Ordens de Transporte", "OrdemDeTransporte", "Index");
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
            AdicionarItem("Cotações de Frete", "ProcessoCotacaoFrete", "Index");
            AdicionarItem("Cotações de Material", "ProcessoCotacaoMaterial", "Index");
            AdicionarItem("Ordens de Transporte", "OrdemDeTransporte", "Index");
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

    internal class MenuQuotas: Menu
    {
        public MenuQuotas() : base("Quotas")
        {
            AdicionarItem("Cadastro", "Quota", "Cadastro");
        }
        
    }

    internal class MenuAgendamentoDeCarga: Menu
    {
        public MenuAgendamentoDeCarga() : base("Agendamentos de Carga")
        {
            AdicionarItem("Listar", "Quota", "QuotaPorFornecedor");
        }
    }

    internal class MenuConferenciaDeCargas: Menu
    {
        public MenuConferenciaDeCargas() : base("Cargas Agendadas")
        {
            AdicionarItem("Pesquisar", "ConferenciaDeCarga", "Pesquisar");
        }
        
    }
    internal  class  MenuRelatorioDeAgendamentos: Menu
    {
        public MenuRelatorioDeAgendamentos() : base("Relatório de Agendamentos")
        {
            AdicionarItem("Relatórios", "RelatorioAgendamento", "Relatorio");
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
