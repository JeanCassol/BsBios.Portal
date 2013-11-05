using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class MunicipioControllerTests
    {
        [TestMethod]
        public void QuandoConsultoPorNomeControllerRetornaListaNoFormatoJSonComMunicipiosEncontrados()
        {
            var municipiosMock = new Mock<IConsultaMunicipio>(MockBehavior.Strict);

            municipiosMock.Setup(x => x.NomeComecandoCom(It.IsAny<string>()))
                .Returns(new List<AutoCompleteVm>
                {
                    new AutoCompleteVm
                    {
                        Codigo = "1" ,
                        label = "Torres/RS",
                        value = "Torres/RS"
                    }
                })
                .Callback((string nome) => Assert.AreEqual("tor", nome));


            var municipioController = new MunicipioController(municipiosMock.Object);

            JsonResult resultado = municipioController.Buscar("tor");

            municipiosMock.Verify(x => x.NomeComecandoCom(It.IsAny<string>()), Times.Once());

            var municipios = (List<AutoCompleteVm>) resultado.Data;

            Assert.AreEqual(1, municipios.Count);

            var municipio = municipios.First();

            Assert.AreEqual("Torres/RS", municipio.value);
            Assert.AreEqual("Torres/RS", municipio.label);
            Assert.AreEqual("1", municipio.Codigo);

        }

        [TestMethod]
        public void Teste()
        {
            int numero = int.Parse("1000",NumberStyles.AllowThousands,CultureInfo.CurrentCulture);
            Assert.AreEqual(1000, numero);
        }
    
    }
}
