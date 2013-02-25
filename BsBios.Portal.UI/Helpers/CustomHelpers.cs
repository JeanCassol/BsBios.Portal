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
            string html = @coluna.GeraLabel().ToHtmlString() +
                          @coluna.GeraInput().ToHtmlString();

            if (coluna.ExibirMensagemDeValidacao)
            {
                html +=
                "<p class=\"mensagemErro\">" +
                @coluna.GeraMensagemDeValidacao() +
                "</p>";

            }

            return
                "<div class=\"coluna\">" +
                    html +
                "</div>";
        }

        public static IHtmlString LinhaComDuasColunas<TModel1, TValue1, TValue2>(this HtmlHelper<TModel1> html, 
            Coluna<TModel1, TValue1> coluna1, Coluna<TModel1, TValue2> coluna2)
        {
            coluna1.HtmlHelper = html;
            coluna2.HtmlHelper = html; 
            string retorno =
                "<div class=\"linha\"> " +
                    GeraColuna(coluna1) +
                    GeraColuna(coluna2) +
                "</div>";
            return new HtmlString(retorno);
        }
    }


}