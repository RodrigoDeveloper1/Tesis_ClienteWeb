var idColegio;
var idAnoEscolarActivo;

function MensajeNotificacion() {
    mensajeNotificacion = $(this).find("span").html();

    $('#p-mensaje-notificacion').html(mensajeNotificacion);
    $('#div-cuerpo-mensaje-notificacion p').css("display", "block");

    DialogoMensajeNotificaction();
}

//Diálogo mensaje de la notificación en particular
function DialogoMensajeNotificaction() {
    $("#dialog-mensaje").dialog({
        draggable: false,
        height: 220,
        hide: "explode",
        modal: true,
        resizable: false,
        show: "puff",
        title: "Mensaje de la notificación",
        width: 400,
        buttons: {
            "Cerrar": function () {
                $(this).dialog("close");
                $(this).dialog("destroy");
            }
        }
    });
}

$(document).ready(function () {
    //Obtener el año escolar y la lista de cursos de ese año escolar
    $("#select-colegio").change(function () {
        idColegio = $(this).val();

        $("#ano-escolar").val("Cargando el año escolar");
        $("#select-curso").find('option').remove().end().append('<option>Cargando la lista de cursos...</option>');
        $("#select-curso").selectpicker("refresh");

        if (idColegio != "") {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerAnoEscolarActivoEnLabel",
                data: {
                    idColegio: idColegio
                },
                success: function (data) {
                    if (data[0].success) {
                        $("#ano-escolar").val(data[0].label);
                        idAnoEscolarActivo = data[0].idAnoEscolar;

                        //Sección de llenado de la tabla con las notificaciones automáticas de ese colegio.
                        $.ajax({
                            type: "POST",
                            url: "/Bridge/ObtenerJsonNotificacionesaAutomaticasPorColegio",
                            data: {
                                idColegio: idColegio,
                                idAnoEscolar: idAnoEscolarActivo
                            },
                            success: function (data2) {
                                var lista = "";

                                if (data2[0].success) {
                                    //Borrando la tabla de notificaciones
                                    $("#table-lista-notificaciones-automaticas tbody tr").remove();

                                    for (var i = 0; i < data2.length; i++) {
                                        lista += (
                                            '<tr id=' + data2[i].idNotification + ' >' +
                                                '<td class="td-tipoalerta">' + data2[i].tipoAlerta + '</td>' +
                                                '<td class="td-atribucion">' + data2[i].atribucion + '</td>' +
                                                '<td class="td-fechaenvio">' + data2[i].fechaEnvio + '</td>' +
                                                '<td class="td-mensaje centrar">' +
                                                    '<button type="button" class="btn btn-primary mensaje-notif">' +
                                                        'Ver mensaje' +
                                                        '<span class="hidden span-mensaje">' +
                                                            data2[i].mensaje +
                                                        '</span>' +
                                                    '</button>' +
                                                '</td>' +
                                            '</tr>');
                                    }

                                    $('#table-lista-notificaciones-automaticas').find('tbody').end().append(lista);

                                    //Para visualizar el mensaje de la notificación
                                    $(".mensaje-notif").bind("click", MensajeNotificacion);
                                }
                                else {
                                    swal("¡No hay notificaciones!",
                                         "Este colegio no tiene ninguna notificación asociada.",
                                         "warning");

                                    //Borrando la tabla de notificaciones
                                    $("#table-lista-notificaciones-automaticas tbody tr").remove();
                                }
                            }
                        });
                    }
                    else
                        $("#ano-escolar").val("No posee año escolar activo");                        
                }
            });

            hideProgress();
        }
        else {            
            //Borrando el text-box del año escolar
            $("#ano-escolar").val("");            
            //Borrando la tabla de notificaciones
            $("#table-lista-notificaciones-automaticas tbody tr").remove();
        }
    });    
});