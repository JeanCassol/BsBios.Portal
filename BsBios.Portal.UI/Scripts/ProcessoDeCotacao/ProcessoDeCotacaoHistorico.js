﻿ProcessoCotacaoHistorico = {
    configurarGrid: function() {
        $("#gridCotacaoHistorico").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Data: { type: "string" },
                            Usuario: { type: "string" },
                            Acao: { type: "string" }
                        }
                    }
                },
                serverFiltering: true,
                serverPaging: false,
                transport: {
                    read: {
                        url: UrlPadrao.ListarHistoricoProcessoCotacao,
                        type: 'GET',
                        cache: false /*,
                        data: function () {
                            return { IdProcessoCotacao: $('#Id').val() };
                        }*/
                    }
                }
            },
            groupable: false,
            scrollable: true,
            //selectable: 'row',
            pageable: false,
            autoBind: true,

            columns: [
                {
                    field: "Data",
                    width: 100,
                    title: "Data"
                },
                {
                    field: "Usuario",
                    width: 200,
                    title: "Usuário"
                },
                {
                    field: "Acao",
                    width: 195,
                    title: "Ação"
                }
            ]
        });

    },
    carregarGrid: function() {
        $("#gridCotacaoHistorico").data("kendoGrid").dataSource.read();
    }
}