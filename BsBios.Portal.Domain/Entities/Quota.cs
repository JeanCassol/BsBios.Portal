using System;
using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class Quota
    {
        public Enumeradores.FluxoDeCarga FluxoDeCarga { get; set; }
        public Fornecedor Transportadora { get; set; }
        public string Terminal { get; set; }
        public DateTime Data { get; set; }
        public decimal Peso { get; set; }

        public Quota(Enumeradores.FluxoDeCarga fluxoDeCarga, Fornecedor transportadora, string terminal, DateTime data, decimal peso)
        {
            FluxoDeCarga = fluxoDeCarga;
            Transportadora = transportadora;
            Terminal = terminal;
            Data = data;
            Peso = peso;
        }
    }
}