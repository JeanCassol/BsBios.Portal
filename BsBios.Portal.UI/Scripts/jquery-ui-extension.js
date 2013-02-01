$(function () {
    /*ajusta as propriedades default para o componente datepicker do jquery UI*/
    $.datepicker.setDefaults($.datepicker.regional["pt-BR"]);
    $.datepicker.setDefaults({
        changeYear: true,
        gotoCurrent: true,
        dateFormat: "dd/mm/yy",
        defaultDate: null
    });
});
