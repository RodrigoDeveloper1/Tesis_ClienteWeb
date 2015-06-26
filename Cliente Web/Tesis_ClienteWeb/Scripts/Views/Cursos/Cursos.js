var idColegio = "";

function ContarMaterias() {
    var nroMaterias = $('#select-to').find('option').length;
    $('#input-nro-asignaturas').val(nroMaterias);
}

function CargarMaterias(grado) {
    if (idColegio != "") {
        showProgress();

        $.ajax({
            type: "POST",
            url: "/Bridge/ObtenerTablaCrearMateriaPorIdColegio",
            data: {
                idColegio: idColegio,
                grado: grado
            },
            success: function (data) {
                if (data[0].success) {
                    var lista;

                    for (var i = 0; i < data.length; i++) {
                        lista += ('<option value="' + data[i].idMateria + '">' +
                                        data[i].nombre +
                                  '</option>');
                    }

                    $("#select-from").find('option').remove().end().append(lista);
                    $("#select-to").find('option').remove().end();

                    hideProgress();
                }
                else {
                    hideProgress();
                    swal("¡No hay materias!", "El curso seleccionado no tiene materias asociadas.", "info");
                    $("#select-from").find('option').remove();
                    $("#select-to").find('option').remove();
                }
            }
        });
    }
    else
        swal("¡Oops!", "Seleccione un colegio primero", "warning");
};

function CargarTablaCursos() {
    showProgress();

    var lista = "";

    $('#table-lista-cursos').find('tbody').find('tr').remove();   

    $.post("/Cursos/ObtenerSelectListCursosPorColegio",
    {
        idColegio: idColegio
    },
    function (data) {
        if (data[0].success) {
            for (var i = 0; i < data.length; i++)
            {
                lista += (
                    '<tr id=' + data[i].idCurso + ' >' +
                        '<td class="td-nombre">' + data[i].nombre + '</td>' +
                        '<td class="periodo-escolar">' + data[i].anoescolar + '</td>' +
                        '<td class="td-status">' + data[i].grado + '</td>' +
                        '<td class="td-status">' + data[i].seccion + '</td>' +
                    '</tr>');
            }
            $('#table-lista-cursos').find('tbody').end().append(lista);
            hideProgress();
        }
        else {
            swal("¡No hay cursos!", "El colegio no tiene ningún curso asociado", "info");
            lista = (
                '<tr>' +
                    '<td class="td-nombre"></td>' +
                    '<td class="periodo-escolar"></td>' +
                    '<td class="td-status"></td>' +
                    '<td class="td-status"></td>' +
                '</tr>');
            $('#table-lista-cursos').find('tbody').find('tr').end().append(lista);
            hideProgress();
        }
    });
}

function AgregarCurso() {
    
    var curso= [];
    var listaMaterias = [];
   // var i = 1;
    curso.push(idColegio);

    var name = $("#input-nombre-auto").val();
    var grado = $("#select-grado option:selected").val();
    var seccion = $("#select-seccion option:selected").val();

    curso.push(name, grado, seccion);
    // metodo que selecciona todos los values del select de materias 
    $('#select-to option').each(function () {
        listaMaterias.push($(this).val());
    });

    var postData = {
        values: curso,
        materias : listaMaterias
    };
    showProgress();

    $.ajax({
        type: "POST",
        url: "/Cursos/CrearCurso",
        traditional: true,
        data: postData,
        success: function (r) {
            window.location.href = 'CrearCurso';
        }
    });
}

$(document).ready(function () {
    //Inicializando componentes:
    $('#input-nombre-auto').val('');
    $('#input-nro-asignaturas').val(0);
    $('#input-estatus').val('');

    //Selección de colegio
    $("#select-colegio-crear").change(function () {
        idColegio = $(this).val();
        $("#ano-escolar").val("Cargando el año escolar");

        if (idColegio != "") {
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
                        CargarTablaCursos();
                    }
                    else {
                        //Limpiando el text-box del año escolar
                        $("#ano-escolar").val("No posee año escolar activo");
                        hideProgress();
                    }
                }
            });
        }
        else {
            $("#ano-escolar").val(""); //Limpiando el text-box del año escolar
            idColegio = "" //Inicializando la variable
        }
    });
    $('#table-lista-cursos tbody tr').click(function (e) {
        var state = $(this).hasClass('active');
        $('.active').removeClass('active');

        if (!state) {
            $(this).addClass('active');
        }
                

        var lista = "";
        var detallecurso_parte_1 = "";
        var detallecurso_parte_2 = "";
        var detallecurso_parte_3 = "";
        var detallecurso_parte_4 = "";

        if ($(this).attr('id') != "") {
            $('#table-lista-alumnos').find('tbody').find('tr').remove();
            $('#Detalle-Curso-Panel-Div-Parte-1').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').remove();
            $('#detalle_rendimiento').find('p').remove();
            idCurso = $(this).attr('id');
                     
            $.ajax({
                url: "/Bridge/ObtenerTablaAlumnosPorIdCurso",
                type: "POST",
                data: {
                    "idCurso": idCurso
                }                
            }).done(function (data) {
                if (data != null && data.length > 0) {

                    $.ajax({
                        url: "/Bridge/ObtenerDetalleCurso",
                        type: "POST",
                        data: {
                            "idCurso": idCurso
                        }

                    }).done(function (data2) {

                        detallecurso_parte_1 = (
                       '<p><strong>Grado del curso: </strong><span id="span-grado">' + data2[0].nombrecurso + '</span></p>' +
                       '<p><strong>Número de alumnos: </strong><span id="span-nro-alumnos">' + data2[0].numeroestudiantes + '</span>' +
                       '<p><strong>Lapso en curso: </strong><span id="span-lapso">' + data2[0].lapsoencurso + '</span>'+
                        ' <p><strong>Período escolar: </strong><span id="span-periodo-escolar">' + data2[0].periodoescolar + '</span></p>' 
                     
                       );
                        $('#Detalle-Curso-Panel-Div-Parte-1').find('p').end().append(detallecurso_parte_1);

                        detallecurso_parte_2 = (
                          '<p><strong>Lapsos: </strong></p>'
                        );
                        $('#Detalle-Curso-Panel-Div-Parte-2').find('p').end().append(detallecurso_parte_2);

                        detallecurso_parte_3 = (
                        '<ul><li>I Lapso: <span id="span-lapso-I">' + data2[0].primerlapso + '</span></li>' +
                        '<li>II Lapso: <span id="span-lapso-II">' + data2[0].segundolapso + '</span></li>' +
                        '<li>III Lapso: <span id="span-lapso-III">' + data2[0].tercerlapso + '</span></li></ul>'
                        );
                        $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').end().append(detallecurso_parte_3);

                        detallecurso_parte_4 = (
                           '  <p><strong>Cantidad de materias: </strong>' +
                            '<span>' + data2[0].cantidadMaterias + '</span></p>' +
                        '<p> <strong>Cantidad de evaluaciones: </strong>'+
                        ' <span>' + data2[0].cantidadEvaluaciones + '</span></p>'
                    
                      );
                        $('#detalle_rendimiento').find('p').end().append(detallecurso_parte_4);

                    });


                    for (var i = 0; i < data.length; i++) {

                        lista += ('<tr>' +                         
                                    '<td class="td-numero-alumno">' + data[i].numerolista + '</td>' +
                                    '<td class="td-apellidos-alumno">' + data[i].apellido1 + ", " + data[i].apellido2 + '</td>' +
                                    '<td class="td-nombres-alumno">' + data[i].nombre1 + ", " + data[i].nombre2 + '</td>' +
                                  '</tr>');
                    }
                    $('#table-lista-alumnos').find('tbody').end().append(lista);

                   

                }
                else {

                    lista = ('<tr>' +                                    
                                    '<td class="td-numero-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +
                            '</tr>');

                    $('#table-lista-alumnos').find('tbody').find('tr').end().append(lista);

                    detallecurso_parte_1 = (
                   '<p><strong>Grado del curso: </strong><span id="span-grado"></span></p>' +
                   '<p><strong>Número de alumnos: </strong><span id="span-nro-alumnos"></span>' +
                   '<p><strong>Lapso en curso: </strong><span id="span-lapso"></span>' 
                   
                   );
                    $('#Detalle-Curso-Panel-Div-Parte-1').find('p').end().append(detallecurso_parte_1);

                    detallecurso_parte_2 = (
                      ' <p><strong>Período escolar: </strong><span id="span-periodo-escolar"></span></p>' +
                      '<p><strong>Lapsos: </strong></p>'
                      );
                    $('#Detalle-Curso-Panel-Div-Parte-2').find('p').end().append(detallecurso_parte_2);

                    detallecurso_parte_3 = (
                    '<ul><li>I Lapso: <span id="span-lapso-I"></span></li>' +
                    '<li>II Lapso: <span id="span-lapso-II"></span></li>' +
                    '<li>III Lapso: <span id="span-lapso-III"></span></li></ul>'
                    );
                    $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').end().append(detallecurso_parte_3);


                }
            })
        }
        else {

            $('#table-lista-alumnos').find('tbody').find('tr').remove();
            $('#Detalle-Curso-Panel-Div-Parte-1').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').remove();

            lista = ('<tr>' +
                                    '<td class="td-numero-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +
                    '</tr>');

            $('#table-lista-alumnos').find('tbody').find('tr').end().append(lista);

            detallecurso_parte_1 = (
                  '<p><strong>Grado del curso: </strong><span id="span-grado"></span></p>' +
                  '<p><strong>Número de alumnos: </strong><span id="span-nro-alumnos"></span>' +
                  '<p><strong>Lapso en curso: </strong><span id="span-lapso"></span>'
                  
                  );

            $('#Detalle-Curso-Panel-Div-Parte-1').find('p').end().append(detallecurso_parte_1);

            detallecurso_parte_2 = (
                     ' <p><strong>Período escolar: </strong><span id="span-periodo-escolar"></span></p>' +
                     '<p><strong>Lapsos: </strong></p>'
                     );
            $('#Detalle-Curso-Panel-Div-Parte-2').find('p').end().append(detallecurso_parte_2);

            detallecurso_parte_3 = (
            '<ul><li>I Lapso: <span id="span-lapso-I"></span></li>' +
            '<li>II Lapso: <span id="span-lapso-II"></span></li>' +
            '<li>III Lapso: <span id="span-lapso-III"></span></li></ul>'
            );
            $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').end().append(detallecurso_parte_3);

        }


                
    });

    //Configuración de los componentes de tipo fecha
    $('.datepicker').datepicker({ beforeShowDay: $.datepicker.noWeekends });
    $('#fec-ini-1').datepicker({ /*onSelect: CalcularFechas1,*/ });
    $('#fec-fin-1').datepicker({ /*onSelect: CalcularFechas1,*/ });
    
    /* Añadir y eliminar materias a la lista */
    $('#btn-add').click(function () {
        $('#select-from option:selected').each(function () {
            $('#select-to').append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
            $(this).remove();
        });

        ContarMaterias();
    });
    $('#btn-remove').click(function () {
        $('#select-to option:selected').each(function () {
            $('#select-from').append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
            $(this).remove();
        });

        ContarMaterias();
    });

    /*Para generar automáticamente el nombre*/
    $('#select-grado').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        var grado = '';

        if (valueSelected != 0) //Primer valor del Combobox: 'Seleccione un grado'
            CargarMaterias(valueSelected);

        if      (valueSelected == 1)  { grado = grado + '1er Grado'; }
        else if (valueSelected == 2)  { grado = grado + '2do Grado'; }
        else if (valueSelected == 3)  { grado = grado + '3er Grado'; }
        else if (valueSelected == 4)  { grado = grado + '4to Grado'; }
        else if (valueSelected == 5)  { grado = grado + '5to Grado'; }
        else if (valueSelected == 6)  { grado = grado + '6to Grado'; }
        else if (valueSelected == 7)  { grado = grado + '1er Año';   }
        else if (valueSelected == 8)  { grado = grado + '2do Año';   } 
        else if (valueSelected == 9)  { grado = grado + '3er Año';   }
        else if (valueSelected == 10) { grado = grado + '4to Año';   }
        else if (valueSelected == 11) { grado = grado + '5to Año';   }
        else { grado = ''; }

        $('#input-nombre-auto').val(grado);
    });
    $('#select-seccion').on('change', function (e) {
        var valueSelected = $(this).val();
        var seccion = ' - Sección ';
        var curso = $('#input-nombre-auto').val();
        var separador = curso.split("-");
        curso = separador[0].trim(' ');

        if (curso != '') {
            if      (valueSelected == 'U') { seccion = seccion + 'única'; }
            else if (valueSelected == 'A') { seccion = seccion + 'A'; }
            else if (valueSelected == 'B') { seccion = seccion + 'B'; }
            else if (valueSelected == 'C') { seccion = seccion + 'C'; }
            else if (valueSelected == 'D') { seccion = seccion + 'D'; }
            else if (valueSelected == 'E') { seccion = seccion + 'E'; }
            else if (valueSelected == 'F') { seccion = seccion + 'F'; }
            else { seccion = ''; }

            curso = curso + seccion;
            $('#input-nombre-auto').val(curso);
        }
    });

    //Eliminar cursos
    $(".eliminarcurso").click(function () {
        var selectedId = $(this).data("id");
        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este curso!",
            type: "warning", showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                swal("¡Borrado!", "Su curso ha sido borrado.", "success");
                jQuery.get(
                "/Cursos/EliminarCurso"
                , { "id": selectedId }
            ).done(function (data) {

                setTimeout("location.href ='/Cursos/ListaCursos';", 3000) /* 3 seconds */
            });
            } else {
                swal("¡Cancelado!", "Su curso está a salvo :)", "error");
            }
        });

      

    });
    $("#CrearCursoBoton").click(function (e) {
        AgregarCurso();
    });
    $("#boton_est_curso").click(function (e) {
        location.href = '/Estadisticas/EstadisticasCursos';
    });
});