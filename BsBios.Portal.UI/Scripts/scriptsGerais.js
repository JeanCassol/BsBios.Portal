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
        var val = parseFloat(valor);
        if (isNaN(val))
            return 0;
        else
            return val;
    }
};

$.fn.customKendoGrid = function (configuracao) {
    configuracao.groupable = false;
    configuracao.sortable = true;
    configuracao.pageable =
    {
        refresh: true,
        pageSizes: true,
        messages: {
            display: '{0} - {1} de {2} registros',
            empty: 'Nenhum registro encontrado',
            itemsPerPage: 'registros por página',
            first: 'Ir para a primeira página',
            previous: 'Ir para a página anterior',
            next: 'Ir para a próxima página',
            last: 'Ir para a última página',
            refresh: 'Atualizar'
        }
    };
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
    configuracao.beforeClose = function () {
        $(this).empty();
    };

    if (!configuracao.width) {
        configuracao.width = 800;
    }

    this.dialog(configuracao);
};


$(function () {
    /*seleciona todos os campos datepicker para inicializar o componente do jquery UI*/
    var camposDatePicker = $('.campoDatePicker');
    if ($(camposDatePicker).length > 0) {
        $(camposDatePicker).datepicker();
    }
    

});