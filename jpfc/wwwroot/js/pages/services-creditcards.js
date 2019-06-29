var Jpfc = Jpfc || {};

Jpfc.ServicesCreditCards = function () {
    var initFancyBox = function () {
        $('a.image').fancybox();
    };

    var init = function () {
        initFancyBox();
    };

    return {
        init: init
    };
}();