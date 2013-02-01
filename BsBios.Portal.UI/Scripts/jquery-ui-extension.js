$(function () {
    /*ajusta as propriedades default para o componente datepicker do jquery UI*/
    $.datepicker.setDefaults($.datepicker.regional["pt-BR"]);
    $.datepicker.setDefaults({
        changeYear: true,
        gotoCurrent: true,
        dateFormat: "dd/mm/yy",
        defaultDate: null
    });
    /*ajusta a validação para campos do tipo date. Sem este ajuste não estava funcionando no Chrome e no Safari*/
    $.validator.methods["date"] = function (value, element) {
        var shortDateFormat = "dd/mm/yy";
        var res = true;
        try {
            $.datepicker.parseDate(shortDateFormat, value);
        } catch (error) {
            res = false;
        }
        return res;
    };
});
