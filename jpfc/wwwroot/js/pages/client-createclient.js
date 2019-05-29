var Jpfc = Jpfc || {};

Jpfc.ClientCreateClient = function () {
    var bindEvents = function () {
        $('#create-client-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client').find('i').show();
            }
        });
    };

    var init = function () {
        bindEvents();
    };

    return {
        init: init
    };
}();