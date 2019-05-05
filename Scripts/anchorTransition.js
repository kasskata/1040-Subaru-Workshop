function AnchorAttach() {
    $(".anchor-transition").click(function () {
        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 800);
    });
}

AnchorAttach();