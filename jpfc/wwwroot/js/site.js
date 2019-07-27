Jpfc = Jpfc || {};

Jpfc.Site = function () {
    var initTooltip = function () {
        $('[data-toggle="tooltip"]').tooltip();
    };

    var initMarquee = function () {
        $('.marquee').marquee({
            //duration in milliseconds of the marquee
            duration: 25000,
            //gap in pixels between the tickers
            gap: 50,
            //time in milliseconds before the marquee will start animating
            delayBeforeStart: 0,
            //'left' or 'right'
            direction: 'left',
            //true or false - should the marquee be duplicated to show an effect of continues flow
            duplicated: false
        });
    };

    var initLazy = function () {
        $('.lazy-image').lazy({
            effect: "fadeIn",
            effectTime: 500,
            threshold: 0        });
        $('.lazy-logo').lazy();
    };

    var init = function () {
        initTooltip();
        initMarquee();
        initLazy();
    };

    return {
        init: init,
        initLazy: initLazy
    };
}();