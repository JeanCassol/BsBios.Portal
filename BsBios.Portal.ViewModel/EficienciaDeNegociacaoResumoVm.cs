namespace BsBios.Portal.ViewModel
{
    public class EficienciaDeNegociacaoResumoVm:ListagemVm
    {
        public string Comprador { get; set; }
        public string Produto  { get; set; }
        public int IdProcessoCotacao { get; set; }
        public int IdProcessoCotacaoItem { get; set; }
        public string NumeroDaRequisicao { get; set; }
        public string NumeroDoItem { get; set; }
        public decimal PercentualDeEficiencia { get; set; }
        public decimal ValorDeEficiencia { get; set; }
    }
}