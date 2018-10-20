ProcessoCotacaoHistorico = {
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
                        cache: false
                    }
                }
            },
            groupable: false,
            scrollable: true,
            //selectable: 'row',
            pageable: false,
            autoBind: false,
            noRecords: true,
            columns: [
                {
                    field: "Data",
                    width: 80,
                    title: "Data"
                },
                {
                    field: "Usuario",
                    width: 200,
                    title: "Usuário"
                },
                {
                    field: "Acao",
                    width: 250,
                    title: "Ação"
                }
            ]
        });

    },
    carregarGrid: function(idFornecedorParticipante) {
        $("#gridCotacaoHistorico").data("kendoGrid").dataSource.read({IdFornecedorParticipante: idFornecedorParticipante});
    }
}