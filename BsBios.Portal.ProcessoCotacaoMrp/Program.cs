using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.IoC;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.PrecessoCotacaoMrp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executando Processo de Cotação MRP");
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBios"].ConnectionString);
            IoCWorker.Configure();
            var consulta = ObjectFactory.GetInstance<IConsultaRequisicaoDeCompra>();
            IList<RequisicaoDeCompraVm> requisicoes = consulta.RequisicoesDoGrupoDeCompras("102");
            if (requisicoes.Count > 0)
            {
                foreach (RequisicaoDeCompraVm requisicaoDeCompraVm in requisicoes)
                {
                    Console.WriteLine("Requisição: " + requisicaoDeCompraVm.NumeroRequisicao + " - Item: " + requisicaoDeCompraVm.NumeroItem);
                }
           }
            else
            {
                Console.WriteLine("Não foram encontradas requisicoes");
            }
        }
    }
}
