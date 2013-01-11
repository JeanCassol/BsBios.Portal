using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{

    public class MenuItem
    {
        public string Descricao { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

    public class MenuUsuario
    {
        public MenuUsuario(string titulo)
        {
            Titulo = titulo;
            Itens = new List<MenuItem>();
        }

        public string Titulo { get; set; }
        public List<MenuItem> Itens { get; set; }
        public void AddMenuItem(string descricao, string controller, string action)
        {
            Itens.Add(new MenuItem() { Descricao = descricao, Controller = controller, Action = action });
        }

    } 
}