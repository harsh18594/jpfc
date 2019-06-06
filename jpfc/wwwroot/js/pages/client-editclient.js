var Jpfc = Jpfc || {};

Jpfc.ClientEditClient = function () {
    var loadingReceiptSpinner = null;
    var receiptDataTable;

    var initReceiptDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY HH:mm:ss');
        $.fn.dataTable.moment('DD/MMM/YYYY HH:mm:ss');
        receiptDataTable = $('#receipt-table').on('preXhr.dt', function () {
            loadingReceiptSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('receipt-table'));
        }).DataTable({
            "order": [0, "desc"],
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
                    data: "finalAmountStr"
                },
                {
                    data: "paymentDateStr"
                },
                {
                    data: "paymentAmountStr",
                    render: function (data, type, row) {
                        var html = '';
                        if (row.paymentDateStr) {
                            html = '<span>' + data + '</span> <span class="badge badge-info">' + row.paymentMethod + '</span>';
                        }
                        return html;
                    }
                },
                {
                    render: function (data, type, row) {
                        var html = '';
                        if (row.paymentDateStr) {
                            html = '<i class="fa fa-close text-danger"></i>';
                            if (row.isPaidInterestOnly) {
                                html = '<i class="fa fa-check text-success"></i>';
                            }
                        }
                        return html;
                    }
                },
                {
                    render: function (data, type, row) {
                        var html = '<a href="javascript:void(0)" class="btn btn-primary download-receipt" title="Download Receipt" data-receipt-id="' + row.clientReceiptId + '"><i class="fa fa-file-pdf-o text-white"></i> <i class="fa fa-spinner fa-spin receipt-loading-spinner" style="display: none;"></i></a>' +
                            ' <a href="javascript:void(0)" class="btn btn-primary download-loan-schedule" title="Download Loan Schedule" data-receipt-id="' + row.clientReceiptId + '"><i class="fa fa-clock-o text-white"></i> <i class="fa fa-spinner fa-spin loading-spinner" style="display: none;"></i></a>' +
                            ' <a href="/Client/Receipt?clientId=' + row.clientId + '&receiptId=' + row.clientReceiptId + '" class="btn btn-primary" title="Edit Item"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-warning copy-receipt" href="javascript:void(0);" title="Copy Item" data-receipt-id="' + row.clientReceiptId + '"><i class="fa fa-copy text-white"></i></a>' +
                            ' <a class="btn btn-danger delete-receipt" title="Delete Item" data-receipt-id="' + row.clientReceiptId + '"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var deleteReceipt = function (id) {
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
                        receiptId: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        receiptDataTable.ajax.reload(null, false);
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

    var copyReceipt = function (id) {
        swal({
            title: 'Are you sure?',
            text: "Selected receipt will be duplicated.",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#337ab7',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, copy it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    "url": "/Client/DuplicateClientReceipt",
                    "method": "post",
                    "data": {
                        receiptId: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        // redirect to receipt edit page
                        window.location = '/Client/Receipt?clientId=' + $('#edit-client-form #ClientId').val() + '&receiptId=' + result.receiptId + '';
                        toastr.success('Receipt duplicated successfully', '', Jpfc.Toastr.config);
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function (jqXhr, textStatus) {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var downloadReceipt = function (id, $btn) {
        $btn.find('.receipt-loading-spinner').show();
        $.ajax({
            url: "/Client/ExportReceipt",
            method: "post",
            data: {
                clientReceiptId: id
            }
        }).done(function (result) {
            if (result.success) {
                var bytes = Jpfc.Helper.base64ToArrayBuffer(result.fileBytes);
                Jpfc.Helper.downloadFile(result.fileName, bytes);
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
            $btn.find('.receipt-loading-spinner').hide();
        }).fail(function () {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            $btn.find('.receipt-loading-spinner').hide();
        });
    };

    var downloadLoanSchedule = function (id, $btn) {
        $btn.find('.loading-spinner').show();
        $.ajax({
            url: "/Client/ExportLoanSchedule",
            method: "post",
            data: {
                clientReceiptId: id
            }
        }).done(function (result) {
            if (result.success) {
                var bytes = Jpfc.Helper.base64ToArrayBuffer(result.fileBytes);
                Jpfc.Helper.downloadFile(result.fileName, bytes);
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
            $btn.find('.loading-spinner').hide();
        }).fail(function () {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            $btn.find('.loading-spinner').hide();
        });
    };

    var bindEvents = function () {
        $('#edit-client-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client').find('i').show();
            }
        });

        $('#receipt-table').on('click', '.delete-receipt', function () {
            deleteReceipt($(this).data('receipt-id'));
        });

        $('#receipt-table').on('click', '.copy-receipt', function () {
            copyReceipt($(this).data('receipt-id'));
        });

        $('#receipt-table').on('click', '.download-receipt', function () {
            downloadReceipt($(this).data('receipt-id'), $(this));
        });

        $('#receipt-table').on('click', '.download-loan-schedule', function () {
            downloadLoanSchedule($(this).data('receipt-id'), $(this));
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