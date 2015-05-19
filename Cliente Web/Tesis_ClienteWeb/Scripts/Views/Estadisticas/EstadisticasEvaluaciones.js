var idCurso;
var idLapso;
var idMateria;
var idEvaluacion = "";
var countChange = 0;

function SortByCalculoAsc(x, y) {
    return y.nota - x.nota;
}
function SortByCalculoDesc(x, y) {
    return x.nota - y.nota;
}
function SortByCalculoAscPrimaria(x, y) {

    return (x.nota > y.nota) ? 1 : -1;
}
function SortByCalculoDescPrimaria(x, y) {

    return (y.nota > x.nota) ? 1 : -1;
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
                    console.log("Entro 2");
                    $("#select-lapso").find('option').remove().end().append(lista);
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-lapso").find('option').remove().end().append('<option>No se encontraron lapso activos....</option>');
                    $("#select-lapso").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-lapso').find('option').remove().end().append('<option>Seleccione el lapso...</option>');
            $("#select-lapso").selectpicker("refresh");
            idEvaluacion = ""; //Reseteando la variable;

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
                    $("#select-materia").find('option').remove().end().append('<option>No se encontraron materias activas....</option>');
                    $("#select-materia").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-materia').find('option').remove().end().append('<option>Seleccione la materia...</option>');
            $("#select-materia").selectpicker("refresh");
            idEvaluacion = ""; //Reseteando la variable;

            hideProgress();
        }
    });
    $("#select-materia").change(function () {
        if ($(this).val() != "") {
            showProgress();

            idMateria = $(this).val();
            idCurso = $("#select-curso option:selected").val();
            idLapso = $("#select-lapso option:selected").val();

            $("#select-evaluacion").find('option').remove().end().append("<option>Cargando las evaluaciones...</option>");
            $("#select-evaluacion").selectpicker("refresh");

            $.post("/Bridge/ObtenerSelectListEvaluacionesDeCASUS",
            {
                idMateria: idMateria,
                idCurso: idCurso,
                idLapso: idLapso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    var lista = '<option value="">Seleccione la evaluación...</option>';

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idEvaluacion + '">' + data[i].nombre + '</option>');

                    }
                    $("#select-evaluacion").find('option').remove().end().append(lista);
                    $("#select-evaluacion").selectpicker("refresh");

                    hideProgress();
                }
                else {
                    $("#select-evaluacion").find('option').remove().end().append('<option>No se encontraron evaluaciones activas....</option>');
                    $("#select-evaluacion").selectpicker("refresh");

                    hideProgress();
                }
            });
        }
        else {
            $('#select-evaluacion').find('option').remove().end().append('<option>Seleccione la evaluación...</option>');
            $("#select-evaluacion").selectpicker("refresh");
            idEvaluacion = ""; //Reseteando la variable;

            hideProgress();
        }
    });
    $("#select-evaluacion").change(function () {    
        countChange++;

        idCurso = $("#select-curso option:selected").val();
        idLapso = $("#select-lapso option:selected").val();
        idMateria = $("#select-materia option:selected").val();
        idEvaluacion = $(this).val();

        if (idEvaluacion != "")
        {
            showProgress();

            var postData = {
                idCurso: idCurso,
                idLapso: idLapso,
                idMateria: idMateria,
                idEvaluacion: idEvaluacion
            };

            $.ajax({
                type: "POST",
                url: "/Bridge/Estadistica_ObtenerNotasPor_Evaluacion",
                traditional: true,
                data: {
                    idEvaluacion: idEvaluacion,
                },
                success: function (data) {
                    if (data != null && data.length > 0) {
                        $("#ranking-alumnos-div").find('div').remove().end().append(
                            "<div style='height:30em; padding-bottom:8em;' id=morris-bar-rankingalumnos" +
                            countChange + ">" +
                            "</div>");
                        var graphelement = 'morris-bar-rankingalumnos' + countChange;

                        $("#aprovvsrepro-div").find('div').remove().end().append(
                            "<div id=morris-donut-aprovsrepro" + countChange + ">" +
                            "</div>");
                        var graphelement2 = 'morris-donut-aprovsrepro' + countChange;

                        $("#proporcion-notas-evaluacion-div").find('div').remove().end().append(
                          "<div id=proporcion-notas-evaluacion-chart" + countChange + ">" +
                          "</div>");
                        var graphelement3 = 'proporcion-notas-evaluacion-chart' + countChange;

                        $("#ranking-peores-div").find('div').remove().end().append(
                            "<div style='height:30em; padding-bottom:8em;' id=morris-ranking-peores" +
                            countChange + ">" +
                            "</div>");
                        var graphelement4 = 'morris-ranking-peores' + countChange;

                        /*Variables declaradas*/
                        var dataTune = [];
                        var dataTune2 = [];
                        var letras = [];
                        var nota_ = [];
                        var nota_x = [];
                        var coloresBarras = [];
                        var count = 0;
                        var alumnosPasados = 0;
                        var alumnosAplazados = 0;
                        var porcPasados = 0;
                        var porcAplazados = 0;
                        var tope = 0;
                        var dataOP;
                        var dataOT;
                        var alumnosPasadosDescripcion;
                        var alumnosReprobadosDescripcion;
                        var totalAlumnos = data[0].totalAlumnos;
                        
                        //Variables para el gráfico de proporción de notas
                        var cant0 = 0; var cant1 = 0; var cant2 = 0; var cant3 = 0; var cant4 = 0;
                        var cant5 = 0; var cant6 = 0; var cant7 = 0; var cant8 = 0; var cant9 = 0;
                        var cant10 = 0; var cant11 = 0; var cant12 = 0; var cant13 = 0; var cant14 = 0;
                        var cant15 = 0; var cant16 = 0; var cant17 = 0; var cant18 = 0; var cant19 = 0;
                        var cant20 = 0;

                        //Variables para el gráfico de torta (Primaria)
                        var cantA = 0;
                        var cantB = 0;
                        var cantC = 0;
                        var cantD = 0;
                        var cantE = 0;
                        
                        //Definiendo el tope de notas a mostrar
                        if (data.length >= 10)
                            tope = 10;
                        else
                            tope = data.lenght; //Si no hay 10 notas o más para mostrar, el tope es el # de notas actual

                        /******************************* Sección Bachillerato *******************************/
                        if (data[0].grado == "Bachillerato") {
                            /*********** Sección - Gráfico de barras. Mejores resultados ***********/
                            data.sort(SortByCalculoAsc);
                            
                            for (t = 0; t <= tope; t++) {
                                count++;
                                dataOP = {
                                    y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                                    a: Math.round(data[t].nota),
                                    b: count.toString()
                                }                                    
                                dataTune.push(dataOP);
                            }
                            
                            /* Gráfico de barras */
                            Morris.Bar({
                                element: graphelement.toString(),
                                data: dataTune,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Evaluación'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#1DescripciónEvaluacion").html(
                                "Se observa el ranking de los 10 primeros resultados más" +
                                " destacados de la evaluación previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>1. " + dataTune[0].y + ": " + dataTune[0].a +
                                " pts.</strong></br>" +
                                "2. " + dataTune[1].y + ": " + dataTune[1].a + " pts.</br>" +
                                "3. " + dataTune[2].y + ": " + dataTune[2].a + " pts.</br>" +
                                "4. " + dataTune[3].y + ": " + dataTune[3].a + " pts.</br>" +
                                "5. " + dataTune[4].y + ": " + dataTune[4].a + " pts.</br>" +
                                "6. " + dataTune[5].y + ": " + dataTune[5].a + " pts.</br>" +
                                "7. " + dataTune[6].y + ": " + dataTune[6].a + " pts.</br>" +
                                "8. " + dataTune[7].y + ": " + dataTune[7].a + " pts.</br>" +
                                "9. " + dataTune[8].y + ": " + dataTune[8].a + " pts.</br>" +
                                "10. " + dataTune[9].y + ": " + dataTune[9].a + " pts.</br>");

                            /*********** Sección - Gráfico de barras. Peores resultados ***********/
                            data.sort(SortByCalculoDesc);
                            
                            for (t = 0; t <= tope; t++) {
                                count++;
                                dataOT = {
                                    y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                                    a: Math.round(data[t].nota),
                                    b: count.toString()
                                }
                                coloresBarras.push('#FF0000');
                                dataTune2.push(dataOT);
                            }

                            /* Gráfico de barras */
                            Morris.Bar({
                                element: graphelement4,
                                data: dataTune2,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Evaluación'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                barColors: coloresBarras,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#2DescripciónEvaluacion").html(
                                "Se observa el ranking de los 10 resultados más deficientes de la evaluación " +
                                "previamente seleccionada <strong>(En este gráfico no se están involucrando aquellos" +
                                " alumnos sin notas cargadas)</strong>." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: red'>1. " + dataTune2[0].y + ": " + dataTune2[0].a +
                                " pts.</strong></br>" +
                                "2. " + dataTune2[1].y + ": " + dataTune2[1].a + " pts.</br>" +
                                "3. " + dataTune2[2].y + ": " + dataTune2[2].a + " pts.</br>" +
                                "4. " + dataTune2[3].y + ": " + dataTune2[3].a + " pts.</br>" +
                                "5. " + dataTune2[4].y + ": " + dataTune2[4].a + " pts.</br>" +
                                "6. " + dataTune2[5].y + ": " + dataTune2[5].a + " pts.</br>" +
                                "7. " + dataTune2[6].y + ": " + dataTune2[6].a + " pts.</br>" +
                                "8. " + dataTune2[7].y + ": " + dataTune2[7].a + " pts.</br>" +
                                "9. " + dataTune2[8].y + ": " + dataTune2[8].a + " pts.</br>" +
                                "10. " + dataTune2[9].y + ": " + dataTune2[9].a + " pts.</br>");

                            /*********** Sección - Gráfico de torta. Aprobados vs Reprobados ***********/
                            for (t = 0; t <= data.length - 1; t++) {
                                if (data[t].nota >= 10)
                                    alumnosPasados++;
                            }

                            porcPasados = Math.round(alumnosPasados * 100 / totalAlumnos);
                            porcAplazados = Math.round((totalAlumnos - alumnosPasados) * 100 / totalAlumnos);

                            alumnosPasadosDescripcion = Math.round(porcPasados * totalAlumnos / 100);
                            alumnosReprobadosDescripcion = Math.round(porcAplazados * totalAlumnos / 100);

                            /* Gráfico de torta */
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

                            $("#3DescripciónEvaluacion").html(
                                "Se presenta el porcentaje de estudiantes aprobados y aplazados, en la" +
                                " evaluación previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>Aprobados: " + porcPasados + "% => " +
                                alumnosPasadosDescripcion + " de " + totalAlumnos + " alumnos." +
                                "</strong></br>" +

                                "<strong style='color: red'>Aplazados: " + porcAplazados + "% => " +
                                alumnosReprobadosDescripcion + " de " + totalAlumnos + " alumnos." +
                                "</strong></br>");

                            /*********** Sección - Gráfico de torta. Proporción de notas ***********/
                            for (t = 0; t <= data.length - 1; t++) {
                                if (data[t].nota.toString() == "0" || data[t].nota.toString() == "00")
                                    cant0++;
                                else if (data[t].nota.toString() == "1" || data[t].nota.toString() == "01")
                                    cant1++;
                                else if (data[t].nota.toString() == "2" || data[t].nota.toString() == "02")
                                    cant2++;
                                else if (data[t].nota.toString() == "3" || data[t].nota.toString() == "03")
                                    cant3++;
                                else if (data[t].nota.toString() == "4" || data[t].nota.toString() == "04")
                                    cant4++;
                                else if (data[t].nota.toString() == "5" || data[t].nota.toString() == "05")
                                    cant5++;
                                else if (data[t].nota.toString() == "6" || data[t].nota.toString() == "06")
                                    cant6++;
                                else if (data[t].nota.toString() == "7" || data[t].nota.toString() == "07")
                                    cant7++;
                                else if (data[t].nota.toString() == "8" || data[t].nota.toString() == "08")
                                    cant8++;
                                else if (data[t].nota.toString() == "9" || data[t].nota.toString() == "09")
                                    cant9++;
                                else if (data[t].nota.toString() == "10")
                                    cant10++;
                                else if (data[t].nota.toString() == "11")
                                    cant11++;
                                else if (data[t].nota.toString() == "12")
                                    cant12++;
                                else if (data[t].nota.toString() == "13")
                                    cant13++;
                                else if (data[t].nota.toString() == "14")
                                    cant14++;
                                else if (data[t].nota.toString() == "15")
                                    cant15++;
                                else if (data[t].nota.toString() == "16")
                                    cant16++;
                                else if (data[t].nota.toString() == "17")
                                    cant17++;
                                else if (data[t].nota.toString() == "18")
                                    cant18++;
                                else if (data[t].nota.toString() == "19")
                                    cant19++;
                                else if (data[t].nota.toString() == "20")
                                    cant20++;
                            }

                            /* Gráfico de torta */
                            Morris.Donut({
                                element: graphelement3,
                                data: [
                                    { label: "Alumnos con 20", value: cant20 },
                                    { label: "Alumnos con 19", value: cant19 },
                                    { label: "Alumnos con 18", value: cant18 },
                                    { label: "Alumnos con 17", value: cant17 },
                                    { label: "Alumnos con 16", value: cant16 },
                                    { label: "Alumnos con 15", value: cant15 },
                                    { label: "Alumnos con 14", value: cant14 },
                                    { label: "Alumnos con 13", value: cant13 },
                                    { label: "Alumnos con 12", value: cant12 },
                                    { label: "Alumnos con 11", value: cant11 },
                                    { label: "Alumnos con 10", value: cant10 },
                                    { label: "Alumnos con 9", value: cant9 },
                                    { label: "Alumnos con 8", value: cant8 },
                                    { label: "Alumnos con 7", value: cant7 },
                                    { label: "Alumnos con 6", value: cant6 },
                                    { label: "Alumnos con 5", value: cant5 },
                                    { label: "Alumnos con 4", value: cant4 },
                                    { label: "Alumnos con 3", value: cant3 },
                                    { label: "Alumnos con 2", value: cant2 },
                                    { label: "Alumnos con 1", value: cant1 },
                                    { label: "Alumnos sin notas", value: cant0 }],
                                colors: ["#000099", "#0000CC", "#0000FF", "#3333FF", "#004C99", "#0066CC", "#0080FF",
                                    "#3399FF", "#66B2FF", "#99CCFF", "#CCE5FF", "#FFCCFF", "#FFCCCC", "#FFCC99",
                                    "#FFB266", "#FF99FF", "#FF9999", "#FF6666", "#FF3333", "#F92525", "#FF0000"],
                                resize: true
                            });

                            $("#4DescripciónEvaluacion").html(
                                "Se observa la proporción de notas alcanzadas por los estudiantes en la" +
                                " evaluación previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "20 pts.: " + cant20 + " Alumnos </br> 19 pts.: " + cant19 + " Alumnos </br>" +
                                "18 pts.: " + cant18 + " Alumnos </br> 17 pts.: " + cant17 + " Alumnos </br>" +
                                "16 pts.: " + cant16 + " Alumnos </br> 15 pts.: " + cant15 + " Alumnos </br>" +
                                "14 pts.: " + cant14 + " Alumnos </br> 13 pts.: " + cant13 + " Alumnos </br>" +
                                "12 pts.: " + cant12 + " Alumnos </br> 11 pts.: " + cant11 + " Alumnos </br>" +
                                "10 pts.: " + cant10 + " Alumnos </br> 09 pts.: " + cant9 + " Alumnos </br>" +
                                "08 pts.: " + cant8 + " Alumnos </br> 07 pts.: " + cant7 + " Alumnos </br>" +
                                "06 pts.: " + cant6 + " Alumnos </br> 05 pts.: " + cant5 + " Alumnos </br>" +
                                "04 pts.: " + cant4 + " Alumnos </br> 03 pts.: " + cant3 + " Alumnos </br>" +
                                "02 pts.: " + cant2 + " Alumnos </br> 01 pts.: " + cant1 + " Alumnos </br>");
                            hideProgress();
                        }

                        /******************************* Sección Primaria *******************************/
                        else {
                            /*********** Sección - Gráfico de barras. Mejores resultados ***********/
                            for (t = 0; t <= tope; t++) {
                                count++;

                                if (data[t].nota == "A") {
                                    nota_.push(5);
                                    letras.push("A");
                                }
                                else if (data[t].nota == "B") {
                                    nota_.push(4);
                                    letras.push("B");
                                }
                                else if (data[t].nota == "C") {
                                    nota_.push(3);
                                    letras.push("C");
                                }
                                else if (data[t].nota == "D") {
                                    nota_.push(2);
                                    letras.push("D");
                                }
                                else if (data[t].nota == "E") {
                                    nota_.push(1);
                                    letras.push("E");
                                }

                                dataOP = {
                                    y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                                    a: Math.round(nota_[t]),
                                    b: count.toString()
                                }
                                dataTune.push(dataOP);
                            }
                            
                            /*Gráfico de barras*/
                            Morris.Bar({
                                element: graphelement.toString(),
                                data: dataTune,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Evaluación'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#1DescripciónEvaluacion").html(
                                "Se observa el ranking de los 10 primeros resultados más" +
                                " destacados de la evaluación previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>1. " + dataTune[0].y + ": " + letras[0] + "</strong></br>" +
                                "2. " + dataTune[1].y + ": " + letras[1] + "</br>" +
                                "3. " + dataTune[2].y + ": " + letras[2] + "</br>" +
                                "4. " + dataTune[3].y + ": " + letras[3] + "</br>" +
                                "5. " + dataTune[4].y + ": " + letras[4] + "</br>" +
                                "6. " + dataTune[5].y + ": " + letras[5] + "</br>" +
                                "7. " + dataTune[6].y + ": " + letras[6] + "</br>" +
                                "8. " + dataTune[7].y + ": " + letras[7] + "</br>" +
                                "9. " + dataTune[8].y + ": " + letras[8] + "</br>" +
                                "10. " + dataTune[9].y + ": " + letras[9] + "</br>");

                            /*********** Sección - Gráfico de barras. Peores resultados ***********/
                            data.sort(SortByCalculoDescPrimaria);

                            letras = []; //Inicializando la variable
                            count = 0; //Inicializando la variable
                            
                            if (data.length >= 10) {
                                for (t = 0; t <= 10 - 1; t++) {
                                    count++;

                                    if (data[t].nota == "A" || data[t].nota == "a") {
                                        nota_x.push(5);
                                        letras.push("A");
                                    } else
                                        if (data[t].nota == "B" || data[t].nota == "b") {
                                            nota_x.push(4);
                                            letras.push("B");
                                        } else
                                            if (data[t].nota == "C" || data[t].nota == "c") {
                                                nota_x.push(3);
                                                letras.push("C");
                                            } else
                                                if (data[t].nota == "D" || data[t].nota == "d") {
                                                    nota_x.push(2);
                                                    letras.push("C");
                                                } else
                                                    if (data[t].nota == "E" || data[t].nota == "e") {
                                                        nota_x.push(1);
                                                        letras.push("E");
                                                    }
                                    dataOT = {
                                        y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                                        a: Math.round(nota_x[t]),
                                        b: count.toString()
                                    }
                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            } else {
                                for (t = 0; t <= data.length - 1; t++) {
                                    count++;
                                    if (data[t].nota == "A" || data[t].nota == "a") {
                                        nota_x.push(5);
                                        letras.push("A");
                                    } else
                                        if (data[t].nota == "B" || data[t].nota == "b") {
                                            nota_x.push(4);
                                            letras.push("B");
                                        } else
                                            if (data[t].nota == "C" || data[t].nota == "c") {
                                                nota_x.push(3);
                                                letras.push("C");
                                            } else
                                                if (data[t].nota == "D" || data[t].nota == "d") {
                                                    nota_x.push(2);
                                                    letras.push("D");
                                                } else
                                                    if (data[t].nota == "E" || data[t].nota == "e") {
                                                        nota_x.push(1);
                                                        letras.push("E");
                                                    }
                                    dataOT = {
                                        y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                                        a: Math.round(nota_x[t]),
                                        b: count.toString()
                                    }
                                    coloresBarras.push('#FF0000');
                                    dataTune2.push(dataOT);
                                }
                            }


                            Morris.Bar({
                                element: graphelement4,
                                data: dataTune2,
                                xkey: 'y',
                                ykeys: ['a'],
                                labels: ['Nota Evaluación'],
                                hideHover: 'true',
                                xLabelAngle: 73,
                                resize: true,
                                barColors: coloresBarras,
                                gridTextSize: 15,
                                gridTextColor: "#000000"
                            });

                            $("#2DescripciónEvaluacion").html(
                                "Se observa el ranking de los 10 resultados más deficientes de la evaluación " +
                                "previamente seleccionada <strong>(En este gráfico no se están involucrando " +
                                "aquellos alumnos sin notas cargadas)</strong>." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: red'>1. " + dataTune[0].y + ": " + letras[0] + "</strong></br>" +
                                "2. " + dataTune[1].y + ": " + letras[1] + "</br>" +
                                "3. " + dataTune[2].y + ": " + letras[2] + "</br>" +
                                "4. " + dataTune[3].y + ": " + letras[3] + "</br>" +
                                "5. " + dataTune[4].y + ": " + letras[4] + "</br>" +
                                "6. " + dataTune[5].y + ": " + letras[5] + "</br>" +
                                "7. " + dataTune[6].y + ": " + letras[6] + "</br>" +
                                "8. " + dataTune[7].y + ": " + letras[7] + "</br>" +
                                "9. " + dataTune[8].y + ": " + letras[8] + "</br>" +
                                "10. " + dataTune[9].y + ": " + letras[9] + "</br>");

                            for (t = 0; t <= data.length - 1; t++) {
                                if (data[t].nota != "E")
                                    alumnosPasados++;
                            }

                            var porcAplazados = Math.round((totalAlumnos - alumnosPasados) * 100 / totalAlumnos);
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

                            $("#3DescripciónEvaluacion").html(
                                "Se presenta el porcentaje de estudiantes aprobados y aplazados, en la" +
                                " evaluación previamente seleccionada." +
                                "</br>------------------------------------------------------</br>" +
                                "<strong style='color: blue'>Aprobados: " + porcPasados + "% => " +
                                alumnosPasadosDescripcion + " de " + totalAlumnos + " alumnos." + "</strong></br>" +

                                "<strong style='color: red'>Reprobados: " + porcAplazados + "% => " +
                                alumnosReprobadosDescripcion + " de " + totalAlumnos + " alumnos." + "</strong></br>");

                            /**************** Sección - Gráfico de torta. Proporción de notas ************/                            
                            for (t = 0; t <= data.length - 1; t++) {
                                if (data[t].nota == "A") cantA++;
                                if (data[t].nota == "B") cantB++;
                                if (data[t].nota == "C") cantC++;
                                if (data[t].nota == "D") cantD++;
                                if (data[t].nota == "E") cantE++;
                            }

                            Morris.Donut({
                                element: graphelement3,
                                data: [
                                    { label: "Alumnos con A", value: cantA },
                                    { label: "Alumnos con B", value: cantB },
                                    { label: "Alumnos con C", value: cantC },
                                    { label: "Alumnos con D", value: cantD },
                                    { label: "Alumnos con E", value: cantE }],
                                colors: ["#0000FF", "#66B2FF", "#CCE5FF", "#FF9999", "#FF0000"],
                                resize: true
                            });

                            $("#4DescripciónEvaluacion").html("Se observa la proporción de notas de los estudiantes " +
                                                     "en la evaluación previamente seleccionada." + "</br>" +
                                                     "------------------------------------------------------</br>" +
                                                     "Literal A: " + cantA + " Alumnos </br>" +
                                                     "Literal B: " + cantB + " Alumnos </br>" +
                                                     "Literal C: " + cantC + " Alumnos </br>" +
                                                     "Literal D: " + cantD + " Alumnos </br>" +
                                                     "Literal E: " + cantE + " Alumnos </br>");
                            hideProgress();
                        }
                    }
                    else {

                        $("#ranking-alumnos-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                          "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + ">" +
                          "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#aprovvsrepro-div").find('div').remove().end().append("<div class='centrar' id=morris-donut-aprovsrepro" +
                          countChange + ">" + "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#proporcion-notas-evaluacion-div").find('div').remove().end().append(
                          "<div class='centrar' id=proporcion-notas-evaluacion-chart" + countChange + ">" +
                          "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#ranking-peores-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                          "padding-bottom:8em;' id=morris-ranking-peores" + countChange + ">" +
                          "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
                        $("#1DescripciónEvaluacion").html(" ");
                        $("#2DescripciónEvaluacion").html(" ");
                        $("#3DescripciónEvaluacion").html(" ");
                        $("#4DescripciónEvaluacion").html(" ");

                        hideProgress();
                        swal("¡No existen notas en esta evaluación!", "No existe ninguna nota cargada para la evaluación" +
                        " seleccionada.", "warning");
                    }

                }
            });
        }
        else
            idEvaluacion = ""; //Reseteando la variable;
    });
});