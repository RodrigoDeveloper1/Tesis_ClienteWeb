$(document).ready(function () {
    $('.eliminarrol').click(function () {
        var selectedId = $(this).data("id");

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este rol!",
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
                    url: "/Administrador/EliminarRol",
                    data:
                        {
                            id: selectedId
                        },
                    success: function (data) {
                        if (data[0].success) {
                            swal({
                                title: "¡Eliminado!",
                                text: "Se ha eliminado el rol correctamente",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarRoles';
                            });
                        }
                        else {
                            swal("¡No se eliminó!", "No se pudo eliminar el rol", "warning");
                            window.location.href = 'ListarRoles';
                        }
                    }
                });
            }
            else {
                hideProgress();
                swal("¡Cancelado!", "El rol no ha sido eliminado", "info");
            }
        }
        );
    });
});