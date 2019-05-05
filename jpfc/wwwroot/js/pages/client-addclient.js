var Jpfc = Jpfc || {};

Jpfc.ClientAddClient = function () {
    var itemDatatable;
    var loadingBelongingSpinner = null;
    var loadingPriceSpinner = null;

    var initDatePickers = function () {
        $('.datepicker').datepicker({
            autoclose: true,
            format: "mm-dd-yyyy"
        });
    };

    var initItemsDatatable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        itemDatatable = $('#belonging-table').on('preXhr.dt', function () {
            loadingBelongingSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('price-table'));
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
                    if (loadingBelongingSpinner !== null) {
                        loadingBelongingSpinner.stop();
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

    var initDropdowns = function () {
        // add other option
        $('#MetalId').append($("<option />").val("other").text("Other"));
    };

    var populateKaratDropdown = function (karatId) {
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

                    // select karatId if required
                    if (karatId) {
                        $('#KaratId').val(karatId);
                    }
                }).fail(function () {
                });
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
        loadingPriceSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('client-belonging-form'));

        // delay actual execution, if it was a change in metal, it will load karat values from server
        setTimeout(function () {
            var metalId = $('#MetalId').val();
            var karatId = $('#KaratId').val();
            var date = $('#BelDate').val();
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
                        clientAction: $('#ClientAction').val()
                    }
                }).done(function (result) {
                    if (result.success) {
                        $('#ItemPrice').val(result.price).trigger('keyup');
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                    }

                    if (loadingPriceSpinner !== null) {
                        loadingPriceSpinner.stop();
                    }
                }).fail(function () {
                    toastr.error("Unexpected error occurred while fetching the price", '', Jpfc.Toastr.config);

                    if (loadingPriceSpinner !== null) {
                        loadingPriceSpinner.stop();
                    }
                });
            } else {
                if (loadingPriceSpinner !== null) {
                    loadingPriceSpinner.stop();
                }
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

    var clearBelongingForm = function () {

        $("#ClientAction").val($("#ClientAction option:first").val());
        $('#MetalId').val('');
        $('#KaratId').val('');
        $('#MetalOther').val('');
        $('#KaratOther').val('');
        $('#Weight').val('');
        $('#ItemPrice').val('');
        $('#FinalPrice').val('');
        $('#BelDate').datepicker('setDate', new Date());

        $('#client-belonging-form').find('.field-validation-error').html('');
        $('#client-belonging-form').find('.field-validation-error').removeClass('field-validation-error').addClass('field-validation-valid');
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

        }
    };

    var bindEvents = function () {
        $('#add-client-form').on('submit', function () {
            if ($(this).valid()) {
                $('#btn-save-client').find('i').show();
            }
        });

        $('#MetalId').on('change', function () {
            populateKaratDropdown();
        });

        $('#KaratId').on('change', function () {
            handleViewForKaratDropdown();
        });

        $('#BelDate, #ClientAction, #MetalId, #KaratId').on('change', function () {
            fetchPrice();
        });

        $('#Weight, #ItemPrice').on('keyup', function () {
            calculateFinalPrice();
        });

        $("#belonging-modal").on("hidden.bs.modal", function () {
            clearBelongingForm();
        });

        $('#btn-save-belonging').on('click', function () {
            saveBelonging();
        });
    };

    var init = function () {
        initItemsDatatable();
        bindEvents();
        initDatePickers();
        initDropdowns();
    };

    return {
        init: init
    };
}();