SelecionarFornecedor = {
    FornecedorSelecionado: null,
    Configurar: function(url) {
        $('body').append('<div id="divSelecionarFornecedor" class="janelaModal"></div>');
        $('#divSelecionarFornecedor').customDialog({
            title: 'Selecionar Fornecedor',
            buttons: {
                "Confirmar": function() {
                    if (FornecedorSelecionado == null) {
                        Mensagem.ExibirMensagemDeErro("É necessário selecionar um Fornecedor.");
                        return;
                    }
                    $('#CodigoFornecedor').val(FornecedorSelecionado.Codigo);
                    $('#Fornecedor').val(unescape(FornecedorSelecionado.Nome));
                    $(this).dialog("close");
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
        $('#btnSelecionarFornecedor').click(function() {

            $('#divSelecionarFornecedor').load(url
                + '/?Codigo=' + $('#CodigoFornecedor').val() + '&Nome=' + escape($('#Fornecedor').val()),
                function(response, status, xhr) {
                    $('#divSelecionarFornecedor').dialog('open');
                });
        });

    }
}