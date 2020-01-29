var Jpfc = Jpfc || {};

Jpfc.CareerCreate = function () {
    var viewModel;

    var initDatePickers = function () {
        $('#JobStartDate, #JobCloseDate').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy",
            clearBtn: true,
            todayBtn: 'linked'
        });
    };

    var initTinyMce = function () {
        tinymce.init({ selector: '#Requirements' });

        tinymce.init({ selector: '#Description' });
    };

    var init = function (model) {
        viewModel = model;
        
        initDatePickers();
        initTinyMce();
    };

    return {
        init: init
    };
}();