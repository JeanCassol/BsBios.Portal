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

            if (_perfis.Contains(Enumeradores.Perfil.GerenciadorDeQuotas) || _perfis.Contains(Enumeradores.Perfil.CompradorLogistica))
            {
                menus.Add(new MenuQuotas());
                menus.Add(new MenuRelatorios(_perfis));
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
            AdicionarItem("Listar", "ProcessoDeCotacaoDeFrete", "Index");
            AdicionarItem("Adicionar", "ProcessoDeCotacaoDeFrete", "NovoCadastro");
            AdicionarItem("Ordens de Transporte", "OrdemDeTransporte", "Index");
            AdicionarItem("Monitor de Ordens de Transporte", "MonitorDeOrdemDeTransporte", "Index",true);
            AdicionarItem("Conhecimento de Transporte", "ConhecimentoDeTransporte", "Index");
        }
    }

    internal class MenuSuprimentos : Menu
    {
        public MenuSuprimentos()
            : base("Cotações de Materiais")
        {
            AdicionarItem("Listar", "ProcessoDeCotacaoDeMaterial", "Index");
        }
    }

    internal class MenuFornecedor : Menu
    {
        public MenuFornecedor()
            : base("Minhas Cotações")
        {
            AdicionarItem("Cotações de Frete", "ProcessoDeCotacaoDeFrete", "Index");
            //AdicionarItem("Cotações de Material", "ProcessoDeCotacaoDeMaterial", "Index");
            AdicionarItem("Ordens de Transporte", "OrdemDeTransporte", "Index");
        }
    }

    internal class MenuAdministrativo : Menu
    {
        public MenuAdministrativo()
            : base("Administrativo")
        {
            AdicionarItem("Usuários", "Usuario", "Index");
            AdicionarItem("Relatório de Usuários", "RelatorioDeUsuario", "Index");
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
    internal  class  MenuRelatorios: Menu
    {
        public MenuRelatorios(IList<Enumeradores.Perfil> perfis) : base("Relatórios")
        {

            if (perfis.Contains(Enumeradores.Perfil.GerenciadorDeQuotas))
            {
                AdicionarItem("Agendamento de Cargas", "RelatorioAgendamento", "Relatorio");
            }

            if (perfis.Contains(Enumeradores.Perfil.CompradorLogistica))
            {
                AdicionarItem("Processo de Cotação de Frete", "RelatorioDeProcessoDeCotacaoDeFrete", "Relatorio");
                AdicionarItem("Ordem de Transporte", "RelatorioDeOrdemDeTransporte", "Relatorio");
            }

        }
    }

}
