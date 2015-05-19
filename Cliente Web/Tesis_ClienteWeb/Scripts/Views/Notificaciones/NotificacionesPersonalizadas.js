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

                        //Sección de llenado de la tabla con las notificaciones personzalidas de ese colegio.
                        $.ajax({
                            type: "POST",
                            url: "/Bridge/ObtenerJsonNotificacionesPersonalizadasPorColegio",
                            data: {
                                idColegio: idColegio,
                                idAnoEscolar: idAnoEscolarActivo
                            },
                            success: function (data3) {
                                var lista = "";

                                if (data3[0].success) {
                                    //Borrando la tabla de notificaciones
                                    $("#table-lista-notificaciones-personalizadas tbody tr").remove();

                                    for (var i = 0; i < data3.length; i++) {
                                        lista += (
                                            '<tr id=' + data3[i].idNotification + ' >' +
                                                '<td class="td-tipoalerta">' + data3[i].tipoAlerta + '</td>' +
                                                '<td class="td-atribucion">' + data3[i].atribucion + '</td>' +
                                                '<td class="td-sujeto">' + data3[i].sujeto + '</td>' +                                            
                                                '<td class="td-enviadopor">' + data3[i].nombreUsuario + '</td>' +
                                                '<td class="td-curso">' + data3[i].curso + '</td>' +
                                                '<td class="td-fechaenvio">' + data3[i].fechaEnvio + '</td>' +
                                                '<td class="td-mensaje centrar">' +
                                                    '<button type="button" class="btn btn-primary mensaje-notif">' +
                                                        'Ver mensaje' +
                                                        '<span class="hidden span-mensaje">' +
                                                            data3[i].mensaje +
                                                        '</span>' +
                                                    '</button>' +
                                                '</td>' +
                                            '</tr>');
                                    }

                                    $('#table-lista-notificaciones-personalizadas').find('tbody').end().append(lista);

                                    //Para visualizar el mensaje de la notificación
                                    $(".mensaje-notif").bind("click", MensajeNotificacion);

                                    hideProgress();
                                }
                                else {
                                    swal("¡No hay notificaciones!", "No hay notificaciones asociadas a este colegio", "warning");
                                    //Borrando la tabla de notificaciones
                                    $("#table-lista-notificaciones-personalizadas tbody tr").remove();

                                    hideProgress();
                                }
                            }
                        });
                    }
                    else {                        
                        $("#ano-escolar").val("No posee año escolar activo");

                        hideProgress();
                    }
                }
            });
        }
        else {            
            //Borrando el text-box del año escolar
            $("#ano-escolar").val("");            
            //Borrando la tabla de notificaciones
            $("#table-lista-notificaciones-personalizadas tbody tr").remove();
        }       
    });    
});