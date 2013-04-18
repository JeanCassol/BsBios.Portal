Globalize.culture('pt-BR');

Mensagem = {
    ExibirMensagemDeErro: function (mensagem) {
        alert(mensagem);
    },

    ExibirMensagemDeSucesso: function (mensagem) {
        alert(mensagem);
    }
};

String.prototype.boolean = function () {
    return this.match(/^(true|True)$/i) !== null;
};

Numero = {
    GetFloat: function (valor) {
        var val = Globalize.parseFloat(valor);
        if (isNaN(val))
            return 0;
        else
            return val;
    }
    
};
Formato = {
    formataCnpj: function (v) {
        if (v == null || v == "") {
            return v;
        }

        //Remove tudo o que não é dígito
        v = v.replace(/\D/g, "");
        //Coloca ponto entre o segundo e o terceiro dígitos
        v = v.replace(/^(\d{2})(\d)/, "$1.$2");

        //Coloca ponto entre o quinto e o sexto dígitos
        v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3");

        //Coloca uma barra entre o oitavo e o nono dígitos
        v = v.replace(/\.(\d{3})(\d)/, ".$1/$2");

        //Coloca um hífen depois do bloco de quatro dígitos
        v = v.replace(/(\d{4})(\d)/, "$1-$2");

        return v;
    }
};

$.fn.customKendoGrid = function (configuracao) {
    configuracao.groupable = false;
    configuracao.resizable = true;
    //configuracao.sortable = true;
    if (configuracao.pageable == undefined) {
        configuracao.pageable =
        {
            refresh: true,
            pageSizes: true,
            messages: {
                display: '{0:n0} - {1:n0} de {2:n0} registros',
                empty: 'Nenhum registro encontrado',
                itemsPerPage: 'registros por página',
                first: 'Ir para a primeira página',
                previous: 'Ir para a página anterior',
                next: 'Ir para a próxima página',
                last: 'Ir para a última página',
                refresh: 'Atualizar'
            }
        };
    }
    configuracao.selectable = 'row';
    configuracao.dataSource.serverFiltering = true;
    configuracao.dataSource.serverPaging = true;
    configuracao.dataSource.pageSize = 10;

    this.kendoGrid(configuracao);
};

$.fn.customDialog = function (configuracao) {
    configuracao.autoOpen = false;
    configuracao.resizable = false;
    configuracao.modal = true;
    configuracao.position = { at: "top" };
    configuracao.beforeClose = function () {
        $(this).empty();
    };

    if (!configuracao.width) {
        configuracao.width = 800;
    }

    this.dialog(configuracao);
};

function atualizaMensagemDeErro(mensagem) {
    $('#divErro').html(mensagem);
}

function inicializaCamposDatePicker() {
    /*seleciona todos os campos datepicker para inicializar o componente do jquery UI*/
    var camposDatePicker = $('.campoDatePicker');
    if ($(camposDatePicker).length > 0) {
        $(camposDatePicker).datepicker();
    }
}

function validarCNPJ(cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    var tamanho = cnpj.length - 2;
    var numeros = cnpj.substring(0, tamanho);
    var digitos = cnpj.substring(tamanho);
    var soma = 0;
    var pos = tamanho - 7;
    for (var i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

}

function dataValida(valor) {
    var date = valor;
    var ExpReg = new RegExp("(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}");
    var ardt = date.split("/");
    var erro = false;
    if (date.search(ExpReg) == -1) {
        erro = true;
    }
    else if (((ardt[1] == 4) || (ardt[1] == 6) || (ardt[1] == 9) || (ardt[1] == 11)) && (ardt[0] > 30))
        erro = true;
    else if (ardt[1] == 2) {
        if ((ardt[0] > 28) && ((ardt[2] % 4) != 0))
            erro = true;
        if ((ardt[0] > 29) && ((ardt[2] % 4) == 0))
            erro = true;
    }
    return !erro;
}
function aplicaMascaraCnpj() {
    var camposCnpj = $('.maskcnpj');
    if ($(camposCnpj).length == 0) {
        return;
    }
    $(camposCnpj).mask("99.999.999/9999-99");
}

function aplicaMascaraMoeda() {
    var campos = $('.maskmoeda');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).each(function () {
        var val = Globalize.parseFloat($(this).val());
        if (!isNaN(val)) {
            $(this).val(Globalize.format(val,"n2"));
        }
    });

    $(campos).setMask('moeda-portal');
}

function aplicaMascaraQuantidade() {
    var campos = $('.maskquantidade');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).each(function () {
        var val = Globalize.parseFloat($(this).val());
        if (!isNaN(val)) {
            $(this).val(Globalize.format(val, "n3"));
        }
    });

    $(campos).setMask('quantidade-portal');

}
function aplicaMascaraData() {
    var camposData = $('.maskdata');
    if ($(camposData).length == 0) {
        return;
    }
    $(camposData).mask("99/99/9999",
    {
        completed: function () {
            if (!dataValida(this.val())) {
                alert("Data inválida.");
                this.focus();
            }
              
         }
    });
}
function aplicaMascaraPlaca() {
    var campos = $('.maskplaca');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).mask("aaa-9999");
}
function aplicaMascaraNumeroNf() {
    var campos = $('.masknumeronf');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).setMask('numeronf-portal');
}
function aplicaMascaraSerieNf() {
    var campos = $('.maskserienf');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).setMask('serienf-portal');
}
function aplicaMascaraNumeroContrato() {
    var campos = $('.masknumerocontrato');
    if ($(campos).length == 0) {
        return;
    }
    $(campos).setMask('numerocontrato-portal');
}

function bloqueiaPagina() {
    $('#todaPagina').block("Processando...");
}

function desbloqueiaPagina() {
    $("#todaPagina").unblock();
}

UrlPadrao = {};

TipoDeCotacao = {};

$(function () {
    inicializaCamposDatePicker();
    $('.campoDesabilitado').attr('readonly', true);
    $('#btnPesquisar').die("click");
    $('#btnPesquisar').live("click", function (e) {
        e.preventDefault();
        $(".divGrid :last").data("kendoGrid").dataSource.page(1);
    });

    //aplicaMascaras();
});

$(document).ajaxComplete(function (event, request, ajaxOptions) {
    if (ajaxOptions.dataType != "json") {
        return;
    }
    var resposta = JSON.parse(request.responseText);
    if (resposta.SessaoExpirada) {
        Mensagem.ExibirMensagemDeErro(resposta.Mensagem);
        location.href = resposta.ReturnUrl;
    }
});