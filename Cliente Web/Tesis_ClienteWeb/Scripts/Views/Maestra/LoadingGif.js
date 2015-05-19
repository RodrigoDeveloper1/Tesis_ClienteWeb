var spinnerVisible = false;

function showProgress() {
    if (!spinnerVisible) {
        $("div#loading-gif").fadeIn("fast");
        spinnerVisible = true;
    }
};

function hideProgress() {
    if (spinnerVisible) {
        var spinner = $("div#loading-gif");
        spinner.stop();
        spinner.fadeOut("fast");
        spinnerVisible = false;
    }
};