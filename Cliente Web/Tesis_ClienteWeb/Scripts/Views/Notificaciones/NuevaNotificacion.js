var sujeto = "";
var idCurso = 0;
var elSujeto = "";
var tipoNotificacion = "";
var mensaje = "";
var atribucion = "";

//Variable que define el comportamiento del diálogo cuando se cierra
var refresh = false;

function DialogoNuevaNotificacion() {
    $("#dialog-nueva-notificacion").dialog({
        draggable: false,
        dialogClass: "no-close",
        height: 660,
        hide: "explode",
        modal: true,
        position: {
            my: "center",
            at: "center",
            of: window
        },
        resizable: false,
        show: "puff",
        title: "Enviar una nueva notificación",
        width: 470,
        buttons: {
            "Enviar": function (e) {
                mensaje = $("#text-area-notificacion").val();

                if (elSujeto == "")
                    swal("¡No hay sujeto!", "Por favor seleccione el sujeto a quien va dirigida la " +
                        "notificación.", "warning");
                else if (tipoNotificacion == "")
                    swal("¿Y el tipo de notificación?", "Por favor seleccione el tipo de notificación",
                        "warning");
                else if (mensaje == "") {
                    swal("¡No ha escrito un mensaje!", "Por favor escriba un mensaje para ser enviada la" +
                        " notificación.", "warning");
                }
                else {
                    swal({
                        title: "¿Estás seguro?",
                        text: "¿Revisaste bien los datos de la notificación antes de enviarla?",
                        type: "info",
                        showCancelButton: true,
                        confirmButtonColor: "#1A2AE0", //Azul
                        confirmButtonText: "¡Enviar!",
                        cancelButtonText: "¡No, cancélalo!",
                        closeOnConfirm: true,
                        closeOnCancel: true
                    },
                    function (isConfirm) {
                        if (isConfirm) {
                            showProgress();

                            //Validación tipo de sujeto: Todos los representantes
                            if (elSujeto == "Todos los representantes")
                            {
                                sujeto = "Curso";
                                elSujeto = idCurso;
                            }

                            $.ajax({
                                type: "POST",
                                url: "/Notificaciones/EnviarNotificacion",
                                data: {
                                    idSujeto: elSujeto,
                                    tipoSujeto: sujeto,
                                    mensajeNotificacion: mensaje,
                                    tipoNotificacion: tipoNotificacion,
                                    atribucion: atribucion
                                },
                                success: function (enviado) {
                                    if(enviado[0].success) {
                                        swal({
                                            title: "¡Enviada!",
                                            text: "La notificación se ha enviado satisfactoriamente",
                                            type: "success",
                                            confirmButtonColor: "green",
                                            showCancelButton: true,
                                            closeOnConfirm: true,
                                        },
                                        function (isConfirm) {
                                            if (isConfirm) 
                                                window.location.href = 'NotificacionesPersonalizadas';
                                        });
                                    }
                                    else {
                                        swal({
                                            title: "¡Error!",
                                            text: "Ha ocurrido un error, la notificación no se pudo enviar",
                                            type: "error",
                                            confirmButtonColor: "red",
                                            showCancelButton: false,
                                            closeOnConfirm: true,
                                        },
                                        function (isConfirm) {
                                            if (isConfirm) 
                                                window.location.href = 'NotificacionesPersonalizadas';
                                        });
                                    }
                                },
                            });
                        }
                    });
                }
            },
            "Cerrar": function () {
                refresh = false;
                $(this).dialog("close");
                $(this).dialog("destroy");
            }
        },
        close: function () {
            if (refresh) //refresh = Variable booleana
                window.location.href = 'Buzon';
        }
    });
}

$(document).ready(function () {
    //Para abrir el diálogo de nueva notificación
    $("#btn-nueva-notificacion").click(function () {
        DialogoNuevaNotificacion();
    });

    /*Propiedades de los elementos del diálogo*/
    $("#select-sujeto").change(function () {
        sujeto = $(this).val();

        elSujeto = ""; //Limpiando el sujeto
        tipoNotificacion = ""; //Limpiando el tipo de notificación

        if (sujeto == "Representante") {
            //Habilitando la lista de cursos
            $("#select-cursos").prop("disabled", false);
            $("#select-cursos").selectpicker("refresh");
            
            //Deshabilitando la lista de sujetos a escoger
            $("#select-el-sujeto").prop("disabled", true);
            $("#select-el-sujeto").selectpicker("refresh");
            
            //Limpiando la variable idCurso
            idCurso = "";

            //Carga del tipo de notificación
            $.ajax({
                type: "POST",
                url: "/Bridge/ListaTiposNotificacion_CoordinadorRepresentante",
                success: function (data) {
                    var lista = '<option value="">Seleccione el tipo de notificación...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].tipo + '">' + data[i].tipo + '</option>');
                    }

                    $("#select-tipo-notificacion").find('option').remove().end().append(lista);
                    $("#select-tipo-notificacion").prop("disabled", false);
                    $("#select-tipo-notificacion").selectpicker("refresh");
                }
            });
        }
        else if (sujeto == "Usuario") {
            //Deshabilitando la lista de cursos
            $("#select-cursos").prop("disabled", true);
            $("#select-cursos").selectpicker("refresh");

            //Habilitando la lista de sujetos a escoger
            $("#select-el-sujeto").find('option').remove().end().append("<option>Cargando docentes...</option>");
            $("#select-el-sujeto").prop("disabled", false);
            $("#select-el-sujeto").selectpicker("refresh");

            //Limpiando la variable idCurso
            idCurso = "";

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerSujetosDeUsuario",
                success: function (data) {
                    if (data[0].success) {
                        var lista = '<option value="">Seleccione el docente...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].idDocente + '">' +
                                data[i].nombre + '</option>');
                        }

                        $("#select-el-sujeto").find('option').remove().end().append(lista);
                        $("#select-el-sujeto").selectpicker("refresh");
                    }
                    else {
                        $("#select-el-sujeto").find('option').remove().end()
                            .append('<option>No existen docentes asociados</option>');
                        $("#select-el-sujeto").selectpicker("refresh");

                        swal("¡No hay docentes!", "No existen docentes asociados");
                    }
                }
            });

            //Carga del tipo de notificación
            $.ajax({
                type: "POST",
                url: "/Bridge/ListaTiposNotificacion_CoordinadorDocente",
                success: function (data) {
                    var lista = '<option value="">Seleccione el tipo de notificación...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].tipo + '">' + data[i].tipo + '</option>');
                    }

                    $("#select-tipo-notificacion").find('option').remove().end().append(lista);
                    $("#select-tipo-notificacion").prop("disabled", false);
                    $("#select-tipo-notificacion").selectpicker("refresh");
                }
            });
        }        
        else if (sujeto == "") {
            //Deshabilitando la lista de cursos
            $("#select-cursos").prop("disabled", true);
            $("#select-cursos").selectpicker("refresh");

            //Deshabilitando la lista de sujetos a escoger
            $("#select-el-sujeto").prop("disabled", true);
            $("#select-el-sujeto").selectpicker("refresh");

            //Limpiando la variable idCurso
            idCurso = "";

            //Limpiando la lista de sujetos
            $("#select-el-sujeto").find('option').remove().end().append('<option>Seleccione el sujeto...</option>');
            $("#select-el-sujeto").selectpicker("refresh");

            //Deshabilitando la lista de tipos de notificación
            $("#select-tipo-notificacion").prop("disabled", true);
            $("#select-tipo-notificacion").selectpicker("refresh");
        }
    });

    $("#select-cursos").change(function () {
        idCurso = $(this).val();
        elSujeto = ""; //Limpiando el sujeto

        if (idCurso != "") {
            //Habilitando la lista de sujetos a escoger
            $("#select-el-sujeto").find('option').remove().end().append("<option>Cargando representantes...</option>");
            $("#select-el-sujeto").prop("disabled", false);
            $("#select-el-sujeto").selectpicker("refresh");

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerSujetosDeRepresentante",
                data: {
                    idCurso: idCurso
                },
                success: function (data) {
                    if (data[0].success) {
                        var lista = '<option value="">Seleccione un representante...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].idDocente + '">' +
                                data[i].nombre + '</option>');
                        }

                        $("#select-el-sujeto").find('option').remove().end().append(lista);
                        $("#select-el-sujeto").selectpicker("refresh");
                    }
                    else {
                        $("#select-el-sujeto").find('option').remove().end()
                            .append('<option>No existen representantes asociados</option>');
                        $("#select-el-sujeto").selectpicker("refresh");

                        swal("¡No hay representantes!", "No existen representantes asociados =(");
                    }
                }
            });
        }
        else {
            //Limpiando la lista de sujetos
            $("#select-el-sujeto").find('option').remove().end().append('<option>Seleccione un representante...</option>');
            $("#select-el-sujeto").selectpicker("refresh");

            //Deshabilitando la lista de sujetos a escoger
            $("#select-el-sujeto").prop("disabled", true);
            $("#select-el-sujeto").selectpicker("refresh");

            //Limpiando la variable idCurso
            idCurso = "";
        }
    });

    $("#select-el-sujeto").change(function () {
        elSujeto = $(this).val();
    });

    $("#select-tipo-notificacion").change(function () {
        tipoNotificacion = $(this).val();
    });

    $("#select-atribucion").change(function () {
        atribucion = $(this).val();
    });
    /*Fin de Propiedades de los elementos del diálogo*/
});