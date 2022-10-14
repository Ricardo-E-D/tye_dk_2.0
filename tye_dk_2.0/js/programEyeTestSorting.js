
$(window).ready(function () {

    $('#lnkSortEnable').on('click', function () {
        $('.buttons').fadeOut();
        $('.sort-hide').fadeOut();
        $('.sort-handle').fadeIn();
        $('#toolbarDefault').fadeOut(function () {
            $('#toolbarSorting').hide().removeClass('hidden').fadeIn();
        });

        $("#tblProgram").sortable({
            handle: '.sort-handle',
            items: 'tr:not(.sort-hide)',
            helper: function (event, ui) {
                var $clone = $(ui).clone();
                $clone.css('position', 'absolute');
                return $clone.get(0);
            }
        });
        $("#tblProgram").disableSelection();
    });

    $('#lnkSortCancel').on('click', function () {
        window.location.reload();
        //$('.buttons').fadeIn();
        //$('.sort-hide').fadeIn();
        //$('.sort-handle').fadeOut();
        //$('#toolbarSorting').fadeOut(function () {
        //    $('#toolbarDefault').fadeIn();
        //});
    });

    $('#lnkSortSave').on('click', function () {
        //alert("here we go again");
        setTimeout(function () {
           // $('#toolbarSorting').fadeOut();
        }, 250);

        var rows = $('#tblProgram').find('tr[data-programid]'),
            commasep = "";

        for (var i = 0, len = rows.length; i < len; i++) {
            commasep += $(rows[i]).attr('data-testid') + ",";
        }
        var url = "program.aspx?action=sort&order=" + commasep + "0&programid=" + $(rows[0]).attr('data-programid');
        //alert(url);
        window.location.href = url;

    });
});