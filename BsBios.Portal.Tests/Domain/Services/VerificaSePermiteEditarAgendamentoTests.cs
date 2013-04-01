using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class VerificaSePermiteEditarAgendamentoTests
    {
        [TestMethod]
        public void QuandoUsuarioPossuirPerfilDeAgendadorDeCargaADataForMenorQueADataDoAgendamentoEoAgendamentoNaoEstiverRealizadoPermiteEditar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();
            Assert.IsTrue(verificaSePodeEditarAgendamento.PermiteEditar(agendamento, usuario));
        }

        [TestMethod]
        public void QuandoAgendamentoEstiverRealizadoNaoPermiteEditar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);
            agendamento.Realizar();

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteEditar(agendamento, usuario));
            
        }

        [TestMethod]
        public void QuandoDataDoAgendamentoNaoForPosteriorADataAtualNaoPermiteEditar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today), 100);

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteEditar(agendamento, usuario));

        }

        [TestMethod]
        public void QuandoUsuarioNaoPossuirPerfilDeAgendadorDeCargasNaoPermiteEditar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteEditar(agendamento, usuario));
            
        }

        [TestMethod]
        public void QuandoUsuarioPossuirPerfilDeAgendadorDeQuotasEDataDaQuotaNaoForAnteriorADataAtualPermiteAdicionarAgendamento ()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();
            Assert.IsTrue(verificaSePodeEditarAgendamento.PermiteAdicionar(quota, usuario));

        }

        [TestMethod]
        public void QuandoUsuarioNaoPossuirPerfilDeAgendadorDeQuotasNaoPermiteAdicionarAgendamento()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();
            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteAdicionar(quota, usuario));

        }

        [TestMethod]
        public void QuandoDataDaQuotaForAnteriorADataAtualNaoPermiteAdicionarAgendamento()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(-1));
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();
            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteAdicionar(quota, usuario));

        }

        [TestMethod]
        public void QuandoUsuarioPossuirPerfilDeConferidorDeCargasEAgendamentoNaoEstiverRealizadoPermiteRealizar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.ConferidorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsTrue(verificaSePodeEditarAgendamento.PermiteRealizar(agendamento, usuario));
            
        }

        [TestMethod]
        public void QuandoUsuarioNaoPossuirPerfilDeConferidorDeCargasNaoPermiteRealizar()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);

            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteRealizar(agendamento, usuario));

        }

        [TestMethod]
        public void QuandoAgendamentoJaEstiverRealizadoNaoPermiteRealizarNovamente()
        {
            AgendamentoDeCarga agendamento = DefaultObjects.ObtemAgendamentoDeCarregamentoComPesoEspecifico(
                DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today.AddDays(1)), 100);

            agendamento.Realizar();
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.ConferidorDeCargas);

            var verificaSePodeEditarAgendamento = ObjectFactory.GetInstance<IVerificaPermissaoAgendamento>();

            Assert.IsFalse(verificaSePodeEditarAgendamento.PermiteRealizar(agendamento, usuario));
            
        }

    }
}
