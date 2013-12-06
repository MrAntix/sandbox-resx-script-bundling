;
(function ($, resources) {

    $(function() {

        var $el = $("<em>")
            .text(" - " + resources.app.HelloWorld)
            .appendTo("h3:first");

        var showNHide  = function() {
            $el
                .fadeOut("slow")
                .fadeIn({duration:"slow",complete:showNHide});
        };

        showNHide();

    });

})(jQuery, window.resources);