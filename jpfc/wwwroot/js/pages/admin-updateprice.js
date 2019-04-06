var Jpfc = Jpfc || {};

Jpfc.AdminUpdatePrice = function () {
    var viewModel;
    var table;
    var $form = $('#price-update-form');
    var $saveSpinner = $("#btn-save-price").find('i');
    var isSaving = false;

    var initDatePickers = function () {
        $('.datepicker').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy"
        });
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        table = $('#price-table').DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2, -3],
                "orderable": false
            }],
            "ajax": {
                "url": "/Admin/ListPrices",
                "method": "get",
                "data": function (d) {
                    return d;
                },
                "dataSrc": ""
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
                    data: "amount"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary" title="Edit Price" onclick="Jpfc.AdminUpdatePrice.editPrice(' + row.priceId + ')"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-warning" title="Copy Price" onclick="Jpfc.AdminUpdatePrice.copyPrice(' + row.priceId + ')"><i class="fa fa-copy text-white"></i></a>' +
                            ' <a class="btn btn-danger" title = "Delete Price"  onclick="Jpfc.AdminUpdatePrice.deletePrice(' + row.priceId + ')"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var clearFormForNewEntry = function () {
        $form.find('#PriceId').val('0');
        $form.find('#Amount').val('');
        $form.find('#KaratId').val('');
        $form.find('#Date').datepicker('setDate', new Date());

        $form.validate().resetForm();
    };

    var populateFormForEdit = function (data) {
        $form.find('#PriceId').val(data.priceId);
        $form.find('#Amount').val(data.amount);
        $form.find('#KaratId').val(data.karatId);
        $form.find('#MetalId').val(data.metalId);
        $form.find('#Date').val(data.dateStr).datepicker('update');
    };

    var savePrice = function () {
        if ($form.valid() && !isSaving) {
            isSaving = true;
            $saveSpinner.show();
            $.ajax({
                url: "/Admin/SavePrice",
                method: "post",
                data: $form.serialize()
            }).done(function (result) {
                if (result.success) {
                    table.ajax.reload();
                    clearFormForNewEntry();
                    toastr.success('Price saved successfully', '', Jpfc.Toastr.config);
                } else {
                    toastr.error(result.error, '', Jpfc.Toastr.config);
                }
                $saveSpinner.hide();
                isSaving = false;
            }).fail(function (jqXhr, textStatus) {
                $saveSpinner.hide();
                isSaving = false;
                toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            });
        }
    };

    var editPrice = function (id) {
        $.ajax({
            "url": "/Admin/EditPrice",
            "method": "get",
            "data": {
                id: id
            }
        }).done(function (result) {
            if (result.success) {
                populateFormForEdit(result.model);
                $('html,body').animate({
                    scrollTop: $('#content-container').offset().top - 100
                }, 'slow');
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
        }).fail(function (jqXhr, textStatus) {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
        });
    };

    var copyPrice = function (id) {
        swal({
            title: 'Are you sure?',
            text: "Price details will be copied over for today's date.",
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#337ab7',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, copy it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    "url": "/Admin/CopyPrice",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        table.ajax.reload(null, false);
                        toastr.success('Price copied successfully', '', Jpfc.Toastr.config);
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function (jqXhr, textStatus) {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var deletePrice = function (id) {
        swal({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#337ab7',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.value) {
                $.ajax({
                    "url": "/Admin/DeletePrice",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        table.ajax.reload(null, false);
                        toastr.success('Price deleted successfully', '', Jpfc.Toastr.config);
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
        $('#price-update-form').on('submit', function (e) {
            e.preventDefault();
            savePrice();
        });
    };

    var init = function (model) {
        console.log('Jpfc.AdminUpdatePrice init');

        viewModel = model;
        initDatePickers();
        initDataTable();
        bindEvents();
    };

    return {
        init: init,
        deletePrice: deletePrice,
        copyPrice: copyPrice,
        editPrice: editPrice
    };
}();