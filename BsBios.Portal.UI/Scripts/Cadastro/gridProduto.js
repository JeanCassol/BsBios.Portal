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
        loadMask: true,
        //viewConfig: {
        //    id: 'gv',
        //    trackOver: false,
        //    stripeRows: true
        //},
        // grid columns
        columns: [
        {
            xtype: 'actioncolumn',
            width: 60,
            sortable: false,
            items: [
            {
                icon: '/Images/icons/editar_22.png',
                tooltip:'Editar Produto',
                handler: function (gridProduto, rowIndex) {
                    var rec = gridProduto.getStore().getAt(rowIndex);
                    $('#body').load('/Produto/EditarCadastro/?idProduto=' + rec.get('Id'));
                }
            }
            ]
        },
        {
            xtype: 'actioncolumn',
            width: 60,
            sortable: false,
            items: [
            {
                icon: '/Images/icons/delete_24.png',
                tooltip: 'Excluir Produto',
                handler:function() {
                    alert('Confirma a exclusão do Item?');
                }
            }
            ]            
        },
        {
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
            width: 400,
            sortable: true
        }],
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
                    click:function() {
                        alert('Testando clique');
                    }
                }
            }]
        }),
        renderTo: 'gridProdutos'
    });

    // trigger the data store load
    store.loadPage(1);
});
