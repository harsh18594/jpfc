var Jpfc = Jpfc || {};

Jpfc.ServicesBuySellGold = function () {
    var viewModel;

    var initDataTable = function () {
        $('#price-table').DataTable({
            dom: 'Bfrtip',
            "paging": false,
            "info": true,
            "searching": false,
            buttons: [
                //'copyHtml5',
                'excelHtml5',
                //'csvHtml5',
                'pdfHtml5'
            ]
        });
    };

    var init = function () {
        initDataTable();
    };

    return {
        init: init
    };
}();