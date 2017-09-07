function SelecionarItinerario() {

    this.itinerarioSelecionado = null;
    var me = this;
    
    function configurarGridDeSelecao() {

        $("#gridItinerarios").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Descricao: { type: "string" },
                            Codigo: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: UrlPadrao.ListarItinerario,
                        type: 'GET',
                        cache: false,
                        data: function () {
                            return {
                                Codigo: $('#CodigoFiltro').val(),
                                Local1: $('#Local1').val(),
                                Local2: $('#Local2').val()
                            };
                        }
                    }
                },
                pageSize: 10
            },
            dataBound: function (e) {
                if (me.itinerarioSelecionado != null) {
                    $('input[name=radioItinerario][data-codigoitinerario="' + me.itinerarioSelecionado.Codigo + '"]').attr('checked', true);
                }
            },

            columns:
            [
                {
                    field: 'Codigo',
                    title: ' ',
                    width: 30,
                    sortable: false,
                    template: '<input type="radio" name="radioItinerario" data-codigoitinerario="${Codigo}"></input>'
                },
                {
                    width: 170,
                    field: "Codigo",
                    title: "Código"
                },
                {
                    field: "Descricao",
                    width: 300,
                    title: "Descrição"
                }
            ]
        });
        
        $('#gridItinerarios').find('input[name=radioItinerario]').die("change");
        $('#gridItinerarios').find('input[name=radioItinerario]').live("change", function () {
            if (!$(this).is(':checked')) {
                return;
            }
            var grid = $('#gridItinerarios').data("kendoGrid");
            var dataItem = grid.dataItem(grid.select());
            me.itinerarioSelecionado = { Codigo: dataItem.Codigo, Descricao: dataItem.Descricao };

        });

    }
    
    function configurarJanelaModal(idDoCampoDoCodigoDoItinerario, idDoCampoDaDescricaoDoItinerario, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoItinerario) {

        $('body').append('<div id="' + idDaDivDaJanelaDeDialogo + '" class="janelaModal"></div>');
        $('#' + idDaDivDaJanelaDeDialogo).customDialog({
            title: 'Selecionar Itinerário',
            buttons: {
                "Confirmar": function () {
                    if (me.itinerarioSelecionado == null) {
                        Mensagem.ExibirMensagemDeErro("É necessário selecionar um Itinerário.");
                        return;
                    }
                    $(idDoCampoDoCodigoDoItinerario).val(me.itinerarioSelecionado.Codigo);
                    $(idDoCampoDaDescricaoDoItinerario).val(unescape(me.itinerarioSelecionado.Descricao));
                    $(this).dialog("close");

                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
        
        $(idDoBotaoDeSelecaoDoItinerario).click(function () {
            
            var codigoDoItinerario = $(idDoCampoDoCodigoDoItinerario).val();
            var descricaoDoItinerario = escape($(idDoCampoDaDescricaoDoItinerario).val());

            if (codigoDoItinerario && descricaoDoItinerario) {
                me.itinerarioSelecionado = {
                    Codigo: codigoDoItinerario,
                    Descricao: descricaoDoItinerario
                };
            }

            $('#' + idDaDivDaJanelaDeDialogo).customLoad({ url: UrlPadrao.SelecionarItinerario
                + '/?Codigo=' + codigoDoItinerario + '&Descricao=' + escape(descricaoDoItinerario)
            }, configurarGridDeSelecao);
        });

    }

    this.configurar = function(idDoCampoDoCodigoDoItinerario, idDoCampoDaDescricaoDoItinerario, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoItinerario) {
        configurarJanelaModal(idDoCampoDoCodigoDoItinerario, idDoCampoDaDescricaoDoItinerario, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoItinerario);
    };

}