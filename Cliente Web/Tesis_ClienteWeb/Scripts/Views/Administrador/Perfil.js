$(document).ready(function () {
    $("#btn-agregar-perfil").click(function () {
        showProgress();
    });

    $('.eliminarperfil').click(function () {
        var selectedId = $(this).data("id");

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este perfil!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();
                jQuery.get("/Administrador/EliminarPerfil",
                    {
                        "id": selectedId
                    })
                    .done(function (data) {
                        hideProgress();
                        swal({
                            title: "¡Borrado!",
                            text: "Su perfil ha sido borrado.",
                            type: "success",
                            showCancelButton: false,
                            closeOnConfirm: true,
                        }, 
                        function (isConfirm) {
                            if (isConfirm)
                                window.location.href = 'GestionPerfiles';
                        })
                    });
            }
        });
    });

    $(".fa-pencil").click(function () {
        showProgress();
    });

    $("#btn-editar-perfil").click(function () {
        showProgress();
    });
})