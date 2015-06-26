var idCompetencia = "";
var idEvaluacion = "";
var listaIndicadores = [];
var listaIdIndicadores = [];
var listaIdPrincipales = [];
var listaDataActualizada = [];
var listaAuxiliar = [];
var listaAuxiliar2 = [];
var listaAuxiliar3 = [];
var cabeceraLiterales = ["A", "B", "C", "D", "E"];
var control;

//Variables para renderizar la tabla
var container;
var excelTable;
var rowsData = [];

//Función para reiniciar las variables de arreglos definidas y la vista
function ReiniciandoDatos()
{
    listaIndicadores = [];
    listaIdIndicadores = [];
    listaIdPrincipales = [];
    listaAuxiliar = [];
    listaAuxiliar2 = [];
    listaAuxiliar3 = [];
    listaDataActualizada = [];
    rowsData = [];
    idCompetencia = "";

    $("#matriz-indicadores-literales").empty();
    $("#fila-lista-indicadores").empty();

    $("#btn-testing").addClass("hidden");
    $("#porcentaje-competencia-alcanzado").empty();
}
//Función que renderiza la tabla de excel
function RenderizarTabla() {
    //Asignándole los datos principales a la tabla
    excelTable = new Handsontable(container,
        {
            data: rowsData, //Valor de las filas
            rowHeaders: true, 
            colHeaders: cabeceraLiterales, //Fila #1 - Cabecera
            minSpareRows: 0,
            persistentState: true,
            fixedColumnsLeft: 0,
            fixedRowsTop: 0,
        });

    MostrarBotonTesting();
}
//Función que actualiza la data asociada a la tabla de excel
function ActualizarData()
{
    showProgress();
    control = true;

    for (var i = 0; i <= (rowsData.length - 1) ; i++) {
        for (var j = 0; j <= (cabeceraLiterales.length - 1) ; j++) {
            var selectedCell = excelTable.getData()[i][j];

            if (selectedCell < 1 && selectedCell > 5)
                control = false;
            else 
                listaAuxiliar3.push(selectedCell);
        }
        listaDataActualizada.push(listaAuxiliar3);
        listaAuxiliar3 = [];//Reiniciando el arreglo.
    }

    if (control) {
        var postData = {
            principalIds: listaIdPrincipales,
            asignaciones: listaDataActualizada
        };

        $.ajax({
            type: "POST",
            url: "/Evaluaciones/AsociacionIndicadoresLiterales",
            traditional: true,
            data: postData,
            success: function (data) {
                hideProgress();

                if (data[0].Success) {
                    swal({
                        title: "¡Actualización exitosa!",
                        text: "Se han actualizado los valores de asignación entre indicadores y literales.",
                        type: "success",
                        showCancelButton: false,
                        closeOnConfirm: true
                    },
                    function () {
                        showProgress();
                        window.location.href = '/Evaluaciones/Evaluaciones';
                    });
                }
                else {
                    swal({
                        title: "¡Error!",
                        text: "Ha ocurrido un error, por favor intente de nuevo.",
                        type: "error",
                        showCancelButton: false,
                        closeOnConfirm: true
                    },
                    function () {
                        showProgress();
                        window.location.href = '/Evaluaciones/Evaluaciones';
                    });
                }
            },
            error: function () {
                hideProgress();
                swal('¡Error!', 'Ha ocurrido un error, por favor intente de nuevo', 'error');
            }
        });
    }
    else {
        hideProgress();
        swal('¡Data inválida!', 'Los valores de asignación deben estar entre 1 y 5', 'warning');
    }
}
//Función que muestra el botón de testeo del % de competencia alcanzado
function MostrarBotonTesting() {
    $("#btn-testing").removeClass("hidden");
}
//Función que calcula el porcentaje de competencia alcanzado con base a la matriz de indicadores/literales
function CalculoPorcentajeCompetenciaAlcanzado()
{
    control = true;
    listaAuxiliar3 = []; //Reiniciando el arreglo
    listaDataActualizada = []; //Reiniciando el arreglo

    for (var i = 0; i <= (rowsData.length - 1) ; i++) {
        for (var j = 0; j <= (cabeceraLiterales.length - 1); j++) {
            var selectedCell = excelTable.getData()[i][j];

            if (selectedCell < 1 && selectedCell > 5)
                control = false;
            else
                listaAuxiliar3.push(selectedCell);
        }
        listaDataActualizada.push(listaAuxiliar3);
        listaAuxiliar3 = [];//Reiniciando el arreglo.
    }

    if (control) {
        var postData = {
            asignaciones: listaDataActualizada,
            idEvaluacion: idEvaluacion
        };

        $.ajax({
            type: "POST",
            url: "/Bridge/CalculoPorcentajeCompetenciaAlcanzado",
            traditional: true,
            data: postData,
            success: function (data) {
                if (data[0].Success) {
                    $("#porcentaje-competencia-alcanzado").empty();
                    $("#porcentaje-competencia-alcanzado").append(data[0].porcentajeCompetencia + "%");
                }                    
            },
            error: function () {
                swal('¡Error!', 'Ha ocurrido un error, por favor intente de nuevo', 'error');
            }
        });
    }
    else {
        hideProgress();
        swal('¡Data inválida!', 'Los valores de asignación deben estar entre 1 y 5', 'warning');
    }
}

$(document).ready(function () {
    idEvaluacion = $("#id-evaluacion").val(); //Obteniendo el id de la evaluación.
    container = document.getElementById('matriz-indicadores-literales');

    $("#select-competencias").change(function () {
        ReiniciandoDatos();

        idCompetencia = $(this).val();

        if (idCompetencia != "") {
            showProgress();

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerJsonIndicadoresLiterales",
                data: {
                    idCompetencia: idCompetencia,
                    idEvaluacion: idEvaluacion
                },
                success: function (data) {
                    if (data[0].Success) {
                        //Obteniendo información de los indicadores
                        for (var i = 0; i < data.length; i++) {
                            listaIndicadores.push(data[i].IndicatorDescription);
                            listaIdIndicadores.push(data[i].IndicatorId);
                        };
                        listaIndicadores = jQuery.unique(listaIndicadores); //Para eliminar duplicados
                        listaIdIndicadores = jQuery.unique(listaIdIndicadores); //Para eliminar duplicados

                        //Obteniendo información de las asignaciones de los indicadores
                        for (var i = 0; i < listaIdIndicadores.length; i++) {
                            for (var j = 0; j < data.length; j++) {
                                if (data[j].IndicatorId == listaIdIndicadores[i]) {
                                    listaAuxiliar.push(data[j].Assignation);
                                    listaAuxiliar2.push(data[j].PrincipalId);
                                }
                            };
                            rowsData.push(listaAuxiliar);
                            listaIdPrincipales.push(listaAuxiliar2);

                            listaAuxiliar = [] //Reiniciando la lista;
                            listaAuxiliar2 = [] //Reiniciando la lista;
                        };
                                                
                        //Agregando título: Lista de indicadores asociados
                        $("#fila-lista-indicadores").append(
                            '<div class="col-xs-12">' +
                                '<h4>Lista de indicadores asociados:</h4>' +
                            '</div>' +

                            '<div class="col-xs-offset-1 col-xs-11" id="fila-descrip-lista-indicadores">' +
                            '</div>');                        

                        //Agregando la lista de indicadores
                        for (var i = 0; i < listaIndicadores.length; i++)
                        {
                            $("#fila-descrip-lista-indicadores").append(
                                '<p>' + listaIndicadores[i] + '</p>');
                        }

                        //Agregando separadores
                        $("#fila-lista-indicadores").append(
                            '<div class="form-group col-xs-12"></div>' +

                            '<div class="col-lg-12">' +
                                '<div class="separador"></div>' +
                            '</div>');

                        RenderizarTabla(); //Para mostrar la tabla de excel
                        hideProgress();
                    }
                    else {
                        hideProgress();
                        ReiniciandoDatos();
                        swal('¡Oops!', 'Ha ocurrido un error de carga. Intente de nuevo', 'error');
                    }
                },
                error: function () {
                    hideProgress();

                    $("#fila-lista-indicadores").empty(); //Limpiando la lista de indicadores
                    ReiniciandoArreglos();
                    swal('¡Oops!', 'Ha ocurrido un error de carga. Intente de nuevo', 'error');
                },
            });
        }
        else
            ReiniciandoDatos();
    });

    $("#btn-actualizar").click(function () {
        if (idCompetencia == "")
            swal('¡Oops! Falta algo', 'Por favor selecciona primero una competencia, para actualizar los' +
                ' datos asociados.', 'info');
        else {
            ActualizarData();
        }
    });

    $("#btn-testing").click(function () {
        CalculoPorcentajeCompetenciaAlcanzado();
    });
});