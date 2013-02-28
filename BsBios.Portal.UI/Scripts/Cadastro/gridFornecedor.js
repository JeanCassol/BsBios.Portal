GridFornecedor = {
    CarregarGrid: function(codigoProduto, divParaCarregar, url) {
        $(divParaCarregar).kendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        id: 'Codigo',
                        fields: {
                            Codigo: { type: "string" },
                            Nome: { type: "string" },
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
                        data: function () {
                            return {
                                codigoProduto: codigoProduto
                            };
                        }
                    }
                },
                pageSize: 10
            },
            groupable: false,
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
            columns:
            [
                {
                    field: 'Codigo',
                    title: ' ', /*coloco um espaço para deixar o header sem título*/
                    width: 60,
                    sortable: false,
                    template: '<input type="button" class="button_edit" data-codigofornecedor="${Codigo}"></input>'
                },
                {
                    field: "Codigo",
                    width: 90,
                    title: "Codigo"
                }, {
                    field: "Nome",
                    width: 300,
                    title: "Nome"
                }, {
                    width: 150,
                    field: "Email",
                    title: "E-mail"
                }
            ]
        });

    }
}