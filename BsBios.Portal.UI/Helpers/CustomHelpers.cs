using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Helpers
{
    public static class CustomHelpers
    {

        private static string GeraColuna(Coluna coluna)
        {
            return
                "<div class=\"coluna\">" +
                    @coluna.Label + 
                    @coluna.Campo +
                    "<p class=\"mensagemErro\">" +
                        @coluna.MensagemDeValidacao +
                    "</p>" +
                "</div>";
        }


        public static IHtmlString LinhaComDuasColunas(this HtmlHelper html, Coluna coluna1, Coluna coluna2)
        {
            string retorno =
                "<div class=\"linha\"> " +
                    GeraColuna(coluna1) +
                    GeraColuna(coluna2) +
                "</div>";
                return new HtmlString(retorno);
        }

        public static IHtmlString LinhaComDuasColunas<TModel, TValue>(this HtmlHelper<TModel> html, Coluna coluna1, Expression<Func<TModel, TValue>> expressao)
        {
            var coluna2 = new Coluna(System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expressao),
                                    System.Web.Mvc.Html.EditorExtensions.EditorFor(html, expressao),
                                    System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(html, expressao));

            return LinhaComDuasColunas(html, coluna1, coluna2);
        }
    }

    public class Coluna//<T> where T:IViewModel
    {
        public MvcHtmlString Label { get; protected set; }
        public MvcHtmlString Campo { get; protected set; }
        public MvcHtmlString MensagemDeValidacao { get; protected set; }

        public Coluna(MvcHtmlString label, MvcHtmlString campo, MvcHtmlString mensagemDeValidacao)
        {
            Label = label;
            Campo = campo;
            MensagemDeValidacao = mensagemDeValidacao;
        }
        //public Coluna (Func<T, TProperty> funcao )
        //{
        //}
    }
}