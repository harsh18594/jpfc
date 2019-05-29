var Jpfc = Jpfc || {};

Jpfc.ClientEditClient = function () {   
    var bindEvents = function () {
        $('#edit-client-form').on('submit', function () {
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