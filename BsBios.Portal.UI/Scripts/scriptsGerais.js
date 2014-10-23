Globalize.culture('pt-BR');

Mensagem = {

    gerarCaixaDeMensagem: function (mensagem, titulo, classeParaAdicionar, classeParaRemover, funcaoConfirmacao, exibirBotaoCancelar) {

        var mensagemModal = $('.mensagemModal');

        if (mensagemModal.length == 0) {

            $('body').append(
                '<div class="mensagemModal" title="BS NET - '+ titulo + '" class="janelaModal">' +
                        '<span class="ui-icon ' + classeParaAdicionar + '" style="float: left; margin: 0 7px 50px 0;"></span>' +
                        '<span id="conteudoDoMessageBox">' + mensagem + '</span>' +
                '</div>');
        } else {
            $(mensagemModal).find('#conteudoDoMessageBox').text(mensagem);
            $(mensagemModal).find('.ui-icon')
                .removeClass(classeParaRemover)
                .addClass(classeParaAdicionar);
        }
        
        var botoes = {
            OK: function () {
                Mensagem.confirmado = true;
                $(this).dialog("close");
                if (funcaoConfirmacao) {
                    funcaoConfirmacao();
                }
            }
        };

        if (exibirBotaoCancelar) {
            botoes.Cancelar = function() {
                $(this).dialog("close");
            };
        }

        $(".mensagemModal").dialog({
            modal: true,
            buttons: botoes
        });
    },

    ExibirMensagemDeErro: function (mensagem, funcaoConfirmacao) {
        this.gerarCaixaDeMensagem(mensagem, 'Erro', 'ui-icon-alert', 'ui-icon-check', funcaoConfirmacao);
    },

    ExibirMensagemDeSucesso: function (mensagem) {
        this.gerarCaixaDeMensagem(mensagem,'Sucesso','ui-icon-check','ui-icon-alert');
    },
    ExibirMensagemDeConfirmacao: function (mensagem, funcaoConfirmacao) {
        this.gerarCaixaDeMensagem(mensagem, 'Confirmação', 'ui-icon-check', 'ui-icon-alert',funcaoConfirmacao, true);
    },
    ExibirJanelaComHtml: function (html) {
        var janela = $('#divJanelaComHtml');
        if (janela.length == 0) {
            $('body').append('<div id="divJanelaComHtml"></div>');

            $('#divJanelaComHtml').customDialog({
                title: 'Mensagem',
                width:1024
                
            });
        }
        $('#divJanelaComHtml').html(html);
        $('#divJanelaComHtml').dialog('open');
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

        var larguraDaViewPort = $(window).width();
        var buttonCount;
        if (larguraDaViewPort > 800) {
            buttonCount = 10;
        } else {
            buttonCount = 3;
        }

        configuracao.pageable =
        {
            refresh: true,
            pageSizes: true,
            buttonCount: buttonCount,
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
    if (configuracao.scrollable === undefined) {
        configuracao.scrollable = true;
    }

    this.kendoGrid(configuracao);

    this.data("kendoGrid").obterRegistroSelecionado = function () {
        var linhaSelecionada = this.select();
        return this.dataItem(linhaSelecionada);
    };
};

$.fn.customDialog = function (configuracao) {
    configuracao.autoOpen = false;
    configuracao.resizable = false;
    configuracao.modal = true;
    configuracao.position = { at: "top" };
    configuracao.beforeClose = function () {
        $(this).empty();
    };
    configuracao.create = function (event, ui) {

        //$(event.target).parent().find('.ui-dialog-buttonpane').addClass('divBotao');
        var buttonSet = $(event.target).parent().find('.ui-dialog-buttonset');
        
        var buttons = buttonSet.children();
        buttons.removeClass('ui-widget ui-state-default').addClass('blue');

    };

    if(configuracao.buttons){
        $.each(configuracao.buttons, function(index, button) {
            button.mousemove = function() {
                $(this).removeClass("ui-state-hover");
            };
        });
        
    }
    
    this.dialog(configuracao);
};

$.fn.customLoad = function (url, callBack) {

    var divParaCarregar = this;
    
    var larguraDaViewPort = $(window).width();

    if (larguraDaViewPort > 800) {
        $(divParaCarregar).dialog("option", "width", 800);
    } else {
        $(divParaCarregar).dialog("option", "width", '99%');
    }

    if (!url) {
        divParaCarregar.dialog("open");
        return;
    }
    
    this.load(url, function (responseText, textStatus, xmlHttpRequest) {
        var contentType = xmlHttpRequest.getResponseHeader('Content-Type');
        if (contentType.indexOf("json") > -1) {
            if (sessaoEstaExpirada(xmlHttpRequest)) {
                return;
            }
        }
        if (callBack) {
            callBack();
        }
        
        divParaCarregar.dialog("open");
    });
    
};

$.fn.serializeObject = function() {
    var inputs = $(this).find(":input:not([type=reset])");
    var object = {};
    $.each(inputs, function (index, input) {
        var valorDoInput;
        if ($(input).attr('type') == 'checkbox') {
            valorDoInput = $(input).is(':checked');
        } else {
            valorDoInput = $(input).val();
        }
        object[input.name || input.id] = valorDoInput;
    });

    return object;
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

function aplicaMascaraInteiro() {
    var campos = $('.masknumerointeiro');
    if ($(campos).length == 0) {
        return;
    }

    $(campos).setMask('integer');

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
                var campo = this;
                campo.datepicker("hide");
                campo.val('');
                Mensagem.ExibirMensagemDeErro("Data inválida.", function() {
                    campo.focus();
                });
                
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

function tratarMenusQueDevemSerAbertosEmOutraJanela() {
    $('.abrirEmNovaJanela').click(function (e) {
        e.preventDefault();
        window.open($(this).attr('href'));
    });

}

$(function () {
    tratarMenusQueDevemSerAbertosEmOutraJanela();
    inicializaCamposDatePicker();
    $('.campoDesabilitado').attr('readonly', true);
    $('#btnPesquisar').die("click");
    $('#btnPesquisar').live("click", function (e) {
        e.preventDefault();
        $(".divGrid :last").data("kendoGrid").dataSource.page(1);
    });

    //$('.hover').bind('touchstart touchend', function (e) {
    //    e.preventDefault();
    //    console.log(e.type);
    //    $(this).toggleClass('hover_effect');
    //});

    $('.hover').bind('touchstart', function (e) {
        e.preventDefault();
        $(this).addClass('hover_effect');
    });

    $('li.hover_effect ul a').bind('click blur', function (e) {
        console.log(e.type);
    });


});

function responseIsJsonDataType(ajaxOptions) {
    return (ajaxOptions.dataType == "json")
        || (ajaxOptions.dataTypes && ajaxOptions.dataTypes.indexOf("json") > -1);
}

function sessaoEstaExpirada(request) {

    var sessaoExpirada = false;

    var resposta = $.parseJSON(request.responseText);
    if (resposta.SessaoExpirada) {
        sessaoExpirada = true;
        Mensagem.ExibirMensagemDeErro(resposta.Mensagem, function() {
            location.href = resposta.ReturnUrl;
        });
        
    }

    return sessaoExpirada;

}

function trataFalhaEmRequisicoesAjax(jqXHR){
    if (jqXHR.getResponseHeader('Content-Type').indexOf('html') > -1) {
        Mensagem.ExibirJanelaComHtml(jqXHR.responseText);
    } else {
        Mensagem.ExibirMensagemDeErro(jqXHR.responseText);
    }
}

$.ajaxSetup({
    cache: false
});

$(document).ajaxComplete(function (event, request, ajaxOptions) {
    if (responseIsJsonDataType(ajaxOptions)) {
        sessaoEstaExpirada(request, ajaxOptions);
    }
    
});