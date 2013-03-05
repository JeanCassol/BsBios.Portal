using System.ComponentModel;

namespace BsBios.Portal.Common
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

        public enum StatusProcessoCotacao
        {
            [Description("Não Iniciado")]
            NaoIniciado = 1,
            [Description("Aberto")]
            Aberto = 2,
            [Description("Fechado")]
            Fechado = 3,
            [Description("Cancelado")]
            Cancelado = 4
        }
        public enum TipoDeCotacao
        {
            Material = 1,
            Frete = 2
        }
        public enum TipoDeImposto
        {
            Icms = 1,
            IcmsSubstituicao = 2,
            Ipi = 3,
            Pis = 4,
            Cofins = 5
        }
    }
}
