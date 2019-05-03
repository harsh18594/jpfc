var Jpfc = Jpfc || {};

Jpfc.AdminUpdatePrice = function () {
    var viewModel;
    var table;
    var $form = $('#price-update-form');
    var $saveSpinner = $("#btn-save-price").find('i');
    var isSaving = false;
    var loadingPriceSpinner = null;

    var initDatePickers = function () {
        $('.datepicker').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy"
        });
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#price-table').on('preXhr.dt', function () {
            loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('price-table'));
        }).DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2, -3, -4],
                "orderable": false
            }],
            "ajax": {
                "url": "/Admin/ListPrices",
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
                    data: "metal"
                },
                {
                    data: "karat"
                },
                {
                    data: "buyPriceStr"
                },
                {
                    data: "sellPriceStr"
                },
                {
                    data: "loanPriceStr"
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

    var populateKaratDropdown = function (karatId) {
        var metalId = $('#MetalId').val();
        var $dropdown = $('#KaratId');
        if (metalId) {
            $.ajax({
                url: "/Karat/FetchKarats",
                method: "get",
                data: {
                    metalId: metalId
                }
            }).done(function (result) {

                $dropdown.find('option').not(':first').remove();
                $.each(result.model, function () {
                    $dropdown.append($("<option />").val(this.value).text(this.text));
                });

                // select karatId if required
                if (karatId) {
                    $('#KaratId').val(karatId);
                }
            }).fail(function () {
            });
        } else {
            $dropdown.find('option').not(':first').remove();
        }
    };

    var clearFormForNewEntry = function (isCancelButtonRequest) {
        $form.find('#PriceId').val('0');
        $form.find('#BuyPrice').val('');
        $form.find('#SellPrice').val('');
        $form.find('#LoanPrice').val('');
        $form.find('#KaratId').val('');
        $form.find('#Date').datepicker('setDate', new Date());

        $form.validate().resetForm();

        if (isCancelButtonRequest) {
            $form.find('.field-validation-error').html('');
            $form.find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
            $form.find('#MetalId').val('');
            $form.find('#LoanPricePercent').val('');

            $('#KaratId').find('option').not(':first').remove();
        }
    };

    var populateFormForEdit = function (data) {
        $form.find('#PriceId').val(data.priceId);
        $form.find('#BuyPrice').val(data.buyPrice);
        $form.find('#SellPrice').val(data.sellPrice);
        $form.find('#LoanPrice').val(data.loanPrice);
        $form.find('#LoanPricePercent').val(data.loanPricePercent);
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
                populateKaratDropdown(result.model.karatId);
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
            text: "Once deleted, you will not be able to recover this data.",
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

    var calculateLoanPrice = function () {
        //var loanPrice = $('#LoanPrice').val();
        //var isLoanPriceAlreadyAdded = loanPrice !== null && loanPrice !== undefined && loanPrice !== '';
        //if (!isLoanPriceAlreadyAdded) {
        var buyPrice = $('#BuyPrice').val();
        var loanPricePercent = $('#LoanPricePercent').val();
        if (Number(buyPrice) && Number(loanPricePercent)) {
            var loanPrice = (buyPrice * loanPricePercent) / 100;
            $('#LoanPrice').val(loanPrice);
        } else {
            $('#LoanPrice').val('');
        }
        //}
    };

    var calculateLoanPercent = function () {
        var buyPrice = $('#BuyPrice').val();
        var loanPrice = $('#LoanPrice').val();
        if (Number(buyPrice) && Number(loanPrice)) {
            var loanPricePercent = (100 * loanPrice) / buyPrice;
            $('#LoanPricePercent').val(loanPricePercent.toFixed(2));
        } else {
            $('#LoanPricePercent').val('');
        }
    };

    var bindEvents = function () {
        $('#price-update-form').on('submit', function (e) {
            e.preventDefault();
            savePrice();
        });

        $('#btn-cancel-price').on('click', function () {
            clearFormForNewEntry(true);
        });

        $('#BuyPrice, #LoanPricePercent').on('keyup', function () {
            calculateLoanPrice();
        });

        $('#LoanPrice').on('keyup', function () {
            calculateLoanPercent();
        });

        $('#MetalId').on('change', function () {
            populateKaratDropdown();
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