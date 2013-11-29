ProcessoDeCotacaoOperacoes = {
    Configurar: function(tipoDeCotacao) {
        var idProcessoCotacao = $('#Id').val();
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
                            IdProcessoCotacao: idProcessoCotacao,
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
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao consultar os Fornecedores Selecionados. Detalhe: ' + textStatus + errorThrown);
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
                data: { IdProcessoCotacao: $('#Id').val(), QuantidadeAdquiridaTotal: quantidadeAdquiridaTotal },
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
        
        function obterIdProcesso(mensagemDeErro) {
            var idDoProcessoDeCotacao = $('#Id').val();
            if (!idDoProcessoDeCotacao) {
                Mensagem.ExibirMensagemDeErro(mensagemDeErro);
            }

            return idDoProcessoDeCotacao;

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
                        var idCotacao = $(this).find('input[name=IdCotacao]').val();
                        if (idCotacao == 0) {
                            //cotação ainda não preenchida pelo fornecedor. O valor 0 indica que a cotação não foi criada
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
                            IdCotacao: idCotacao,
                            Selecionada: selecionada,
                            QuantidadeAdquirida: quantidadeAdquirida
                        };
                        
                        if (tipoDeCotacao == TipoDeCotacao.Material){
                            var codigoIva = $(this).find('select[name=CodigoIva]').val();
                            cotacao.CodigoIva = codigoIva;
                        } else {

                            var cadencia = Numero.GetFloat($(this).find('input[name=Cadencia]').val());

                            if (selecionada && cadencia < 0) {
                                Mensagem.ExibirMensagemDeErro("Deve ser preenchida a Cadência de todos os Fornecedores selecionados.");
                                dadosValidos = false;
                                return;
                            }

                            cotacao.Cadencia = cadencia;

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
                        data: JSON.stringify({ IdProcessoCotacao: $('#Id').val(), Cotacoes: cotacoes}),
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
            $('#divSelecionarFornecedores').customLoad(UrlPadrao.SelecionarFornecedores
                + "/?codigoProduto=" + $('#CodigoMaterial').val()
                + "&idProcessoCotacao=" + $('#Id').val()
                + "&TipoDeCotacao=" + tipoDeCotacao);

        });

        $('#btnSelecionarCotacoes').click(function () {
            var urlDaTela = (tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.AbrirTelaDeSelecaoDeCotacoesDeMaterial : UrlPadrao.AbrirTelaDeSelecaoDeCotacoesDeFrete)
                + "/?idProcessoCotacao=" + $('#Id').val();
            $('#divSelecionarCotacoes').customLoad(urlDaTela);
        });

        $('#btnAbrirProcesso').click(function () {
            var idDoProcessoDeCotacao = obterIdProcesso('Não é possível abrir o Processo de Cotação antes de salvá-lo.');
            if (!idDoProcessoDeCotacao) {
                return;
            }
            bloqueiaPagina();
            $.ajax({
                url: tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.AbrirProcessoDeCotacaoDeMaterial : UrlPadrao.AbrirProcessoDeCotacaoDeFrete,
                type: 'POST',
                cache: false,
                data: { idProcessoCotacao: idDoProcessoDeCotacao },
                dataType: 'json',
                success: function(data) {
                    if (data.Sucesso) {
                        $('#spanStatus').html('Aberto');
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

        $('#btnFecharProcesso').click(function () {
            var idDoProcessoDeCotacao = obterIdProcesso('Não é possível fechar o Processo de Cotação antes de salvá-lo.');
            if (!idDoProcessoDeCotacao) {
                return;
            }
            bloqueiaPagina();
            $.ajax({
                url: tipoDeCotacao == TipoDeCotacao.Material ? UrlPadrao.FecharProcessoDeCotacaoDeMaterial : UrlPadrao.FecharProcessoDeCotacaoDeFrete,
                type: 'POST',
                cache: false,
                data: { idProcessoCotacao: idDoProcessoDeCotacao },
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        $('#spanStatus').html('Fechado');
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

        });
        
        $('#btnCancelarProcesso').click(function () {
            var idDoProcessoDeCotacao = obterIdProcesso('Não é possível cancelar o Processo de Cotação antes de salvá-lo.');
            if (!idDoProcessoDeCotacao) {
                return;
            }
            
            bloqueiaPagina();
            $.ajax({
                url: UrlPadrao.CancelarProcessoDeCotacao,
                type: 'POST',
                cache: false,
                data: { idProcessoCotacao: idDoProcessoDeCotacao},
                dataType: 'json',
                success: function (data) {
                    if (data.Sucesso) {
                        $('#spanStatus').html('Cancelado');
                        Mensagem.ExibirMensagemDeSucesso(data.Mensagem);
                    } else {
                        Mensagem.ExibirMensagemDeErro(data.Mensagem);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao cancelar o Processo. Detalhe: ' + textStatus + errorThrown);
                },
                complete: function () {
                    desbloqueiaPagina();
                }
            });

        });

    }
    

}