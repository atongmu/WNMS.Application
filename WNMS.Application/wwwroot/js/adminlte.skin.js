﻿/**
 * AdminLTE Demo Menu
 * ------------------
 * Skins pannel
 */
(function ($, AdminLTE) {

    "use strict";

    /**
     * List of all the available skins
     *
     * @type Array
     */
    var my_skins = [
        "skin-black",
        "skin-blue",
        "skin-red",
        "skin-yellow",
        "skin-purple",
        "skin-green",
       
    ];

    //Create the new tab
    var tab_pane = $("<div />", {
        "id": "control-sidebar-theme-demo-options-tab",
        "class": "tab-pane active"
    });

    ////Create the tab button
    //var tab_button = $("<li />", { "class": "active" })
    //    .html("<a href='#control-sidebar-theme-demo-options-tab' data-toggle='tab'>"
    //        + "<i class='fa fa-wrench'></i>"
    //        + "</a>");

    ////Add the tab button to the right sidebar tabs
    //$("[href='#control-sidebar-home-tab']")
    //    .parent()
    //    .before(tab_button);

    //Create the menu
    var demo_settings = $("<div />");
    
    var skins_list = $("<ul />", { "class": 'list-unstyled clearfix' });

    //Dark sidebar skins
    var skin_black =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-black' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div style='box-shadow: 0 0 2px rgba(0,0,0,0.1)' class='clearfix'><span style='display:block; width: 20%; float: left; height: 7px; background: #fefefe;'></span><span style='display:block; width: 80%; float: left; height: 7px; background: #fefefe;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>白灰</p>");
    skins_list.append(skin_black);
    var skin_blue =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-blue' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div><span style='display:block; width: 20%; float: left; height: 7px; background: #367fa9;'></span><span class='bg-light-blue' style='display:block; width: 80%; float: left; height: 7px;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222d32;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>蓝灰</p>");
    skins_list.append(skin_blue);
    
    var skin_purple =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-purple' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div><span style='display:block; width: 20%; float: left; height: 7px;' class='bg-purple-active'></span><span class='bg-purple' style='display:block; width: 80%; float: left; height: 7px;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222d32;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>紫灰</p>");
    skins_list.append(skin_purple);
    var skin_green =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-green' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div><span style='display:block; width: 20%; float: left; height: 7px;' class='bg-green-active'></span><span class='bg-green' style='display:block; width: 80%; float: left; height: 7px;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222d32;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>绿灰</p>");
    skins_list.append(skin_green);
    var skin_red =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-red' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div><span style='display:block; width: 20%; float: left; height: 7px;' class='bg-red-active'></span><span class='bg-red' style='display:block; width: 80%; float: left; height: 7px;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222d32;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>红灰</p>");
    skins_list.append(skin_red);
    var skin_yellow =
        $("<li />", { style: "float:left; width: 33.33333%; padding: 5px;" })
            .append("<a data-skin='skin-yellow' style='display: block; box-shadow: 0 0 3px rgba(0,0,0,0.4)' class='clearfix full-opacity-hover'>"
                + "<div><span style='display:block; width: 20%; float: left; height: 7px;' class='bg-yellow-active'></span><span class='bg-yellow' style='display:block; width: 80%; float: left; height: 7px;'></span></div>"
                + "<div><span style='display:block; width: 20%; float: left; height: 20px; background: #222d32;'></span><span style='display:block; width: 80%; float: left; height: 20px; background: #f4f5f7;'></span></div>"
                + "</a>"
                + "<p class='text-center no-margin'>黄灰</p>");
    skins_list.append(skin_yellow);

    

    demo_settings.append("<h4 class='control-sidebar-heading'>皮肤</h4>");
    demo_settings.append(skins_list);

    tab_pane.append(demo_settings);
    $("#ctrlbar-content").append(tab_pane);

    setup();

    /**
     * Toggles layout classes
     *
     * @param String cls the layout class to toggle
     */
    function change_layout(cls) {
        $("body").toggleClass(cls);
        AdminLTE.layout.fixSidebar();
        //Fix the problem with right sidebar and layout boxed
        if (cls === "layout-boxed")
            AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
        if ($('body').hasClass('fixed') && cls === 'fixed') {
            AdminLTE.pushMenu.expandOnHover();
            AdminLTE.layout.activate();
        }
        AdminLTE.controlSidebar._fix($(".control-sidebar-bg"));
        AdminLTE.controlSidebar._fix($(".control-sidebar"));
    }

    /**
     * Replaces the old skin with the new skin
     * @param String cls the new skin class
     * @returns Boolean false to prevent link's default action
     */
    function change_skin(cls) {
        $.each(my_skins, function (i) {
            $("body").removeClass(my_skins[i]);
        });

        $("body").addClass(cls);
        store('skin', cls);
        return false;
    }

    /**
     * Store a new settings in the browser
     *
     * @param String name Name of the setting
     * @param String val Value of the setting
     * @returns void
     */
    function store(name, val) {
        if (typeof (Storage) !== "undefined") {
            localStorage.setItem(name, val);
        } else {
            window.alert('Please use a modern browser to properly view this template!');
        }
    }

    /**
     * Get a prestored setting
     *
     * @param String name Name of of the setting
     * @returns String The value of the setting | null
     */
    function get(name) {
        if (typeof (Storage) !== "undefined") {
            return localStorage.getItem(name);
        } else {
            window.alert('Please use a modern browser to properly view this template!');
        }
    }

    /**
     * Retrieve default settings and apply them to the template
     */
    function setup() {
        var tmp = get('skin');
        if (tmp && $.inArray(tmp, my_skins))
            change_skin(tmp);

        //Add the change skin listener
        $("[data-skin]").on('click', function (e) {
            if ($(this).hasClass('knob'))
                return;
            e.preventDefault();
            change_skin($(this).data('skin'));
        });

        //Add the layout manager
        $("[data-layout]").on('click', function () {
            change_layout($(this).data('layout'));
        });

        $("[data-controlsidebar]").on('click', function () {
            change_layout($(this).data('controlsidebar'));
            var slide = !AdminLTE.options.controlSidebarOptions.slide;
            AdminLTE.options.controlSidebarOptions.slide = slide;
            if (!slide)
                $('.control-sidebar').removeClass('control-sidebar-open');
        });

        $("[data-sidebarskin='toggle']").on('click', function () {
            var sidebar = $(".control-sidebar");
            if (sidebar.hasClass("control-sidebar-dark")) {
                sidebar.removeClass("control-sidebar-dark");
                sidebar.addClass("control-sidebar-light");
            } else {
                sidebar.removeClass("control-sidebar-light");
                sidebar.addClass("control-sidebar-dark");
            }
        });

        $("[data-enable='expandOnHover']").on('click', function () {
            $(this).attr('disabled', true);
            AdminLTE.pushMenu.expandOnHover();
            if (!$('body').hasClass('sidebar-collapse'))
                $("[data-layout='sidebar-collapse']").click();
        });

    }
})(jQuery, $.AdminLTE);
