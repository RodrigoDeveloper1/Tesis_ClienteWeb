var idCurso = "";
var idLapso;
var idMateria;
var idEvaluacion;
var countChange = 0;
function SortByPromedioAsc(x, y) {
    return y.promedio - x.promedio;
}
function SortByPromedioDesc(x, y) {
    return x.promedio - y.promedio;
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
        showProgress();
        countChange++;
        idCurso = $(this).val();

        if ($(this).val() != "") {
            $.post("/Bridge/Estadistica_RendimientoAcademicoPor_Materia_Lapso",
            {
                idCurso: idCurso
            },
            function (data) {
                if (data != null && data.length > 0) {
                    $("#ranking-alumnos-div").find('div').remove().end().append("<div style='height:30em;" +
                     "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + "></div>");
                    var graphelement = 'morris-bar-rankingalumnos' + countChange;

                    $("#ranking-peores-div").find('div').remove().end().append("<div style='height:30em;" +
                     "padding-bottom:8em;' id=morris-ranking-peores" + countChange + "></div>");
                    var graphelement2 = 'morris-ranking-peores' + countChange;

                    $("#ranking-peores-div2").find('div').remove().end().append("<div style='height:30em;" +
                     "padding-bottom:8em;' id=morris-ranking-peores2" + countChange + "></div>");
                    var graphelement3 = 'morris-ranking-peores2' + countChange;

                    $("#ranking-peores-div3").find('div').remove().end().append("<div style='height:30em;" +
                     "padding-bottom:8em;' id=morris-ranking-peores3" + countChange + "></div>");
                    var graphelement4 = 'morris-ranking-peores3' + countChange;

                    /*Declaración de variables*/
                    if (data[0].grado == "Bachillerato") {

                        var descripción = "";
                        var llaves = [];
                        var labels = [];
                        var dataTune = [];
                        var dataOp = {};
                        var dataOp2 = {};
                        var dataOp3 = {};
                        var count = 1;
                        var count2 = 0;
                        var count3 = 0;
                        var arrayLetras = ['a','b','c','d','e','f','g','h','i','j','k','l','m','n']
                        var matPrimerLapso = [];
                        var matSegundoLapso = [];
                        var matTercerLapso = [];

                        dataOp.y = "1er Lapso";
                        dataOp2.y = "2do Lapso";
                        dataOp3.y = "3er Lapso";

                        for (h = 0; h <= data.length - 1; h++) {
                            if (data[h].lapso == "1er Lapso") {
                                matPrimerLapso.push(data[h]);
                                count++;

                                if (count == 1) {
                                    dataOp.a = Math.round(data[h].promedio);
                                    descripción+= data[h].materia+": "+Math.round(data[h].promedio) +"</br>";
                                }
                                else if (count == 2) {
                                    dataOp.b = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 3) {
                                    dataOp.c = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 4) {
                                    dataOp.d = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                else if (count == 5) {
                                    dataOp.e = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                else if (count == 6) {
                                    dataOp.f = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 7) {
                                    dataOp.g = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 8) {
                                    dataOp.h = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 9) {
                                    dataOp.i = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 10) {
                                    dataOp.j = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 11) {
                                    dataOp.k = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 12) {
                                    dataOp.l = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 13) {
                                    dataOp.m = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count == 14) {
                                    dataOp.n = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }

                                labels.push(data[h].materia);
                            }
                       
                            if (data[h].lapso == "2do Lapso") {
                                matSegundoLapso.push(data[h]);

                                if (count2 == 0) {
                                    descripción += "------------------------------------------------------</br>";
                                    descripción += "2do Lapso: </br>";

                                }
                                count2++;

                                if (count2 == 1) {
                                    dataOp2.a = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }                        
                                else if(count2 == 2){
                                        dataOp2.b = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                else if (count2 == 3){
                                    dataOp2.c = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                }
                                else if (count2 == 4) {
                                        dataOp2.d = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 5){
                                        dataOp2.e = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 6){
                                        dataOp2.f = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 7){
                                        dataOp2.g = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 8){
                                        dataOp2.h = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 9){
                                        dataOp2.i = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                        else if (count2 == 10){
                                        dataOp2.j = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 11){
                                        dataOp2.k = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 12){
                                        dataOp2.l = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 13){
                                        dataOp2.m = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count2 == 14){
                                        dataOp2.n = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                            }
                    
                                if (data[h].lapso == "3er Lapso") {
                               
                                    matTercerLapso.push(data[h]);
                                    if (count3 == 0) {
                                        descripción += "------------------------------------------------------</br>";                                                   
                                        descripción += "3er Lapso: </br>";
                                        descripción += "------------------------------------------------------</br>";

                                    }
                                    count3++;
                                    if (count3 == 1){
                                        dataOp3.a = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 2){
                                        dataOp3.b = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 3){
                                        dataOp3.c = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 4){
                                        dataOp3.d = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 5){
                                        dataOp3.e = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 6){
                                        dataOp3.f = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 7){
                                        dataOp3.g = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 8){
                                        dataOp3.h = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 9){
                                        dataOp3.i = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 10){
                                        dataOp3.j = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 11){
                                        dataOp3.k = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 12){
                                        dataOp3.l = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 13){
                                        dataOp3.m = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                    else if (count3 == 14){
                                        dataOp3.n = Math.round(data[h].promedio);
                                        descripción += data[h].materia + ": " + Math.round(data[h].promedio) + "</br>";
                                    }
                                }
                            
                            }
                        dataTune.push(dataOp);
                        dataTune.push(dataOp2);
                        dataTune.push(dataOp3);
                        
                        for (i = 0; i < count; i++)
                        {
                            llaves.push(arrayLetras[i])
                        }

                        Morris.Bar({
                            element: graphelement.toString(),
                            data: dataTune,
                            xkey: 'y',
                            ykeys: llaves,
                            labels: labels,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#1DescripciónCurso").html(
                            "Se observa el rendimiento académico por cada una de las materias " +
                            "del curso previamente seleccionado." + "</br>" +
                            "------------------------------------------------------</br>" +
                            "1er Lapso </br>" +
                            "------------------------------------------------------</br>" +
                            descripción);



                        var descripción2 = "";
                        var llaves2 = [];
                        var labels2 = [];
                        var dataTune2 = [];
                        var dataOp4 = {};
                        dataOp4.y = "1er Lapso";
                        count = 0;
                        matPrimerLapso.sort(SortByPromedioAsc);
                        for (w = 0; w <= matPrimerLapso.length - 1; w++) {
                            count++;

                            if (count == 1) {
                                dataOp4.a = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 2){
                                dataOp4.b = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 3){
                                dataOp4.c = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 4){
                                dataOp4.d = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 5){
                                dataOp4.e = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 6){
                                dataOp4.f = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 7){
                                dataOp4.g = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 8){
                                dataOp4.h = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 9){
                                dataOp4.i = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 10){
                                dataOp4.j = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 11){
                                dataOp4.k = Math.round(matPrimerLapso[w].promedio);
                                descripción += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 12){
                                dataOp4.l = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 13){
                                dataOp4.m = Math.round(matPrimerLapso[w].promedio);
                                descripción += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                            }
                            else if (count == 14){
                                dataOp4.n = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + Math.round(matPrimerLapso[w].promedio) + "</br>";
                        }
                            labels2.push(matPrimerLapso[w].materia);
                        }
                        dataTune2.push(dataOp4);

                        for (i = 0; i < count; i++) {
                            llaves2.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement2.toString(),
                            data: dataTune2,
                            xkey: 'y',
                            ykeys: llaves2,
                            labels: labels2,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#2DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                       "del curso previamente seleccionado." + "</br>" +
                                                       "------------------------------------------------------</br>" +
                                                       "1er Lapso </br>" +
                                                       "------------------------------------------------------</br>" +
                                                       descripción2);
                        var descripción3 = "";
                        var llaves3 = [];
                        var labels3 = [];
                        var dataTune3 = [];
                        var dataOp5 = {};
                        dataOp5.y = "2do Lapso";
                        count2 = 0;
                        matSegundoLapso.sort(SortByPromedioAsc);
                   
                        for (w = 0; w <= matSegundoLapso.length - 1; w++) {
                            count2++;

                            if (count == 1){
                                dataOp5.a = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 2){
                                dataOp5.b = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 3){
                                dataOp5.c = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 4){
                                dataOp5.d = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 5){
                                dataOp5.e = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 6){
                                dataOp5.f = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 7){
                                dataOp5.g = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2== 8){
                                dataOp5.h = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 9){
                                dataOp5.i = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 10){
                                dataOp5.j = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 11){
                                dataOp5.k = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 12){
                                dataOp5.l = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 13){
                                dataOp5.m = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            else if (count2 == 14){
                                dataOp5.n = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + Math.round(matSegundoLapso[w].promedio) + "</br>";
                            }
                            labels3.push(matSegundoLapso[w].materia);
                        }
                        dataTune3.push(dataOp5);

                        for (i = 0; i < count2; i++) {
                            llaves3.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement3.toString(),
                            data: dataTune3,
                            xkey: 'y',
                            ykeys: llaves3,
                            labels: labels3,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });
                        $("#3DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                      "del curso previamente seleccionado." + "</br>" +
                                                      "------------------------------------------------------</br>" +
                                                      "2do Lapso </br>" +
                                                      "------------------------------------------------------</br>" +
                                                      descripción3);

                        var descripción4 = "";
                        var llaves4 = [];
                        var labels4 = [];
                        var dataTune4 = [];
                        var dataOp6 = {};
                        dataOp6.y = "3er Lapso";
                        count3 = 0;
                        matTercerLapso.sort(SortByPromedioAsc);

                        for (w = 0; w <= matTercerLapso.length - 1; w++) {
                            count3++;

                            if (count3 == 1){
                                dataOp6.a = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 2){
                                dataOp6.b = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 3){
                                dataOp6.c = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 4){
                                dataOp6.d = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 5){
                                dataOp6.e = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 6){
                                dataOp6.f = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 7){
                                dataOp6.g = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 8){
                                dataOp6.h = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 9){
                                dataOp6.i = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 10){
                                dataOp6.j = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 11){
                                dataOp6.k = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 12){
                                dataOp6.l = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 13){
                                dataOp6.m = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            else if (count3 == 14){
                                dataOp6.n = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + Math.round(matTercerLapso[w].promedio) + "</br>";
                            }
                            labels4.push(matTercerLapso[w].materia);
                        }
                        dataTune4.push(dataOp6);

                        for (i = 0; i < count3; i++) {
                            llaves4.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement4.toString(),
                            data: dataTune4,
                            xkey: 'y',
                            ykeys: llaves4,
                            labels: labels4,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#4DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                     "del curso previamente seleccionado." + "</br>" +
                                                     "------------------------------------------------------</br>" +
                                                     "3er Lapso </br>" +
                                                     "------------------------------------------------------</br>" +
                                                     descripción4);

                        hideProgress();
                    }else
                    {

                        var dataTune = [];
                        var llaves = [];
                        var labels = [];
                        var dataObject = {};
                        var dataOp = {};
                        var dataOp2 = {};
                        var dataOp3 = {};
                        var count = 0;
                        var count2 = 0;
                        var count3 = 0;
                        var arrayLetras = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n']
                        var matPrimerLapso = [];
                        var matSegundoLapso = [];
                        var matTercerLapso = [];
                        var descripción = "";
                        dataOp.y = "1er Lapso";
                        dataOp2.y = "2do Lapso";
                        dataOp3.y = "3er Lapso";
                        for (h = 0; h <= data.length - 1; h++) {

                            if (data[h].lapso == "1er Lapso") {
                                count++;
                                matPrimerLapso.push(data[h]);
                                if (count == 1){
                                    dataOp.a = Math.round(data[h].promedio); 
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 2) {
                                    dataOp.b = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 3){
                                    dataOp.c = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 4){
                                    dataOp.d = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 5){
                                    dataOp.e = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 6){
                                    dataOp.f = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 7){
                                    dataOp.g = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 8){
                                    dataOp.h = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 9){
                                    dataOp.i = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 10){
                                    dataOp.j = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 11){
                                    dataOp.k = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 12){
                                    dataOp.l = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 13){
                                    dataOp.m = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count == 14){
                                    dataOp.n = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                labels.push(data[h].materia);
                            }

                            if (data[h].lapso == "2do Lapso") {
                                matSegundoLapso.push(data[h]);
                                if (count2 == 0) {
                                    descripción += "------------------------------------------------------</br>";
                                    descripción += "2do Lapso: </br>";
                                    descripción += "------------------------------------------------------</br>"

                                }
                                count2++;
                                if (count2 == 1){
                                    dataOp2.a = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 2){
                                    dataOp2.b = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 3){
                                    dataOp2.c = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 4){
                                    dataOp2.d = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 5){
                                    dataOp2.e = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 6){
                                    dataOp2.f = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 7){
                                    dataOp2.g = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 8){
                                    dataOp2.h = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 9){
                                    dataOp2.i = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 10){
                                    dataOp2.j = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 11){
                                    dataOp2.k = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 12){
                                    dataOp2.l = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 13){
                                    dataOp2.m = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count2 == 14){
                                    dataOp2.n = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                            }

                            if (data[h].lapso == "3er Lapso") {
                                matTercerLapso.push(data[h]);
                                if (count3 == 0) {
                                    descripción += "------------------------------------------------------</br>";
                                    descripción += "3er Lapso: </br>";
                                    descripción += "------------------------------------------------------</br>"

                                }
                                count3++;
                                if (count3 == 1){
                                    dataOp3.a = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 2){
                                    dataOp3.b = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 3){
                                    dataOp3.c = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 4){
                                    dataOp3.d = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 5){
                                    dataOp3.e = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 6){
                                    dataOp3.f = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 7){
                                    dataOp3.g = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 8){
                                    dataOp3.h = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 9){
                                    dataOp3.i = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 10){
                                    dataOp3.j = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 11){
                                    dataOp3.k = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 12){
                                    dataOp3.l = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 13){
                                    dataOp3.m = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                                else if (count3 == 14){
                                    dataOp3.n = Math.round(data[h].promedio);
                                    descripción += data[h].materia + ": " + data[h].promedio_literal + "</br>";
                                }
                            }

                        }
                        dataTune.push(dataOp);
                        dataTune.push(dataOp2);
                        dataTune.push(dataOp3);
                        for (i = 0; i < count; i++) {
                            llaves.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement.toString(),
                            data: dataTune,
                            xkey: 'y',
                            ykeys: llaves,
                            labels: labels,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#1DescripciónCurso").html("Se observa el rendimiento académico por cada una de las materias" +
                                                  "del curso previamente seleccionado." + "</br>" +
                                                  "------------------------------------------------------</br>" +
                                                  "1er Lapso </br>" +
                                                  "------------------------------------------------------</br>" +
                                                  descripción);
                        var descripción2 = "";
                        var llaves2 = [];
                        var labels2 = [];
                        var dataTune2 = [];
                        var dataOp4 = {};
                        dataOp4.y = "1er Lapso";
                        count = 0;
                        matPrimerLapso.sort(SortByPromedioAsc);
                        for (w = 0; w <= matPrimerLapso.length - 1; w++) {
                            count++;

                            if (count == 1){
                                dataOp4.a = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                        }
                            else if (count == 2){
                                dataOp4.b = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 3){
                                dataOp4.c = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 4){
                                dataOp4.d = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 5){
                                dataOp4.e = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 6){
                                dataOp4.f = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 7){
                                dataOp4.g = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 8){
                                dataOp4.h = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 9){
                                dataOp4.i = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 10){
                                dataOp4.j = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 11){
                                dataOp4.k = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 12){
                                dataOp4.l = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 13){
                                dataOp4.m = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count == 14){
                                dataOp4.n = Math.round(matPrimerLapso[w].promedio);
                                descripción2 += matPrimerLapso[w].materia + ": " + matPrimerLapso[w].promedio_literal + "</br>";
                            }
                            labels2.push(matPrimerLapso[w].materia);
                        }
                        dataTune2.push(dataOp4);

                        for (i = 0; i < count; i++) {
                            llaves2.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement2.toString(),
                            data: dataTune2,
                            xkey: 'y',
                            ykeys: llaves2,
                            labels: labels2,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#2DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                      "del curso previamente seleccionado." + "</br>" +
                                                      "------------------------------------------------------</br>" +
                                                      "1er Lapso </br>" +
                                                      "------------------------------------------------------</br>" +
                                                      descripción2);
                        var descripción3 = "";
                        var llaves3 = [];
                        var labels3 = [];
                        var dataTune3 = [];
                        var dataOp5 = {};
                        dataOp5.y = "2do Lapso";
                        count2 = 0;
                        matSegundoLapso.sort(SortByPromedioAsc);

                        for (w = 0; w <= matSegundoLapso.length - 1; w++) {
                            count2++;

                            if (count2 == 1){
                                dataOp5.a = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                       
                            }
                            else if (count2 == 2){
                                dataOp5.b = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 3){
                                dataOp5.c = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 4){
                                dataOp5.d = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 5){
                                dataOp5.e = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 6){
                                dataOp5.f = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 7){
                                dataOp5.g = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 8){
                                dataOp5.h = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 9){
                                dataOp5.i = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 10){
                                dataOp5.j = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 11){
                                dataOp5.k = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 12){
                                dataOp5.l = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 13){
                                dataOp5.m = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            else if (count2 == 14){
                                dataOp5.n = Math.round(matSegundoLapso[w].promedio);
                                descripción3 += matSegundoLapso[w].materia + ": " + matSegundoLapso[w].promedio_literal + "</br>";
                            }
                            labels3.push(matSegundoLapso[w].materia);
                        }
                        dataTune3.push(dataOp5);

                        for (i = 0; i < count2; i++) {
                            llaves3.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement3.toString(),
                            data: dataTune3,
                            xkey: 'y',
                            ykeys: llaves3,
                            labels: labels3,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });

                        $("#3DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                      "del curso previamente seleccionado." + "</br>" +
                                                      "------------------------------------------------------</br>" +
                                                      "2do Lapso </br>" +
                                                      "------------------------------------------------------</br>" +
                                                      descripción3);
                        var descripción4 = "";
                        var llaves4 = [];
                        var labels4 = [];
                        var dataTune4 = [];
                        var dataOp6 = {};
                        dataOp6.y = "3er Lapso";
                        count3 = 0;
                        matTercerLapso.sort(SortByPromedioAsc);

                        for (w = 0; w <= matTercerLapso.length - 1; w++) {
                            count3++;

                            if (count3 == 1){
                                dataOp6.a = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 2){
                                dataOp6.b = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 3){
                                dataOp6.c = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 4){
                                dataOp6.d = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 5){
                                dataOp6.e = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 6){
                                dataOp6.f = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 7){
                                dataOp6.g = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 8){
                                dataOp6.h = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 9){
                                dataOp6.i = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 10){
                                dataOp6.j = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 11){
                                dataOp6.k = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 12){
                                dataOp6.l = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 13){
                                dataOp6.m = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            else if (count3 == 14){
                                dataOp6.n = Math.round(matTercerLapso[w].promedio);
                                descripción4 += matTercerLapso[w].materia + ": " + matTercerLapso[w].promedio_literal + "</br>";
                            }
                            labels4.push(matTercerLapso[w].materia);
                        }
                        dataTune4.push(dataOp6);

                        for (i = 0; i < count3; i++) {
                            llaves4.push(arrayLetras[i])
                        }


                        Morris.Bar({
                            element: graphelement4.toString(),
                            data: dataTune4,
                            xkey: 'y',
                            ykeys: llaves4,
                            labels: labels4,
                            hideHover: 'true',
                            xLabelAngle: 0,
                            resize: true,
                            gridTextSize: 15,
                            gridTextColor: "#000000"
                        });
                        $("#4DescripciónCurso").html("Se observa las materias con mayor y menor dificultad " +
                                                    "del curso previamente seleccionado." + "</br>" +
                                                    "------------------------------------------------------</br>" +
                                                    "3er Lapso </br>" +
                                                    "------------------------------------------------------</br>" +
                                                    descripción4);

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
               

                    $("#ranking-peores-div2").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                     "padding-bottom:8em;' id=morris-ranking-peores2" + countChange + ">" +
                      "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
               

                    $("#ranking-peores-div3").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                     "padding-bottom:8em;' id=morris-ranking-peores3" + countChange + ">" +
                      "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
      
                    idCurso = "";
                    hideProgress();
                    ValidacionNotasVacias();
                }
            });
        }
        else {
            $("#ranking-alumnos-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
                     "padding-bottom:8em;' id=morris-bar-rankingalumnos" + countChange + ">" +
                      "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
            $("#aprovvsrepro-div").find('div').remove().end().append("<div id=morris-donut-aprovsrepro" +
                countChange + ">" +
                 "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");
       
            $("#ranking-peores-div").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
           "padding-bottom:8em;' id=morris-ranking-peores" + countChange + ">" +
            "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");


            $("#ranking-peores-div2").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
             "padding-bottom:8em;' id=morris-ranking-peores2" + countChange + ">" +
              "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");


            $("#ranking-peores-div3").find('div').remove().end().append("<div class='centrar' style='height:30em;" +
             "padding-bottom:8em;' id=morris-ranking-peores3" + countChange + ">" +
              "<i class='fa fa-ban fa-6 iconogris' style='font-size:27em;'></i>" + "</div>");

            idCurso = "";
            hideProgress();
        }

    });
});