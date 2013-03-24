
GridAgendamentosDeCarga = {
    ConfigurarGrid: function (configuracao) {
        /// <summary>Configura os campos e as colunas do grid de agendamentos de um dia</summary>
        /// <param name="configuracao" type="Object">TipoComplexo=UrlDeLeitura: url utilizada para fazer a leitura dos dados do grid;
        ///UrlDeEdicao: url utilizada para editar um registro do grid;UrlDeExclusao: url utilizada para excluir um registro do grid</param>
        $("#gridQuotas").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Id: { type: "number" },
                            PesoTotal: { type: "number" },
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
                        cache: false
                    }
                }
            },
            columns:
            [
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_edit" data-id="${Id}"></input>'
                },
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_remove" data-id="${Id}"></input>'
                },
                {
                    width: 100,
                    field: "Data"
                },
                {
                    field: "PesoTotal",
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
            $('#divCadastroAgendamento').load(configuracao.UrlDeEdicao + '/?id=' + $(this).attr('data-id'));
            $('#divCadastroAgendamento').dialog("open");
        });
        
        $('.button_remove').die("click");
        $('.button_remove').live("click", function (e) {
            e.preventDefault();
            var resposta = confirm("Confirma a exclusão do Agendamento?");
            if (!resposta) {
                return;
            }
            location.href = configuracao.UrlDeExclusao + '/?id=' + $(this).attr('data-id');
        });


    }
}