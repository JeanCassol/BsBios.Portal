GridCotacaoResumida = {
    Configurar: function(idProcessoCotacao) {
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
                        url: UrlPadrao.CotacaoResumida + '/?idProcessoCotacao=' + idProcessoCotacao,
                        type: 'GET',
                        cache: false
                    }
                }
            },
            groupable: false,
            selectable: 'row',
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
                    width: 65,
                    title: "Reenviar E-mail",
                    template: '<input type="button" class="button_sendmail" data-idfornecedorparticipante="${IdFornecedorParticipante}"></input>'
                },
                {
                    field: 'Selecionado',
                    title: 'Selecionado?',
                    width: 60
                },
                {
                    field: "QuantidadeDisponivel",
                    width: 50,
                    title: "Disponivel",
                    format: "{0:n2}"
                },
                {
                    field: "ValorComImpostos",
                    width: 80,
                    title: "Valor Com Impostos",
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
                data: { IdProcessoCotacao: idProcessoCotacao, IdFornecedorParticipante: idFornecedorParticipante },
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


