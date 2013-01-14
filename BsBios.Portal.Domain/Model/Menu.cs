using System.Collections.Generic;

namespace BsBios.Portal.Domain.Model
{
    public class Menu
    {
        public string Descricao { get; set; }    
        public IList<MenuItem> Itens { get; protected set; }

        internal Menu(string descricao)
        {
            Descricao = descricao;
            Itens = new List<MenuItem>();
        }
        public void AdicionarItem(string descricao, string controller, string action)
        {
            Itens.Add(new MenuItem(descricao, controller, action));
        }

    }

    public class MenuItem
    {
        public MenuItem(string descricao, string controller, string action)
        {
            Descricao = descricao;
            Controller = controller;
            Action = action;
        }

        public string Descricao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}