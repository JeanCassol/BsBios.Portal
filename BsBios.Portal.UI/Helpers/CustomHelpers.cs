using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Helpers
{
    public static class CustomHelpers
    {

        private static string GeraColuna<TModel, TValue>(Coluna<TModel, TValue> coluna)
        {
            return
                "<div class=\"coluna\">" +
                    @coluna.GeraLabel() +
                    @coluna.GeraInput() +
                    "<p class=\"mensagemErro\">" +
                        @coluna.GeraMensagemDeValidacao() +
                    "</p>" +
                "</div>";
        }

        public static IHtmlString LinhaComDuasColunas<TModel, TValue>(this HtmlHelper html, Coluna<TModel, TValue> coluna1, Coluna<TModel, TValue> coluna2)
        {
            string retorno =
                "<div class=\"linha\"> " +
                    GeraColuna(coluna1) +
                    GeraColuna(coluna2) +
                "</div>";
            return new HtmlString(retorno);
        }
    }


}