using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroDeConhecimentoDeTransporte : ICadastroDeConhecimentoDeTransporte
    {
        private readonly IUnitOfWorkNh _unitOfWorkNh;
        private readonly IConhecimentosDeTransporte _conhecimentosDeTransporte;
        private readonly IOrdensDeTransporte _ordensDeTransporte;
        private readonly IProcessadorDeColeta _processadorDeColeta;

        public CadastroDeConhecimentoDeTransporte(IUnitOfWorkNh unitOfWorkNh, IConhecimentosDeTransporte conhecimentosDeTransporte, 
            IProcessadorDeColeta processadorDeColeta, IOrdensDeTransporte ordensDeTransporte)
        {
            _unitOfWorkNh = unitOfWorkNh;
            _conhecimentosDeTransporte = conhecimentosDeTransporte;
            _processadorDeColeta = processadorDeColeta;
            _ordensDeTransporte = ordensDeTransporte;
        }

        private IEnumerable<ConhecimentoDeTransporte> RealizarCadastro(IEnumerable<ConhecimentoDeTransporteVm> conhecimentosDeTransporteVm)
        {

            var conhecimentosDeTransporteGerados = new List<ConhecimentoDeTransporte>();
            try
            {
                _unitOfWorkNh.BeginTransaction();

                foreach (var conhecimentoDeTransporteVm in conhecimentosDeTransporteVm)
                {
                    var conhecimentoDeTransporte =
                        new ConhecimentoDeTransporte(conhecimentoDeTransporteVm.ChaveEletronica,
                            conhecimentoDeTransporteVm.CnpjDoFornecedor,
                            conhecimentoDeTransporteVm.CnpjDaTransportadora,
                            Convert.ToDateTime(conhecimentoDeTransporteVm.DataDeEmissao),
                            conhecimentoDeTransporteVm.Serie, conhecimentoDeTransporteVm.Numero,
                            conhecimentoDeTransporteVm.NumeroDoContrato,
                            conhecimentoDeTransporteVm.ValorRealDoFrete, conhecimentoDeTransporteVm.PesoTotalDaCarga);

                    foreach (var notasFiscalVm in conhecimentoDeTransporteVm.NotasFiscais)
                    {
                        conhecimentoDeTransporte.AdicionarNotaFiscal(notasFiscalVm.Chave, notasFiscalVm.Numero,
                            notasFiscalVm.Serie);

                    }

                    _conhecimentosDeTransporte.Save(conhecimentoDeTransporte);

                    conhecimentosDeTransporteGerados.Add(conhecimentoDeTransporte);
                }
                _unitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                _unitOfWorkNh.RollBack();
                throw;
            }

            return conhecimentosDeTransporteGerados;
            
        }

        //private void Processar(IEnumerable<ConhecimentoDeTransporte> conhecimentosDeTransporte)
        //{
        //    var unitOfWorkNh = new UnitOfWorkNh(ObjectFactory.GetInstance<ISessionFactory>());
        //    var ordensDeTransporte = new OrdensDeTransporte(unitOfWorkNh);
        //    var fornecedores = new Fornecedores(unitOfWorkNh);
        //    var processadorDeColeta = new ProcessadorDeColeta(ordensDeTransporte, fornecedores);

        //    foreach (var conhecimentoDeTransporte in conhecimentosDeTransporte)
        //    {
        //        try
        //        {
        //            unitOfWorkNh.BeginTransaction();
        //            IQueryable<ConhecimentoDeTransporte> queryable = unitOfWorkNh.Session.Query<ConhecimentoDeTransporte>();

        //            ConhecimentoDeTransporte cte = queryable.Single(x => x.ChaveEletronica == conhecimentoDeTransporte.ChaveEletronica);

        //            processadorDeColeta.Processar(cte);
        //            unitOfWorkNh.Commit();
        //        }
        //        catch (Exception)
        //        {
        //            unitOfWorkNh.RollBack();
        //            throw;
        //        }

        //    }
        //}

        private void Processar(IEnumerable<string> chavesEletronicas)
        {
            foreach (var chaveEletronica in chavesEletronicas)
            {
                try
                {
                    _unitOfWorkNh.BeginTransaction();

                    ConhecimentoDeTransporte conhecimentoDeTransporte = _conhecimentosDeTransporte
                        .ComChaveEletronica(chaveEletronica)
                        .IncluirNotasFiscais()
                        .Single();
                    OrdemDeTransporte ordemDeTransporteVinculada = _processadorDeColeta.Processar(conhecimentoDeTransporte);

                    _conhecimentosDeTransporte.Save(conhecimentoDeTransporte);

                    if (ordemDeTransporteVinculada != null)
                    {
                        _ordensDeTransporte.Save(ordemDeTransporteVinculada);
                    }

                    _unitOfWorkNh.Commit();
                }
                catch (Exception)
                {
                    _unitOfWorkNh.RollBack();
                    throw;
                }

            }
        }

        public void Salvar(IList<ConhecimentoDeTransporteVm> conhecimentosDeTransporteVm)
        {
            IEnumerable<ConhecimentoDeTransporte> conhecimentosDeTransporteGerados = RealizarCadastro(conhecimentosDeTransporteVm);

            var processar = new Task(() => Processar(conhecimentosDeTransporteGerados.Select(x => x.ChaveEletronica)));

            processar.Start();

        }

        public void Reprocessar()
        {
            try
            {
                _unitOfWorkNh.BeginTransaction();

                IList<ConhecimentoDeTransporte> conhecimentosDeTransporte = _conhecimentosDeTransporte
                    .ComErroOuSemOrdemDeTransporte()
                    .IncluirNotasFiscais()
                    .List();

                foreach (var conhecimentoDeTransporte in conhecimentosDeTransporte)
                {
                    OrdemDeTransporte ordemDeTransporteVinculada = _processadorDeColeta.Processar(conhecimentoDeTransporte);

                    _conhecimentosDeTransporte.Save(conhecimentoDeTransporte);

                    if (ordemDeTransporteVinculada != null)
                    {
                        _ordensDeTransporte.Save(ordemDeTransporteVinculada);
                    }

                }

                _unitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                _unitOfWorkNh.RollBack();
                throw;
            }
        }
    }
}