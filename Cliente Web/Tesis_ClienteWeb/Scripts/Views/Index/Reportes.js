function ReportePorEvaluacion() {
    $.ajax({
        type: "POST",
        data: {
            idEvaluacion: idEvaluacion //Variable guardada en Index.js
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
                swal("¡Error!", "Ha ocurrido un error que impidió que el reporte se generara", "error");
            }
        },
        error: function (data) {
            hideProgress();
            swal("¡Error!", "Ha ocurrido un error que impidió que el reporte se generara", "error");
        }
    })
};

$(document).ready(function () {
    $('#reporte-inicio-torta').click(function () {
        swal({
            title: "Reporte de evaluación",
            text: "¿Desea imprimir el reporte por la evaluación respectiva?",
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
                ReportePorEvaluacion();
            }
        });
    });
    $('#reporte-inicio-barras').click(function () {
        swal({
            title: "Reporte de evaluación",
            text: "¿Desea imprimir el reporte por la evaluación respectiva?",
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
                ReportePorEvaluacion();
            }
        });
    });    
});