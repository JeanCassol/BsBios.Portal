using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Helpers
{
    public abstract class Coluna<TModel, TValue>
    {
        protected readonly Expression<Func<TModel, TValue>> Expressao;
        protected readonly HtmlHelper<TModel> HtmlHelper;
        protected readonly string InputClass;

        protected Coluna(Expression<Func<TModel, TValue>> expressao, HtmlHelper<TModel> htmlHelper, string inputClass)
        {
            Expressao = expressao;
            HtmlHelper = htmlHelper;
            InputClass = inputClass;
        }

        public  MvcHtmlString GeraLabel()
        {
            return System.Web.Mvc.Html.LabelExtensions.LabelFor(HtmlHelper, Expressao);
        }

        public MvcHtmlString GeraMensagemDeValidacao()
        {
            return System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(HtmlHelper, Expressao);
        }

        public abstract MvcHtmlString GeraInput();
    }
    public class ColunaComEditor<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComEditor(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expressao)
            : base(expressao, htmlHelper, "")
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.EditorExtensions.EditorFor(HtmlHelper, Expressao);
        }
    }

    public class ColunaComTextBox<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComTextBox(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expressao, string inputClass)
            : base(expressao, htmlHelper, inputClass)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(HtmlHelper, Expressao, new { @class = InputClass });
        }
    }

    public class ColunaComTextArea<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComTextArea(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expressao)
            : base(expressao, htmlHelper, "")
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(HtmlHelper, Expressao, new { @rows = "5" });
        }
    }
    public class ColunaComDropDown<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComDropDown(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expressao,
            IEnumerable<SelectListItem> items, string nome, string inputClass = "")
            : base(expressao, htmlHelper, inputClass)
        {
            _items = items;
            _nome = nome;
        }

        private readonly IEnumerable<SelectListItem> _items;
        private readonly string _nome;

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(HtmlHelper, _nome, _items, "Selecione >>");
        }
    }
}