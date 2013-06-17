function DesabilitaEdicaoDosCamposDoItem() {
    var inputs = $('#formCotacaoItem').find('input,textarea');
    $(inputs).addClass('campoDesabilitado');
    $(inputs).attr('readonly', true);
}

function configurarEventoDeConsultaDoGrid() {
    $('#divGridItens').off('click', '.button_visualize', false);
    $('#divGridItens').on('click', '.button_visualize', function () {
        var grid = $('#divGridItens').data("kendoGrid");
        var dataItem = grid.dataItem(grid.select());
        $('#divCotacaoItem').customLoad({
            url: UrlPadrao.EditarItemDoCadastroDeCotacaoDeMaterial
                    + "/?idProcessoCotacao=" + $('#IdProcessoCotacao').val()
                    + "&codigoDoFornecedor=" + $('#CodigoFornecedor').val()
                    + "&numeroDaRequisicao=" + dataItem.NumeroRequisicao
                    + "&numeroDoItemDaRequisicao=" + dataItem.NumeroItem
        },
            function () {
                aplicaMascaraMoeda('#divCotacaoItem');
                aplicaMascaraQuantidade('#divCotacaoItem');
                aplicaMascaraData('#divCotacaoItem');
                DesabilitaEdicaoDosCamposDoItem();
            }
        );
    });

}

function configurarEventoDeEdicaoDoGrid() {
    $('#divGridItens').off('click','.button_edit',false);
    $('#divGridItens').on('click', '.button_edit', function () {
        var idCotacao = $('#IdCotacao').val();
        if (idCotacao == 0) {
            Mensagem.ExibirMensagemDeErro('Não é possível informar a cotação de um item antes de salvar as Informações Gerais.');
            return;
        }
        var grid = $('#divGridItens').data("kendoGrid");
        var dataItem = grid.dataItem(grid.select());
        $('#divCotacaoItem').customLoad({
            url: UrlPadrao.EditarItemDoCadastroDeCotacaoDeMaterial
                + "/?idProcessoCotacao=" + $('#IdProcessoCotacao').val()
                + "&codigoDoFornecedor=" + $('#CodigoFornecedor').val()
                + "&numeroDaRequisicao=" + dataItem.NumeroRequisicao
                + "&numeroDoItemDaRequisicao=" + dataItem.NumeroItem,
            validar: true
        },
            function () {
                //ocultaCamposDeValorDosImpostos();
                aplicaMascaraMoeda('#divCotacaoItem');
                aplicaMascaraQuantidade('#divCotacaoItem');
                aplicaMascaraData('#divCotacaoItem');
                inicializaCamposDatePicker('#divCotacaoItem');
            }
        );

    });
}
