GridFornecedor = {
    CarregarGrid: function (codigoProduto, divParaCarregar, url, incluirBotaoAdicionar, funcaoFiltros) {
        var arrayDeColunas = new Array();
        if (incluirBotaoAdicionar) {
            arrayDeColunas.push({
                field: 'Codigo',
                title: ' ', /*coloco um espaço para deixar o header sem título*/
                width: 60,
                sortable: false,
                template: '<input type="button" class="button_add" data-codigofornecedor="${Codigo}"></input>'
            });
        }
        arrayDeColunas = arrayDeColunas.concat(
            {
                field: "Codigo",
                width: 120,
                title: "Codigo"
            },
            {
                field: "Nome",
                width: 350,
                title: "Nome"
            },
            {
                field: "Cnpj",
                width: 150,
                title: "CNPJ"
            
            },
            {
                width: 250,
                field: "Email",
                title: "E-mail"
            });

        $(divParaCarregar).kendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        id: 'Codigo',
                        fields: {
                            Codigo: { type: "string" },
                            Nome: { type: "string" },
                            Cnpj: { type: "string" },
                            Email: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros',
                    type: 'json'
                },
                serverFiltering: true,
                serverPaging: true,
                transport: {
                    read: {
                        url: url,
                        type: 'GET',
                        cache: false,
                        data: funcaoFiltros
                    }
                },
                pageSize: 10
            },
            groupable: false,
            scrollable: true,
            sortable: true,
            pageable:
            {
                refresh: true,
                pageSizes: true,
                messages: {
                    display: '{0} - {1} de {2} registros',
                    empty: 'Nenhum registro encontrado',
                    itemsPerPage: 'registros por página',
                    first: 'Ir para a primeira página',
                    previous: 'Ir para a página anterior',
                    next: 'Ir para a próxima página',
                    last: 'Ir para a última página',
                    refresh: 'Atualizar'
                }
            },
            selectable: 'row',
            columns: arrayDeColunas
        });

    }
}