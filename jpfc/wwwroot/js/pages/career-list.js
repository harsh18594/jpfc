var Jpfc = Jpfc || {};

Jpfc.CareerList = function () {
    var loadingCareerSpinner = null;
    var table;

    var setSearchLoading = function () {
        loadingCareerSpinner = new Spinner(Jpfc.Spin.config).spin(document.getElementById('career-table'));
    };

    var clearSearchLoading = function () {
        if (loadingCareerSpinner !== null) {
            loadingCareerSpinner.stop();
        }
    };

    var initDataTable = function () {
        $.fn.dataTable.moment('DD-MMM-YYYY');
        $.fn.dataTable.moment('DD/MMM/YYYY');
        table = $('#career-table').on('preXhr.dt', function () {
            clearSearchLoading();
            setSearchLoading();
        }).DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1, -2, -3],
                "orderable": false
            }],
            "ajax": {
                "url": "/Career/GetListData",
                "method": "get",
                "data": function () {
                    return {
                        activeOnly: $('#cb-active-only').prop('checked')
                    };
                },
                "dataSrc": function (result) {
                    clearSearchLoading();
                    if (result.success) {
                        return result.data;
                    } else {
                        toastr.error(result.error, '', Jpfc.Toastr.config);
                        return [];
                    }
                }
            },
            "columns": [
                {
                    data: "title"
                },
                {
                    data: "location"
                },
                {
                    data: "startDateStr"
                },
                {
                    data: "type"
                },
                {
                    data: "isDraft"
                },
                {
                    data: "isClose"
                },
                {
                    render: function (data, type, row) {
                        var html = '<a class="btn btn-primary" title="Edit Job" href="/Career/EditJob/' + row.jobPostId + '"><i class="fa fa-pencil text-white"></i></a>' +
                            ' <a class="btn btn-danger" title = "Delete Job"  onclick="Jpfc.CareerList.deleteJob(' + row.jobPostId + ')"><i class="fa fa-trash text-white"></i></a>';
                        return html;
                    }
                }
            ]
        });
    };

    var deleteJob = function (id) {
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
                    "url": "/Career/DeleteJob",
                    "method": "post",
                    "data": {
                        id: id
                    }
                }).done(function (result) {
                    if (result.success) {
                        table.ajax.reload(null, false);
                        toastr.success('Job deleted successfully', '', Jpfc.Toastr.config);
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
        $('#cb-active-only').on('change', function () {
            table.ajax.reload();
        });
    };

    var init = function () {
        initDataTable();
        bindEvents();
    };

    return {
        init: init,
        deleteJob: deleteJob
    };
}();