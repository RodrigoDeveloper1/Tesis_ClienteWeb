$(document).ready(function () {
    $(".buzon-enviados").click(function () {
        $('#table-notificaciones').find('tbody').remove();

        showProgress();
        var lista = "";

        $.ajax({
            type: "POST",
            url: "/Notificaciones/NotificacionesEnviadas",
            success: function (data) {
                if (data[0].Success) {
                    for (var i = 0; i < data.length; i++) {
                        lista += (
                            "<tr>" +
                                "<td class=''></td>" +
                                "<td class='mailbox-name' style='font-style: italic'>" + data[i].From + "</td>" +
                                "<td class='mailbox-subject'>" +
                                    "<b>" + data[i].Attribution + "</b> - " +
                                    data[i].Notification +
                                "</td>" +
                                "<td class='mailbox-attachment'></td>" +
                                "<td class='mailbox-date'>" + data[i].DateOfCreation + "</td>" +
                            "</tr>"
                            );
                    }

                    $('#table-notificaciones').find('tbody').end().append(lista);
                    hideProgress();
                }
                else {
                    hideProgress();

                    swal('¡No hay notificaciones!', 'El usuario aún no ha enviado notificaciones', 'info');
                    $('#table-notificaciones').find('tbody').remove();
                }
            },
            error: function () {
                hideProgress();

                swal({
                    title: " ¡Error!",
                    text: "Ha ocurrido un error con la carga de notificaciones enviadas.",
                    type: "error",
                    closeOnConfirm: true,
                }, function (isConfirm) {
                    if (isConfirm) {
                        showProgress();

                        window.location.href = 'Buzon';
                    }
                });
            }
        });
    });

    $(".btn-refresh").click(function () {
        $('#table-notificaciones').find('tbody').remove();

        showProgress();
        var lista = "";

        $.ajax({
            type: "POST",
            url: "/Notificaciones/NotificacionesRecibidas",
            success: function (data) {
                if (data[0].Success) {
                    for (var i = 0; i < data.length; i++) {

                        lista += (
                            "<tr>" +
                                "<td class=''></td>" +
                                "<td class='mailbox-name' style='font-style: italic'>" + data[i].From + "</td>" +
                                "<td class='mailbox-subject'>" +
                                    "<b>" + data[i].Attribution + "</b> - " +
                                    data[i].Notification +
                                "</td>" +
                                "<td class='mailbox-attachment'></td>" +
                                "<td class='mailbox-date'>" + data[i].DateOfCreation + "</td>" +
                            "</tr>"
                            );
                    }

                    $('#table-notificaciones').find('tbody').end().append(lista);
                    hideProgress();
                }
            },
            error: function () {
                hideProgress();

                swal({
                    title: " ¡Error!",
                    text: "Ha ocurrido un error con la carga de notificaciones enviadas.",
                    type: "error",
                    closeOnConfirm: true,
                }, function (isConfirm) {
                    if (isConfirm) {
                        showProgress();

                        window.location.href = 'Buzon';
                    }
                });
            }
        });
    });
});