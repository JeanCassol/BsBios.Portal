CadastroDeAgendamentoDeCarga = {
    CriarDialogoAgendamentoDeCarregamento: function(urlParaSalvar) {

        $('#divCadastroAgendamento').dialog({
            autoOpen: false,
            width: 800,
            resizable: false,
            title: 'Cadastrar Agendamento',
            modal: true,
            beforeClose: function() {
                $('#divCadastroAgendamento').empty();
            },
            buttons: {
                "Confirmar": function() {
                    var codigosDosFornecedoresSelecionados = new Array();
                    $.each(fornecedoresSelecionados, function(indice, fornecedorSelecionado) {
                        codigosDosFornecedoresSelecionados.push(fornecedorSelecionado.Codigo);
                    });

                    $.ajax({
                        url: urlParaSalvar,
                        type: 'POST',
                        cache: false,
                        data: JSON.stringify({
                            IdProcessoCotacao: idProcessoCotacao,
                            CodigoFornecedoresSelecionados: codigosDosFornecedoresSelecionados
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function(data) {
                            if (data.Sucesso) {
                                
                                $('#divSelecionarFornecedores').dialog('close');
                                $("#gridCotacaoFornecedor").data("kendoGrid").dataSource.read();
                            } else {
                                Mensagem.ExibirMensagemDeErro(data.Mensagem);
                            }
                        },
                        error: function(jqXHR, textStatus, errorThrown) {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao consultar os Fornecedores Selecionados. Detalhe: ' + textStatus + errorThrown);
                        }
                    });


                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}
        
    
