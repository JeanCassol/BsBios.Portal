using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Helpers
{
    public abstract class Coluna<TModel, TValue>
    {
        protected readonly Expression<Func<TModel, TValue>> Expressao;
        public HtmlHelper<TModel> HtmlHelper { get; set; }
        protected readonly string InputClass;
        public string LabelClass { get; set; }
        public bool ExibirMensagemDeValidacao { get; protected set; }

        protected Coluna(Expression<Func<TModel, TValue>> expressao, string inputClass, string labelClass, bool exibirMensagemDeValidacao)
        {
            Expressao = expressao;
            InputClass = inputClass;
            LabelClass = labelClass;
            ExibirMensagemDeValidacao = exibirMensagemDeValidacao;
        }

        public MvcHtmlString GeraLabel()
        {
            return System.Web.Mvc.Html.LabelExtensions.LabelFor(HtmlHelper, Expressao, string.IsNullOrEmpty(LabelClass) ? null : new { @class = LabelClass });
        }

        public MvcHtmlString GeraMensagemDeValidacao()
        {
            return System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(HtmlHelper, Expressao);
        }

        public abstract MvcHtmlString GeraInput();
    }

    public class ColunaComEditor<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComEditor(Expression<Func<TModel, TValue>> expressao)
            : base(expressao, "","", true)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.EditorExtensions.EditorFor(HtmlHelper, Expressao);
        }
    }

    public class ColunaComTextBox<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComTextBox( Expression<Func<TModel, TValue>> expressao,
                                string inputClass)
            : base(expressao, inputClass,"", true)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.InputExtensions.TextBoxFor(HtmlHelper, Expressao, new {@class = InputClass});
        }
    }

    public class ColunaComTextArea<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComTextArea(Expression<Func<TModel, TValue>> expressao)
            : base(expressao, "","", true)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(HtmlHelper, Expressao, new {@rows = "5"});
        }
    }

    public class ColunaComDropDown<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComDropDown(Expression<Func<TModel, TValue>> expressao,
                                 IEnumerable<SelectListItem> items, string nome, string inputClass = "")
            : base(expressao, inputClass,"", true)
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
    
    public class ColunaComLabel<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComLabel(Expression<Func<TModel, TValue>> expressao) 
            : base(expressao, "","labelNaLinha", false)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.DisplayExtensions.DisplayFor(HtmlHelper, Expressao, new { @class = InputClass });
        }
    }


}