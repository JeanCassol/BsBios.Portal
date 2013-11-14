using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;

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

        protected string FormatarValor(Expression<Func<TModel, TValue>> expressao, string formatacao)
        {
            var valorFormatado = System.Web.Mvc.Html.DisplayExtensions.DisplayFor(HtmlHelper, Expressao, new { @class = InputClass }).ToString(); ;
            if (string.IsNullOrEmpty(formatacao)) return valorFormatado;
            
            decimal valorConvertido;
            
            if (decimal.TryParse(valorFormatado, out valorConvertido))
            {
                valorFormatado = valorConvertido.ToString(formatacao);    
            }

            return valorFormatado;

        }

    }

    public class ColunaComEditor<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComEditor(Expression<Func<TModel, TValue>> expressao, string inputClass = "")
            : base(expressao, inputClass,"", true)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.EditorExtensions.EditorFor(HtmlHelper, Expressao, new { @class = InputClass });
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
        public ColunaComDropDown(Expression<Func<TModel, TValue>> expressao,IEnumerable<SelectListItem> items, 
            string nome, string inputClass = "", string optionLabel = "Selecione >>")
            : base(expressao, inputClass,"", true)
        {
            _items = items;
            _nome = nome;
            _optionLabel = optionLabel;
        }

        private readonly IEnumerable<SelectListItem> _items;
        private readonly string _nome;
        private readonly string _optionLabel;

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(HtmlHelper, _nome, _items, _optionLabel);
        }
    }
    
    public class ColunaComLabel<TModel, TValue> : Coluna<TModel, TValue>
    {
        private readonly string _formatacao;

        public ColunaComLabel(Expression<Func<TModel, TValue>> expressao, string formatacao = null) 
            : base(expressao, "","labelNaLinha", false)
        {
            _formatacao = formatacao;
        }

        public override MvcHtmlString GeraInput()
        {
            var valorFormatado = FormatarValor(Expressao, _formatacao);
            return new MvcHtmlString(valorFormatado);
        }
    }    
    
    public class ColunaComLabelEmDestaque<TModel, TValue> : Coluna<TModel, TValue>
    {
        private readonly string _idDoDestaque;
        private readonly string _formatacaoDoDestaque;

        public ColunaComLabelEmDestaque(Expression<Func<TModel, TValue>> expressao, string idDoDestaque, string formatacaoDoDestaque) 
            : base(expressao, "","labelNaLinha", false)
        {
            _idDoDestaque = idDoDestaque;
            _formatacaoDoDestaque = formatacaoDoDestaque;
        }

        public override MvcHtmlString GeraInput()
        {
            var valorDoLabel = FormatarValor(Expressao, _formatacaoDoDestaque);
            var elemento = "<span class=\"labelDestaque\" id=\"" + _idDoDestaque + "\">" + valorDoLabel + "</span>";

            return new MvcHtmlString(elemento);
        }
    }

    public class ColunaComCheckBox<TModel, TValue> : Coluna<TModel, TValue>
    {
        public ColunaComCheckBox(Expression<Func<TModel, TValue>> expressao): base(expressao, "", "labelNaLinha", false)
        {
        }

        public override MvcHtmlString GeraInput()
        {
            return System.Web.Mvc.Html.InputExtensions.CheckBoxFor(HtmlHelper,(Expression<Func<TModel, bool>>) (object)  Expressao);
        }
    }


}