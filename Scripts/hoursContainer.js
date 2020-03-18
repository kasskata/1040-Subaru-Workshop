function manageOpenCloseSection() {

    var date = new Date();
    var dayofWeek = date.getDay();
    var closeHour = getCloseHour(dayofWeek);
    var openHour = getOpenHour(dayofWeek);
    var countdownDate = new Date();
    var isOpened;

    if (openHour <= date && date <= closeHour) {
        countdownDate = closeHour;
        isOpened = true;
    }
    else if (closeHour < date) {
        openHour.setDate(openHour.getDate() + 1);
        isOpened = false;
        countdownDate = openHour;
    }
    else {
        isOpened = false;
        countdownDate = openHour;
    }

    if (isOpened) {
        $("#open-count").show();
        $("#close-count").hide();
    } else {
        $("#close-count").show();
        $("#open-count").hide();
    }

    $(".countdown").countdown(countdownDate, function (event) {
        $(this).text(
            event.strftime('%I:%M:%S')
        );
    });


}

function getOpenHour(dayofWeek) {
    var openHour = new Date();

    if (dayofWeek === 0) {
        openHour.setDate(openHour.getDate() + 1);
    }

    openHour.setHours(10, 0, 0);
    return openHour;
}

function getCloseHour(dayofWeek) {
    var closeHour = new Date();

    if (dayofWeek >= 1 && dayofWeek <= 5) {
        closeHour.setHours(22, 0, 0);
    } else if (dayofWeek === 6) {
        closeHour.setHours(15, 0);
    } else {
        closeHour.setDate(closeHour.getDate() + 1);
        closeHour.setHours(22, 0, 0);
    }

    return closeHour;
}

manageOpenCloseSection();