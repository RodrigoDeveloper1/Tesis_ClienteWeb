function DialogoNuevoEvento() {
    $("#dialog-nuevo-evento").dialog({
        draggable: false,
        dialogClass: "no-close",
        height: 560,
        hide: "explode",
        modal: true,
        resizable: false,
        show: "puff",
        title: "Crear nuevo evento",
        width: 400,
        buttons: {
            "Cargar": function () {
                var name = $("#tituloevento").val();
                var tipoevento = $("#select-seleccionartipoevento option:selected").val();
                var descripcion = $("#descripcionevento").val();
                var startdate = $("#fecha-inicio").val();
                var finishdate = $("#fecha-finalizacion").val();
                var horainicio = $("#horainicioevento").val();
                var horafin = $("#horaeventofin").val();
                var color = $("#colorevento").val();
                showProgress();

                $.ajax({
                    url: "/Eventos/CrearEventoProf",
                    type: "POST",
                    data: {
                        "Name": name,
                        "Description": descripcion,
                        "StartDate": startdate,
                        "FinishDate": finishdate,
                        "StartHour": horainicio,
                        "EndHour": horafin,
                        "Color": color,
                        "TipoEvento": tipoevento
                    },
                    success: function(data) {
                        if(data[0].Success == false) {
                            hideProgress();

                            swal({
                                title: "¡Oops!",
                                text: "Faltó un dato para la creación de la evaluación",
                                type: "warning",
                                showCancelButton: false,
                                closeOnConfirm: true,
                            },
                            function () {
                                showProgress();
                                window.location.href = 'CalendarioEventos';
                            });
                        }
                        else {
                            window.location.href = 'CalendarioEventos';
                        }
                    }
                });
            },
            "Cerrar": function () {
                $(this).dialog("close");
            }
        }
    });
}

$(document).ready(function () {
    $("#btn-crear-evento").click(function () { DialogoNuevoEvento(); });
});