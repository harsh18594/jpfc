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
        tinymce.init({
            selector: '#Requirements',
            plugins: "lists",
            toolbar: "undo redo | bullist | bold italic underline | link | alignleft aligncenter alignright alignjustify | outdent indent"
        });

        tinymce.init({
            selector: '#Description',
            plugins: "lists",
            toolbar: "undo redo | bullist | bold italic underline | link | alignleft aligncenter alignright alignjustify | outdent indent"
        });
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