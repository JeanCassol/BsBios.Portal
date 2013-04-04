GridFornecedor = {
    CarregarGrid: function (codigoProduto, divParaCarregar, url, incluirBotaoAdicionar, funcaoFiltros) {
        var arrayDeColunas = new Array();
        if (incluirBotaoAdicionar) {
            arrayDeColunas.push({
                field: 'Codigo',
                title: ' ', /*coloco um espaço para deixar o header sem título*/
                width: 30,
                sortable: false,
                template: '<input type="button" class="button_add" data-codigofornecedor="${Codigo}"></input>'
            });
        }
        arrayDeColunas = arrayDeColunas.concat(
            {
                field: "Codigo",
                width: 80,
                title: "Codigo"
            },
            {
                field: "Nome",
                width: 250,
                title: "Nome"
            },
            {
                field: "Cnpj",
                width: 120,
                title: "CNPJ"
            
            },
            {
                width: 250,
                field: "Email",
                title: "E-mail"
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
        );

        $(divParaCarregar).customKendoGrid({
            dataSource: {
                schema: {
                    data: 'Registros',
                    model: {
                        id: 'Codigo',
                        fields: {
                            Codigo: { type: "string" },
                            Nome: { type: "string" },
                            Cnpj: { type: "string" },
                            Municipio: { type: "string" },
                            Uf: { type: "string" },
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
                }
            },
            groupable: false,
            scrollable: true,
            selectable: 'row',
            columns: arrayDeColunas
        });

    }
}