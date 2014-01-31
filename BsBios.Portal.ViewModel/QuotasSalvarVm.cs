using System;
using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class QuotasSalvarVm
    {
        //public QuotasSalvarVm()
        //{
        //    Quotas = new List<QuotaSalvarVm>();
        //}
        public DateTime Data { get; set; }
        public string CodigoDoTerminal { get; set; }
        public List<QuotaSalvarVm> Quotas { get; set; }
    }
}
