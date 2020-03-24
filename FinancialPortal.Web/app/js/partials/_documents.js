$('#filter-letters-link').on('click', function () {
    var filtersLetters = $('#filter-letters');
    var search = $('#search');
    var link = $('#filter-link');

    if (filtersLetters.hasClass('hide-filter-letters') && search.hasClass('hide-filter-letters')) {
        filtersLetters.removeClass('hide-filter-letters');
        search.removeClass('hide-filter-letters');
        link.removeClass('jw-icon-arrow')
        link.addClass('jw-icon-arrow-down');
    } else {
        filtersLetters.addClass('hide-filter-letters');
        search.addClass('hide-filter-letters');
        link.removeClass('jw-icon-arrow-down');
        link.addClass('jw-icon-arrow');
    }
});

$('#change-account').on('click', function () {
    openNav();
});

var close = $('.closebtn').on('click', function (e) {
    e.preventDefault();
    closeNav();
});

function openNav() {
    $("#accountsPanel").css('width', "350px");
}

function closeNav() {
    $("#accountsPanel").css('width', "0");
}

$(document).ready(function () {
    $('.btn-view-document').on('click', function () {
        var row = $(this).parent().parent();

        var readDateColContent = $(row).find('.documents-read-content').text();

        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        var today = dd + '/' + mm + '/' + yyyy;

        if (readDateColContent === null || readDateColContent === undefined || readDateColContent === '' || readDateColContent == 'Read: ') {
            $(row).find('.documents-read-content-td').text(today);
            $(row).find('.documents-read-content-card').text('Read: ' + today);

            $(row).find("td").each(function () {
                $(this).removeClass("bold");
            });

            $(row).find("i").each(function () {
                $(this).removeClass("fontawesome-icon-envelope unread-document");
                $(this).addClass("fontawesome-icon-envelope-open read-document");
            });

            $(row).find("a").each(function () {
                $(this).removeClass("bold");
            });

            var cardRow = $(row).find('.documents-read-content-card').parent().parent();

            $(cardRow).find(".documents-card-subject").each(function () {
                $(this).removeClass("documents-card-unread");
                $(this).addClass("documents-card-read");
            });

            $(cardRow).find(".documents-card-date").each(function () {
                $(this).removeClass("documents-card-unread");
                $(this).addClass("documents-card-read");
            });

            var cardRrowIcon = $(cardRow).siblings();

            $(cardRrowIcon).find(".card-icon").each(function () {
                $(this).removeClass("fontawesome-icon-envelope unread-document");
                $(this).addClass("fontawesome-icon-envelope-open read-document");
            });
        }
    })
})