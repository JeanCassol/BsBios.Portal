$(document).ready(function () {

    configurarGridDeNotasFiscais();
    configurarGridDeOrdensDeTransporte();

    function configurarGridDeNotasFiscais() {
        $('#notasFiscais').customKendoGrid({
            pageable: false,
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Chave: { type: "string" },
                            Numero: { type: "string" },
                            Serie: { type: "string" },
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: UrlPadrao.ListarNotasFiscaisDeConhecimentoDeTransporte + '/?chaveEletronica=' + $('#ChaveEletronica').val(),
                        type: 'GET',
                        cache: false
                    }
                }
            },
            columns:
            [
                {
                    field: "Chave",
                    title: "Chave Eletrônica",
                    width: '50%'
                },

                {
                    field: "Numero",
                    title: "Número"
                },
                {
                    field: 'Serie',
                    title: 'Série'
                }
            ]
        });

    }

    function configurarGridDeOrdensDeTransporte() {
        $("#ordensDeTransporte").customKendoGrid({
            pageable: false,
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        id: 'Id',
                        fields: {
                            Id: { type: "number" },
                            Material: { type: "string" },
                            CodigoDoFornecedor: { type: "string" },
                            NomeDoFornecedor: { type: "string" },
                            QuantidadeLiberada: { type: "number" },
                            QuantidadeColetada: { type: "number" },
                            QuantidadeRealizada: { type: "number" }
                        }
                    },
                    total: 'QuantidadeDeRegistros',
                    type: 'json'
                },
                transport: {
                    read: {
                        url: UrlPadrao.OrdensDeTransporteDoConhecimento + '/?chaveEletronica=' + $('#ChaveEletronica').val(),
                        type: 'GET',
                        cache: false,
                        data: function () {
                            return $('form').serializeObject();
                        }
                    }
                }
            },
            scrollable: true,
            columns:
            [
                {
                    field: "Id",
                    width: 60,
                    title: "Nº Ordem"
                }, {
                    field: "Material",
                    width: 120,
                    title: "Material"
                },
                {
                    width: 80,
                    field: "CodigoDoFornecedor",
                    title: "Código"
                },
                {
                    width: 200,
                    field: "NomeDoFornecedor",
                    title: "Fornecedor"
                },
                {
                    width: 70,
                    field: "QuantidadeLiberada",
                    title: "Liberada",
                    format: "{0:n3}"
                },
                {
                    width: 70,
                    field: "QuantidadeColetada",
                    title: "Coletada",
                    format: "{0:n3}"
                },
                {
                    width: 80,
                    field: "QuantidadeRealizada",
                    title: "Descarregada",
                    format: "{0:n3}"
                },
                {
                    template: function() {
                        var permiteAtribuir = $('#PermiteAtribuir').val().boolean();
                        return permiteAtribuir
                            ? '<input type="button" id="atribuir" value="Atribuir" class="blue"/>'
                            : '';
                    },
                    attributes: {
                        style: "text-align: center;"
                    },
                    title: 'Ação'
                }

            ]
        });

        $("#ordensDeTransporte").off('click', '#atribuir');
        $("#ordensDeTransporte").on('click', '#atribuir', function(event) {
            var kendoGrid = $('#ordensDeTransporte').data("kendoGrid");
            var ordemSelecionada = kendoGrid.obterRegistroSelecionado();
            $.ajax({
                url: UrlPadrao.AtribuirConhecimentoParaOrdemDeTransporte,
                type: 'POST',
                data: JSON.stringify(
                {
                    ChaveDoConhecimento: $('#ChaveEletronica').val(),
                    IdDaOrdemDeTransporte: ordemSelecionada.Id
                }),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                beforeSend: function() {
                    bloqueiaPagina("Atribuindo Ordem de Transporte. Aguarde...");
                },
                success: function(data) {
                    if (data.Sucesso) {
                        $('#PermiteAtribuir').val(false);
                        kendoGrid.dataSource.read();
                        $('#DescricaoDoStatus').val('Atribuido');
                    } else {
                        Mensagem.ExibirMensagemDeErro(data.Mensagem);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atribuir o Conhecimento de Transporte para a Ordem de Transporte. Detalhe: ' + textStatus + errorThrown);
                },
                complete: function() {
                    desbloqueiaPagina();
                }

            });

        });


    }
});