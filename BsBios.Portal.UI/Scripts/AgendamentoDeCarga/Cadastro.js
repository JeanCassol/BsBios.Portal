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

                    var form = $('form');
                    if (!$(form).validate().form()) {
                        return;
                    }

                    var formData = $(form).serialize();
                    $.post(urlParaSalvar, formData,
                        function(data) {
                            if (data.Sucesso) {
                                $(this).dialog("close");
                                GridAgendamentosDeCarga.AtualizarGrid();
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
    AdicionarNotaFiscal: function(notaFiscal) {
        notasFiscais.push(notaFiscal);
    }
};
        
    
