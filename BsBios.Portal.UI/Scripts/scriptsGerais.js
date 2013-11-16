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

    if (!configuracao.width) {
        configuracao.width = 800;
    }

    this.dialog(configuracao);
};

$.fn.customLoad = function (url, callBack) {

    var divParaCarregar = this;
    
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
    var inputs = $(this).find(":input");
    var object = {};
    $.each(inputs, function (index, input) {

        
        object[input.name] = $(input).val();
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

function responseIsJsonDataType(ajaxOptions) {
    return (ajaxOptions.dataType == "json")
        || (ajaxOptions.dataTypes && ajaxOptions.dataTypes.indexOf("json") > -1);
}

function sessaoEstaExpirada(request) {

    var sessaoExpirada = false;

    var resposta = JSON.parse(request.responseText);
    if (resposta.SessaoExpirada) {
        sessaoExpirada = true;
        Mensagem.ExibirMensagemDeErro(resposta.Mensagem, function() {
            location.href = resposta.ReturnUrl;
        });
        
    }

    return sessaoExpirada;

}


$(document).ajaxComplete(function (event, request, ajaxOptions) {
    if (responseIsJsonDataType(ajaxOptions)) {
        sessaoEstaExpirada(request, ajaxOptions);
    }
    
});