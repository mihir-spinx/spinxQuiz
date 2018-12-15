
function funInputPlaceholder() {
    $('.form-label').find('input,textarea').each(function () {
        if ($.trim($(this).val()) != '')
            $(this).attr('data-empty', !$(this).val());
    })
    $('.form-label').find('select').each(function () {
        if ($.trim($(this).val()).toString().indexOf('?') < 0)
            $(this).attr('data-empty', !$(this).val());
    })
    $('.form-label').find('input,textarea').on('input', function (e) {
        $(e.currentTarget).attr('data-empty', !e.currentTarget.value);
    });
    $('.form-label').find('select').on('change', function (e) {
        $(e.currentTarget).attr('data-empty', !e.currentTarget.value);
    });
   
    
}


function funResetInputPlaceholder() {
    $('.form-label').find('input,textarea').each(function () {
        if ($.trim($(this).val())== '')
            $(this).attr('data-empty', true);
    })
    $('.form-label').find('select').each(function () {
        if ($.trim($(this).val()).toString().indexOf('?') > 0)
            $(this).attr('data-empty', true);
    })    
}