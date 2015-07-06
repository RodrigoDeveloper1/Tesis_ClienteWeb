/************************************************************************************************************/
/* Referencia de:                                                                                           */
/* http://mrbool.com/how-to-add-edit-and-delete-rows-of-a-html-table-with-jquery/26721#ixzz3DEXgbakd        */
/*                                                                                                          */
/************************************************************************************************************/
var UltimoNroAlumno;

function Add() {
    console.log("Función Add()");
    UltimoNroAlumno = parseInt($('#div-table-lista-alumnos-cargar tr:last').prev().find("td:first").text()) + 1;

    if (!UltimoNroAlumno)
        UltimoNroAlumno = 1;

    /*Se borra la fila que contenía solo el símbolo de añadir un nuevo alumno*/
    var par = $(this).parent().parent();
    par.remove();

    /*Se inserta la primera fila para agregar un alumno nuevo*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" + UltimoNroAlumno + "</td>" +
            "<td class='td-primerapellido-alumno' id='PrimerApellidoID" +UltimoNroAlumno+"'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundoapellido-alumno' id='SegundoApellidoID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-primernombre-alumno' id='PrimerNombreID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundonombre-alumno' id='SegundoNombreID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            
            "<td class='td-eliminar-alumno'><i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>" +
            "</td>" +
        "</tr>");

    /*Se inserta una segunda fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
           "<td class='td-primerapellido-alumno' id='PrimerApellidoID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundoapellido-alumno' id='SegundoApellidoID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-primernombre-alumno' id='PrimerNombreID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundonombre-alumno' id='SegundoNombreID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
            
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $("#i-add-fila-alumno").bind("click", Add);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
   
};

function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdNroAlumno = par.children("td:nth-child(1)");
    var tdPrimerApellido = par.children("td:nth-child(2)");
    var tdSegundoApellido = par.children("td:nth-child(3)");
    var tdPrimerNombre = par.children("td:nth-child(4)");
    var tdSegundoNombre = par.children("td:nth-child(5)");
    var tdEliminar = par.children("td:nth-child(7)");

            "<td class='td-eliminar-alumno'><i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>" +
            "</td>" +

    tdPrimerApellido.html("<input type='text' class='form-control input-sm' value='" + tdPrimerApellido.html() + "'/>");
    tdSegundoApellido.html("<input type='text' class='form-control input-sm' value='" + tdSegundoApellido.html() + "'/>");
    tdPrimerNombre.html("<input type='text' class='form-control input-sm' value='" + tdPrimerNombre.html() + "'/>");
    tdSegundoNombre.html("<input type='text' class='form-control input-sm' value='" + tdSegundoNombre.html() + "'/>");
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");

    $(".i-eliminar-fila-alumno").bind("click", Delete);
};

function Delete() {
    var numeroLista = 1;
    var par = $(this).parent().parent(); //tr
    par.remove();

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-alumnos-cargar tr:last').remove();

    //Se reescriben los números de cada fila
    $('#div-table-lista-alumnos-cargar tbody tr').each(function () {
        var tdNroAlumno = $(this).children("td:nth-child(1)");
        tdNroAlumno.html(numeroLista);

        numeroLista = parseInt(numeroLista + 1);
    });

    //Se agrega la última fila solo con el botón de añadir
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +  
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $("#i-add-fila-alumno").bind("click", Add);
};

/************************************************************************************************************/
/* Fin de Referencia:                                                                                       */
/************************************************************************************************************/

function BorrarTodo() {
    //Se borra todas las filas de la tabla
    $("#div-table-lista-alumnos-cargar tbody tr").remove();

    /*Se inserta una fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $("#i-add-fila-alumno").bind("click", Add);
};

$(document).ready(function () {
    /* Botones de edición de las filas de la tabla de lista de alumnos */
    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
    $("#i-add-fila-alumno").bind("click", Add);
    $("#i-eliminar-todas-filas").bind("click", BorrarTodo);
    /* Fin de Botones de edición de las filas de la tabla de lista de alumnos */

    $("#eliminaralumno").click(function () {

        var selectedId = $(this).data("id");
        console.log(selectedId);

        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este alumno!",
            type: "warning", showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                console.log(selectedId);
                swal("¡Borrado!", "Su alumno ha sido borrado.", "success");
                jQuery.get(
                "/Alumnos/EliminarAlumno"
                , { "id": selectedId }
            ).done(function (data) {

                setTimeout("location.href ='/Alumnos/Alumnos';", 3000) /* 3 seconds */
            });
            } else {
                swal("¡Cancelado!", "Su alumno está a salvo :)", "error");
            }
        });


    });
   
    $("#CargarAlumnosBoton").click(function (e) {
        console.log("Acción: click -> CargarAlumnosBoton");


        $('#div-table-lista-alumnos-cargar tbody tr').each(function () {


            var tdNroAlumno = $(this).children("td:nth-child(1)")
            var tdPrimerApellido = $(this).children("td:nth-child(2)");
            var tdSegundoApellido = $(this).children("td:nth-child(3)");
            var tdPrimerNombre = $(this).children("td:nth-child(4)");
            var tdSegundoNombre = $(this).children("td:nth-child(5)");
            var tdEliminar = $(this).children("td:nth-child(7)");

            var PrimerApellido = tdPrimerApellido.children("input[type=text]").val();
            var SegundoApellido = tdSegundoApellido.children("input[type=text]").val();
            var PrimerNombre = tdPrimerNombre.children("input[type=text]").val();
            var SegundoNombre = tdSegundoNombre.children("input[type=text]").val();


            $.ajax({
                url: "/Bridge/CargarAlumnos",
                type: "POST",
                data: {

                    //  "ListNumber": listnumber,
                    "FirstLastName": PrimerApellido,
                    "SecondLastName": SegundoApellido,
                    "FirstName": PrimerNombre,
                    "SecondName": SegundoNombre
                },
                sucess: function (data) {
                    window.location.href = 'ModificarAlumnos';
                    console.log("SE REALIZO AJAX");
                },
                error: function (data) {
                    alert("No se cargaron los alumnos. ");
                }
            });


        });

    });
    
    $("#CargarAlumnosPorExcelBoton").click(function (e) {
        console.log("Acción: click -> CargarAlumnosPorExcelBoton");
        
        var FileName = $("#input-load-file");
        console.log(FileName);
            $.ajax({
                url: "/Alumnos/LeerAlumnosDesdeExcel",
                type: "POST",
                data: {

                    "FileName": FileName
                },
                sucess: function (data) {                   
                    console.log("SE REALIZO AJAX");
                },
                error: function (data) {
                    alert("No se cargaron los alumnos. ");
                }
            });
    });

    $('#span-btn-files :file').on('fileselect', function (event, numFiles, label) {
        var input = $(this).parents('.input-group').find(':text'),
            log = numFiles > 1 ? numFiles + ' files selected' : label;

        if (input.length) {
            input.val(log);
        }
        else {
            if (log) alert(log);
        }
    });

    //Obtener tabla de cursos 
    $("#select-curso").change(function () {
        var lista = "";
        var detallecurso_parte_1 = "";
        var detallecurso_parte_2 = "";
        var detallecurso_parte_3 = "";
        var detallecurso_parte_4 = "";

        if ($(this).val() != "") {
            showProgress();

            $('#table-lista-alumnos').find('tbody').find('tr').remove();
            $('#Detalle-Curso-Panel-Div-Parte-1').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').remove();
            $('#detalle_rendimiento').find('p').remove();
            idCurso = $(this).val();
                     
            $.ajax({
                url: "/Bridge/ObtenerTablaAlumnosPorIdCurso",
                type: "POST",
                data: {
                    "idCurso": idCurso
                },
                success: function (data) {
                    if (data != null && data.length > 0) {
                        $.ajax({
                            url: "/Bridge/ObtenerDetalleCurso",
                            type: "POST",
                            data: {
                                "idCurso": idCurso
                            },
                            success: function (data2) {
                                detallecurso_parte_1 = (
                               '<p><strong>Grado del curso: </strong><span id="span-grado">' + data2[0].nombrecurso + '</span></p>' +
                               '<p><strong>Número de alumnos: </strong><span id="span-nro-alumnos">' + data2[0].numeroestudiantes + '</span>' +
                               '<p><strong>Lapso en curso: </strong><span id="span-lapso">' + data2[0].lapsoencurso + '</span>' +
                               '<p><strong>Período escolar: </strong><span id="span-periodo-escolar">' + data2[0].periodoescolar + '</span></p>'

                                );

                                $('#Detalle-Curso-Panel-Div-Parte-1').find('p').end().append(detallecurso_parte_1);

                                detallecurso_parte_2 = ('<p><strong>Lapsos: </strong></p>');

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
                                '<p> <strong>Cantidad de evaluaciones: </strong>' +
                                ' <span>' + data2[0].cantidadEvaluaciones + '</span></p>');

                                $('#detalle_rendimiento').find('p').end().append(detallecurso_parte_4);

                                for (var i = 0; i < data.length; i++) {
                                    lista += ('<tr>' +
                                                '<td class="td-apellidos-alumno">' + data[i].numerolista + '</td>' +
                                                '<td class="td-apellidos-alumno">' + data[i].matricula + '</td>' +
                                                '<td class="td-apellidos-alumno">' + data[i].apellido1 + '</td>' +
                                                '<td class="td-apellidos-alumno">' + data[i].apellido2 + '</td>' +
                                                '<td class="td-nombres-alumno">' + data[i].nombre1 + '</td>' +
                                                '<td class="td-nombres-alumno">' + data[i].nombre2 + '</td>' +
                                              '</tr>');
                                }
                                $('#table-lista-alumnos').find('tbody').end().append(lista);

                                hideProgress();
                            }
                        });
                    }
                    else {
                        lista = (
                            '<tr>' +
                                '<td class="td-apellidos-alumno"></td>' +
                                '<td class="td-apellidos-alumno"></td>' +
                                '<td class="td-apellidos-alumno"></td>' +
                                '<td class="td-apellidos-alumno"></td>' +
                                '<td class="td-nombres-alumno"></td>' +
                                '<td class="td-nombres-alumno"></td>' +
                            '</tr>');

                        $('#table-lista-alumnos').find('tbody').find('tr').end().append(lista);

                        detallecurso_parte_1 = (
                       '<p><strong>Grado del curso: </strong><span id="span-grado"></span></p>' +
                       '<p><strong>Número de alumnos: </strong><span id="span-nro-alumnos"></span>' +
                       '<p><strong>Lapso en curso: </strong><span id="span-lapso"></span>' +
                       '<p><strong>Período escolar: </strong><span id="span-periodo-escolar"></span></p>'

                       );
                        $('#Detalle-Curso-Panel-Div-Parte-1').find('p').end().append(detallecurso_parte_1);

                        detallecurso_parte_2 = (
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
                }
            });
        }
        else {
            $('#table-lista-alumnos').find('tbody').find('tr').remove();
            $('#Detalle-Curso-Panel-Div-Parte-1').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('p').remove();
            $('#Detalle-Curso-Panel-Div-Parte-2').find('ul').remove();

            lista = ('<tr>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
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
                  '  <p><strong>Cantidad de materias: </strong>' +
                            '<span></span></p>' +
                        '<p> <strong>Cantidad de evaluaciones: </strong>' +
                        ' <span></span></p>'

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

    $("#boton_est_alumnos").click(function (e) {
        showProgress();
        location.href = '/Estadisticas/EstadisticasCursos';
    });
});


/********************************************************************************************************/
/* Referencias de:                                                                                      */
/* http://www.abeautifulsite.net/whipping-file-inputs-into-shape-with-bootstrap-3/                     */
/* http://www.codeproject.com/Questions/763540/How-to-browse-only-xls-and-xlsx-file-in-html-file        */
/*                                                                                                      */
/********************************************************************************************************/
$(document).on('change', '#span-btn-files :file', function () {
    var extensionesExcel = new Array(".xlsx", ".xls");
    var extensionArchivo = $(this).val();
    extensionArchivo = extensionArchivo.substring(extensionArchivo.lastIndexOf('.'));

    if (extensionesExcel.indexOf(extensionArchivo) < 0)
        $("#div-alerta-ext-file").show(400).delay(2000).slideUp(400);
    else {
        var input = $(this),
        numFiles = input.get(0).files ? input.get(0).files.length : 1,
        label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

        input.trigger('fileselect', [numFiles, label]);
    }
});
/********************************************************************************************************/
/* Fin de referencias                                                                                   */
/********************************************************************************************************/