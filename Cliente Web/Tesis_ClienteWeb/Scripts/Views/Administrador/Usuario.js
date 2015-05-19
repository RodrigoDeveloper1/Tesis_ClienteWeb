$(document).ready(function () {
    $('.eliminarusuario').click(function () {
        var selectedId = $(this).data("id");
        
        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este usuario!",
            type: "warning", showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnConfirm: true,
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();

                $.ajax({
                    type: "POST",
                    url: "/Administrador/EliminarUsuario",
                    data:
                        {
                            id: selectedId
                        },
                    success: function (data) {
                        if (data[0].success) {
                            swal({
                                title: "¡Eliminado!",
                                text: "Adios usuario mío ='(",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarUsuarios';
                            });
                        }
                        else {
                            swal("¡No se eliminó!", "No se pudo eliminar el usuario", "warning");
                            window.location.href = 'ListarUsuarios';
                        }
                    }
                });
            }
            else {
                hideProgress();
                swal("¡Salvado!", "El usuario no ha sido eliminado", "info");
            }
        }
        );
    });
});