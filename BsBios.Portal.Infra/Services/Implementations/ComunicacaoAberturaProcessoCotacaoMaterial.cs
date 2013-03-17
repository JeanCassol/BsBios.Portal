﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoAberturaProcessoCotacaoMaterial : IComunicacaoSap
    {
        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            return new ApiResponseMessage(){Retorno = new Retorno(){Codigo = "200" ,Texto = "Esta comunicação ainda não foi implementada." }};
        }
    }
}