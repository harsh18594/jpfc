﻿var Jpfc = Jpfc || {};

Jpfc.ClientReceipt = function () {
    var itemDatatable;
    var loadingBelongingSpinner = null;
    var loadingPriceSpinner = null;

    var startBelongingModalSpinner = function () {
        loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('client-belonging-form'));
    };

    var stopBelongingModalSpinner = function () {
        if (loadingPriceSpinner !== null) {
            loadingPriceSpinner.stop();
        }
    };

    var initDatePickers = function () {
        $('#PaymentDate').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy",
            clearBtn: true,
            todayBtn: 'linked'
        });
    };

    // load belongings 
    var initItemsDatatable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        itemDatatable = $('#belonging-table').on('preXhr.dt', function () {
            loadingBelongingSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('belonging-table'));
        }).DataTable({
            "order": [0, "asc"],
            "columnDefs": [{
                "targets": "_all",
                "orderable": false
            }],
            "ajax": {
                "url": "/Client/GetClientBelongingList",
                "method": "get",
                "data": function (d) {
                    return {
                        clientReceiptId: $('#client-belonging-form #ClientReceiptId').val()
                    };
                },
                "dataSrc": function (d) {
                    if (loadingBelongingSpinner !== null) {
                        loadingBelongingSpinner.stop();
                    }
                    // update amount summary
                    fetchReceiptSummary();
                    return d;
                }
            },
            "columns": [
                {
                    data: "metal",
                    render: function (data, type, row) {
                        var itemDesc = '';
                        if (row.itemDescription) {
                            itemDesc = "(" + row.itemDescription + ")";
                        }
                        return data + " " + itemDesc;
                    }
                },
                {
                    data: "karat"
                },
                {
                    data: "weightStr"
                },
                {
                    data: "itemPriceStr",
                },
                {
                    className: 'font-weight-bold',
                    render: function (data, type, row) {
                        var className = '';
                        var hstText = "";
                        if (row.businessPaysMoney) {
                            className = 'text-danger';
                        } else if (row.businessGetsMoney) {
                            className = 'text-success bold';
                        }

                        if (row.hstAmount > 0) {
                            hstText = "*";
                        }
                        return hstText + ' <span class="' + className + '">' + row.finalPriceStr + '</span> <span>' + ' [' + row.replacementValueStr + '] </span> <span class="badge badge-info">' + row.transactionAction + '</span>';
                    }
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary edit-belonging" title="Edit Item" data-belonging-id="' + row.clientBelongingId + '"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger delete-belonging" title="Delete Item" data-belonging-id="' + row.clientBelongingId + '"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var initDropdowns = function () {
        // add other option
        $('#MetalId').append($("<option />").val("other").text("Other"));
    };

    var clearBelongingForm = function () {

        $("#TransactionAction").val($("#TransactionAction option:first").val());
        $('#MetalId').val('');
        $('#KaratId').val('');
        $('#MetalOther').val('');
        $('#KaratOther').val('');
        $('#ItemDescription').val('');
        $('#Weight').val('');
        $('#ItemPrice').val('');
        $('#FinalPrice').val('');
        $('#ReplacementValue').val('');
        $('#HstAmount').val('');
        $('#ClientBelongingId').val('0');

        $('#client-belonging-form').find('.field-validation-error').html('');
        $('#client-belonging-form').find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');

        $('#client-belonging-form').validate().resetForm();

        $("#karat-dd-container").show();
        $("#karat-other-container").hide();
        $("#metal-other-container").hide();
    };

    var populateBelongingForm = function (data) {
        $("#TransactionAction").val(data.transactionAction);
        // handle karat other first
        if (data.karatOther) {
            $('#KaratId').val("other");
            $('#KaratOther').val(data.karatOther);
            $("#karat-dd-container").hide();
            $("#karat-other-container").show();
        } else {
            $('#KaratId').val(data.karatId);
        }

        // handle metal other, if metal other is selected, hide karat dropdown and show karat other
        if (data.metalOther) {
            $('#MetalId').val("other");
            $('#MetalOther').val(data.metalOther);
            $("#metal-other-container").show();

            $('#KaratId').val("");
            $("#karat-dd-container").hide();
            $("#karat-other-container").show();
        } else {
            $('#MetalId').val(data.metalId);
        }

        $('#ItemDescription').val(data.itemDescription);
        $('#Weight').val(data.weight);
        $('#ItemPrice').val(data.itemPrice);
        $('#FinalPrice').val(data.finalPrice);
        $('#ReplacementValue').val(data.replacementValue);
        $('#HstAmount').val(data.hstAmount);
        $('#ClientBelongingId').val(data.clientBelongingId);
    };

    var fetchKaratList = function (populateData, data) {
        stopBelongingModalSpinner();
        startBelongingModalSpinner();
        var $dropdown = $('#KaratId');
        $.ajax({
            url: "/Karat/FetchKarats",
            method: "get",
            data: {
                metalId: $('#MetalId').val()
            }
        }).done(function (result) {
            $dropdown.find('option').not(':first').remove();
            $.each(result.model, function () {
                $dropdown.append($("<option />").val(this.value).text(this.text));
            });

            // add other option
            $dropdown.append($("<option />").val("other").text("Other"));

            // populate form if required
            if (populateData) {
                populateBelongingForm(data);
            }
            stopBelongingModalSpinner();
        }).fail(function () {
            stopBelongingModalSpinner();
        });
    };
    var populateKaratDropdown = function () {
        var metalId = $('#MetalId').val();
        var $dropdown = $('#KaratId');
        $("#metal-other-container").hide();
        $("#karat-other-container").hide();
        $("#karat-dd-container").show();

        if (metalId) {
            if (metalId === "other") {
                $dropdown.find('option').not(':first').remove();
                $("#metal-other-container").show();
                $("#karat-other-container").show();
                $("#karat-dd-container").hide();
            } else {
                fetchKaratList();
            }

        } else {
            $dropdown.find('option').not(':first').remove();
        }
    };

    var handleViewForKaratDropdown = function () {
        var karatId = $('#KaratId').val();
        $("#karat-other-container").hide();
        if (karatId === "other") {
            $("#karat-other-container").show();
        }
    };

    var fetchPrice = function () {
        stopBelongingModalSpinner();
        startBelongingModalSpinner();

        // delay actual execution, if it was a change in metal, it will load karat values from server
        setTimeout(function () {
            var metalId = $('#MetalId').val();
            var karatId = $('#KaratId').val();
            var date = $('#add-receipt-form #Date').val();
            $('#ItemPrice').val('');
            $('#FinalPrice').val('');

            if (metalId && metalId !== "other" && karatId && karatId !== "other" && date) {
                $.ajax({
                    url: '/Admin/FetchMetalPrice',
                    method: 'get',
                    data: {
                        metalId: metalId,
                        karatId: karatId,
                        date: date,
                        transactionAction: $('#TransactionAction').val()
                    }
                }).done(function (result) {
                    if (result.success) {
                        $('#ItemPrice').val(result.price).trigger('keyup');
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }

                    stopBelongingModalSpinner();
                }).fail(function () {
                    toastr.error("Unexpected error occurred while fetching the price", '', Jpfc.Toastr.config);
                    stopBelongingModalSpinner();
                });
            } else {
                stopBelongingModalSpinner();
            }
        }, 500);
    };

    var calculateFinalPrice = function () {
        var itemPrice = $('#ItemPrice').val();
        var weight = $('#Weight').val();
        if (Number(itemPrice) && Number(weight)) {
            var finalPrice = itemPrice * weight;
            $('#FinalPrice').val(finalPrice.toFixed(2));
        } else {
            $('#FinalPrice').val('');
        }
    };

    var isBelongingFormValid = function () {
        var isValid = true;

        if (!$('#client-belonging-form').valid()) {
            isValid = false;
        }

        return isValid;
    };

    var saveBelonging = function () {
        if (isBelongingFormValid()) {
            startBelongingModalSpinner();
            $.ajax({
                url: "/Client/SaveClientBelonging",
                method: "post",
                data: $('#client-belonging-form').serialize()
            }).done(function (result) {
                if (result.success) {
                    toastr.success("Items has been added successfully", '', Jpfc.Toastr.config);
                    $("#belonging-modal").modal('hide');
                    itemDatatable.ajax.reload();
                } else {
                    toastr.error(result.error, '', Jpfc.Toastr.config);
                }
                stopBelongingModalSpinner();
            }).fail(function () {
                toastr.error("Unexpected error occurred while processing your request", '', Jpfc.Toastr.config);
                stopBelongingModalSpinner();
            });
        }
    };

    var editBelonging = function (id) {
        $.ajax({
            url: "/Client/EditClientBelonging",
            method: "get",
            data: {
                belongingId: id
            }
        }).done(function (result) {
            if (result.success) {
                $('#MetalId').val(result.model.metalId);
                // this function will handle populating  form because it has to make an ajax call
                fetchKaratList(true, result.model);
                $("#belonging-modal").modal('show');
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
        }).fail(function () {
            toastr.error("Unexpected error occurred while processing your request", '', Jpfc.Toastr.config);
        });
    };

    var deleteBelonging = function (id) {
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
                    "url": "/Client/DeleteClientBelonging",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        itemDatatable.ajax.reload(null, false);
                        toastr.success('Item removed successfully', '', Jpfc.Toastr.config);
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }
                }).fail(function (jqXhr, textStatus) {
                    toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                });
            }
        });
    };

    var fetchReceiptSummary = function () {
        $('#summary-loading-spinner').show();
        $.ajax({
            url: '/Client/FetchReceiptSummary',
            method: 'get',
            data: {
                clientReceiptId: $('#client-belonging-form #ClientReceiptId').val()
            }
        }).done(function (result) {
            if (result.success) {
                $('#total-purchase-amount').html(result.model.purchaseTotalStr);
                $('#total-sell-amount').html(result.model.sellTotalStr);
                $('#principal-loan-amount').html(result.model.principalLoanAmountStr);
                $('#interest-amount').html(result.model.interestAmountStr);
                $('#service-fee-amount').html(result.model.serviceFeeStr);
                $('#storage-fee-amount').html(result.model.storageFeeStr);
                $('#hst-amount').html(result.model.hstAmountStr);
                $('#summary-blurb').html(result.model.summaryBlurb);
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
            $('#summary-loading-spinner').hide();
        }).fail(function () {
            $('#summary-loading-spinner').hide();
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
        });
    };

    var downloadReceipt = function () {
        $('#btn-print-receipt').find('.loading-spinner').show();
        $.ajax({
            url: "/Client/ExportReceipt",
            method: "post",
            data: {
                clientReceiptId: $('#ClientReceiptId').val()
            }
        }).done(function (result) {
            if (result.success) {
                var bytes = Jpfc.Helper.base64ToArrayBuffer(result.fileBytes);
                Jpfc.Helper.downloadFile(result.fileName, bytes);
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
            $('#btn-print-receipt').find('.loading-spinner').hide();
        }).fail(function () {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            $('#btn-print-receipt').find('.loading-spinner').hide();
        });
    };

    var downloadLoanSchedule = function () {
        $('#btn-download-loan-schedule').find('.loading-spinner').show();
        $.ajax({
            url: "/Client/ExportLoanSchedule",
            method: "post",
            data: {
                clientReceiptId: $('#ClientReceiptId').val()
            }
        }).done(function (result) {
            if (result.success) {
                var bytes = Jpfc.Helper.base64ToArrayBuffer(result.fileBytes);
                Jpfc.Helper.downloadFile(result.fileName, bytes);
            } else {
                toastr.error(result.error, '', Jpfc.Toastr.config);
            }
            $('#btn-download-loan-schedule').find('.loading-spinner').hide();
        }).fail(function () {
            toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
            $('#btn-download-loan-schedule').find('.loading-spinner').hide();
        });
    };

    var isReceiptPaid = function () {
        var isPaid = true;
        if (Jpfc.Helper.isNullOrEmpty($('#PaymentDate').val()) || Jpfc.Helper.isNullOrEmpty($('#PaymentAmount').val())) {
            isPaid = false;
        }
        return isPaid;
    };
    var downloadPaymentReceipt = function () {
        if (isReceiptPaid()) {
            $('#btn-download-payment-receipt').find('.loading-spinner').show();
            $.ajax({
                url: "/Client/ExportPaymentReceipt",
                method: "post",
                data: {
                    clientReceiptId: $('#ClientReceiptId').val()
                }
            }).done(function (result) {
                if (result.success) {
                    var bytes = Jpfc.Helper.base64ToArrayBuffer(result.fileBytes);
                    Jpfc.Helper.downloadFile(result.fileName, bytes);
                } else {
                    toastr.error(result.error, '', Jpfc.Toastr.config);
                }
                $('#btn-download-payment-receipt').find('.loading-spinner').hide();
            }).fail(function () {
                toastr.error('Unexpected error occurred while processing your request', '', Jpfc.Toastr.config);
                $('#btn-download-payment-receipt').find('.loading-spinner').hide();
            });
        } else {
            swal({
                title: 'Error',
                text: "Payment Date or Payment Amount is invalid.",
                type: 'info'
            });
        }
    };

    var bindEvents = function () {
        $('#add-receipt-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client-receipt').find('i').show();
            }
        });

        $('#MetalId').on('change', function () {
            populateKaratDropdown();
        });

        $('#KaratId').on('change', function () {
            handleViewForKaratDropdown();
        });

        $('#TransactionAction, #MetalId, #KaratId').on('change', function () {
            fetchPrice();
        });

        $('#Weight, #ItemPrice').on('keyup', function () {
            calculateFinalPrice();
        });

        $("#belonging-modal").on("hidden.bs.modal", function () {
            clearBelongingForm();
            stopBelongingModalSpinner();
        });

        $('#btn-save-belonging').on('click', function () {
            saveBelonging();
        });

        $('#belonging-table').on('click', '.edit-belonging', function () {
            editBelonging($(this).data('belonging-id'));
        });
        $('#belonging-table').on('click', '.delete-belonging', function () {
            deleteBelonging($(this).data('belonging-id'));
        });

        $('#client-belonging-form').on('keypress', function (e) {
            if (e.keyCode === 13) {
                saveBelonging();
            }
        });
        $('#btn-print-receipt').on('click', function () {
            downloadReceipt();
        });

        $('#btn-download-loan-schedule').on('click', function () {
            downloadLoanSchedule();
        });

        $('#btn-download-payment-receipt').on('click', function () {
            downloadPaymentReceipt();
        });
    };

    var initMask = function () {
        $('.ca-phone-mask').mask('(000) 000-0000');
    };

    var init = function () {
        initItemsDatatable();
        bindEvents();
        initDatePickers();
        initDropdowns();
        initMask();
    };

    return {
        init: init
    };
}();