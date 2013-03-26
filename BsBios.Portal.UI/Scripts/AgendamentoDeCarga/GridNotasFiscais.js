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

GridNotasFiscais = {
    ConfigurarGrid: function (configuracao) {
        /// <summary>Configura os campos e as colunas do grid de agendamentos de um dia</summary>
        /// <param name="configuracao" type="Object">TipoComplexo=UrlDeLeitura: url utilizada para fazer a leitura dos dados do grid;
        ///UrlDeEdicao: url utilizada para editar um registro do grid;UrlDeExclusao: url utilizada para excluir um registro do grid;</param>
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
            columns:
            [
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_edit"></input>'
                },
                {
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 40,
                    sortable: false,
                    template: '<input type="button" class="button_remove"></input>'
                },
                {
                    width: 100,
                    field: "Numero",
                    title:"Número"
                },
                {
                    width: 100,
                    field: "Serie",
                    title: "Série"
                },
                {
                    width: 100,
                    field: "DataDeEmissao",
                    title: "Data de Emissao"
                },
                {
                    width: 300,
                    field: "NomeDoEmitente",
                    title: "Emitente"
                },

                {
                    field: "Peso",
                    width: 80
                },
                {
                    field: "Valor",
                    width: 80
                }
            ]
        });

        $("#divGridNotasFiscaisAdicionadas").find('.button_edit').die("click");
        $("#divGridNotasFiscaisAdicionadas").find('.button_edit').live("click", function (e) {
            e.preventDefault();
            indiceEdicao = $(this).parents('tr:first')[0].rowIndex;
            var notaFiscal = NotasFiscaisAdicionadas[indiceEdicao];

            $('#NotaFiscal_Numero').val(notaFiscal.Numero);
            $('#NotaFiscal_Serie').val(notaFiscal.Serie);
            $('#NotaFiscal_DataDeEmissao').val(notaFiscal.DataDeEmissao);
            $('#NotaFiscal_CnpjDoEmitente').val(notaFiscal.CnpjDoEmitente);
            $('#NotaFiscal_NomeDoEmitente').val(notaFiscal.NomeDoEmitente);
            $('#NotaFiscal_CnpjDoContratante').val(notaFiscal.CnpjDoContratante);
            $('#NotaFiscal_NomeDoContratante').val(notaFiscal.NomeDoContratante);
            $('#NotaFiscal_NumeroDoContrato').val(notaFiscal.NumeroDoContrato);
            $('#NotaFiscal_Peso').val(notaFiscal.Peso);
            $('#NotaFiscal_Valor').val(notaFiscal.Valor);

            $('#btnSalvarNotaFiscal').val('Atualizar');
            $('#btnCancelarEdicao').show();
        });
        
        $("#divGridNotasFiscaisAdicionadas").find('.button_remove').die("click");
        $("#divGridNotasFiscaisAdicionadas").find('.button_remove').live("click", function (e) {
            e.preventDefault();
            var indice = $(this).parents('tr:first')[0].rowIndex;
            NotasFiscaisAdicionadas.splice(indice, 1);
            atualizarGrid();
        });
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
        
}
    
}