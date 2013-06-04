namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoValoresVm
    {
        //remover o login e o nome do comprador se for confirmado que 
        //os dados da consulta devem ser agrupados por comprador
        public string Login { get; set; }
        public string Nome { get; set; }

        public string CodigoDoProduto { get; set; }
        public string DescricaoDoProduto  { get; set; }
        public string NumeroDaRequisicao { get; set; }
        public string NumeroDoItem { get; set; }
        public decimal PrecoInicial { get; set; }
        public decimal PrecoDeFechamento { get; set; }
    }
}