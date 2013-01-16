Ext.Loader.setConfig({enabled: true});

//Ext.Loader.setPath('Ext.ux', '../ux/');
Ext.require([
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.util.*',
    'Ext.toolbar.Paging',
    //'Ext.ux.PreviewPlugin',
    'Ext.ModelManager',
    'Ext.tip.QuickTipManager'
]);



Ext.onReady(function(){
    Ext.tip.QuickTipManager.init();

    Ext.define('Produto', {
        extend: 'Ext.data.Model',
        fields: [
            'Id', 'CodigoSap', 'Descricao'
        ],
        idProperty: 'id'
    });
    
    var proxyProduto = new Ext.data.HttpProxy({
        url: urlGridProdutos, method: 'GET', reader: {
            root: 'registros',
            totalProperty: 'totalCount'
        }
    });
    // create the Data Store
    var store = Ext.create('Ext.data.Store', {
        pageSize: 20,
        model: 'Produto',
        remoteSort: true,
        proxy: proxyProduto,
        sorters: [{
            property: 'codigoSap',
            direction: 'ASC'
        }]
    });

    var grid = Ext.create('Ext.grid.Panel', {
        width: 700,
        height:300,
        title: 'Listagem de Produtos',
        store: store,
        disableSelection: true,
        loadMask: true,
        viewConfig: {
            id: 'gv',
            trackOver: false,
            stripeRows: false//,
        },
        // grid columns
        columns:[{
            id: 'id',
            text: "Topic",
            dataIndex: 'Id',
            flex: 1,
            sortable: false,
            hidden:true,
        },{
            text: "C&oacute;digo Sap",
            dataIndex: 'CodigoSap',
            width: 150,
            sortable: true
        },{
            text: "Descri&ccedil;&atilde;o",
            dataIndex: 'Descricao',
            width: 300,
            sortable: true
        }],
        // paging bar on the bottom
        bbar: Ext.create('Ext.PagingToolbar', {
            store: store,
            displayInfo: true,
            displayMsg: 'Mostrando {0} - {1} de {2}',
            emptyMsg: "Nenhum registro encontrado",
            items: ['-', {
                xtype: 'button',
                text: 'Adicionar',
                listeners: {
                    click:function() {
                        alert('Testando clique');
                    }
                }
            }]
        }),
        renderTo: 'topic-grid'
    });

    // trigger the data store load
    store.loadPage(1);
});
