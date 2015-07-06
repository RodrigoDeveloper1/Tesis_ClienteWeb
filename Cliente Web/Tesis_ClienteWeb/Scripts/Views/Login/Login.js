$(document).ready(function () {
    $("#download-app-mobile-btn").click(function () {
        $.ajax({
            type: "POST",
            datatype: "json",
            url: "/Login/DownloadAPK",
            success: function (data) { 
                window.location = '/Login/DownloadAPK_Response/?path=' + data[0].path;
            },
            error: function (data) {                
            }
        })
    });
});