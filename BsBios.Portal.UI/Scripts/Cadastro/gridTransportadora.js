Ext.Loader.setConfig({enabled: true});

Ext.Loader.setPath('Ext.ux', '/Scripts/ux');

Ext.require([
    'Ext.grid.*',
    'Ext.data.*',
    'Ext.util.*',
    'Ext.toolbar.Paging',
    'Ext.ModelManager',
    'Ext.tip.QuickTipManager',
    'Ext.ux.CheckColumn'
]);

Ext.onReady(function(){
    Ext.tip.QuickTipManager.init();

    Ext.define('Fornecedor', {
        extend: 'Ext.data.Model',
        fields: [{ name: 'Selecionado', type: 'bool' }, 'Nome'],
        idProperty: 'id'
    });
    
    var proxyFornecedor = new Ext.data.HttpProxy({
        url: '/Fornecedor/Listar', method: 'GET', reader: {
            root: 'registros',
            totalProperty: 'totalCount'
        }
    });
    // create the Data Store
    var store = Ext.create('Ext.data.Store', {
        pageSize: 20,
        model: 'Fornecedor',
        remoteSort: true,
        proxy: proxyFornecedor,
        sorters: [{
            property: 'Nome',
            direction: 'ASC'
        }]
    });


    var grid = Ext.create('Ext.grid.Panel', {
        width: 500,
        height:200,
        title: 'Fornecedores',
        store: store,
        loadMask: true,
        // grid columns
        columns:[{
            xtype: 'checkcolumn',
            header: 'Selecionar?',
            dataIndex: 'Selecionado',
            width: 75,
        }, {
            header: 'Fornecedor',
            dataIndex: 'Nome',
            width:300
        }
        ],
      
        renderTo: 'divSelecionarFornecedor'
    });

    // trigger the data store load
    store.loadPage(1);
});
