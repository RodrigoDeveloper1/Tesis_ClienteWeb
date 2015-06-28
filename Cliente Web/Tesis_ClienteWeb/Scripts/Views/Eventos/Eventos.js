$(document).ready(function () {
    //Obteniendo el id del usuario
    var calendar = $('#calendar').fullCalendar({
        lang: 'es',
        defaultView: 'month',
        editable: true,
        allDayDefault: false,
        allDay: true,
        allDaySlot: false,
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        eventLimit: true, // allow "more" link when too many events
        eventClick: function (calEvent, jsEvent, view) {

            mensajeEvento = calEvent.description;
            eliminareventobool = calEvent.deleteevent;
            $('#p-mensaje-evento').html(mensajeEvento);
            $('#div-cuerpo-mensaje-evento p').css("display", "block");

            console.log(eliminareventobool);

            if (eliminareventobool == true) {

                $("#dialog-mensaje").dialog({
                    draggable: false,
                    height: 220,
                    hide: "explode",
                    modal: true,
                    resizable: false,
                    show: "puff",
                    title: "Mensaje del evento",
                    width: 400,
                    buttons: {
                        "Cerrar": function () {
                            $(this).dialog("close");
                        },
                        "Eliminar Evento": function () {
                            $(this).dialog("close");
                            swal({
                                title: "¿Estás Seguro?",
                                text: "¡No serás capaz de recuperar este evento!",
                                type: "warning", showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "¡Si, bórralo!",
                                cancelButtonText: "¡No, cancelalo!",
                                closeOnConfirm: false,
                                closeOnCancel: false
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    console.log("Entro");
                                    calendar.fullCalendar('removeEvents', calEvent.id);
                                    swal("¡Borrado!", "Su evento ha sido borrado.", "success");
                                    jQuery.post(
                                    "/Eventos/EliminarEvento"
                                    , { "id": calEvent.id }
                                ).done(function (data) {

                                    setTimeout("location.href ='/Eventos/CalendarioEventos';", 3000) /* 3 seconds */
                                });

                                } else {
                                    swal("¡Cancelado!", "Su evento está a salvo :)", "error");
                                }
                            });
                        }
                    }
                });

            } else {

                if (eliminareventobool == false) {
                    $("#dialog-mensaje").dialog({
                        draggable: false,
                        height: 220,
                        hide: "explode",
                        modal: true,
                        resizable: false,
                        show: "puff",
                        title: "Mensaje del evento",
                        width: 400,
                        buttons: {
                            "Cerrar": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            }
        }
    });

    var moment = $('#calendar').fullCalendar('getDate');
    var fecha = moment.format();

    //Carga de los eventos en el momento de renderizar la ventana
    showProgress();
    $.ajax({
        url: "/Bridge/ObtenerJsonListaEventosDeHoy",
        type: "POST",
        success: function (r) {
            if (r[0].Success) {
                for (var i = 0; i < r.length; i++) {
                    if (r[i].restadiasfechas >= 2) {
                        console.log('Entra caso I');

                        var newEvent1 = {
                            id: r[i].id,
                            title: 'Inicio - ' + r[i].name,
                            description: 'Inicio - ' + r[i].description,
                            start: r[i].startdate + 'T' + r[i].starthour,
                            end: r[i].startdate + 'T' + r[i].starthour,
                            deleteevent: r[i].deleteevent,
                            backgroundColor: '#' + r[i].color
                        };
                        $('#calendar').fullCalendar('renderEvent', newEvent1, 'stick');
                        console.log('Entra caso II');

                        var newEvent2 = {
                            id: r[i].id,
                            title: 'Fin - ' + r[i].name,
                            description: 'Fin - ' + r[i].description,
                            start: r[i].finishdate + 'T' + r[i].endhour,
                            end: r[i].finishdate + 'T' + r[i].endhour,
                            deleteevent: r[i].deleteevent,
                            backgroundColor: '#' + r[i].color
                        };
                        $('#calendar').fullCalendar('renderEvent', newEvent2, 'stick');
                    }
                    else {
                        console.log('Entra caso III');

                        var newEvent3 = {
                            id: r[i].id,
                            title: r[i].name,
                            description: r[i].description,
                            start: r[i].startdate + 'T' + r[i].starthour,
                            end: r[i].finishdate + 'T' + r[i].endhour,
                            deleteevent: r[i].deleteevent,
                            backgroundColor: '#' + r[i].color
                        };

                        $('#calendar').fullCalendar('renderEvent', newEvent3, 'stick');
                    }
                };

                hideProgress();
            }
        }
    });

    //Configuración de los componentes de tipo fecha y timepicker
    $('.datepicker').datepicker({ beforeShowDay: $.datepicker.noWeekends });
    $(".pick-a-color").pickAColor({ inlineDropdown: true });

    $('#horainicioevento').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $('#horaeventofin').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });
    //Fin de Configuración de los componentes de tipo fecha y timepicker

    //Al hacer click al botón de atrás
    $(".fc-corner-left").click(function () {
        $('#calendar').fullCalendar('removeEvents');
        moment = $('#calendar').fullCalendar('getDate');
        fecha = moment.format();

        $.ajax({
            url: "/Bridge/ObtenerJsonListaEventosPorDia",
            type: "POST",
            data: {
                fecha: fecha
            }                
        }).done(function (r) {
            for (var i = 0; i < r.length; i++) {
                if (r[i].restadiasfechas >= 2) {
                    var newEvent1 = {
                        id: r[i].id,
                        title: 'Inicio - ' + r[i].name,
                        description: 'Inicio - ' + r[i].description,
                        start: r[i].startdate + 'T' + r[i].starthour,
                        end: r[i].startdate + 'T' + r[i].starthour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };
                    $('#calendar').fullCalendar('renderEvent', newEvent1, 'stick');

                    var newEvent2 = {
                        id: r[i].id,
                        title: 'Fin - ' + r[i].name,
                        description: 'Fin - ' + r[i].description,
                        start: r[i].finishdate + 'T' + r[i].endhour,
                        end: r[i].finishdate + 'T' + r[i].endhour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };
                    $('#calendar').fullCalendar('renderEvent', newEvent2, 'stick');

                } else {
                    var newEvent3 = {
                        id: r[i].id,
                        title: r[i].name,
                        description: r[i].description,
                        start: r[i].startdate + 'T' + r[i].starthour,
                        end: r[i].finishdate + 'T' + r[i].endhour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };

                    $('#calendar').fullCalendar('renderEvent', newEvent3, 'stick');
                }
            };
        });
    });

    //Al hacer click al botón de adelante
    $(".fc-corner-right").click(function () {
        $('#calendar').fullCalendar('removeEvents');
        moment = $('#calendar').fullCalendar('getDate');
        fecha = moment.format();

        $.ajax({
            url: "/Bridge/ObtenerJsonListaEventosPorDia",
            type: "POST",
            data: {
                fecha: fecha
            }
        }).done(function (r) {
            for (var i = 0; i < r.length; i++) {
                if (r[i].restadiasfechas >= 2) {
                    var newEvent1 = {
                        id: r[i].id,
                        title: 'Inicio - ' + r[i].name,
                        description: 'Inicio - ' + r[i].description,
                        start: r[i].startdate + 'T' + r[i].starthour,
                        end: r[i].startdate + 'T' + r[i].starthour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };
                    $('#calendar').fullCalendar('renderEvent', newEvent1, 'stick');

                    var newEvent2 = {
                        id: r[i].id,
                        title: 'Fin - ' + r[i].name,
                        description: 'Fin - ' + r[i].description,
                        start: r[i].finishdate + 'T' + r[i].endhour,
                        end: r[i].finishdate + 'T' + r[i].endhour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };
                    $('#calendar').fullCalendar('renderEvent', newEvent2, 'stick');

                } else {
                    var newEvent3 = {
                        id: r[i].id,
                        title: r[i].name,
                        description: r[i].description,
                        start: r[i].startdate + 'T' + r[i].starthour,
                        end: r[i].finishdate + 'T' + r[i].endhour,
                        deleteevent: r[i].deleteevent,
                        backgroundColor: '#' + r[i].color
                    };

                    $('#calendar').fullCalendar('renderEvent', newEvent3, 'stick');
                }
            };
        });
    });
});