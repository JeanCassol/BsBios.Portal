ProcessoDeCotacaoItensGrid = {
    
    configurar: function (configuracao) {
        /// <summary>Configura grid que exibe os itens de um processo de cotação de material</summary>
        /// <param name="configuracao.schemaData" type="string">Nome da propriedade json que contém o array de registros ou uma função que retorne um array de registros</param>
        /// <param name="configuracao.transportUrl" type="string">Url de leitura dos dados. Não deve ser informado quando utilizar um array local</param>
        /// <param name="configuracao.transportData" type="string">Função que gera os filtros para que serão passados para a action que filtra os dados</param>
        /// <param name="configuracao.renderTo">elemento html que o grid deve ser renderizado</param>
        /// <param name="configuracao.exibirBotaoAdicionar">indica se deve ser exibida uma coluna com botão "Adicionar" para cada registro do grid</param>
        /// <param name="configuracao.exibirBotaoEditar">indica se deve ser exibida uma coluna com botão "Editar" para cada registro do grid</param>
        /// <param name="configuracao.exibirBotaoRemover">indica se deve ser exibida uma coluna com botão "Remover" para cada registro do grid</param>
        /// <param name="configuracao.pageable">true = para paginar o gride; false = para não paginar o grid</param>
        /// <param name="configuracao.exibirDetalhesDaRequisicao">indica se devem ser exibidos campos especificos da requisição de compra</param>
        /// <param name="configuracao.exibirBotaoSelecionarCotacao">indica se deve ser exibida uma coluna com botão "Selecionar Cotações" para cada registro do grid</param>
        /// <param name="configuracao.exibirBotaoVisualizar">indica se deve ser exibida uma coluna com botão "Visualizar" para cada registro do grid</param>
        /// <returns type="">sem retorno</returns>
        var arrayDeColunas = [
            {
                width: 180,
                field: "Material",
                title: "Material"
            },
            {
                width: 80,
                field: "Quantidade",
            },
            {
                width: 100,
                field: "UnidadeMedida",
                title: "Unidade de Medida"
            }
        ];

        if (configuracao.exibirDetalhesDaRequisicao) {
            arrayDeColunas = arrayDeColunas.concat(
                {
                    width: 80,
                    field: "NumeroRequisicao",
                    title: "Requisição"
                },
                {
                    width: 60,
                    field: "NumeroItem",
                    title: "Item"
                },
                {
                    width: 100,
                    field: "DataDeSolicitacao",
                    title: "Data de Solicitação"
                },
                {
                    width: 100,
                    field: "CodigoGrupoDeCompra",
                    title: "Grupo de Compras"
                }
            );
        }
        if (configuracao.exibirBotaoAdicionar) {
            //unshift adiciona elemento no início do array
            arrayDeColunas.unshift({
                title: ' ',
                width: 30,
                template: '<input type="button" class="button16 button_add"></input>'
            });
        }
        
        if (configuracao.exibirBotaoEditar) {
            arrayDeColunas.unshift({
                title: ' ',
                width: 30,
                template: '<input type="button" class="button16 button_edit"></input>'
            });
        }
        
        if (configuracao.exibirBotaoVisualizar) {
            arrayDeColunas.unshift({
                title: ' ',
                width: 30,
                template: '<input type="button" class="button16 button_visualize"></input>'
            });
        }

        if (configuracao.exibirBotaoSelecionarCotacao) {
            arrayDeColunas.unshift({
                title: ' ',
                width: 30,
                template: '<input type="button" class="button16 button_select"></input>'
            });
        }

        if (configuracao.exibirBotaoRemover) {
            //unshift adiciona elemento no início do array
            arrayDeColunas.unshift({
                title: ' ',
                width: 30,
                template: '<input type="button" class="button16 button_remove"></input>'
            });
        }

        var configuracaoDoGrid = {
            dataSource: {
                schema: {
                    data: configuracao.schemaData,
                    model: {
                        fields: {
                            Id: { type: "number" },
                            IdProcessoCotacaoItem: { type: "number" },
                            Material: { type: "string" },
                            Quantidade: { type: "number" },
                            UnidadeMedida: { type: "string" },
                            NumeroRequisicao: { type: "string" },
                            NumeroItem: { type: "string" },
                            DataDeSolicitacao: { type: "string" },
                            CodigoGrupoDeCompra: { type: "string" }
                        }
                    },
                    total: 'QuantidadeDeRegistros'
                },
                transport: {
                    read: {
                        url: configuracao.transportUrl,
                        type: 'GET',
                        cache: false,
                        data: configuracao.transportData
                    }
                }
            },
            scrollable: true,
            columns: arrayDeColunas
        };

        if (!configuracao.pageable) {
            configuracaoDoGrid.pageable = false;
        }

        $(configuracao.renderTo).customKendoGrid(configuracaoDoGrid);

    }
}