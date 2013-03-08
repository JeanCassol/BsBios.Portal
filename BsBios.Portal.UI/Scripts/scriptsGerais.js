Mensagem = {
    ExibirMensagemDeErro: function (mensagem) {
        alert(mensagem);
    },

    ExibirMensagemDeSucesso: function (mensagem) {
        alert(mensagem);
    }
};

String.prototype.boolean = function () {
    return "true" == this;
};

$(function () {
    /*seleciona todos os campos datepicker para inicializar o componente do jquery UI*/
    $('.campoDatePicker').datepicker();
    
    $.fn.customKendoGrid = function (c) {
        c.groupable = false;
        c.sortable = true;
        c.pageable =
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
        c.selectable = 'row';
        c.dataSource.serverFiltering = true;
        c.dataSource.serverPaging = true;
        c.dataSource.pageSize = 10;
        
        this.kendoGrid(c);
    };
});