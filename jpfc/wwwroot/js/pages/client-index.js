var Jpfc = Jpfc || {};

Jpfc.ClientIndex = function () {
    var loadingPriceSpinner = null;

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#client-table').on('preXhr.dt', function () {
            loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('price-table'));
        }).DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2],
                "orderable": false
            }],
            "ajax": {
                "url": "/Client/GetClientList",
                "method": "get",
                "data": function (d) {
                    return d;
                },
                "dataSrc": function (d) {
                    if (loadingPriceSpinner !== null) {
                        loadingPriceSpinner.stop();
                    }
                    return d;
                }
            },
            "columns": [
                {
                    data: "dateStr"
                },
                {
                    data: "name"
                },
                {
                    data: "referenceNumber"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary" title="Edit Price" href="/Client/AddClient/' + row.clientId + '"><i class="fa fa-pencil text-white"></i></a>' +
                                   ' <a class="btn btn-danger" title = "Delete Price"  onclick="Jpfc.ClientIndex.deleteClient(' + row.clientId + ')"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
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