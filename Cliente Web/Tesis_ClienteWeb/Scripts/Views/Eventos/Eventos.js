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
                                closeOnConfirm: true,
                                closeOnCancel: false
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    showProgress();

                                    calendar.fullCalendar('removeEvents', calEvent.id);

                                    $.ajax({
                                        type: "POST",
                                        url: "/Eventos/EliminarEvento",
                                        data: {
                                            id: calEvent.id
                                        },
                                        success: function (data) {
                                            hideProgress();

                                            swal("¡Borrado!", "Su evento ha sido borrado.", "success");
                                        },
                                        error: function (data) {
                                            hideProgress();

                                            swal({
                                                title: "Error",
                                                text: "Ha ocurrido un error que ha impedido que se borre" +
                                                    " el evento.",
                                                type: "error",
                                                showCancelButton: false,
                                                closeOnConfirm: true,
                                            },
                                            function (isConfirm) {
                                                if (isConfirm) {
                                                    showProgress();
                                                    window.location.href = 'CalendarioEventos';
                                                }
                                            });
                                        }
                                    });
                                }
                                else {
                                    swal("¡Cancelado!", "Su evento está a salvo :)", "error");
                                }
                            });
                        }
                    }
                });
            }
            else {
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
                    }
                    else {
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