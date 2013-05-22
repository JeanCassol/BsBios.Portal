namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoDeMaterialItemVm: ListagemVm
    {
        public int Id { get; set; }
        public int IdProcessoCotacaoItem { get; set; }
        public string Material { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
        public string NumeroRequisicao { get; set; }
        public string NumeroItem { get; set; }
        public string DataDeSolicitacao { get; set; }
        public string CodigoGrupoDeCompra { get; set; }
    }
}
