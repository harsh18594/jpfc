var Jpfc = Jpfc || {};

Jpfc.ClientCreateClient = function () {
    var bindEvents = function () {
        $('#create-client-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client').find('i').show();
            }
        });
    };

    var initMask = function () {
        $('.ca-phone-mask').mask('(000) 000-0000');
    };

    var init = function () {
        bindEvents();
        initMask();
    };

    return {
        init: init
    };
}();