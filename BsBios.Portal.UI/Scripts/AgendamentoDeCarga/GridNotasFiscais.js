var NotasFiscaisAdicionadas = new Array();
var indiceEdicao = -1;
function existeNotaFiscal(notaFiscalParaVerificar) {
    var notaFiscalEncontrada = false;
    $.each(NotasFiscaisAdicionadas, function (indice, notaFiscal) {
        if (notaFiscalEncontrada) {
            return;
        }
        if (indice == indiceEdicao) {
            return;
        }
        if (notaFiscal.CnpjEmitente == notaFiscalParaVerificar.CnpjEmitente
            && notaFiscal.Numero == notaFiscalParaVerificar.Numero
            && notaFiscal.Serie == notaFiscalParaVerificar.Serie) {
            notaFiscalEncontrada = true;
        }
    });
    return notaFiscalEncontrada;
}

function atualizarGrid() {
    var grid = $("#divGridNotasFiscaisAdicionadas").data("kendoGrid");
    grid.dataSource.read();
}

function carregaCamposDaNotaFiscal(notaFiscal) {
    $('#NotaFiscal_Numero').val(notaFiscal.Numero);
    $('#NotaFiscal_Serie').val(notaFiscal.Serie);
    $('#NotaFiscal_DataDeEmissao').val(notaFiscal.DataDeEmissao);
    $('#NotaFiscal_CnpjDoEmitente').val(Formato.formataCnpj(notaFiscal.CnpjDoEmitente));
    $('#NotaFiscal_NomeDoEmitente').val(notaFiscal.NomeDoEmitente);
    $('#NotaFiscal_CnpjDoContratante').val(Formato.formataCnpj(notaFiscal.CnpjDoContratante));
    $('#NotaFiscal_NomeDoContratante').val(notaFiscal.NomeDoContratante);
    $('#NotaFiscal_NumeroDoContrato').val(notaFiscal.NumeroDoContrato);
    $('#NotaFiscal_Peso').val(Globalize.format(notaFiscal.Peso));
    $('#NotaFiscal_Valor').val(Globalize.format(notaFiscal.Valor));
}


function criarEventoEdit() {
    $("#divGridNotasFiscaisAdicionadas").find('.button_edit').die("click");
    $("#divGridNotasFiscaisAdicionadas").find('.button_edit').live("click", function (e) {
        e.preventDefault();
        indiceEdicao = $(this).parents('tr:first')[0].rowIndex;
        var notaFiscal = NotasFiscaisAdicionadas[indiceEdicao];
        carregaCamposDaNotaFiscal(notaFiscal);

        $('#btnSalvarNotaFiscal').val('Atualizar');
        $('#btnCancelarEdicao').show();
    });

}

function criarEventoRemove() {
    $("#divGridNotasFiscaisAdicionadas").find('.button_remove').die("click");
    $("#divGridNotasFiscaisAdicionadas").find('.button_remove').live("click", function (e) {
        e.preventDefault();
        var indice = $(this).parents('tr:first')[0].rowIndex;
        NotasFiscaisAdicionadas.splice(indice, 1);
        atualizarGrid();
    });

}

function criarEventoVisualize() {
    $("#divGridNotasFiscaisAdicionadas").find('.button_visualize').die("click");
    $("#divGridNotasFiscaisAdicionadas").find('.button_visualize').live("click", function (e) {
        e.preventDefault();
        indiceEdicao = $(this).parents('tr:first')[0].rowIndex;
        var notaFiscal = NotasFiscaisAdicionadas[indiceEdicao];
        carregaCamposDaNotaFiscal(notaFiscal);

        $('#btnSalvarNotaFiscal').val('Atualizar');
        $('#btnCancelarEdicao').show();
    });

}


GridNotasFiscais = {
    ConfigurarGrid: function (configuracao) {
        /// <summary>Configura os campos e as colunas do grid de agendamentos de um dia</summary>
        /// <param name="configuracao" type="Object">PermiteEditar: indica se pode editar as notas fiscais</param>
        
        var arrayDeColunas = new Array();

        if (configuracao.PermiteEditar) {
            arrayDeColunas.push(
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 30,
                    sortable: false,
                    template: '<input type="button" class="button_edit"></input>'
                });
            arrayDeColunas.push({
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 30,
                    sortable: false,
                    template: '<input type="button" class="button_remove"></input>'
                });
        } else {
            arrayDeColunas.push({
                title: ' ', /*coloco um espaço para deixar o header sem título*/
                width: 30,
                sortable: false,
                template: '<input type="button" class="button_visualize"></input>'
            });
        }

        arrayDeColunas = arrayDeColunas.concat(
            {
                width: 100,
                field: "Numero",
                title: "Número"
            },
            {
                width: 40,
                field: "Serie",
                title: "Série"
            },
            {
                width: 70,
                field: "DataDeEmissao",
                title: "Emissão"
            },
            {
                width: 300,
                field: "NomeDoEmitente",
                title: "Emitente"
            },
            {
                field: "Peso",
                width: 80,
                format: "{0:n3}"
            },
            {
                field: "Valor",
                width: 80,
                format: "{0:n}"
            });

        $("#divGridNotasFiscaisAdicionadas").customKendoGrid({
            dataSource: {
                schema: {
                    data: function() { return NotasFiscaisAdicionadas; },
                    model: {
                        fields: {
                            Numero: { type: "string" },
                            Serie: { type: "string" },
                            DataDeEmissao: { type: "string" },
                            NomeDoEmitente: { type: "string" },
                            Peso: { type: "number" },
                            Valor: { type: "number" }
                        }
                    }
                },
                serverFiltering: true,
                serverPaging: true
            },
            columns: arrayDeColunas,
            pageable:false
        });

        if (configuracao.PermiteEditar) {
            criarEventoEdit();
            criarEventoRemove();
        } else {
            criarEventoVisualize();
        }
        
    },
    SalvarNotaFiscal: function (notaFiscal) {
        if (existeNotaFiscal(notaFiscal)) {
            throw "Já existe Nota Fiscal cadastrada para o CNPJ " + notaFiscal.CnpjDoEmitente +
                " com Número " + notaFiscal.Numero + " e Série " + notaFiscal.Serie + ". Edite a Nota Fiscal existente.";
        }
        if (indiceEdicao == -1) {
            NotasFiscaisAdicionadas.push(notaFiscal);
        } else {
            NotasFiscaisAdicionadas[indiceEdicao] = notaFiscal;
        }
        indiceEdicao = -1;
        atualizarGrid();
    },
    NotasFiscais: function() {
        return NotasFiscaisAdicionadas;
    },
    CarregarNotas: function (urlDeLeitura) {
        $.ajax({
        url: urlDeLeitura,
        type: 'GET',
        cache: false,
        async: false,
        dataType: 'json',
        success: function (data) {
            if (!data.Sucesso) {
                Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao consultar as Notas Fiscais do Agendamento. Detalhe: ' + data.Mensagem);
                return;
            }
            NotasFiscaisAdicionadas = data.NotasFiscais;

        },
        error: function (jqXHR, textStatus, errorThrown) {
            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao consultar as Notas Fiscais do Agendamento.. Detalhe: ' + textStatus + errorThrown);
        }
    });
        
},
    CarregarCamposParaPrimeiraNota: function() {
        if (NotasFiscaisAdicionadas.length == 0) {
            return;
        }
        // get a reference to the grid widget
        var grid = $("#divGridNotasFiscaisAdicionadas").data("kendoGrid");
        // selects first grid item
        grid.select(grid.tbody.find(">tr:first"));
        carregaCamposDaNotaFiscal(NotasFiscaisAdicionadas[0]);
    }
}