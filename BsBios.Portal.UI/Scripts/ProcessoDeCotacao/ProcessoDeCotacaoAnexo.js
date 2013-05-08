GridAnexo = {
    Atualizar: function () {
        $(GridAnexo.container).data("kendoGrid").dataSource.read();
    },
    Configurar: function (idProcessoCotacao, divGrid, permitirExclusao) {
        GridAnexo.container = divGrid;
        var colunas = [                 
        {
            width: 200,
            field: "FileName",
            title: "Arquivo"
        },
        {
            width: 70,
            title: "Download",
            template: '<a href="' + UrlPadrao.ProcessoCotacaoDownloadAnexo + '/?idProcessoCotacao=' + idProcessoCotacao + '&nomeDoArquivo=${FileName}' + '" class="button16 button_download"></a>'
        }];
        if (permitirExclusao) {
            colunas.push(
                {
                    width: 70,
                    title: "Excluir",
                    template: '<input type="button" class="button_remove" data-filename="${FileName}"></input>'
                });
        }
        $(divGrid).customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            FileName: { type: "string" },
                            Url: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                transport: {
                    read: {
                        url: UrlPadrao.ProcessoCotacaoListarAnexos + '/?idProcessoCotacao=' + idProcessoCotacao,
                        type: 'GET',
                        cache: false
                    }
                }
            },
            pageable: false,
            columns: colunas
        });
        
        //$(divGrid).find('.button_download').die('click');
        //$(divGrid).find('.button_download').live('click', function () {
            
        //});
        $(divGrid).find('.button_remove').die('click');
        $(divGrid).find('.button_remove').live('click', function () {
            var nomeDoArquivo = $(this).attr('data-filename');
            if (!Mensagem.Confirmacao('Confirma a exclusão do arquivo ' + nomeDoArquivo + '?')) {
                return;
            }
            $.ajax({
                url: UrlPadrao.ProcessoCotacaoExcluirAnexo,
                type: 'POST',
                data: JSON.stringify({IdProcessoCotacao:idProcessoCotacao,NomeDoArquivo:nomeDoArquivo }),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        Mensagem.ExibirMensagemDeSucesso(data.Mensagem);
                        GridAnexo.Atualizar();
                    } else {
                        Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao excluir o anexo. Detalhe: ' + data.Mensagem);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao excluir o anexo. Detalhe: ' + textStatus + errorThrown);
                }
            });
        });
        

    }
}