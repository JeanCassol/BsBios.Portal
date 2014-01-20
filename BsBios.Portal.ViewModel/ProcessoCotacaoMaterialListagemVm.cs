namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Classe utilizada no grid de processo de cotação de material
    /// </summary>
    public class ProcessoCotacaoListagemVm: ListagemVm
    {
        public int Id { get; set; }
        public string CodigoMaterial { get; set; }
        public string Material { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeDeMedida { get; set; }
        public string Status { get; set; }
        public string DataTermino { get; set; }
        public string Terminal { get; set; }
    }

}
