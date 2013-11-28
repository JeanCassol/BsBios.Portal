function criarAutoCompleteDeMunicipio(idDoCampoDeCodigo, idDoCampoDeNome) {

    $(idDoCampoDeNome).autocomplete({
        source: UrlPadrao.BuscaDeMunicipios,
        minLength: 3,
        select: function (event, ui) {
            $(idDoCampoDeCodigo).val(ui.item.Codigo);
        },
        change: function(event, ui) {
            if (!ui.item) {
                $(idDoCampoDeNome + ',' + idDoCampoDeCodigo).val('');
            }
        }
    });
    
}