GridCotacaoFrete = {
    UrlListar: "",
    Inicializar: function (urlListar) {
        this.UrlListar = urlListar;
    },
    CarregarGrid: function () {
        Ext.Loader.setConfig({ enabled: true });

        Ext.require([
            'Ext.grid.*',
            'Ext.data.*',
            'Ext.util.*',
            'Ext.toolbar.Paging',
            'Ext.ModelManager',
            'Ext.tip.QuickTipManager'
        ]);

        Ext.onReady(function () {
            Ext.tip.QuickTipManager.init();

            Ext.define('CotacaoFrete', {
                extend: 'Ext.data.Model',
                fields: [
                    'Id', 'CodigoMaterialSap', 'Material','Quantidade', 'Unidade', 'Status','DataInicio', 'DataTermino'
                ],
                idProperty: 'id'
            });

            var proxyProduto = new Ext.data.HttpProxy({
                url: GridCotacaoFrete.UrlListar,
                method: 'GET',
                reader: {
                    root: 'registros',
                    totalProperty: 'totalCount'
                }
            });
            // create the Data Store
            var store = Ext.create('Ext.data.Store', {
                pageSize: 20,
                model: 'CotacaoFrete',
                remoteSort: true,
                proxy: proxyProduto,
                sorters: [{
                    property: 'Inicio',
                    direction: 'DESC'
                }]
            });

            var grid = Ext.create('Ext.grid.Panel', {
                width: 810,
                height: 500,
                title: 'Cota&ccedil;&otilde;es de Frete',
                store: store,
                loadMask: true,
                // grid columns
                columns: [
                {
                    xtype: 'actioncolumn',
                    width: 60,
                    sortable: false,
                    items: [
                    {
                        icon: '/Images/icons/editar_22.png',
                        tooltip: 'Editar Produto',
                        handler: function (gridProduto, rowIndex) {
                            var rec = gridProduto.getStore().getAt(rowIndex);
                            $('#body').load('/Produto/EditarCadastro/?idProduto=' + rec.get('Id'));
                        }
                    }
                    ]
                },
                {
                    id: 'id',
                    dataIndex: 'Id',
                    flex: 1,
                    sortable: false,
                    hidden: true,
                }, {
                    text: "C&oacute;digo Sap",
                    dataIndex: 'CodigoMaterialSap',
                    width: 80,
                    sortable: true
                }, {
                    text: "Material",
                    dataIndex: 'Material',
                    width: 200,
                    sortable: true
                },
                {
                    text: "Quantidade",
                    dataIndex: 'Quantidade',
                    width: 70,
                    sortable: true
                },
                {
                    text: "Unidade",
                    dataIndex: 'Unidade',
                    width: 90,
                    sortable: true
                },
                {
                    text: "Status",
                    dataIndex: 'Status',
                    width: 100,
                    sortable: true
                },
                {
                    text: "Data de Inicio",
                    dataIndex: 'DataInicio',
                    width: 100,
                    sortable: true
                },
                {
                    text: "Data de T&eacute;rmino",
                    dataIndex: 'DataTermino',
                    width: 100,
                    sortable: true
                }
                ],
                // paging bar on the bottom
                bbar: Ext.create('Ext.PagingToolbar', {
                    store: store,
                    displayInfo: true,
                    beforePageText: 'P&aacutegina',
                    afterPageText: 'de {0}',
                    displayMsg: 'Mostrando {0} - {1} de {2}',
                    emptyMsg: "Nenhum registro encontrado",
                    items: ['-', {
                        xtype: 'button',
                        text: 'Adicionar',
                        listeners: {
                            click: function () {
                                location.href = '/CotacaoFrete/NovoCadastro';
                            }
                        }
                    }]
                }),
                renderTo: 'gridCotacaoFrete'
            });

            // trigger the data store load
            store.loadPage(1);
        });
    }
};

