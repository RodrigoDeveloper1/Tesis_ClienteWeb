$(document).ready(function () {
    //Ícono de notificaciones
    $("#icono-notificaciones").click(function () {
        $("#nro-notificaciones").css("display", "none");
    });

    //Lista de colegios para administrador
    $("#colegios-administrador").change(function (e) {
        var colegioAdministrador = $(this).val();

        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de cambiar de colegio?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F5AE22", //naranja
            confirmButtonText: "Cambiar",
            cancelButtonText: "Cancelar",
            closeOnConfirm: false,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    type: "POST",
                    url: "/Bridge/ActualizacionSesionColegio",
                    data: {
                        id: colegioAdministrador
                    },
                    success: function (data) {
                        window.location.href = 'Inicio';
                    }
                });
            }
            else {
                window.location.href = 'Inicio';
            }
        });
    });

    //Botón de cerrar sesión
    $("#logout-action").click(function () {
        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de cerrar la sesión?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#F5AE22", //naranja
            confirmButtonText: "Cerrar",
            cancelButtonText: "Cancelar",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();
                $.ajax({
                    type: "POST",
                    url: "/Bridge/CerrarSesion",
                    success: function (data) {
                        window.location.href = '../Login/Index';
                    }
                });
            }
        });
    });
});