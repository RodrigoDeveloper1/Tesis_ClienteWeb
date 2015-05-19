function ActualizarRepresentantes() {
    /* Variables que se encuentran declaradas en GestionListasRepresentantes:
     *
     * var idEstudiante;
     * var representante1_id;
     * var representante2_id;
     * var poseeRepresentante_1;
     * var poseeRepresentante_2;
     */

    if (idEstudiante == 0)
        swal("¡No hay un alumno seleccionado!", "Por favor seleccione un alumno", "warning");
    else
    {
        var representante1_cedula = $("#select-cedula-1").val() + $("#cedula-representante-1").val();
        var representante1_sexo = ($("#select-sexo-1").val() == 1 ? true : false); //Masculino = 1, Femenino = 0
        var representante1_nombre = $("#nombre-representante-1").val();
        var representante1_apellido1 = $("#apellido-1-representante-1").val();
        var representante1_apellido2 = $("#apellido-2-representante-1").val();
        var representante1_correo = $("#correo-1").val();

        var representante2_cedula = $("#select-cedula-2").val() + $("#cedula-representante-2").val();
        var representante2_sexo = ($("#select-sexo-2").val() == 1 ? true : false); //Masculino = 1, Femenino = 0
        var representante2_nombre = $("#nombre-representante-2").val();
        var representante2_apellido1 = $("#apellido-1-representante-2").val();
        var representante2_apellido2 = $("#apellido-2-representante-2").val();
        var representante2_correo = $("#correo-2").val();

        showProgress();
        $.ajax({
            type: "POST",
            url: "/Alumnos/AsociarRepresentantes",
            data: {
                idEstudiante: idEstudiante,
                poseeRepresentante_1: poseeRepresentante_1,
                representante1_id: representante1_id,
                representante1_cedula: representante1_cedula,
                representante1_sexo: representante1_sexo,
                representante1_nombre: representante1_nombre,
                representante1_apellido1: representante1_apellido1,
                representante1_apellido2: representante1_apellido2,
                representante1_correo: representante1_correo,
                poseeRepresentante_2: poseeRepresentante_2,
                representante2_id: representante2_id,
                representante2_cedula: representante2_cedula,
                representante2_sexo: representante2_sexo,
                representante2_nombre: representante2_nombre,
                representante2_apellido1: representante2_apellido1,
                representante2_apellido2: representante2_apellido2,
                representante2_correo: representante2_correo
            },
            success: function (data) {
                window.location.href = 'AsociarRepresentantes';
            }
        });
    }
};

$(document).ready(function () {
    $("#btn-asociar-representantes").bind("click", ActualizarRepresentantes)
});