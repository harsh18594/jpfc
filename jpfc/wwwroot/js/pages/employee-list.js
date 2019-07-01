var Jpfc = Jpfc || {};

Jpfc.EmployeeList = function () {

    var initDatatable = function () {
        $('#employee-table').DataTable({
            "order": [0, "desc"],
            "columnDefs": [{
                "targets": [-1],
                "orderable": false
            }]
        });
    };

    var deleteEmployee = function ($this) {
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
                var href = $this.attr('href');
                window.location = href;
            }
        });
    };

    var bindEvents = function () {
        $('#employee-table').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            deleteEmployee($(this));
        });
    };

    var init = function () {
        initDatatable();
        bindEvents();
    };

    return {
        init: init
    };
}();