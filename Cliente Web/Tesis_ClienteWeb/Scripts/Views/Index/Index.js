var idEvaluacion;
var countChange = 0;

function SortByCalculoAsc(x, y) {
    return y.nota - x.nota;
}

function SortByCalculoDesc(x, y) {
    return x.nota - y.nota;
}

$(document).ready(function () {
    showProgress();
      
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/Bridge/Estadistica_ObtenerNotasUltimaEvaluacionPor_Profesor",
        success: function (data) {
            if (data[0].success) {
                idEvaluacion = data[0].idEvaluacion; //Asociando valores de variables para los reportes

                $("#morris-bar-AprovvsReprovInicio-div").find('div').remove().end().append(
                "<div>" +
                    "<p>" + data[0].curso + ". " + data[0].materia +
                        ": " + data[0].nombreevaluacion +
                    "</p>" +

                    "<div class=col-lg-12>" +
                        "<div class=separador>" +
                    "</div>" +
                "</div>" +

                "<div style='height:30em; padding-bottom:8em;' id=morris-bar-AprovvsReprovInicio" + countChange + ">" +
                "</div>");

                var graphelement = 'morris-bar-AprovvsReprovInicio' + countChange;

                $("#morris-bar-rankingalumnos-div").find('div').remove().end().append(
               "<p>" + data[0].curso + " - " + data[0].materia +
               ": " + data[0].nombreevaluacion + "</p><div class=col-lg-12><div class=separador></div>" +
               "</div><div style='height:30em;" +
               "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + "></div>");

                var graphelement2 = 'morris-bar-rankingalumnos' + countChange;

                /*Variables declaradas*/
                var totalAlumnos = data[0].totalAlumnos;
                var dataTune = [];
                var dataOP;
                var count = 0;
                var alumnosPasados = 0;
                var porcPasados = 0;
                var porcAplazados = 0;
                var letras = [];
                var nota_ = [];
                var tope = 0;

                //Definiendo el tope de notas a mostrar
                if (data.length >= 10) 
                    tope = 10;
                else
                    tope = data.lenght; //Si no hay 10 notas o más para mostrar, el tope es el nro de notas actual

                /*********************************** Sección Bachillerato ***********************************/
                if (data[0].grado == "Bachillerato") {
                    /**************** Sección - Gráfico de torta ****************/
                    for (t = 0; t <= data.length - 1; t++) {
                        if (data[t].nota >= 10) 
                            alumnosPasados++;
                    }

                    porcPasados = Math.round(alumnosPasados * 100 / totalAlumnos);
                    porcAplazados = Math.round((totalAlumnos - alumnosPasados) * 100 / totalAlumnos);

                    /*Gráfico de torta*/
                    Morris.Donut({
                        element: graphelement,
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
                    
                    /***************** Sección - Gráfico de barras *****************/
                    data.sort(SortByCalculoAsc);
                    
                    for (t = 0; t <= tope - 1; t++) {
                        count++;
                        dataOP = {
                            y: data[t].alumnoApellido1 + ", " + data[t].alumnoNombre1,
                            a: Math.round(data[t].nota),
                            b: count.toString()
                        }
                        dataTune.push(dataOP);
                    }

                    /*Gráfico de barra*/
                    Morris.Bar({
                        element: graphelement2.toString(),
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

                    hideProgress();
                }

                /************************************ Sección Primaria ************************************/
                else {
                    /**************** Sección - Gráfico de torta ****************/
                    for (t = 0; t <= data.length - 1; t++) {
                        if (data[t].nota != "E") //Aprobados
                            alumnosPasados++;
                    }

                    porcPasados = Math.round(alumnosPasados * 100 / totalAlumnos);
                    porcAplazados = Math.round((totalAlumnos - alumnosPasados) * 100 / totalAlumnos);

                    /*Gráfico de torta*/
                    Morris.Donut({
                        element: graphelement,
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

                    /**************** Sección - Gráfico de barras ****************/                    
                    for (t = 0; t <= tope - 1; t++) {
                        count++;

                        if (data[t].nota == "A") {
                            nota_.push(5);
                            letras.push("A");
                        } else if (data[t].nota == "B") {
                            nota_.push(4);
                            letras.push("B");
                        } else if (data[t].nota == "C") {
                            nota_.push(3);
                            letras.push("C");
                        } else if (data[t].nota == "D") {
                            nota_.push(2);
                            letras.push("D");
                        } else if (data[t].nota == "E") {
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
                        element: graphelement2.toString(),
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

                    hideProgress();
                }
            }
            else {
                $("#morris-bar-AprovvsReprovInicio-div").find('div').remove().end().append(
                    "<p>No existe ninguna evaluación con notas cargadas</p>" +

                    "<div class=col-lg-12>" +
                        "<div class=separador></div>" +
                    "</div>" +

                    "<div class='centrar' style='height:30em; padding-bottom:8em;' " +
                    "id=morris-bar-AprovvsReprovInicio" + countChange + ">" +
                        "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" +
                    "</div>");

                $("#morris-bar-rankingalumnos-div").find('div').remove().end().append(
                    "<p>No existe ninguna evaluación con notas cargadas</p>" +

                    "<div class=col-lg-12>" +
                        "<div class=separador></div>" +
                    "</div>" +

                    "<div class='centrar' style='height:30em; padding-bottom:8em;' " +
                    "id=morris-bar-rankingalumnos" + countChange + ">" +
                        "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" +
                    "</div>");

                hideProgress();
            }
        },
        error: function (data) {
            swal("Error en carga de estadísticas", "Ha ocurrido un error en la carga de las estadísticas," +
                " por favor actualice la página", "error");
            hideProgress();
        },
    });
});