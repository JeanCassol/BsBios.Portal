CadastroDeAgendamentoDeCarga = {
    CriarDialogoAgendamentoDeCarregamento: function(urlParaSalvar) {

        $('#divCadastroAgendamento').customDialog({
            title: 'Cadastrar Agendamento',
            buttons: {
                "Salvar": function() {

                    var form = $('form');
                    if (!$(form).validate().form()) {
                        return;
                    }

                    var formData = $(form).serialize();
                    $.post(urlParaSalvar, formData,
                        function (data) {
                            if (data.Sucesso) {
                                $(this).dialog("close");
                                GridAgendamentosDeCarga.AtualizarGrid();
                            } else {
                                atualizaMensagemDeErro(data.Mensagem);
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
        
    
