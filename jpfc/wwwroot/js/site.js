Jpfc = Jpfc || {};

Jpfc.Site = function () {
    var initTooltip = function () {
        $('[data-toggle="tooltip"]').tooltip();
    };

    var init = function () {
        console.log("Jpfc.Site init");
        initTooltip();
    };

    return {
        init: init
    };
}();