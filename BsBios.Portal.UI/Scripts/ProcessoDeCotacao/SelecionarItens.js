requisicoesSelecionadas = [];
var contadorDeRegistros = 0;
function removerRequisicao(numeroRequisicao, numeroItem) {
    for (var i = 0; i < requisicoesSelecionadas.length; i++) {
        var requisicao = requisicoesSelecionadas[i];
        if (requisicao.Numero == numeroRequisicao && requisicao.NumeroItem == numeroItem) {
            requisicoesSelecionadas.splice(i, 1);
            break;
        }
    }
}

function adicionarRequisicao(requisicao) {
    if (requisicaoSelecionada(requisicao)) {
        return;
    }
    requisicoesSelecionadas.push(requisicao);
}

function requisicaoSelecionada(requisicao) {
    for (var i = 0; i < requisicoesSelecionadas.length; i++) {
        var requisicaoSelecionada = requisicoesSelecionadas[i];
        if (requisicaoSelecionada.Numero == requisicao.Numero
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
    adicionarRequisicao({
        Codigo: dataItem.Codigo, Nome: dataItem.Nome, Email: dataItem.Email,
        Cnpj: dataItem.Cnpj, Municipio: dataItem.Municipio, Uf: dataItem.Uf
    });
    atualizaGrid();
}

SelecionarItens = {
    ConfigurarTela: function() {

        ProcessoDeCotacaoItensGrid.configurar({
            schemaData: 'Registros',
            renderTo: '#divGridRequisicoesParaSelecionar',
            exibirBotaoAdicionar: true,
            exibirBotaoRemover: false,
            pageable: true,
            transportUrl: UrlPadrao.ListarRequisicoesDeCompra,
            transportData: function() {
                return $('#formFiltroItens').serialize();
            }
        });

        $('#divGridRequisicoesParaSelecionar').find('.button_add').die("click");
        $('#divGridRequisicoesParaSelecionar').find('.button_add').live("click", function() {
            selecionarRequisicaoDoGrid();
        });

        $('#divGridRequisicoesSelecionadas').find('.button_remove').die('click');
        $('#divGridRequisicoesSelecionadas').find('.button_remove').live('click', function() {
            var indice = $(this).parents('tr:first')[0].rowIndex;
            requisicoesSelecionadas.splice(indice, 1);
            atualizaGrid();
        });

        var idProcessoCotacao = $('#Id').val();
        $.ajax({
            url: UrlPadrao.ListarItensDoProcessoDeCotacao + '/?idProcessoCtoacao=' + idProcessoCotacao,
            type: 'GET',
            cache: false,
            data: {
                idProcessoCotacao: idProcessoCotacao
            },
            dataType: 'json',
            success: function(data) {
                $.each(data.Registros, function(indexInArray, valueOfElement) {
                    requisicoesSelecionadas.push(valueOfElement);
                });
                ProcessoDeCotacaoItensGrid.configurar({
                    schemaData: function() { return requisicoesSelecionadas; },
                    renderTo: '#divGridRequisicoesSelecionadas',
                    exibirBotaoAdicionar: false,
                    exibirBotaoRemover: true,
                    pageable: false
                });
            },
            error: function(jqXHR, textStatus, errorThrown) {
                alert('Ocorreu um erro ao consultar as Requisições de Compra selecionadas. Detalhe: ' + textStatus + errorThrown);
            }
        });

        $('#btnPesquisarFornecedorGeral').click(function(e) {
            e.preventDefault();
            $("#divGridFornecedoresGerais").data("kendoGrid").dataSource.read();
        });

        $('#btnLimpar').click(function() {
            $('form input[type!=button]').val('');
        });
    },
    ConfigurarJanelaModal: function() {
        $('#divSelecionarItens').customDialog({
            title: 'Selecionar Itens',
            buttons: {
                "Confirmar": function () {
                    var idDasRequisicoesSelecionadas = new Array();
                    $.each(requisicoesSelecionadas, function (indice, requisicaoSelecionada) {
                        idDasRequisicoesSelecionadas.push(requisicaoSelecionada.Id);
                    });

                    $.ajax({
                        url: UrlPadrao.AtualizarItensDoProcessoDeCotacao,
                        type: 'POST',
                        cache: false,
                        data: JSON.stringify({
                            IdProcessoCotacao: idProcessoCotacao,
                            IdDasRequisicoesSelecionadas: idDasRequisicoesSelecionadas
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data) {
                            if (data.Sucesso) {
                                $('#divSelecionarItens').dialog('close');
                                $("#gridCotacaoFornecedor").data("kendoGrid").dataSource.read();
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
            $('#divSelecionarItens').load(UrlPadrao.SelecionarItens, function () {
                $('#divSelecionarItens').dialog('open');
                SelecionarItens.ConfigurarTela();
            });
        });

    }
};
