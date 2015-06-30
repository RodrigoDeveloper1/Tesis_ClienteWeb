var maxCharacters = 140;
var textareaContent = "";
var caracteresTextArea = 0;
var mensajeNotificacion = "";
var refresh = true;

function DialogoNuevaNotificacion() {
    $("#dialog-nueva-notificacion").dialog({
        draggable: false,
        dialogClass: "no-close",
        height: 560,
        hide: "explode",
        modal: true,
        resizable: false,
        show: "puff",
        title: "Enviar una nueva notificación",
        width: 470,
        buttons: {
            "Enviar": function (e) {
                var Titulo = $("#titulo-notificacion").val();
                var Atribucion = $("#select-atribuciones option:selected").val();
                var TipoAlerta = $("#select-tipos-alerta option:selected").val();
                var Representante = $("#select-representantes option:selected").val();
                var Mensaje = $("#text-area-notificacion").val();

                $.ajax({
                    url: "/Notificaciones/CrearNotificacion",
                    type: "POST",
                    data: {
                        "titulo": Titulo,
                        "mensaje": Mensaje,
                        "atribucion": Atribucion,
                        "representante": Representante,
                        "tipoAlerta": TipoAlerta
                    }
                });

                refresh = true;
                $(this).dialog("close");
            },
            "Cerrar": function () {
                refresh = false;
                $(this).dialog("close");
            }
        },
        close: function () {
            if(refresh) //refresh = Variable booleana
                window.location.href = 'Buzon';
        }
    });
}

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
            }
        }
    });
}

$(document).ready(function () {    
    $('#contador').text(maxCharacters);

    //Para visualizar el mensaje de la notificación
    $('.btn-mensaje').click(function () {
        mensajeNotificacion = $(this).find("span").html();

        $('#p-mensaje-notificacion').html(mensajeNotificacion);
        $('#div-cuerpo-mensaje-notificacion p').css("display", "block");

        DialogoMensajeNotificaction();
    });

    //Para abrir el diálogo de nueva notificación
    $("#btn-nueva-notificacion").click(function () {
        DialogoNuevaNotificacion();
    });

    //Para validar el nro de caracteres en el mensaje de la notificación.
    /*********************************************************************************************************/
    /* Extracto de código sacado de:                                                                         */
    /*      1. http://jsfiddle.net/HgfPU/10/                                                                 */
    /*                                                                                                       */
    /*********************************************************************************************************/
    $("#text-area-notificacion").bind('keyup keydown', function () {
        var count = $("#contador");
        var characters = $(this).val().length;

        if (characters >= maxCharacters) {
            count.removeClass('verde');
            count.addClass('rojo');
            $("#text-area-notificacion").html()
        }
        else {
            count.removeClass('rojo');
            count.addClass('verde');
        }

        count.text(maxCharacters - characters);
    });
    /*********************************************************************************************************/
    /* Fin del extracto de código.                                                                           */
    /*********************************************************************************************************/

    //Para cambiar de pestañas según el tipo de notificación
    $("#li-notif-person").click(function (e) {
        $(this).addClass("active");
        $("#li-notif-auto").removeClass("active");

        $("#table-notif-auto").css("display", "none");
        $("#table-notif-person").css("display", "inline");
    });
    $("#li-notif-auto").click(function (e) {
        $(this).addClass("active");
        $("#li-notif-person").removeClass("active");

        $("#table-notif-auto").css("display", "inline");
        $("#table-notif-person").css("display", "none");
    });
});