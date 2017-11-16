﻿GridCotacaoResumida = {
    Configurar: function (configuracao) {

        $("#gridCotacaoFornecedor").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            IdFornecedorParticipante: { type: "number" },
                            Codigo: { type: "string" },
                            Nome: { type: "string" },
                            Selecionado: { type: "string" },
                            ValorLiquido: { type: "number" },
                            ValorComImpostos: { type: "number" },
                            QuantidadeDisponivel: { type: "number" },
                            VisualizadoPeloFornecedor: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: configuracao.Url,
                        type: 'GET',
                        cache: false,
                        data: function() {
                            return { IdProcessoCotacao: $('#Id').val() };
                        }
                    }
                }
            },
            groupable: false,
            scrollable: true,
            selectable: 'row',
            pageable: false,
            autoBind: configuracao.autoBind,

            columns:
            [
                {
                    width: 60,
                    field: "Codigo",
                    title: "Código"
                },
                {
                    field: "Nome",
                    width: 195,
                    title: "Nome"
                },
                {
                    field: "VisualizadoPeloFornecedor",
                    width: 60,
                    title: "Visualizado?"
                },
                {
                    field: "Resposta",
                    width: 60,
                    title: "Resposta"
                },

                {
                    width: 50,
                    title: "Reenviar",
                    template: '<input type="button" class="button_sendmail" data-idfornecedorparticipante="${IdFornecedorParticipante}"></input>',
                    attributes: {
                        "class": "action-column"
                    }
                },
                {
                    field: 'Selecionado',
                    title: 'Selecionado',
                    width: 65
                },
                {
                    field: "QuantidadeDisponivel",
                    width: 50,
                    title: "Disponivel",
                    format: "{0:n2}"
                },
                {
                    field: "ValorComImpostos",
                    width: 60,
                    title: "Valor",
                    format: "{0:n2}"
                }
            ]
        });

        $("#gridCotacaoFornecedor").find('.button_sendmail').die('click');
        $("#gridCotacaoFornecedor").find('.button_sendmail').live('click', function () {
            var idFornecedorParticipante = $(this).attr('data-idfornecedorparticipante');
            $("#todaPagina").block('Processando...');
            $.ajax({
                url: UrlPadrao.EnviarEmail,
                type: 'GET',
                data: { IdProcessoCotacao: configuracao.IdProcessoCotacao, IdFornecedorParticipante: idFornecedorParticipante },
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        Mensagem.ExibirMensagemDeSucesso(data.Mensagem);
                    } else {
                        Mensagem.ExibirMensagemDeErro(data.Mensagem);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao enviar e-mail para o Fornecedor. Detalhe: ' + textStatus + errorThrown);
                },
                complete: function () {
                    $("#todaPagina").unblock();
                }

            });

        });
    }
}


