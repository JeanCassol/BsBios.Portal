using System;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada quando a tela detalhe de cotação de materiais é salva
    /// </summary>
    public class ProcessoDeCotacaoAtualizarVm
    {
        public int Id { get; set; }
        public DateTime DataLimiteRetorno { get; set; }
    }
}
