$(document).ready(function() {

    aplicaMascaraData();
    configurarGrid();
    configurarEventosDosBotoes();

    $('#reprocessar').click(function() {
        $.ajax({
            url: UrlPadrao.ReprocessarConhecimentoDeTransporteComErro,
            type: 'POST',
            cache: false,
            dataType: 'json',
            beforeSend: function () {
                bloqueiaPagina("Reprocessando conhecimentos de transporte. Aguarde...");
            },
            success: function (data) {
                if (data.Sucesso) {
                    var kendoGrid = $('#conhecimentosDeTransporte').data("kendoGrid");
                    kendoGrid.dataSource.read();
                } else {
                    Mensagem.ExibirMensagemDeErro(data.Mensagem);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao reprocessar os conhecimentos de transporte. Detalhe: ' + textStatus + errorThrown);
            },
            complete: function () {
                desbloqueiaPagina();
            }

        });

    });

    function configurarGrid() {
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
                            NumeroDoConhecimento: { type: "string" },
                            NumeroDoContrato: { type: "string" },
                            DataDeEmissao: { type: "string" },
                            DescricaoDoStatus: { type: "string" },
                            ValorRealDoFrete: { type: "number" },
                            PesoTotalDaCarga: { type: "number" }
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
                        data: function() {
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
                    field: "NumeroDoConhecimento",
                    title: 'Nº Conhecimento',
                    width: 100
                },
                {
                    field: "DataDeEmissao",
                    title: 'Data de Emissão',
                    width: 90
                },
                {
                    field: "NumeroDoContrato",
                    title: 'Nº Contrato',
                    width: 100
                },
                {
                    field: "ValorRealDoFrete",
                    title: 'Valor',
                    format: '{0:n2}',
                    width: 70
                },
                {
                    field: "PesoTotalDaCarga",
                    title: 'Peso',
                    format: '{0:n3}',
                    width: 70
                },
                {
                    field: "DescricaoDoStatus",
                    title: "Status",
                    width: 90
                }
            ]
        });
    }

    function configurarEventosDosBotoes() {

        $("#conhecimentosDeTransporte").on("click", '.button_visualize', function (e) {
            e.preventDefault();
            var grid = $("#conhecimentosDeTransporte").data("kendoGrid");
            var conhecimentoDeTransporte = grid.obterRegistroSelecionado();
            location.href = UrlPadrao.FormularioDeConhecimentoDeTransporte + '/?chaveEletronica=' + conhecimentoDeTransporte.ChaveEletronica;
        });

    }
});
