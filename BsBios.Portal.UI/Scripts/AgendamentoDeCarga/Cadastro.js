var notasFiscais = new Array();
CadastroDeAgendamentoDeCarga = {
    CriarDialogoAgendamentoDeCarregamento: function(urlParaSalvar) {

        $('#divCadastroAgendamento').customDialog({
            title: 'Cadastrar Agendamento de Carregamento',
            buttons: {
                "Salvar": function() {

                    var form = $('form');
                    if (!$(form).validate().form()) {
                        return;
                    }

                    var formData = $(form).serialize();
                    $.post(urlParaSalvar, formData,
                        function(data) {
                            if (data.Sucesso) {
                                $('#divCadastroAgendamento').dialog("close");
                                GridAgendamentosDeCarga.AtualizarTela(data.Quota);
                            } else {
                                atualizaMensagemDeErro(data.Mensagem);
                            }
                        });
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
    },
    CriarDialogoAgendamentoDeDescarregamento: function(urlParaSalvar) {

        $('#divCadastroAgendamento').customDialog({
            title: 'Cadastrar Agendamento de Descarregamento',
            buttons: {
                "Salvar": function() {

                    var form = $('#formAgendamento');
                    if (!$(form).validate().form()) {
                        return;
                    }
                    
                    $.ajax({
                        url: urlParaSalvar,
                        type: 'POST',
                        data: JSON.stringify(
                            {
                                IdQuota: $('#IdQuota').val(),
                                IdAgendamento: $('#IdAgendamento').val(),
                                Placa: $('#Placa').val(),
                                NotasFiscais: GridNotasFiscais.NotasFiscais()
                            }),
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {
                            if (data.Sucesso) {
                                $('#divCadastroAgendamento').dialog("close");
                                GridAgendamentosDeCarga.AtualizarTela(data.Quota);
                            } else {
                                atualizaMensagemDeErro(data.Mensagem);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao salvar o Agendamento de Descarregamento. Detalhe: ' + textStatus + errorThrown);
                        }
                    });


                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
    },
    AdicionarNotaFiscal: function(notaFiscal) {
        notasFiscais.push(notaFiscal);
    }
};
        
    
