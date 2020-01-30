var Jpfc = Jpfc || {};

Jpfc.CareerIndex = function () {
    var initPage = function () {
        $('#jobpost-container').find('ul, ol').addClass('list-style-one');
    };

    var bindEvents = function () {
        $('.btn-viewmore').on('click', function () {
            var $target = $($(this).data('target'));
            $target.toggle("slow");
        });
    };

    var init = function () {
        bindEvents();
        initPage();
    };

    return { init: init };
}();