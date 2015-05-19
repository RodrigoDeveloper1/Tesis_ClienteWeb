/************************************************************************************************************/
/* Referencia de:                                                                                           */
/* http://mrbool.com/how-to-add-edit-and-delete-rows-of-a-html-table-with-jquery/26721#ixzz3DEXgbakd        */
/*                                                                                                          */
/************************************************************************************************************/
var UltimoNroAlumno;

function Add() {
    UltimoNroAlumno = parseInt($('#div-table-lista-alumnos-cargar tr:last').prev().find("td:first").text()) + 1;

    if (!UltimoNroAlumno)
        UltimoNroAlumno = 1;

    /*Se borra la fila que contenía solo el símbolo de añadir un nuevo alumno*/
    var par = $(this).parent().parent(); //tr
    par.remove();

    /*Se inserta la primera fila para agregar un alumno nuevo*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" + UltimoNroAlumno + "</td>" +
            "<td class='td-matricula'><input class='form-control input-sm' type='text'/></td>" +            
            "<td class='td-primerapellido-alumno'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundoapellido-alumno'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-primernombre-alumno'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-segundonombre-alumno'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-agregar-alumno'><i class='ui-icon ui-icon-check i-agregar-alumno'></i></td>" +
            "<td class='td-eliminar-alumno'><i class='fa fa-minus-circle i-eliminar-fila-alumno'></i></td>" +
        "</tr>");

    /*Se inserta una segunda fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-agregar-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $(".i-agregar-alumno").bind("click", Save)
    $("#i-add-fila-alumno").bind("click", Add);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};
function Save() {
    var par = $(this).parent().parent(); //tr
    var tdNroAlumno = par.children("td:nth-child(1)");
    var tdMatricula = par.children("td:nth-child(2)");
    var tdPrimerApellido = par.children("td:nth-child(3)");
    var tdSegundoApellido = par.children("td:nth-child(4)");
    var tdPrimerNombre = par.children("td:nth-child(5)");
    var tdSegundoNombre = par.children("td:nth-child(6)");
    var tdAgregar = par.children("td:nth-child(7)");
    var tdEliminar = par.children("td:nth-child(8)");

    //Para debuggear
    /*console.log("tdNroAlumno.html(): " + tdNroAlumno.html());
    console.log("tdApellidos: " + tdApellidos.html());
    console.log("tdNombres: " + tdNombres.html());
    console.log("tdAgregar: " + tdAgregar.html());
    console.log("tdEliminar: " + tdEliminar.html()); */

    tdNroAlumno.html(tdNroAlumno.html());
    tdMatricula.html(tdMatricula.children("input[type=text]").val());
    tdPrimerApellido.html(tdPrimerApellido.children("input[type=text]").val());
    tdSegundoApellido.html(tdSegundoApellido.children("input[type=text]").val());
    tdPrimerNombre.html(tdPrimerNombre.children("input[type=text]").val());
    tdSegundoNombre.html(tdSegundoNombre.children("input[type=text]").val());
    tdAgregar.html("<i class='fa fa-edit i-editar-alumno'></i>"); //El ícono cambia al de editar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");

    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};
function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdNroAlumno = par.children("td:nth-child(1)");
    var tdMatricula = par.children("td:nth-child(2)");
    var tdPrimerApellido = par.children("td:nth-child(3)");
    var tdSegundoApellido = par.children("td:nth-child(4)");
    var tdPrimerNombre = par.children("td:nth-child(5)");
    var tdSegundoNombre = par.children("td:nth-child(6)");
    var tdEditar = par.children("td:nth-child(7)");
    var tdEliminar = par.children("td:nth-child(8)");

    tdNroAlumno.html(tdNroAlumno.html());
    tdMatricula.html("<input class='form-control input-sm' type='text' value=" + tdMatricula.html() + ">");
    tdPrimerApellido.html("<input class='form-control input-sm' type='text' value="+ tdPrimerApellido.html() + ">");
    tdSegundoApellido.html("<input class='form-control input-sm' type='text' value=" + tdSegundoApellido.html() + ">");
    tdPrimerNombre.html("<input class='form-control input-sm' type='text' value=" + tdPrimerNombre.html() + ">");
    tdSegundoNombre.html("<input class='form-control input-sm' type='text' value=" + tdSegundoNombre.html() + ">");
    tdEditar.html("<i class='ui-icon ui-icon-check i-agregar-alumno'></i>"); //El ícono cambia al de agregar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");

    $(".i-agregar-alumno").bind("click", Save)
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
            "<td class='td-matricula'></td>" +
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
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $("#i-add-fila-alumno").bind("click", Add);
};
function SalvarTodo() {
    //Se salvan todas las filas de las tablas
    $('#div-table-lista-alumnos-cargar tbody tr').each(function () {
        var tdNroAlumno = $(this).children("td:nth-child(1)");
        var tdMatricula = $(this).children("td:nth-child(2)");
        var tdPrimerApellido = $(this).children("td:nth-child(3)");
        var tdSegundoApellido = $(this).children("td:nth-child(4)");
        var tdPrimerNombre = $(this).children("td:nth-child(5)");
        var tdSegundoNombre = $(this).children("td:nth-child(6)");
        var tdAgregar = $(this).children("td:nth-child(7)");
        var tdEliminar = $(this).children("td:nth-child(8)");

        tdNroAlumno.html(tdNroAlumno.html());
        tdMatricula.html(tdMatricula.children("input[type=text]").val());
        tdPrimerApellido.html(tdPrimerApellido.children("input[type=text]").val());
        tdSegundoApellido.html(tdSegundoApellido.children("input[type=text]").val());
        tdPrimerNombre.html(tdPrimerNombre.children("input[type=text]").val());
        tdSegundoNombre.html(tdSegundoNombre.children("input[type=text]").val());
        tdAgregar.html("<i class='fa fa-edit i-editar-alumno'></i>"); //El ícono cambia a editar
        tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-alumno'></i>");
    });

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-alumnos-cargar tr:last').remove();

    /* Se inserta una fila solamente con el botón de añadir más alumnos. Esto se hace debido a que la última 
     * fila queda con un signo de más para agregar, y con el símbolo de editar y de borrar fila. Por eso se 
     * elimina*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-agregar-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
    $("#i-add-fila-alumno").bind("click", Add);
}
function AgregadoAutomatico(data) {    
    $("#div-table-lista-alumnos-cargar tbody tr").remove(); //Se borra todas las filas de la tabla

    UltimoNroAlumno = 1;
    var lista;

    for (var i = 0; i < data.length; i++) {
        lista += (
            "<tr>" +
                "<td class='td-nro-alumno'>" + UltimoNroAlumno + "</td>" +
                "<td class='td-matricula'>" + data[i].matricula + "</td>" +
                "<td class='td-primerapellido-alumno'>" + data[i].apellido1 + "</td>" +
                "<td class='td-segundoapellido-alumno'>" + data[i].apellido2 + "</td>" +
                "<td class='td-primernombre-alumno'>" + data[i].nombre1 + "</td>" +
                "<td class='td-segundonombre-alumno'>" + data[i].nombre2 + "</td>" +
                "<td class='td-agregar-alumno'><i class='fa fa-edit i-editar-alumno'></i></td>" +
                "<td class='td-eliminar-alumno'><i class='fa fa-minus-circle i-eliminar-fila-alumno'></i></td>" +
            "</tr>");

        UltimoNroAlumno++;
    }

    $("#div-table-lista-alumnos-cargar tbody").append(lista);

    /*Se inserta una segunda fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-alumnos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-alumno'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-alumno'></i>" +
            "</td>" +
            "<td class='td-matricula'></td>" +
            "<td class='td-primerapellido-alumno'></td>" +
            "<td class='td-segundoapellido-alumno'></td>" +
            "<td class='td-primernombre-alumno'></td>" +
            "<td class='td-segundonombre-alumno'></td>" +
            "<td class='td-agregar-alumno'></td>" +
            "<td class='td-eliminar-alumno'></td>" +
        "</tr>");

    $(".i-editar-alumno").bind("click", Edit);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
};

$(document).ready(function () {
    /* Botones de edición de las filas de la tabla de lista de alumnos */    
    $("#i-add-fila-alumno").bind("click", Add);
    $(".i-eliminar-fila-alumno").bind("click", Delete);
    $(".i-editar-alumno").bind("click", Edit);
    $("#i-eliminar-todas-filas").bind("click", BorrarTodo);
    $("#i-salvar-todas-filas").bind("click", SalvarTodo);
});