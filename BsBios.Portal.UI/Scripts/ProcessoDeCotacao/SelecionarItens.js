var requisicoesSelecionadas = [];
function adicionarRequisicao(requisicao) {
    if (requisicaoSelecionada(requisicao)) {
        return false;
    }
    requisicoesSelecionadas.push(requisicao);
    return true;
}

function requisicaoSelecionada(requisicao) {
    for (var i = 0; i < requisicoesSelecionadas.length; i++) {
        var requisicaoSelecionada = requisicoesSelecionadas[i];
        if (requisicaoSelecionada.NumeroRequisicao == requisicao.NumeroRequisicao
        && requisicaoSelecionada.NumeroItem == requisicao.NumeroItem) {
            return true;
        }
    }
    return false;
}
function atualizaGrid() {
    var grid = $("#divGridRequisicoesSelecionadas").data("kendoGrid");
    grid.dataSource.read();
}

function selecionarRequisicaoDoGrid() {
    var grid = $('#divGridRequisicoesParaSelecionar').data("kendoGrid");
    var dataItem = grid.dataItem(grid.select());
    if (!adicionarRequisicao({
        Id: dataItem.Id, Material: dataItem.Material, Quantidade: dataItem.Quantidade,
        UnidadeMedida: dataItem.UnidadeMedida, NumeroRequisicao: dataItem.NumeroRequisicao,
        NumeroItem: dataItem.NumeroItem, DataDeSolicitacao: dataItem.DataDeSolicitacao,
        CodigoGrupoDeCompra: dataItem.CodigoGrupoDeCompra})) {
        return;
    }
    atualizaGrid();
}

SelecionarItens = {
    ConfigurarTela: function() {

        aplicaMascaraData();
        $('#formFiltroItens').find('.campoDatePicker').datepicker();
        ProcessoDeCotacaoItensGrid.configurar({
            schemaData: 'Registros',
            renderTo: '#divGridRequisicoesParaSelecionar',
            exibirDetalhesDaRequisicao: true,
            exibirBotaoAdicionar: true,
            exibirBotaoBloquear: true,
            pageable:true,
            transportUrl: UrlPadrao.ListarRequisicoesDeCompra,
            transportData: function() {
                return {
                    IdProcessoCotacao: $('#Id').val(),
                    DataDeSolicitacaoInicial: $('#DataDeSolicitacaoInicial').val(),
                    DataDeSolicitacaoFinal: $('#DataDeSolicitacaoFinal').val(),
                    CodigoDoGrupoDeCompras: $('#CodigoDoGrupoDeCompras').val()
                };
            }
        });

        $('#divGridRequisicoesParaSelecionar').find('.button_add').die("click");
        $('#divGridRequisicoesParaSelecionar').find('.button_add').live("click", function() {
            selecionarRequisicaoDoGrid();
        });
        
        $('#divGridRequisicoesParaSelecionar').off('click', '.button_block', false);
        $('#divGridRequisicoesParaSelecionar').on('click', '.button_block', function () {
            var grid = $('#divGridRequisicoesParaSelecionar').data("kendoGrid");
            var dataItem = grid.dataItem($(this).parents('tr:first'));
            if (!confirm('Confirma o bloqueio do item ' + dataItem.NumeroItem + ' da Requisão de Compra ' + dataItem.NumeroRequisicao + '?')) {
                return;
            }
            
            $.ajax({
                url: UrlPadrao.BloquearRequisicaoDeCompra,
                type: 'POST',
                data: JSON.stringify({ IdRequisicaoCompra: dataItem.Id}),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        $('#divGridRequisicoesParaSelecionar').data("kendoGrid").dataSource.read();
                        Mensagem.ExibirMensagemDeSucesso('A Requisição de Compra foi bloqueada com sucesso.');
                    } else {
                        Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao bloquear a Requisição de Compra. Detalhe: ' + data.Mensagem);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao bloquear a Requisição de Compra. Detalhe: ' + textStatus + errorThrown);
                }
            });

        });

        $('#divGridRequisicoesSelecionadas').find('.button_remove').die('click');
        $('#divGridRequisicoesSelecionadas').find('.button_remove').live('click', function() {
            var indice = $(this).parents('tr:first')[0].rowIndex;
            requisicoesSelecionadas.splice(indice, 1);
            atualizaGrid();
        });


        var idProcessoCotacao = $('#Id').val();
        $.ajax({
            url: UrlPadrao.ListarItensDoProcessoDeCotacao,
            type: 'GET',
            cache: false,
            data: {
                idProcessoCotacao: idProcessoCotacao
            },
            dataType: 'json',
            success: function (data) {
                requisicoesSelecionadas = [];
                $.each(data.Registros, function(indexInArray, valueOfElement) {
                    requisicoesSelecionadas.push(valueOfElement);
                });
                ProcessoDeCotacaoItensGrid.configurar({
                    schemaData: function() { return requisicoesSelecionadas; },
                    renderTo: '#divGridRequisicoesSelecionadas',
                    exibirBotaoAdicionar: false,
                    exibirBotaoRemover: true,
                    exibirDetalhesDaRequisicao: true,
                    pageable: false
                });
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert('Ocorreu um erro ao consultar as Requisições de Compra selecionadas. Detalhe: ' + textStatus + errorThrown);
            }
        });

        $('#btnPesquisarItens').click(function(e) {
            e.preventDefault();
            $("#divGridRequisicoesParaSelecionar").data("kendoGrid").dataSource.read();
        });

    },
    ConfigurarJanelaModal: function() {
        $('#divSelecionarItens').customDialog({
            title: 'Selecionar Itens',
            buttons: {
                "Confirmar": function () {
                    if (requisicoesSelecionadas.length == 0) {
                        Mensagem.ExibirMensagemDeErro('É necessário selecionar pelo menos um item para o Processo de Cotação.');
                        return;
                    }
                    var idDasRequisicoesSelecionadas = new Array();
                    $.each(requisicoesSelecionadas, function (indice, requisicaoSelecionada) {
                        idDasRequisicoesSelecionadas.push(requisicaoSelecionada.Id);
                    });

                    $.ajax({
                        url: UrlPadrao.AtualizarItensDoProcessoDeCotacao,
                        type: 'POST',
                        cache: false,
                        data: JSON.stringify({
                            IdProcessoCotacao: $('#Id').val(),
                            IdsDasRequisicoesDeCompra: idDasRequisicoesSelecionadas
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {
                            if (data.Sucesso) {
                                $('#divSelecionarItens').dialog('close');
                                $("#divGridItens").data("kendoGrid").dataSource.read();
                            } else {
                                Mensagem.ExibirMensagemDeErro(data.Mensagem);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atualizar os Itens do Processo de Cotação. Detalhe: ' + textStatus + errorThrown);
                        }
                    });
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#btnSelecionarItens').click(function () {
            $('#divSelecionarItens').customLoad({ url: UrlPadrao.SelecionarItens }, SelecionarItens.ConfigurarTela);
        });
    }
};
