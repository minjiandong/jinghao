(function ($) {
    $(document).ready(function () {
        $('.styleswitch').click(function () {
            switchStylestyle(this.getAttribute("rel"));
            return false;
        });
        var c = readCookie('style');
        if (c) switchStylestyle(c);

        // Switcher
        jQuery('.demo_changer .demo-icon').click(function () {

            if (jQuery('.demo_changer').hasClass("active")) {
                jQuery('.demo_changer').animate({ "right": "0px" }, function () {
                    jQuery('.demo_changer').toggleClass("active");
                });
            } else {
                jQuery('.demo_changer').animate({ "right": "-145px" }, function () {
                    jQuery('.demo_changer').toggleClass("active");
                });
            }
        });

    });

    function switchStylestyle(styleName) {
        $('link[rel*=style][title]').each(function (i) {
            this.disabled = true;
            if (this.getAttribute('title') == styleName) this.disabled = false;
        });
        createCookie('style', styleName, 365);
    }
})(jQuery);

// Cookie functions
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}
function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function eraseCookie(name) {
    createCookie(name, "", -1);
}



//jQuery('.left_nav .icon-list').click(function (e) {
//    var distance = jQuery('.wrapper').css('left');
//    var elm_class = jQuery(".icon-list").attr("class");

//    if (jQuery('.left_nav').hasClass("active")) {
//        jQuery('.left_nav').animate({ "left": "255px" }, function () {
//            jQuery('.left_nav').toggleClass("active");

//            jQuery(".icon-list").removeClass("open");
//            jQuery('.left-nav').animate({ width: 'toggle' });

//        });
//    } else {
//        jQuery('.left_nav').animate({ "left": "5px" }, function () {

//            jQuery(this).addClass("open");
//            jQuery('.left-nav').animate({ width: 'toggle' });

//            jQuery('.left_nav').toggleClass("active");
//        });
//    }
//});


 