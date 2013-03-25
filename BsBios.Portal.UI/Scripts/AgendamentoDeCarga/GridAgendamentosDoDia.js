
GridAgendamentosDeCarga = {
    ConfigurarGrid: function (configuracao) {
        /// <summary>Configura os campos e as colunas do grid de agendamentos de um dia</summary>
        /// <param name="configuracao" type="Object">TipoComplexo=UrlDeLeitura: url utilizada para fazer a leitura dos dados do grid;
        ///UrlDeEdicao: url utilizada para editar um registro do grid;UrlDeExclusao: url utilizada para excluir um registro do grid;</param>
        $("#gridAgendamentosDeCarga").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Id: { type: "number" },
                            Peso: { type: "number" },
                            Placa: { type: "string" },
                            Realizado: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: configuracao.UrlDeLeitura,
                        type: 'GET',
                        cache: false,
                        data: function () {
                            return {
                                IdQuota: configuracao.IdQuota
                            };
                        }
                    }
                }
            },
            columns:
            [
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_edit" data-idagendamento="${IdAgendamento}"></input>'
                },
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_remove" data-idagendamento="${IdAgendamento}"></input>'
                },
                {
                    width: 100,
                    field: "Placa"
                },
                {
                    field: "Peso",
                    title: "Peso Total",
                    width: 100
                },
                {
                    field: "Realizado",
                    width: 100
                }
            ]
        });

        $('.button_edit').die("click");
        $('.button_edit').live("click", function (e) {
            e.preventDefault();
            $('#divCadastroAgendamento').load(configuracao.UrlDeEdicao + '/?idAgendamento=' + $(this).attr('data-idagendamento'));
            $('#divCadastroAgendamento').dialog("open");
        });
        
        $('.button_remove').die("click");
        $('.button_remove').live("click", function (e) {
            e.preventDefault();
            var resposta = confirm("Confirma a exclusão do Agendamento?");
            if (!resposta) {
                return;
            }
            var idAgendamento = $(this).attr('data-idagendamento');
            $.ajax({
                url: configuracao.UrlDeExclusao,
                type: 'POST',
                data: JSON.stringify({ IdQuota: configuracao.IdQuota, IdAgendamento: idAgendamento}),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        GridAgendamentosDeCarga.AtualizarTela(data.Quota);
                    } else {
                        Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atualizar excluir o Agendamento. Detalhe: ' + data.Mensagem);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao excluir o Agendamento. Detalhe: ' + textStatus + errorThrown);
                }
            });
        });
    },
    AtualizarTela: function (quota) {
        $('#lblPesoAgendado').text(quota.PesoAgendado);
        $('#lblPesoDisponivel').text(quota.PesoDisponivel);
        var grid = $("#gridAgendamentosDeCarga").data("kendoGrid");
        grid.dataSource.read();
    }
}