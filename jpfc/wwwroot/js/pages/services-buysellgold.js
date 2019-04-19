var Jpfc = Jpfc || {};

Jpfc.ServicesBuySellGold = function () {
    var viewModel;
    var loadingPriceSpinner = null;
    var isInitialLoad = true;
    var table;

    var initDatePickers = function () {
        $('#date-filter').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy",
            todayBtn: "linked",
            endDate: new Date()
        });
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#price-table').on('preXhr.dt', function () {
            if (!isInitialLoad) {
                loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('price-table'));
            }
        }).DataTable({
            dom: 'Bfrtip',
            "paging": true,
            "info": false,
            "searching": false,
            buttons: [
                //'copyHtml5',
                'excelHtml5',
                //'csvHtml5',
                'pdfHtml5'
            ],
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2],
                "orderable": false
            }],
            "deferLoading": 0,
            "ajax": {
                "url": "/Services/ListPrices",
                "method": "get",
                "data": function () {
                    return {
                        date: $('#date-filter').val()
                    };
                },
                "dataSrc": function (d) {
                    if (loadingPriceSpinner !== null) {
                        loadingPriceSpinner.stop();
                    }

                    isInitialLoad = false;
                    return d;
                }
            },
            "columns": [
                {
                    data: "dateStr"
                },
                {
                    data: "metal"
                },
                {
                    data: "karat"
                },
                {
                    data: "amountStr"
                }
            ]
        });
    };

    var bindEvents = function () {
        $('#date-filter').on('change', function () {
            table.ajax.reload();
        });
    };

    var init = function () {
        console.log("Jpfc.ServicesBuySellGold init");
        initDatePickers();
        initDataTable();
        bindEvents();
    };

    return {
        init: init
    };
}();