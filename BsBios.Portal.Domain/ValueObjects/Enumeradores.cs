using System.ComponentModel;

namespace BsBios.Portal.Domain.ValueObjects
{
    public class Enumeradores
    {
        public enum Perfil
        {
            [Description("Comprador")]
            Comprador = 1,
            [Description("Fornecedor")]
            Fornecedor = 2
        }

        public enum StatusPedidoCotacao
        {
            NaoIniciado = 1,
            Aberto = 2,
            Fechado = 3,
            Cancelado = 4
        }
    }
}
