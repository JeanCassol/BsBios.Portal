function SelecionarFornecedor() {
    this.fornecedorSelecionado = null;

    var me = this;
    
    function configurarGridDeSelecao() {

        $("#gridFornecedores").customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        fields: {
                            Codigo: { type: "string" },
                            Nome: { type: "string" },
                            Cnpj: { type: "string" },
                            Municipio: { type: "string" },
                            Uf: { type: "string" },
                            Email: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: UrlPadrao.ListarFornecedor,
                        type: 'GET',
                        cache: false,
                        data: function () {
                            return {
                                Codigo: $('#CodigoFiltro').val(),
                                Nome: $('#NomeFiltro').val()
                            };
                        }
                    }
                },
                pageSize: 10
            },
            dataBound: function (e) {
                if (me.fornecedorSelecionado != null) {
                    $('input[name=radioFornecedor][data-codigofornecedor="' + me.fornecedorSelecionado.Codigo + '"]').attr('checked', true);
                }
            },
            columns:
            [
                {
                    field: 'Codigo',
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 30,
                    sortable: false,
                    template: '<input type="radio" name="radioFornecedor" data-codigofornecedor="${Codigo}"></input>'
                },
                {
                    field: "Codigo",
                    width: 100,
                    title: "Codigo"
                },
                {
                    field: "Nome",
                    width: 320,
                    title: "Nome"
                },
                {
                    field: "Cnpj",
                    width: 140,
                    title: "CNPJ"

                },
                {
                    field: "Municipio",
                    width: 150,
                    title: "Município"
                },
                {
                    field: "Uf",
                    width: 30,
                    title: "UF"
                }
            ]
        });

        $('#gridFornecedores').find('input[name=radioFornecedor]').die("change");
        $('#gridFornecedores').find('input[name=radioFornecedor]').live("change", function () {
            if (!$(this).is(':checked')) {
                return;
            }
            var grid = $('#gridFornecedores').data("kendoGrid");
            var dataItem = grid.dataItem(grid.select());
            me.fornecedorSelecionado = { Codigo: dataItem.Codigo, Nome: dataItem.Nome };

        });

        
    };

    function configurarJanelaModal(idDoCampoDoCodigoDoFornecedor, idDoCampoDoNomeDoFornecedor, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoFornecedor) {
        $('body').append('<div id="' + idDaDivDaJanelaDeDialogo + '" class="janelaModal"></div>');
        $('#' + idDaDivDaJanelaDeDialogo).customDialog({
            title: 'Selecionar Fornecedor',
            buttons: {
                "Confirmar": function() {
                    if (me.fornecedorSelecionado == null) {
                        Mensagem.ExibirMensagemDeErro("É necessário selecionar um Fornecedor.");
                        return;
                    }
                    $(idDoCampoDoCodigoDoFornecedor).val(me.fornecedorSelecionado.Codigo);
                    if (idDoCampoDoNomeDoFornecedor) {
                        $(idDoCampoDoNomeDoFornecedor).val(unescape(me.fornecedorSelecionado.Nome));
                    }
                    
                    me.fornecedorSelecionado = null;
                    $(this).dialog("close");
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            }
        });
        $(idDoBotaoDeSelecaoDoFornecedor).click(function () {
            
            var codigoDoFornecedor = $(idDoCampoDoCodigoDoFornecedor).val();
            var nomeDoFornecedor = '';

            if (idDoCampoDoNomeDoFornecedor) {
                nomeDoFornecedor = escape($(idDoCampoDoNomeDoFornecedor).val());
            }

            if (codigoDoFornecedor) {
                me.fornecedorSelecionado = {
                    Codigo: codigoDoFornecedor,
                    Nome: nomeDoFornecedor
                };
            }

            $('#' + idDaDivDaJanelaDeDialogo).customLoad(UrlPadrao.SelecionarFornecedor
                + '/?Codigo=' + codigoDoFornecedor + '&Nome=' + escape(nomeDoFornecedor), configurarGridDeSelecao);
        });

    };

    this.configurar = function(idDoCampoDoCodigoDoFornecedor, idDoCampoDoNomeDoFornecedor, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoFornecedor) {
        configurarJanelaModal(idDoCampoDoCodigoDoFornecedor, idDoCampoDoNomeDoFornecedor, idDaDivDaJanelaDeDialogo, idDoBotaoDeSelecaoDoFornecedor);
    };
}