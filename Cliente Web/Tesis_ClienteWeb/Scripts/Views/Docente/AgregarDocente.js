var idColegio = "";
var idAnoEscolarActivo = "";
var idCurso = "";
var idMateria = "";
var idDocente = "";

function AnoEscolarYCursos()
{
    $("#ano-escolar").val("Cargando el año escolar");
    $("#select-curso").find('option').remove().end().append('<option>Cargando la lista de cursos...</option>');
    $("#select-curso").selectpicker("refresh");

    if (idColegio != "") {
        showProgress();

        $.ajax({
            type: "POST",
            url: "/Bridge/ObtenerAnoEscolarActivoEnLabel",
            data: {
                idColegio: idColegio
            },
            success: function (data) {
                if (data[0].success) {
                    $("#ano-escolar").val(data[0].label);
                    idAnoEscolarActivo = data[0].idAnoEscolar;

                    $.ajax({
                        type: "POST",
                        url: "/Bridge/ObtenerListaNombresDeCursosPorAnoEscolar",
                        data: {
                            idAnoEscolar: idAnoEscolarActivo
                        },
                        success: function (data2) {
                            if (data2[0].success) {
                                var lista = '<option value="">Seleccione el curso...</option>';

                                for (var i = 0; i < data2.length; i++) {
                                    lista += ('<option value="' + data2[i].idCurso + '">' +
                                        data2[i].nombreCurso + '</option>');
                                }

                                $("#select-curso").find('option').remove().end().append(lista);
                                $("#select-curso").selectpicker("refresh");

                                hideProgress();
                            }
                            else {
                                idCurso = 0;
                                $("#select-curso").find('option').remove().end().append('<option>El ' +
                                    'colegio no posee cursos</option>');
                                $("#select-curso").selectpicker("refresh");

                                hideProgress();
                            }
                        }
                    });
                }
                else {
                    idCurso = 0;
                    $("#ano-escolar").val("No posee año escolar activo");
                    $("#select-curso").find('option').remove().end().append('<option>Seleccione ' +
                        'el çurso...</option>');
                    $("#select-curso").selectpicker("refresh");

                    hideProgress();
                }
            }
        });
    }
    else {
        idCurso = "";
        $("#ano-escolar").val("");
        $("#select-curso").find('option').remove().end().append('<option>Seleccione el curso...</option>');
        $("#select-curso").selectpicker("refresh");

        hideProgress();
    }
}
function ListadoMaterias()
{
    $("#select-materia").find('option').remove().end().append('<option>Cargando la lista de materias...</option>');
    $("#select-materia").selectpicker("refresh");

    if(idCurso != "") {
        showProgress();

        $.ajax({
            type: "POST",
            url: "/Bridge/ObtenerJsonMaterias",
            data: {
                idColegio: idColegio,
                idCurso: idCurso
            },
            success: function (data) {
                if (data[0].success) {
                    var lista = '<option value="">Seleccione la materia...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idMateria + '">' +
                            data[i].materia + '</option>');
                    }

                    $("#select-materia").find('option').remove().end().append(lista);
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    idMateria = "";
                    $("#select-materia").find('option').remove().end().append('<option>El curso ' +
                        'no posee materias...</option>');
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                    swal("¡Oops!", "El curso no posee materias", "warning");
                }
            }
        });
    }
    else {
        idMateria = "";
        $("#select-materia").find('option').remove().end().append('<option>Seleccione la materia...</option>');
        $("#select-materia").selectpicker("refresh");
    }
}
function ListadoDocentes() {
    $("#select-docente").find('option').remove().end().append('<option>Cargando la lista de docentes...</option>');
    $("#select-docente").selectpicker("refresh");

    if (idColegio != "") {
        showProgress();

        $.ajax({
            type: "POST",
            url: "/Bridge/ObtenerJsonDocentes",
            data: {
                idColegio: idColegio,
            },
            success: function (data) {
                if (data[0].success) {
                    var lista = '<option value="">Seleccione el docente...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idDocente + '">' +
                            data[i].docente + '</option>');
                    }

                    $("#select-docente").find('option').remove().end().append(lista);
                    $("#select-docente").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    idDocente = "";
                    $("#select-docente").find('option').remove().end().append('<option>El colegio ' +
                        'no posee docentes...</option>');
                    $("#select-docente").selectpicker("refresh");

                    hideProgress();
                    swal("¡Oops!", "El colegio no posee docentes", "warning");
                }
            }
        });
    }
    else {
        idDocente = "";
        $("#select-docente").find('option').remove().end().append('<option>Seleccione el docente...</option>');
        $("#select-docente").selectpicker("refresh");
    }
}
function BtnAgregarDocente() {
    if(idDocente == "" || idMateria == "")
        swal("¡Oops!", "Falta seleccionar el docente o la materia", "warning");
    else {
        showProgress();

        $.ajax({
            type: "POST",
            url: "/Docente/AgregarDocente",
            data: {
                idCurso: idCurso,
                idMateria: idMateria,
                idDocente: idDocente
            },
            success: function (data) {
                if (data[0].success) {
                    hideProgress();

                    swal({
                        title: "¡Agregado exitoso!",
                        text: "Se ha agregado correctamente el docente al curso",
                        type: "success",
                        confirmButtonClass: "btn-primary",
                        confirmButtonText: "Ok",
                        closeOnConfirm: false
                    },
                    function () {
                        window.location.href = "AgregarDocente";
                    });
                }
                else {
                    hideProgress();

                    swal({
                        title: "¡Agregado fallido!",
                        text: "No se ha podido agregar el docente al curso. =(",
                        type: "warning",
                        confirmButtonClass: "btn-warning",
                        confirmButtonText: "Ok",
                        closeOnConfirm: false
                    },
                    function () {
                        window.location.href = "AgregarDocente";
                    });
                }
            }
        });
    }
}

$(document).ready(function () {
    $("#select-colegio").change(function () {
        idColegio = $(this).val();
        AnoEscolarYCursos();
        ListadoDocentes();
    });

    $("#select-curso").change(function () {
        idCurso = $(this).val();
        ListadoMaterias();
    });

    $("#select-materia").change(function () {
        idMateria = $(this).val();
    });

    $("#select-docente").change(function () {
        idDocente = $(this).val();
    });

    $("#btn-agregar-docente").click(function () {
        BtnAgregarDocente();
    });
});