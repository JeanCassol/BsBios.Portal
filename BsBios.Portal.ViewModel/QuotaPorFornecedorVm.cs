using System;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// utilizado na tela que o fornecedor acessa para visualizar as suas quotas
    /// </summary>
    public class QuotaPorFornecedorVm: ListagemVm
    {
        public int IdQuota { get; set; }
        //public string CodigoTerminal { get; set; }
        [Display(Name = "Data: ")]
        public string Data { get; set; }
        //public string CodigoFornecedor { get; set; }
        //public int CodigoMaterial { get; set; }
        [Display(Name = "Material: ")]
        public string DescricaoMaterial { get; set; }

        public int CodigoFluxoDeCarga { get; set; }
        [Display(Name = "Fluxo de Carga: ")]
        public string FluxoDeCarga { get; set; }
        [Display(Name = "Peso Total: ")]
        public decimal PesoTotal { get; set; }
        [Display(Name = "Peso Agendado: ")]
        public decimal PesoAgendado { get; set; }
        [Display(Name = "Peso Disponível: ")]
        public decimal PesoDisponivel { get; set; }
    }
}
