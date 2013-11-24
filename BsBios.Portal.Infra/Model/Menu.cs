using System.Collections.Generic;

namespace BsBios.Portal.Infra.Model
{
    public abstract class Menu
    {
        public string Descricao { get; set; }    
        public IList<MenuItem> Itens { get; protected set; }

        protected Menu(string descricao)
        {
            Descricao = descricao;
            Itens = new List<MenuItem>();
        }
        public void AdicionarItem(string descricao, string controller, string action, bool abrirEmNovaJanela = false)
        {
            Itens.Add(new MenuItem(descricao, controller, action,abrirEmNovaJanela));
        }

    }

    public class MenuItem
    {
        public MenuItem(string descricao, string controller, string action, bool abrirEmNovaJanela = false)
        {
            Descricao = descricao;
            Controller = controller;
            Action = action;
            AbrirEmNovaJanela = abrirEmNovaJanela;
        }

        public string Descricao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public bool AbrirEmNovaJanela { get; set; }
    }
}