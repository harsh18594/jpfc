var Jpfc = Jpfc || {};

Jpfc.ClientIndex = function () {
    var loadingClientSpinner = null;
    var table;

    var setSearchLoading = function () {
        $('#btn-search').find('.loading-spinner').show();
        loadingClientSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('client-table'));
    };

    var clearSearchLoading = function () {
        $('#btn-search').find('.loading-spinner').hide();
        if (loadingClientSpinner !== null) {
            loadingClientSpinner.stop();
        }
    };

    var initDatePickers = function () {
        $('#FilterStartDate, #FilterEndDate').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy",
            clearBtn: true,
            todayBtn: 'linked'
        }).on('change', function () {
            table.ajax.reload();
        });
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#client-table').on('preXhr.dt', function () {
            clearSearchLoading();
            setSearchLoading();
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
                    return {
                        startDate: $('#FilterStartDate').val(),
                        endDate: $('#FilterEndDate').val(),
                        firstName: $('#FirstName').val(),
                        lastName: $('#LastName').val()
                    };
                },
                "dataSrc": function (d) {
                    clearSearchLoading();
                    return d;
                }
            },
            "columns": [
                {
                    data: "dateStr"
                },
                {
                    data: "firstName"
                },
                {
                    data: "lastName"
                },
                {
                    data: "referenceNumber"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary" title="Edit Client" href="/Client/EditClient/' + row.clientId + '"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger" title = "Delete Client"  onclick="Jpfc.ClientIndex.deleteClient(' + row.clientId + ')"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var deleteClient = function (id) {
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
                    "url": "/Client/DeleteClient",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        table.ajax.reload(null, false);
                        toastr.success('Client deleted successfully', '', Jpfc.Toastr.config);
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
        $('#btn-search').on('click', function () {
            table.ajax.reload();
        });

        $('.search-input').on('keypress', function (e) {
            if (e.keyCode === 13) {
                table.ajax.reload();
            }
        });
    };

    var init = function () {
        initDataTable();
        initDatePickers();
        bindEvents();
    };

    return {
        init: init,
        deleteClient: deleteClient
    };
}();