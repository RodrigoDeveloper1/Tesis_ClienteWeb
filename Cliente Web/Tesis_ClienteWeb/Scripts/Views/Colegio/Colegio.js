$(document).ready(function () {
    //Suspender colegio
    $('.suspender-colegio').click(function () {
        var selectedId = $(this).data("id");

        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de suspender este colegio?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55", //Rojo
            confirmButtonText: "¡Suspender!",            
            cancelButtonText: "¡No, cancélalo!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();

                $.ajax({
                    type: "POST",
                    url: "/Colegios/SuspenderColegio",
                    data: {
                        idColegio: selectedId
                    },
                    success: function (suspendido) {
                        if (suspendido) {
                            swal({
                                title: "¡Suspendido!",
                                text: "El colegio acaba de suspenderse.",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: true,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarColegios';
                            });
                        }
                        else {
                            swal({
                                title: "¡Error!",
                                text: "El colegio no se pudo suspender.",
                                type: "error",
                                confirmButtonColor: "red",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarColegios';
                            });
                        }
                    }
                });
            }
        });
    });

    //Habilitar colegio
    $('.habilitar-colegio').click(function () {
        var selectedId = $(this).data("id");

        swal({
            title: "¿Estás seguro?",
            text: "¿Estás seguro de habilitar este colegio?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "green",
            confirmButtonText: "¡Habilitar!",
            cancelButtonText: "¡No, cancélalo!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
        function (isConfirm) {
            if (isConfirm) {
                showProgress();

                $.ajax({
                    type: "POST",
                    url: "/Colegios/HabilitarColegio",
                    data: {
                        idColegio: selectedId
                    },
                    success: function (habilitado) {
                        if (habilitado) {
                            swal({
                                title: "¡Habilitado!",
                                text: "El colegio acaba de habilitarse.",
                                type: "success",
                                confirmButtonColor: "green",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarColegios';
                            });
                        }
                        else {
                            swal({
                                title: "¡Error!",
                                text: "El colegio no se pudo habilitar.",
                                type: "error",
                                confirmButtonColor: "red",
                                showCancelButton: false,
                                closeOnConfirm: false,
                            },
                            function (isConfirm) {
                                if (isConfirm)
                                    window.location.href = 'ListarColegios';
                            });
                        }
                    }
                });
            }
        });
    });
})