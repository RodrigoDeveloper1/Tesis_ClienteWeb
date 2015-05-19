var reporteEvaluacion = false;
var reporteMateria = false;
var reporteCurso = false;

function ReportePorEvaluacion() {
    $.ajax({
        type: "POST",
        data: {
            idEvaluacion: idEvaluacion /* Variable guardada en: EstadisticasEvaluaciones.js */                                        
        },
        dataType: 'json',
        url: "/Bridge/ReportePorEvaluacion",
        success: function (data) {
            if (data[0].success) {
                hideProgress();
                window.location = '/Reportes/GenerarReporte/?path=' + data[0].path;
                swal("¡Reporte generado!", "El reporte ha sido generado", "success");
            }
            else {
                hideProgress();
                MensajeError();
            }
        },
        error: function (data) {
            hideProgress();
            MensajeError();
        }
    });
};
function ReportePorMateria() {
    $.ajax({
        type: "POST",
        data: {
            idMateria: idMateria, // Variable guardada en EstadisticasMaterias.js
            idLapso: idLapso, // Variable guardada en EstadisticasMaterias.js
            idCurso: idCurso // Variable guardada en EstadisticasMaterias.js
        },
        dataType: 'json',
        url: "/Bridge/ReportePorMateria",
        success: function (data) {
            if (data[0].success) {
                hideProgress();
                window.location = '/Reportes/GenerarReporte/?path=' + data[0].path;
                swal("¡Reporte generado!", "El reporte ha sido generado", "success");
            }
            else {
                hideProgress();
                MensajeError();
            }
        },
        error: function (data) {
            hideProgress();
            MensajeError();
        }
    });
};
function ReportePorCurso() {
    $.ajax({
        type: "POST",
        data: {
            idCurso: idCurso // Variable guardada en EstadisticasCursos.js
        },
        dataType: 'json',
        url: "/Bridge/ReportePorCurso",
        success: function (data) {
            if (data[0].success) {
                hideProgress();
                window.location = '/Reportes/GenerarReporte/?path=' + data[0].path;
                swal("¡Reporte generado!", "El reporte ha sido generado", "success");
            }
            else {
                hideProgress();
                MensajeError();
            }
        },
        error: function (data) {
            hideProgress();
            MensajeError();
        }
    });
};
function MensajeError() {
    swal("¡Error!", "Ha ocurrido un error que impidió que el reporte se generara", "error");
};
function MensajeConfirmacionImpresion() {
    swal({
        title: "Generación de reporte",
        text: "¿Desea imprimir el reporte respectivo? Tomará unos cuantos segundos...",
        type: "info",
        showCancelButton: true,
        confirmButtonText: "Generar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true,
        closeOnCancel: true
    },
    function (isConfirm) {
        if (isConfirm) {
            showProgress();
            if (reporteEvaluacion)
                ReportePorEvaluacion();
            else if (reporteMateria)
                ReportePorMateria();
            else if (reporteCurso)
                ReportePorCurso();
        }
    });
};

$(document).ready(function () {
    //Estadísticas de evaluaciones
    $('#reporte-estadisticas-top10peores').click(function () {
        if (idEvaluacion == "" || idEvaluacion == null)
            swal("¡No ha seleccionado una evaluación!", "Por favor, seleccione una evaluación para generar" +
                " el reporte respectivo.", "warning");
        else {
            reporteEvaluacion = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-top10').click(function () {
        if (idEvaluacion == "" || idEvaluacion == null)
            swal("¡No ha seleccionado una evaluación!", "Por favor, seleccione una evaluación para generar" +
                " el reporte respectivo.", "warning");
        else {
            reporteEvaluacion = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-aprobados-vs-reprobados').click(function () {
        if (idEvaluacion == "" || idEvaluacion == null)
            swal("¡No ha seleccionado una evaluación!", "Por favor, seleccione una evaluación para generar" +
                " el reporte respectivo.", "warning");
        else {
            reporteEvaluacion = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-proporcion-notas').click(function () {
        if (idEvaluacion == "" || idEvaluacion == null)
            swal("¡No ha seleccionado una evaluación!", "Por favor, seleccione una evaluación para generar" +
                " el reporte respectivo.", "warning");
        else {
            reporteEvaluacion = true;
            MensajeConfirmacionImpresion();
        }
    });

    //Estadísticas de materias
    $('#reporte-estadisticas-materias-aprobadosReprobados').click(function () {
        if (idMateria == "" || idMateria == null ||
            idLapso == "" || idLapso == null ||
            idCurso == "" || idCurso == null)
            swal("¡Selección de datos incompleta!", "Por favor, seleccione un curso, un lapso y una materia" +
                " para generar el reporte respectivo.", "warning");
        else {
            reporteMateria = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-materias-top10Mejores').click(function () {
        if (idMateria == "" || idMateria == null ||
            idLapso == "" || idLapso == null ||
            idCurso == "" || idCurso == null)
            swal("¡Selección de datos incompleta!", "Por favor, seleccione un curso, un lapso y una materia" +
                " para generar el reporte respectivo.", "warning");
        else {
            reporteMateria = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-materias-top10Peores').click(function () {
        if (idMateria == "" || idMateria == null ||
            idLapso == "" || idLapso == null ||
            idCurso == "" || idCurso == null)
            swal("¡Selección de datos incompleta!", "Por favor, seleccione un curso, un lapso y una materia" +
                " para generar el reporte respectivo.", "warning");
        else {
            reporteMateria = true;
            MensajeConfirmacionImpresion();
        }
    });

    //Estadísticas de cursos
    $('#reporte-estadisticas-cursos-rendimiento').click(function () {
        if (idCurso == "" || idCurso == null)
            swal("¡No ha seleccionado un curso!", "Por favor, seleccione un curso para generar el reporte" +
                "respectivo.", "warning");
        else {
            reporteCurso = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-cursos-ranking-1').click(function () {
        if (idCurso == "" || idCurso == null)
            swal("¡No ha seleccionado un curso!", "Por favor, seleccione un curso para generar el reporte" +
                "respectivo.", "warning");
        else {
            reporteCurso = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-cursos-ranking-2').click(function () {
        if (idCurso == "" || idCurso == null)
            swal("¡No ha seleccionado un curso!", "Por favor, seleccione un curso para generar el reporte" +
                "respectivo.", "warning");
        else {
            reporteCurso = true;
            MensajeConfirmacionImpresion();
        }
    });
    $('#reporte-estadisticas-cursos-ranking-3').click(function () {
        if (idCurso == "" || idCurso == null)
            swal("¡No ha seleccionado un curso!", "Por favor, seleccione un curso para generar el reporte" +
                "respectivo.", "warning");
        else {
            reporteCurso = true;
            MensajeConfirmacionImpresion();
        }
    });
});