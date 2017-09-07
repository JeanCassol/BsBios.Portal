SelecionarComprador = {
    CompradorSelecionado: null,
    ConfigurarJanelaModal: function () {
        var divSelecionarComprador = $('#divSelecionarComprador');
        if (divSelecionarComprador.length == 0) {
            $('body').append('<div id="divSelecionarComprador" class="janelaModal"></div>');
            $('#divSelecionarComprador').customDialog({
                title: 'Selecionar Comprador',
                buttons: {
                    "Confirmar": function () {
                        if (SelecionarComprador.CompradorSelecionado == null) {
                            Mensagem.ExibirMensagemDeErro("É necessário selecionar um Comprador.");
                            return;
                        }
                        $('#LoginComprador').val(SelecionarComprador.CompradorSelecionado.Login);
                        $('#Comprador').val(unescape(SelecionarComprador.CompradorSelecionado.Nome));
                        $(this).dialog("close");
                    },
                    "Cancelar": function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
        $('#btnSelecionarComprador').click(function() {
            $('#divSelecionarComprador').customLoad({url:UrlPadrao.SelecionarComprador
                + '/?Login=' + $('#LoginComprador').val() + '&Nome=' + escape($('#Comprador').val())
            },
                function () {
                    var login = $('#Login').val();
                    var nome = $('#Nome').val();
                    if (login != '' && nome != '') {
                        SelecionarComprador.CompradorSelecionado = { Login: login, Nome: nome };
                    }
                    SelecionarComprador.ConfigurarGrid();
                }
            );
        });

    },
    ConfigurarGrid:function() {
        $("#gridCompradores").customKendoGrid({
            dataSource: {
                schema: {
                    model: {
                        fields: {
                            Login: { type: "string" },
                            Nome: { type: "string" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: UrlPadrao.ListarCompradoresDeSuprimentos,
                        type: 'GET',
                        cache: false,
                        data: function () {
                            return {
                                Login: $('#LoginFiltro').val(),
                                Nome: $('#NomeFiltro').val()
                            };
                        }
                    }
                }
            },
            dataBound: function (e) {
                if (SelecionarComprador.CompradorSelecionado != null) {
                    $('input[name=radioComprador][data-logincomprador=' + SelecionarComprador.CompradorSelecionado.Login + ']').attr('checked', true);
                }
            },
            columns:
            [
                {
                    field: 'Codigo',
                    title: ' ', 
                    width: 30,
                    sortable: false,
                    template: '<input type="radio" name="radioComprador" data-logincomprador="${Login}"></input>'
                },

                {
                    field: "Login",
                    width: 100,
                    title: "Login"
                },
                {
                    field: "Nome",
                    width: 320,
                    title: "Nome"
                }
            ]
        });

        $('#gridCompradores').off('change','input[name=radioComprador]',false);
        $('#gridCompradores').on('change', 'input[name=radioComprador]', function () {
            if (!$(this).is(':checked')) {
                return;
            }
            var grid = $('#gridCompradores').data("kendoGrid");
            var dataItem = grid.dataItem(grid.select());
            SelecionarComprador.CompradorSelecionado = { Login: dataItem.Login, Nome: dataItem.Nome };
        });
    }
}