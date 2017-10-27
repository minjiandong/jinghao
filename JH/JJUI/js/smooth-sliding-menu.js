
$(document).ready(function () {
    $('#nav li').on('click', function (e) {
         
        jQuerythis = $(this);
        e.stopPropagation();

        if (jQuerythis.has('ul').length) {
            e.preventDefault();
            var visibleUL = $('#nav').find('ul:visible').length;
            var ele_class = $('ul', this).attr("class");
            if (ele_class != 'sub-menu opened') {
                //菜单未打开
                $('#nav').find('ul:visible').slideToggle("normal");
                $('#nav').find('ul:visible').removeClass("opened");
                $('.icon-angle-down').addClass("closing");
                $('.closing').removeClass("icon-angle-down");
                $('.closing').addClass("icon-angle-left");
                $('.icon-angle-left').removeClass("closing");
            }
            $('ul', this).slideToggle("normal");
            if (ele_class == 'sub-menu opened') {
                $('ul', this).removeClass("opened");
                $('.arrow', this).removeClass("icon-angle-down");
                $('.arrow', this).addClass("icon-angle-left");
            }
            else {
                //菜单未展开
                $('ul', this).addClass("opened");
                $('.arrow', this).removeClass("icon-angle-left");
                $('.arrow', this).addClass("icon-angle-down");
            }
        }
    });

    //显示被选中的菜单
   
    
    
    


    $(".icon-list").on("click", function (e) {
        e.preventDefault();
        var distance = $('.page-content').css('left');
        var elm_class = $(".icon-list").attr("class");
        if (elm_class == 'icon-list') {
            $(this).addClass("open");
            $('.left-nav').animate({ width: 'toggle' });
            //jQuery('.left_nav').animate({ "left": "5px" });
        } else {
            $(".icon-list").removeClass("open");
            $('.left-nav').animate({ width: 'toggle' });
            //jQuery('.left_nav').animate({ "left": "255px" });
        }
    });

   
});