                $.ajax({
                    url: '@Url.Action("AtualizarPerfis","GerenciadorUsuario")',
                    type: 'POST',
                    data: JSON.stringify({ Login: '@Model.Login', Perfis: codigoPerfisDoUsuario }),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: 'json',
                    success: function (data) {
                        if (data.Sucesso) {
                            Mensagem.ExibirMensagemDeSucesso('Perfis atualizados com sucesso');
                        } else {
                            Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atualizar os perfis do Usuário. Detalhe: ' + data.Mensagem);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        Mensagem.ExibirMensagemDeErro('Ocorreu um erro ao atualizar os perfis do Usuário. Detalhe: ' + textStatus + errorThrown);
                    }
                });
