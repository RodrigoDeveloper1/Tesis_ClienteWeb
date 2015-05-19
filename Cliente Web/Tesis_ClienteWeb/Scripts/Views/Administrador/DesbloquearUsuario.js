$(document).ready(function () {
    $('#btn-bloquear').click(function () {        
        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de bloquear este/estos usuario(s)?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55", //Rojo
            confirmButtonText: "¡Bloquear!",
            cancelButtonText: "¡No, cancélalo!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();
                var porBloquear = [];

                $('#div-tabla-lista-usuarios-habilitados input:checked').each(function () {
                    porBloquear.push(this.id);
                });

                var postData = { values: porBloquear };

                $.ajax({
                    type: "POST",
                    url: "/Administrador/BloquearUsuario",
                    data: postData,
                    traditional: true,
                    success: function (suspendido) {
                        if (suspendido) {
                            hideProgress();
                            swal({
                                title: "¡Suspendido!",
                                text: "El o los usuarios se han bloqueado",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'DesbloquearUsuario';
                            });
                        }
                        else {
                            hideProgress();
                            swal({
                                title: "¡Error!",
                                text: "El o los usuarios no se pudieron bloquear.",
                                type: "error",
                                confirmButtonColor: "red",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'DesbloquearUsuario';
                            });
                        }
                    },
                    error: function () {
                        window.location.href = 'DesbloquearUsuario';
                    }
                });
            }
        });
    });
    $('#btn-desbloquear').click(function () {
        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de desbloquear este/estos usuario(s)?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55", //Rojo
            confirmButtonText: "¡Desbloquear!",
            cancelButtonText: "¡No, cancélalo!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();
                var porDesbloquear = [];

                $('#div-tabla-lista-usuarios-bloqueados input:checked').each(function () {
                    porDesbloquear.push(this.id);
                });

                var postData = { values: porDesbloquear };

                $.ajax({
                    type: "POST",
                    url: "/Administrador/DesbloquearUsuario",
                    data: postData,
                    traditional: true,
                    success: function (suspendido) {
                        if (suspendido) {
                            hideProgress();
                            swal({
                                title: "¡Desbloqueado!",
                                text: "El o los usuarios se han desbloqueado",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'DesbloquearUsuario';
                            });
                        }
                        else {
                            hideProgress();
                            swal({
                                title: "¡Error!",
                                text: "El o los usuarios no se pudieron desbloquear.",
                                type: "error",
                                confirmButtonColor: "red",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'DesbloquearUsuario';
                            });
                        }
                    },
                    error: function ()
                    {
                        window.location.href = 'DesbloquearUsuario';
                    }
                });
            }
        });
    });
});