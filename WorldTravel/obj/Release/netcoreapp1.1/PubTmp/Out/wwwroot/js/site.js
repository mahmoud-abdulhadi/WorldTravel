var $sidebar = $('#sidebar');
var $wrapper = $('#wrapper'); 

var $icon = $('#menuToggler i.fa'); 

$('#menuToggler').on('click', function () {

    $sidebar.toggleClass("hide-sidebar");
    $wrapper.toggleClass('hide-sidebar');

    if ($sidebar.hasClass('hide-sidebar')) {

        $icon.removeClass('fa-angle-left');
        $icon.addClass('fa-angle-right');

    } else {

        $icon.removeClass('fa-angle-right');
        $icon.addClass('fa-angle-left');

    }


});

