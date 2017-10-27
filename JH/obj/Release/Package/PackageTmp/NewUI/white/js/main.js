/**
 * Created by Administrator on 2016/4/21.
 */
(function(){

    function menuSpreadHandler(){
        //点击一级菜单;
        $('.oneMenuTitle').bind('click', function(e) {
            var node = $(this).next();
            $(this).parent().siblings().find('.oneMenuTitle').next().attr('data-state', 0);
            $(this).parent().siblings().find('.oneMenuTitle').next().slideUp();
            $(this).parent().siblings().find('.oneMenuTitle').removeClass('close-menu');

            $(this).parent().siblings().find('.towMenuTitle').next().attr('data-state', 0);
            $(this).parent().siblings().find('.towMenuTitle').next().slideUp();
            $(this).parent().siblings().find('.towMenuTitle').removeClass('towMenuTitle-hover');

            $(this).parent().siblings().find('.threeMenuTitle').removeClass('threeMenuTitle-hover');
            if(node.attr('data-state') == 0) {
                node.slideDown();
                node.attr('data-state', 1);
                $(this).addClass('close-menu');
            }else {
                node.slideUp();
                node.attr('data-state', 0);
                $(this).removeClass('close-menu');
            }
        });

        //点击二级菜单;
        $('.towMenuTitle').bind('click', function() {
            var node = $(this).next();
            $(this).parent().siblings().find('.towMenuTitle').next().attr('data-state', 0);
            $(this).parent().siblings().find('.towMenuTitle').next().slideUp();
            $(this).parent().siblings().find('.towMenuTitle').removeClass('towMenuTitle-hover');
            if(node.attr('data-state') == 0) {
                node.slideDown();
                node.attr('data-state', 1);
                $(this).addClass('towMenuTitle-hover');
            }else {
                node.slideUp();
                node.attr('data-state', 0);
                $(this).removeClass('towMenuTitle-hover');
            }
        });

        //三级菜单选中的状态;
        $('.threeMenuTitle').bind('click',function() {
            $(this).parent().siblings().find('.threeMenuTitle').removeClass('threeMenuTitle-hover');
            $(this).addClass('threeMenuTitle-hover');
        });
    }
    menuSpreadHandler();

    function menuShrinkHandler(){
        $('.treeview').bind('mouseenter', function(e) {
            var node = $(this).find('.oneMenuTitle').next();
            $(this).siblings().find('.oneMenuTitle').next().attr('data-state', 0);
            $(this).siblings().find('.oneMenuTitle').next().slideUp();
            $(this).siblings().find('.oneMenuTitle').removeClass('close-menu');
            node.slideDown();
            node.attr('data-state', 1);
            $(this).find('.oneMenuTitle').addClass('close-menu');
        });
        $('.treeview').bind('mouseleave', function(e) {
            var node = $(this).find('.oneMenuTitle').next();
            node.slideUp();
            node.attr('data-state', 0);
            $(this).find('.oneMenuTitle').removeClass('close-menu');
        });
    }


    //左边收起或者展开;
    $('#sidebar-toggle').bind('click', function() {
        var state = $('#left-sidebar-warp').attr('data-state');
        if(state == 0){
            $('#left-sidebar-warp').attr('data-state', '1');
            $('#content-wrapper').css('margin-left', '284px');
            $('#left-sidebar-warp').addClass('spread');
            $('#left-sidebar-warp').removeClass('shrink');
            $('.oneMenuTitle, .towMenuTitle, .threeMenuTitle').unbind('click');
            menuSpreadHandler();
            $('.treeview').unbind("mouseenter").unbind("mouseleave");
        }else {
            $('#left-sidebar-warp').attr('data-state', '0');
            $('#content-wrapper').css('margin-left', '50px');
            $('#left-sidebar-warp').removeClass('spread');
            $('#left-sidebar-warp').addClass('shrink');
            $('.shrink .towMenuTitle').removeClass('towMenuTitle-hover');
            $('.shrink .threeMenuTitle').removeClass('threeMenuTitle-hover');
            $('.shrink .towMenuTitle').next().attr('data-state', 0);
            $('.shrink .towMenuTitle').next().attr('style','');
            $('.shrink .oneMenuTitle').next().attr('close-menu', 0);
            $('.shrink .oneMenuTitle').next().slideUp();
            $('.oneMenuTitle, .towMenuTitle, .threeMenuTitle').unbind('click');
            menuShrinkHandler();
        }
    });

    //点左右的图片展开左边菜单;
    $('.menu-icon').bind('click', function() {

    });

    //工具栏上的几个下拉框;
    $('.toolbar-title').bind('click', function() {
        $(this).parent().siblings().find('.toolbar-title').next().hide();
        $(this).next().show();
    });

    //点击切换了皮肤后，或者点击了admin下拉里面的选项后，隐藏当前下拉框;
    $('.skin-slide a, #sign-slide a').bind('click', function() {
        $(this).parent().hide();
    });

    $(document).bind('click', function(e) {
        var sign = $('.toolbar').children();
        var f = false;
        for(var i = 0; i < sign.length ; i++) {
            if(!(e.target == sign[0] || $.contains(sign[0], e.target))) {
                f = true;
            }else {
                f = false;
            }
        }
        if(f){
            $('.toolbar-title').next().hide();
        }
    });
})();