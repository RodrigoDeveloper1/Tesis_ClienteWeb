/************************************************************************************************************/
/* Referencia de:                                                                                           */
/* http://mrbool.com/how-to-add-edit-and-delete-rows-of-a-html-table-with-jquery/26721#ixzz3DEXgbakd        */
/*                                                                                                          */
/************************************************************************************************************/


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
            "<td class='td-primerapellido-alumno' id='PrimerApellidoID" + UltimoNroAlumno + "'><input class='form-control input-sm' type='text'/></td>" +
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
    console.log("Function -> Edit()");

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

function SalvarTodo() {
    console.log("Function -> SalvarTodo()");


    //Se salvan todas las filas de las tablas
    $('#div-table-lista-alumnos-cargar tbody tr').each(function () {
        var tdNroAlumno = $(this).children("td:nth-child(1)");
        var tdApellidos = $(this).children("td:nth-child(2)");
        var tdNombres = $(this).children("td:nth-child(3)");
        var tdAgregar = $(this).children("td:nth-child(4)");
        var tdEliminar = $(this).children("td:nth-child(5)");



        tdNroAlumno.html(tdNroAlumno.html());
        tdApellidos.html(tdApellidos.children("input[type=text]").val());
        tdNombres.html(tdNombres.children("input[type=text]").val());
        tdAgregar.html("<i class='fa fa-edit i-editar-alumno'></i>"); //El ícono cambia a editar
        tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");
    });

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-alumnos-cargar tr:last').remove();

    /*Se inserta una fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-apellidos-alumno'></td>" +
            "<td class='td-nombres-alumno'></td>" +
            "<td class='td-agregar-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
    $("#i-add-fila-alumno").bind("click", Add);
}

function ContarMaterias() {
    var nroMaterias = $('#select-to').find('option').length;
    $('#input-nro-asignaturas').val(nroMaterias);
}

/*
function CargarAlumnosCurso() {

   
    
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
       
        var name = $("#input-nombre-auto").val();
        var startdate = $("#fecha-inicio").val();
        var finishdate = $("#fecha-finalizacion").val();

        $.ajax({
            url: "/Cursos/CargarAlumnosCurso",
            type: "POST",
            data: {

                //  "ListNumber": listnumber,,
                "FirstLastName": PrimerApellido,
                "SecondLastName": SegundoApellido,
                "FirstName": PrimerNombre,
                "SecondName": SegundoNombre,
                "Name": name,
                "StartDate": startdate,
                "FinishDate": finishdate
            },
            sucess: function (r) {
                alert("Se han cargado los alumnos");
                console.log("SE REALIZO AJAX");
            },
            error: function (e) {
                alert("No se cargaron los alumnos. ");
            }
        });
    });
}

*/

$(document).ready(function () {    
    $('#table-lista-cursos tbody tr').click(function (e) {
        console.log("Acción->click: Fila de Lista de cursos(#table-lista-cursos tbody tr)");

        var state = $(this).hasClass('active');
        $('.active').removeClass('active');

        if (!state) {
            $(this).addClass('active');
        }

        /*$("#span-grado").text("prueba1");
        $("#span-nro-alumnos").text("prueba2");
        $("#span-lapso").text("prueba3");
        $("#span-estatus").text("prueba4");
        $("#span-icono-status").text("prueba5");
        $("#span-periodo-escolar").text("prueba6");
        $("#span-lapso-I").text("prueba7");
        $("#span-lapso-II").text("prueba8");
        $("#span-lapso-III").text("prueba9");*/
    });

    //Configuración de los componentes de tipo fecha
    $('.datepicker').datepicker({
        beforeShowDay: $.datepicker.noWeekends
    });

    $('#fec-ini-1').datepicker({
        //onSelect: CalcularFechas1,
    });

    $('#fec-fin-1').datepicker({
        //onSelect: CalcularFechas1,
    });

    //Fin de Configuración de los componentes de tipo fecha


    $("#li-datos-generales").click(function (e) {
        console.log("Acción: click -> Pestaña Navegador Datos Generales (#li-datos-generales)");

        $(this).addClass("active");
        $("#li-periodo-escolar").removeClass("active");

        $("#div-panel-datos-generales").css("display", "table");
        $("#div-panel-periodo-escolar").css("display", "none");
    });

    $("#li-periodo-escolar").click(function (e) {
        console.log("Acción: click -> Pestaña Navegador Datos Período Escolar (#li-datos-periodo)");

        $(this).addClass("active");
        $("#li-datos-generales").removeClass("active");

        $("#div-panel-datos-generales").css("display", "none");
        $("#div-panel-periodo-escolar").css("display", "table");
    });

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
    /* Fin de Añadir y eliminar materias a la lista */

    /* Botones de edición de las filas de la tabla de lista de alumnos */
    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
    $("#i-add-fila-alumno").bind("click", Add);
    $("#i-eliminar-todas-filas").bind("click", BorrarTodo);
    $("#i-salvar-todas-filas").bind("click", SalvarTodo);
    /* Fin de Botones de edición de las filas de la tabla de lista de alumnos */

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

    $('#select-grado').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        var grado = 'Curso de ';

        if (valueSelected == 1) { grado = grado + '1er Grado'; }
        else if (valueSelected == 2) { grado = grado + '2do Grado'; }
        else if (valueSelected == 3) { grado = grado + '3er Grado'; }
        else if (valueSelected == 4) { grado = grado + '4to Grado'; }
        else if (valueSelected == 5) { grado = grado + '5to Grado'; }
        else if (valueSelected == 6) { grado = grado + '6to Grado'; }
        else if (valueSelected == 7) { grado = grado + '7mo Grado'; }
        else if (valueSelected == 8) { grado = grado + '8vo Grado'; }
        else if (valueSelected == 9) { grado = grado + '9no Grado'; }
        else if (valueSelected == 10) { grado = grado + '4to Año'; }
        else if (valueSelected == 11) { grado = grado + '5to Año'; }
        else { grado = ''; }

        $('#input-nombre-auto').val(grado);
    });

    $('#select-grado').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        var grado = 'Curso de ';

        if (valueSelected == 1) { grado = grado + '1er Grado'; }
        else if (valueSelected == 2) { grado = grado + '2do Grado'; }
        else if (valueSelected == 3) { grado = grado + '3er Grado'; }
        else if (valueSelected == 4) { grado = grado + '4to Grado'; }
        else if (valueSelected == 5) { grado = grado + '5to Grado'; }
        else if (valueSelected == 6) { grado = grado + '6to Grado'; }
        else if (valueSelected == 7) { grado = grado + '7mo Grado'; }
        else if (valueSelected == 8) { grado = grado + '8vo Grado'; }
        else if (valueSelected == 9) { grado = grado + '9no Grado'; }
        else if (valueSelected == 10) { grado = grado + '4to Año'; }
        else if (valueSelected == 11) { grado = grado + '5to Año'; }
        else { grado = ''; }

        $('#input-nombre-auto').val(grado);
    });

    $('#select-seccion').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        var seccion = ' - Sección ';

        var curso = $('#input-nombre-auto').val();
        var separador = curso.split("-");
        curso = separador[0].trim(' ');

        if (curso != '') {
            if (valueSelected == 'u') { seccion = seccion + 'única'; }
            else if (valueSelected == 'a') { seccion = seccion + 'A'; }
            else if (valueSelected == 'b') { seccion = seccion + 'B'; }
            else if (valueSelected == 'c') { seccion = seccion + 'C'; }
            else if (valueSelected == 'd') { seccion = seccion + 'D'; }
            else if (valueSelected == 'e') { seccion = seccion + 'E'; }
            else if (valueSelected == 'f') { seccion = seccion + 'F'; }
            else { seccion = ''; }

            $('#input-nombre-auto').val(curso + seccion);
        }
    });



    $("#ModificarCursoBoton").click(function (e) {
        alert($("#input-nombre-auto").val());
    });




    $("#CrearCursoBoton").click(function (e) {
        console.log("Acción: click -> Botón Enviar");

        var name = $("#input-nombre-auto").val();
        var studenttotal = $("#input-nro-alumnos").val();
        var startdate = $("#fecha-inicio").val();
        var finishdate = $("#fecha-finalizacion").val();
        var status = $("#input-estatus").val();

        $.ajax({
            // url: "/Cursos/CargarAlumnosCurso",
            url: "/Cursos/CrearCursoNuevo",
            type: "POST",
            data: {

                "Name": name,
                "StudentTotal": studenttotal,
                "StartDate": startdate,
                "FinishDate": finishdate,
                "Status": status

            },
            sucess: function (r) {
                alert("Se han cargado los alumnos");
                console.log("SE REALIZO AJAX");
            },
            error: function (e) {
                alert("No se cargaron los alumnos. ");
            }
        });
        /*
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
                            // url: "/Cursos/CargarAlumnosCurso",
                            url: "/Cursos/CargarAlumnosCurso",
                            type: "POST",
                            data: {

                                "Name": name,
                                //  "ListNumber": listnumber,,
                                "FirstLastName": PrimerApellido,
                                "SecondLastName": SegundoApellido,
                                "FirstName": PrimerNombre,
                                "SecondName": SegundoNombre

                            },
                            sucess: function (r) {
                                alert("Se han cargado los alumnos");
                                console.log("SE REALIZO AJAX");
                            },
                            error: function (e) {
                                alert("No se cargaron los alumnos. ");
                            }
                        });
                    
                });
                */


    });
});

/********************************************************************************************************/
/* Referencias de:                                                                                      */
/* http://www.abeautifulsite.net/whipping-file-inputs-into-shape-with-bootstrap-3/                      */
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