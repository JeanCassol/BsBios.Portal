ProcessoDeCotacaoOperacoes = {
    Configurar: function(tipoDeCotacao) {
        $('#divSelecionarFornecedores').customDialog({
            title: 'Selecionar Fornecedores',
            buttons: {
                "Confirmar": function() {
                    var codigosDosFornecedoresSelecionados = new Array();
                    $.each(fornecedoresSelecionados, function(indice, fornecedorSelecionado) {
                        codigosDosFornecedoresSelecionados.push(fornecedorSelecionado.Codigo);
                    });

                    $.ajax({
                        url: UrlPadrao.AtualizarFornecedoresDoProcessoDeCotacao,
                        type: 'POST',
                        cache: false,
                        data: JSON.stringify({
                            IdProcessoCotacao: $('#Id').val(),
                            CodigoFornecedoresSelecionados: codigosDosFornecedoresSelecionados
                        }),
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function(data) {
                            if (data.Sucesso) {
                                $('#divSelecionarFornecedores').dialog('close');
                                $("#gridCotacaoFornecedor").data("kendoGrid").dataSource.read();
                            } else {
                                Mensagem.ExibirMensagemDeErro(data.Mensagem);
                            }
                        },
                        error: function(jqXHR, textStatus, errorThrown) {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atualizar os Fornecedores do Processo de Cotação. Detalhe: ' + textStatus + errorThrown);
                        }
                    });
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
        
        function verificarQuantidadeAdquirida(quantidadeAdquiridaTotal) {
            var retorno = true;
            $.ajax({
                url: UrlPadrao.VerificarQuantidadeAdquiridaNoProcessoDeCotacao,
                type: 'GET',
                async: false,
                cache: false,
                data: { IdProcessoCotacao: $('#Id').val(), IdItem: $('#IdProcessoCotacaoItem').val(), QuantidadeAdquiridaTotal: quantidadeAdquiridaTotal },
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        if (data.Verificacao.SuperouQuantidadeSolicitada) {
                            retorno = confirm("A quantidade total adquirida (" + Globalize.format(quantidadeAdquiridaTotal,"n3") +
                                ") superou a quantidade solicitada no Processo de Cotação (" +
                                Globalize.format(data.Verificacao.QuantidadeSolicitadaNoProcessoDeCotacao,"n3") +
                                "). Confirma as quantidades informadas para cada Fornecedor?");
                        }
                    } else {
                        Mensagem.ExibirMensagemDeErro(data.Mensagem);
                        retorno = false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao verificar a quantidade adquirida total. Detalhe: ' + textStatus + errorThrown);
                    retorno = false;
                }
            });

            return retorno;

        }

        $('#divSelecionarCotacoes').customDialog({
            title: 'Selecionar Cotações',
            buttons: {
                "Confirmar": function () {
                    var cotacoes = new Array();
                    var dadosValidos = true;
                    var quantidadeAdquiridaTotal = 0;
                    $('#gridCotacoes').find('table > tbody > tr').each(function () {
                        if (!dadosValidos) {
                            return;
                        }
                        var dataItem = $('#gridCotacoes').data("kendoGrid").dataItem(this);
                        
                        if (dataItem.ValorComImpostos == null) {
                            //cotação ainda não preenchida pelo fornecedor. O valor null indica que a cotação não foi criada
                            return;
                        }

                        var selecionada = $(this).find('input[type=checkbox][name=Selecionada]').is(':checked');
                        var quantidadeAdquirida = Numero.GetFloat($(this).find('input[name=QuantidadeAdquirida]').val());
                        if (selecionada && quantidadeAdquirida <= 0) {
                            Mensagem.ExibirMensagemDeErro("Deve ser preenchida a Quantidade de todos os Fornecedores selecionados.");
                            dadosValidos = false;
                            return;
                        }

                        var cotacao = {
                            IdCotacao: dataItem.IdCotacao,
                            Selecionada: selecionada,
                            QuantidadeAdquirida: quantidadeAdquirida
                        };
                        
                        if (tipoDeCotacao == TipoDeCotacao.Material){
                            var codigoIva = $(this).find('select[name=CodigoIva]').val();
                            if (codigoIva == "") {
                                Mensagem.ExibirMensagemDeErro("Deve ser preenchido o Iva de todos os Fornecedores selecionados.");
                                dadosValidos = false;
                                return;
                            }
                            cotacao.CodigoIva = codigoIva;
                        }

                        quantidadeAdquiridaTotal += quantidadeAdquirida;

                        cotacoes.push(cotacao);
                    });
                    if (!dadosValidos) {
                        return;
                    }

                    if (cotacoes.length == 0) {
                        $('#divSelecionarCotacoes').dialog("close");
                        return;
                    }
                    
                    if (tipoDeCotacao == TipoDeCotacao.Material)
                    {
                        if (!verificarQuantidadeAdquirida(quantidadeAdquiridaTotal)) return;
                    }
                   
                    $.ajax({
                        url: tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.SelecionarCotacoesDeMaterial: UrlPadrao.SelecionarCotacoesDeFrete,
                        type: 'POST',
                        cache: false,
                        data: JSON.stringify({ IdProcessoCotacao: $('#Id').val(), IdProcessoCotacaoItem: $('#IdProcessoCotacaoItem').val(), Cotacoes: cotacoes}),
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.Sucesso) {
                                $('#divSelecionarCotacoes').dialog("close");
                                $("#gridCotacaoFornecedor").data("kendoGrid").dataSource.read();
                            } else {
                                Mensagem.ExibirMensagemDeErro(data.Mensagem);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao selecionar as Cotações. Detalhe: ' + textStatus + errorThrown);
                        }
                    });
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#btnSelecionarFornecedores').click(function () {
            if (!$('#Id').val()) {
                Mensagem.ExibirMensagemDeErro("Não é possível selecionar Fornecedores antes de salvar o Processo de Cotação.");
                return;
            }
            $('#divSelecionarFornecedores').customLoad({
                    url: UrlPadrao.SelecionarFornecedores
                        + "/?idProcessoCotacao=" + $('#Id').val()
                        + "&TipoDeCotacao=" + tipoDeCotacao
                });

        });

        $('#btnSelecionarCotacoes').click(function () {
            $('#divSelecionarCotacoes').customLoad({
                    url: (tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.AbrirTelaDeSelecaoDeCotacoesDeMaterial : UrlPadrao.AbrirTelaDeSelecaoDeCotacoesDeFrete)
                        + "/?idProcessoCotacao=" + $('#Id').val()});
        });

        $('#btnAbrirProcesso').click(function () {
            bloqueiaPagina();
            $.ajax({
                url: tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.AbrirProcessoDeCotacaoDeMaterial : UrlPadrao.AbrirProcessoDeCotacaoDeFrete,
                type: 'POST',
                cache: false,
                data: {idProcessoCotacao: $('#Id').val()},
                dataType: 'json',
                success: function(data) {
                    if (data.Sucesso) {
                        $('#spanStatus').html('Aberto');
                        var seletorDesabilitar = '#btnAbrirProcesso,#btnSelecionarFornecedores,#btnSelecionarItens';
                        if (tipoDeCotacao == TipoDeCotacao.Frete) {
                            seletorDesabilitar += ',#btnSalvar';
                        }
                        desabilitarBotao(seletorDesabilitar);
                        habilitarBotao('#btnFecharProcesso,#btnSelecionarCotacoes');
                        Mensagem.ExibirMensagemDeSucesso(data.Mensagem);
                    } else {
                        Mensagem.ExibirMensagemDeErro(data.Mensagem);
                    }
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao abrir o Processo. Detalhe: ' + textStatus + errorThrown);
                },
                complete: function () {
                    desbloqueiaPagina();
                }
            });

        });
        
        function fecharProcessoDeCotacao(urlDeFechamento) {
            var dados = {
                IdProcessoCotacao: $('#Id').val(),
                TextoDeCabecalho: $('#TextoDeCabecalho').val(),
                NotaDeCabecalho: $('#NotaDeCabecalho').val(),
                DocumentoParaGerarNoSap: $('#DocumentoParaGerarNoSap:checked').val()
            };
            dados.IdProcessoCotacao = $('#Id').val();
            bloqueiaPagina();
            $.ajax({
                url: urlDeFechamento,
                type: 'POST',
                cache: false,
                data: dados,
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        $('#spanStatus').html('Fechado');
                        desabilitarBotao('#btnFecharProcesso,#btnSalvarProcesso');
                        Mensagem.ExibirMensagemDeSucesso(data.Mensagem);
                    } else {
                        if (data.MediaType == "text/html") {
                            Mensagem.ExibirJanelaComHtml(data.Mensagem);
                        } else {
                            Mensagem.ExibirMensagemDeErro(data.Mensagem);
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao fechar o Processo. Detalhe: ' + textStatus + errorThrown);
                },
                complete: function () {
                    desbloqueiaPagina();
                }
            });
        }

        $('#divFecharProcessoDeCotacaoDeMaterial').customDialog({
            title: 'Fechar Processo de Cotação',
            buttons: {
                "Confirmar": function () {
                    var form = $('#formFecharProcesso');
                    if (!$(form).validate().form()) {
                        return;
                    }
                    fecharProcessoDeCotacao(UrlPadrao.FecharProcessoDeCotacaoDeMaterial);
                    $(this).dialog("close");
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });

        $('#btnFecharProcesso').click(function () {
            if (tipoDeCotacao == TipoDeCotacao.Material) {
                $('#divFecharProcessoDeCotacaoDeMaterial').customLoad({url:UrlPadrao.AbrirTelaDeFechamentoDeProcessoDeCotacaoDeMaterial, validar:true});
            }
            if (tipoDeCotacao == TipoDeCotacao.Frete) {
                fecharProcessoDeCotacao(UrlPadrao.FecharProcessoDeCotacaoDeFrete);
            }
        });
    }
}