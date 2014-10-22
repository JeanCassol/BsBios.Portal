$(document).ready(function () {
    aplicaMascaraCnpj();
    aplicaMascaraData();

    $("#conhecimentosDeTransporte").customKendoGrid({
        dataSource: {
            schema: {
                data: 'Registros',
                model: {
                    id: 'ChaveEletronica',
                    fields: {
                        ChaveEletronica: { type: "string" },
                        CodigoDoFornecedor: { type: "string" },
                        CodigoDaTransportadora: { type: "string" },
                        DataDeEmissao: { type: "string" },
                        Status: { type: "number" }
                    }
                },

                total: 'QuantidadeDeRegistros'
            },
            serverFiltering: true,
            serverPaging: true,
            transport: {
                read: {
                    url: UrlPadrao.ListarConhecimentoDeTransporte,
                    type: 'GET',
                    cache: false,
                    data: function () {
                        return $('#formularioDeFiltros').serializeObject();
                    }
                }
            }
        },
        columns:
        [
            {
                title: ' ',
                width: 30,
                sortable: false,
                template: '<input type="button" class="button_visualize"></input>'
            },
            {
                field: "CodigoDoFornecedor",
                title: "Fornecedor",
                width: 100
            },
            {
                field: 'CodigoDaTransportadora',
                title: 'Transportadora',
                width: 100
            },
            {
                field: "DataDeEmissao",
                title: 'Data de Emissão',
                width: 100
            },
            {
                field: "Status",
                width: 100
            }
        ]
    });
});
