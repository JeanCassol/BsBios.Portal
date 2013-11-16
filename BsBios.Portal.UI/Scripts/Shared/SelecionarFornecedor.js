SelecionarFornecedor = {
    FornecedorSelecionado: null,
    Configurar: function (idDoCampoDoCodigoDoFornecedor, idDoCampoDoNomeDoFornecedor, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoFornecedor) {
        $('body').append('<div id="' + idDaDivDaJanelaDeDialogo + '" class="janelaModal"></div>');
        $('#' + idDaDivDaJanelaDeDialogo).customDialog({
            title: 'Selecionar Fornecedor',
            buttons: {
                "Confirmar": function() {
                    if (SelecionarFornecedor.FornecedorSelecionado == null) {
                        Mensagem.ExibirMensagemDeErro("É necessário selecionar um Fornecedor.");
                        return;
                    }
                    $(idDoCampoDoCodigoDoFornecedor).val(SelecionarFornecedor.FornecedorSelecionado.Codigo);
                    $(idDoCampoDoNomeDoFornecedor).val(unescape(SelecionarFornecedor.FornecedorSelecionado.Nome));
                    SelecionarFornecedor.FornecedorSelecionado = null;
                    $(this).dialog("close");
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
        $(idDoBotaoDeSelecaoDoFornecedor).click(function() {
            $('#' + idDaDivDaJanelaDeDialogo).customLoad(UrlPadrao.SelecionarFornecedor
                + '/?Codigo=' + $(idDoCampoDoCodigoDoFornecedor).val() + '&Nome=' + escape($(idDoCampoDoNomeDoFornecedor).val()));
        });

    }
}