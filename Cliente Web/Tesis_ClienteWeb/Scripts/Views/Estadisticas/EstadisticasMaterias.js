var idCurso;
var idLapso;
var idMateria;
var countChange = 0;

function SortByCalculoAsc(x, y) {
    return y.calculo - x.calculo;
}
function SortByCalculoDesc(x, y) {
    return x.calculo - y.calculo;
}
function ValidacionNotasVacias() {
    swal({
        title: "¡No existen notas en estas evaluaciones!",
        text: "No existe ninguna nota cargada para las evaluaciones de la materia seleccionada .",
        type: "warning",
        confirmButtonColor: "green",
        showCancelButton: false,
        closeOnConfirm: true,
    },
    function (isConfirm) {
        hideProgress();
        //window.location.href = 'CargarCalificaciones';
    });
}

$(document).ready(function () {
    $("#select-curso").change(function () {
        if ($(this).val() != "") {
            showProgress();

            $("#select-lapso").find('option').remove().end().append("<option>Cargando lapsos...</option>");
            $("#select-lapso").selectpicker("refresh");
            idCurso = $(this).val();

            $.post("/Bridge/ObtenerSelectListLapsosProfesor",
                {
                    idCurso: idCurso
                },
                function (data) {
                    if (data != null && data.length > 0) {
                        var lista = '<option value="">Seleccione el lapso...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].idLapso + '">' + data[i].nombre + '</option>');
                        }

                        $("#select-lapso").find('option').remove().end().append(lista);
                        $("#select-lapso").selectpicker("refresh");

                        hideProgress();
                    }
                    else {
                        $("#select-lapso").find('option').remove().end().append('<option>No se encontraron' + 
                            ' lapsos activos....</option>');
                        $("#select-lapso").selectpicker("refresh");

                        hideProgress();
                    }
                });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");
            idMateria = ""; //Reseteando la variable

            hideProgress();
        }
    });
    $("#select-lapso").change(function () {
        idCurso = $("#select-curso option:selected").val();
        idLapso = $(this).val();

        if ($(this).val() != "") {
            showProgress();

            $("#select-materia").find('option').remove().end().append("<option>Cargando materias...</option>");
            $("#select-materia").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListMaterias",
                {
                    idCurso: idCurso
                },
                function (data) {
                    if (data != null && data.length > 0) {
                        var lista = '<option value="">Seleccione la materia...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].idMateria + '">' + data[i].nombre + '</option>');
                        }

                        $("#select-materia").find('option').remove().end().append(lista);
                        $("#select-materia").selectpicker("refresh");

                        hideProgress();
                    }
                    else {
                        $("#select-materia").find('option').remove().end().append('<option>No se encontraron' +
                            ' materias activas....</option>');
                        $("#select-materia").selectpicker("refresh");

                        hideProgress();
                    }
                });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
            idMateria = ""; //Reseteando la variable

            hideProgress();
        }
    });
    $("#select-materia").change(function () {
        showProgress();
        countChange++;
        if ($(this).val() != "") {
            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();

            $.post("/Bridge/ObtenerSelectListEvaluacionesDeCASUSEstadisticas",
                {
                    idMateria: idMateria,
                    idCurso: idCurso,
                    idLapso: idLapso
                },
                function (data) {
                    if (data != null && data.length > 0) {
                        var listaOrdenadaCalculo = [];
             
                        $("#ranking-alumnos-div").find('div').remove().end().append(
                            "<div style='height:30em; padding-bottom:8em;' id=morris-bar-rankingalumnos" +
                            countChange + ">" +
                            "</div>");
                        var graphelement = 'morris-bar-rankingalumnos' + countChange;

                        $("#aprovvsrepro-div").find('div').remove().end().append(
                            "<div id=morris-donut-aprovsrepro" + countChange + ">" +
                            "</div>");
                        var graphelement2 = 'morris-donut-aprovsrepro' + countChange;

                        $("#ranking-peores-div").find('div').remove().end().append(
                            "<div style='height:30em; padding-bottom:8em;' id=morris-ranking-peores"
                            + countChange + ">" +
                            "</div>");
                        var graphelement3 = 'morris-ranking-peores' + countChange;

                        if (data[0].grado == "Bachillerato") {
                            data.sort(SortByCalculoAsc);
                            var dataTune = [];
                            var dataOP;
                            var count = 0;

                            if (data.length >= 10) {
                                for (t = 0; t <= 9; t++) {
                                    count++;
                                    dataOP = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }

                                    dataTune.push(dataOP);
                                }
                            } else {
                                for (t = 0; t <= data.length - 1; t++) {
                                    count++;
                                    dataOP = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    dataTune.push(dataOP);
                                }
                            }

                            Morris.Bar({
                                element: graphelement.toString(),
                                data: dataTune,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Acumulada'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#1DescripciónMateria").html(
                                "Se observa el ranking de los 10 primeros resultados más destacados obtenidos" +
                                " durante el desarrollo del lapso seleccionado, en la materia seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>1. " + dataTune[0].y + ": " + dataTune[0].a + "</strong></br>" +
                                "2. " + dataTune[1].y + ": " + dataTune[1].a + "</br>" +
                                "3. " + dataTune[2].y + ": " + dataTune[2].a + "</br>" +
                                "4. " + dataTune[3].y + ": " + dataTune[3].a + "</br>" +
                                "5. " + dataTune[4].y + ": " + dataTune[4].a + "</br>" +
                                "6. " + dataTune[5].y + ": " + dataTune[5].a + "</br>" +
                                "7. " + dataTune[6].y + ": " + dataTune[6].a + "</br>" +
                                "8. " + dataTune[7].y + ": " + dataTune[7].a + "</br>" +
                                "9. " + dataTune[8].y + ": " + dataTune[8].a + "</br>" +
                                "10. " + dataTune[9].y + ": " + dataTune[9].a + "</br>");

                            data.sort(SortByCalculoDesc);

                            var dataTune2 = [];
                            var dataOT;
                            var coloresBarras = [];

                            if (data.length >= 10) {
                                for (t = 0; t <= 9; t++) {
                                    count++;
                                    dataOT = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }

                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            } else {
                                for (t = 0; t <= data.length - 1; t++) {
                                    count++;
                                    dataOT = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            }

                            Morris.Bar({
                                element: graphelement3,
                                data: dataTune2,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Acumulada'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                barColors: coloresBarras,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#2DescripciónMateria").html(
                                "Se observa el ranking de los 10 resultados más deficientes obtenidos" +
                                " durante el desarrollo del lapso seleccionado, en la materia seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: red'>1. " + dataTune2[0].y + ": " + dataTune2[0].a + "</strong></br>" +
                                "2. " + dataTune2[1].y + ": " + dataTune2[1].a + "</br>" +
                                "3. " + dataTune2[2].y + ": " + dataTune2[2].a + "</br>" +
                                "4. " + dataTune2[3].y + ": " + dataTune2[3].a + "</br>" +
                                "5. " + dataTune2[4].y + ": " + dataTune2[4].a + "</br>" +
                                "6. " + dataTune2[5].y + ": " + dataTune2[5].a + "</br>" +
                                "7. " + dataTune2[6].y + ": " + dataTune2[6].a + "</br>" +
                                "8. " + dataTune2[7].y + ": " + dataTune2[7].a + "</br>" +
                                "9. " + dataTune2[8].y + ": " + dataTune2[8].a + "</br>" +
                                "10. " + dataTune2[9].y + ": " + dataTune2[9].a + "</br>");

                            var alumnosPasados = 0;
                            var alumnosAplazados = 0;
                            var totalAlumnos = data.length;

                            for (t = 0; t <= data.length - 1; t++) {
                                if (data[t].aprobado == false)
                                    alumnosAplazados++;
                                else
                                    alumnosPasados++;
                            }

                            var porcAplazados = Math.round(alumnosAplazados * 100 / totalAlumnos);
                            var porcPasados = Math.round(alumnosPasados * 100 / totalAlumnos);

                            var alumnosPasadosDescripcion = Math.round(porcPasados * totalAlumnos / 100);
                            var alumnosReprobadosDescripcion = Math.round(porcAplazados * totalAlumnos / 100);

                            Morris.Donut({
                                element: graphelement2,
                                data: [{
                                    label: "Aprobados %",
                                    value: porcPasados
                                }, {
                                    label: "Reprobados %",
                                    value: porcAplazados
                                }],
                                colors: ["#0000FF", "#FF0000"],
                                resize: true
                            });

                            $("#3DescripciónMateria").html("Se observa el porcentaje de estudiantes aprobados" +
                                " y aplazados hasta la fecha, en la materia previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>Aprobados: " + porcPasados + "% => " + alumnosPasadosDescripcion +
                                " de " + totalAlumnos + " alumnos." + "</strong></br>" +
                                "<strong style='color: red'>Aplazados: " + porcAplazados + "% => " + alumnosReprobadosDescripcion +
                                " de " + totalAlumnos + " alumnos." + "</strong></br>");

                            hideProgress();
                        }
                        else
                        {
                            data.sort(SortByCalculoAsc);
                            var letras = [];
                            var dataTune = [];
                            var dataOP;
                            var count = 0;

                            if (data.length >= 10) {
                                for (t = 0; t <= 9; t++) {
                                    count++;
                                    letras.push(data[t].letranota);
                                    dataOP = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    dataTune.push(dataOP);
                                }
                            }
                            else {
                                for (t = 0; t <= data.length - 1; t++) {
                                    count++;
                                    letras.push(data[t].letranota);
                                    dataOP = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    dataTune.push(dataOP);
                                }
                            }
                            Morris.Bar({
                                element: graphelement.toString(),
                                data: dataTune,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Acumulada'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });                   

                            $("#1DescripciónMateria").html("Se observa el ranking de los primeros 10 estudiantes con mejor " +
                                                   "rendimiento en la materia previamente seleccionada." + "</br>" +
                                                   "------------------------------------------------------</br>" +
                                                   "1. " + dataTune[0].y + ": " + letras[0] + "</br>" +
                                                   "2. " + dataTune[1].y + ": " + letras[1] + "</br>" +
                                                   "3. " + dataTune[2].y + ": " + letras[2] + "</br>" +
                                                   "4. " + dataTune[3].y + ": " + letras[3] + "</br>" +
                                                   "5. " + dataTune[4].y + ": " + letras[4] + "</br>" +
                                                   "6. " + dataTune[5].y + ": " + letras[5] + "</br>" +
                                                   "7. " + dataTune[6].y + ": " + letras[6] + "</br>" +
                                                   "8. " + dataTune[7].y + ": " + letras[7] + "</br>" +
                                                   "9. " + dataTune[8].y + ": " + letras[8] + "</br>" +
                                                   "10. " + dataTune[9].y + ": " + letras[9] + "</br>");

                            data.sort(SortByCalculoDesc);

                            letras = [];
                            var dataTune2 = [];
                            var dataOT;
                            var coloresBarras = [];
                            if (data.length >= 10) {
                                for (t = 0; t <= 9; t++) {
                                    count++;
                                    letras.push(data[t].letranota);
                                    dataOT = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            } else {
                                for (t = 0; t <= data.length - 1; t++) {
                                    count++;
                                    letras.push(data[t].letranota);
                                    dataOT = {
                                        y: data[t].estudiante,
                                        a: Math.round(data[t].calculo),
                                        b: count.toString()
                                    }
                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            }
      

                            Morris.Bar({
                                element: graphelement3,
                                data: dataTune2,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Acumulada'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                barColors: coloresBarras,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#2DescripciónMateria").html("Se observa el ranking de los primeros 10 estudiantes con peor " +
                                                          "rendimiento en la materia previamente seleccionada." + "</br>" +
                                                          "------------------------------------------------------</br>" +
                                                          "1. " + dataTune2[0].y + ": " + letras[0] + "</br>" +
                                                          "2. " + dataTune2[1].y + ": " + letras[1] + "</br>" +
                                                          "3. " + dataTune2[2].y + ": " + letras[2] + "</br>" +
                                                          "4. " + dataTune2[3].y + ": " + letras[3] + "</br>" +
                                                          "5. " + dataTune2[4].y + ": " + letras[4] + "</br>" +
                                                          "6. " + dataTune2[5].y + ": " + letras[5] + "</br>" +
                                                          "7. " + dataTune2[6].y + ": " + letras[6] + "</br>" +
                                                          "8. " + dataTune2[7].y + ": " + letras[7] + "</br>" +
                                                          "9. " + dataTune2[8].y + ": " + letras[8] + "</br>" +
                                                          "10. " + dataTune2[9].y + ": " + letras[9] + "</br>");



                            var alumnosPasados = 0;
                            var alumnosAplazados = 0;
                            var totalAlumnos = data.length;

                            for (t = 0; t <= data.length - 1; t++) {

                                if (data[t].aprobado == false) {
                                    alumnosAplazados++;
                                }
                                else {
                                    alumnosPasados++;
                                }
                            }

                            var porcAplazados = Math.round(alumnosAplazados * 100 / totalAlumnos);
                            var porcPasados = Math.round(alumnosPasados * 100 / totalAlumnos);
                    
                            var alumnosPasadosDescripcion = Math.round(porcPasados * totalAlumnos / 100);
                            var alumnosReprobadosDescripcion = Math.round(porcAplazados * totalAlumnos / 100);

                            Morris.Donut({
                                element: graphelement2,
                                data: [{
                                    label: "Aprobados %",
                                    value: porcPasados
                                }, {
                                    label: "Reprobados %",
                                    value: porcAplazados
                                }],
                                colors: ["#0000FF", "#FF0000"],
                                resize: true
                            });

                            $("#3DescripciónMateria").html("Se observa el porcentaje de estudiantes aprobados y aplazados" +
                                                        " actualmente en la materia previamente seleccionada." + "</br>" +
                                                        "------------------------------------------------------</br>" +
                                                        "Aprobados: " + porcPasados + "% => " + alumnosPasadosDescripcion +
                                                        " de " + totalAlumnos + " alumnos." + "</br>" +
                                                        "Aplazados: " + porcAplazados + "% => " + alumnosReprobadosDescripcion +
                                                        " de " + totalAlumnos + " alumnos." + "</br>");


                            hideProgress();
                        }

                    }
                    else {
                        $("#ranking-alumnos-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                          "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + ">" +
                          "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#aprovvsrepro-div").find('div').remove().end().append("<div class='centrar' id=morris-donut-aprovsrepro" +
                        countChange + ">" + "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#ranking-peores-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                          "padding-bottom:8em;' id=morris-ranking-peores" + countChange + ">" +
                          "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#1DescripciónMateria").html(" ");
                        $("#2DescripciónMateria").html(" ");
                        $("#3DescripciónMateria").html(" ");
                        ValidacionNotasVacias();                
                    }
                });
            }
            else {
                $("#ranking-alumnos-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                  "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + ">" +
                  "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                $("#aprovvsrepro-div").find('div').remove().end().append("<div class='centrar' id=morris-donut-aprovsrepro" +
                    countChange + ">" + "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                $("#ranking-peores-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                  "padding-bottom:8em;' id=morris-ranking-peores" + countChange + ">" +
                  "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                $("#1DescripciónMateria").html(" ");
                $("#2DescripciónMateria").html(" ");
                $("#3DescripciónMateria").html(" ");

                idMateria = ""; //Reseteando la variable
                hideProgress();
            }
        });
});