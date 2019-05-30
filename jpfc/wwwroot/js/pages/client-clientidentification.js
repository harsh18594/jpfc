var Jpfc = Jpfc || {};

Jpfc.ClientIdentification = function () {
    var identificationTable;
    var loadingIdSpinner = null;

    var initDataTable = function () {
        identificationTable = $('#client-identification-table').on('preXhr.dt', function () {
            loadingIdSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('client-identification-table'));
        }).DataTable({
            "order": [0, "asc"],
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "columnDefs": [{
                "targets": "_all",
                "orderable": false
            }],
            "ajax": {
                "url": "/Client/GetClientIdentificationList",
                "method": "get",
                "data": function (d) {
                    return {
                        clientId: $('#client-id-form #ClientId').val()
                    };
                },
                "dataSrc": function (d) {
                    if (loadingIdSpinner !== null) {
                        loadingIdSpinner.stop();
                    }
                    return d;
                }
            },
            "columns": [
                {
                    data: "displayValue"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary edit-identification" title="Edit Id" data-id="' + row.clientIdentificationId + '"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger delete-identification" title="Delete Id" data-id="' + row.clientIdentificationId + '"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var populateIdentificationForm = function (data) {
        $('#ClientIdentificationId').val(data.clientIdentificationId);
        $('#IdentificationDocumentId').val(data.identificationDocumentId);
        $('#IdentificationDocumentNumber').val(data.identificationDocumentNumber);
    };

    var saveIdentification = function () {
        if ($('#client-id-form').valid()) {
            $('#btn-save-client-id').find('i').show();
            $.ajax({
                url: '/Client/SaveClientIdentification',
                method: 'post',
                data: $('#client-id-form').serialize()
            }).done(function (result) {
                if (result.success) {
                    identificationTable.ajax.reload();
                    $('#client-id-modal').modal('hide');
                } else {
                    toastr.error(result.error, '', Jpfc.Toastr.config);
                }
                $('#btn-save-client-id').find('i').hide();
            }).fail(function () {
                $('#btn-save-client-id').find('i').hide();
                toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            });
        }
    };

    var editIdentification = function (id) {
        $.ajax({
            url: '/Client/EditClientIdentification',
            method: 'get',
            data: {
                identificationId: id
            }
        }).done(function (result) {
            if (result.success) {
                populateIdentificationForm(result.model);
                $('#client-id-modal').modal('show');
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
        }).fail(function () {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
        });
    };

    var deleteIdentification = function (id) {
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
                    url: '/Client/DeleteClientIdentification',
                    method: 'post',
                    data: {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        identificationTable.ajax.reload();
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function () {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var clearIdentificationForm = function () {
        $('#ClientIdentificationId').val('0');
        $('#IdentificationDocumentId').val('');
        $('#IdentificationDocumentNumber').val('');

        $('#client-id-form').find('.field-validation-error').html('');
        $('#client-id-form').find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');

        $('#client-id-form').validate().resetForm();
    };

    var bindEvents = function () {
        $('#btn-save-client-id').on('click', function () {
            saveIdentification();
        });

        $('#client-identification-table').on('click', '.edit-identification', function () {
            editIdentification($(this).data('id'));
        });

        $('#client-identification-table').on('click', '.delete-identification', function () {
            deleteIdentification($(this).data('id'));
        });

        $("#client-id-modal").on("hidden.bs.modal", function () {
            clearIdentificationForm();
        });

        $('#client-id-form').on('submit', function (e) {
            e.preventDefault();
            saveIdentification();
        });

    };

    var init = function () {
        initDataTable();
        bindEvents();
    };

    return {
        init: init
    };
}();