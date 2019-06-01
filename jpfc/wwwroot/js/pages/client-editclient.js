var Jpfc = Jpfc || {};

Jpfc.ClientEditClient = function () {
    var loadingReceiptSpinner = null;

    var initReceiptDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        itemDatatable = $('#receipt-table').on('preXhr.dt', function () {
            loadingReceiptSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('receipt-table'));
        }).DataTable({
            "order": [0, "asc"],
            "columnDefs": [{
                "targets": "_all",
                "orderable": false
            }],
            "ajax": {
                "url": "/Client/GetClientReceiptList",
                "method": "post",
                "data": function (d) {
                    return {
                        clientId: $('#edit-client-form #ClientId').val()
                    };
                },
                "dataSrc": function (d) {
                    if (loadingReceiptSpinner !== null) {
                        loadingReceiptSpinner.stop();
                    }
                    return d;
                }
            },
            "columns": [
                {
                    data: "dateStr"
                },
                {
                    data: "receiptNumber"
                },
                {
                    data: "amountStr"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a href="/Client/Receipt?clientId=' + row.clientId + '&receiptId=' + row.clientReceiptId + '" class="btn btn-primary" title="Edit Item"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger delete-receipt" title="Delete Item" data-receipt-id="' + row.clientReceiptId + '"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var deleteReceipt = function () {
        swal({
            title: 'Are you sure?',
            text: "Once deleted, you will not be able to recover this data.",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#337ab7',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    "url": "/Client/DeleteClientReceipt",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        itemDatatable.ajax.reload(null, false);
                        toastr.success('Receipt deleted successfully', '', Jpfc.Toastr.config);
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function (jqXhr, textStatus) {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var bindEvents = function () {
        $('#edit-client-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client').find('i').show();
            }
        });

        $('#receipt-table').on('click', '.delete-receipt', function () {
            deleteReceipt();
        });
    };

    var init = function () {
        bindEvents();
        initReceiptDataTable();
    };

    return {
        init: init
    };
}();