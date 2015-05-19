function Add() {
    UltimoNroEvaluacion = parseInt($('#div-table-lista-evaluaciones-cargar tr:last').prev().find("td:first").text()) + 1;

    if (!UltimoNroEvaluacion)
        UltimoNroEvaluacion = 1;
    /*Se borra la fila que contenía solo el símbolo de añadir un nueva evaluacion*/
    var par = $(this).parent().parent(); //tr
    par.remove();
    var linea = ("<tr>" +
            "<td class='td-nombre'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-porcentaje'><input class='form-control input-sm' " +
            "onkeypress='return isNumberKey(event)' maxlength='2' type='text'/></td>" +
            "<td class='td-inicio'><input class='form-control input-sm datepicker' type='text'/></td>" +
            "<td class='td-fin'><input class='form-control input-sm datepicker' type='text'/></td>" +
           "<td class='td-tipo'><select class='form-control selectpicker input-sm'>" +
            "<option value='Prueba'>Prueba</option>" +
            "<option value='Taller'>Taller</option>" +
            "<option value='Exposición'>Exposición</option>" +
            "<option value='Otro'>Otro</option>" +
              " </select>" +
              "</td>" +
            "<td class='td-agregar-evaluaciones'><i class='ui-icon ui-icon-check i-agregar-evaluaciones'></i></td>" +
            "<td class='td-eliminar-evaluaciones'><i class='fa fa-minus-circle i-eliminar-fila-evaluaciones'></i></td>" +
        "</tr>");

    /*Se inserta la primera fila para agregar una nueva evaluacion*/
    $("#div-table-lista-evaluaciones-cargar tbody").append(linea);
    $("#div-table-lista-evaluaciones-cargar tbody").find('.datepicker').datepicker();

    /*Se inserta una segunda fila solamente con el botón de añadir más evaluaciones*/
    $("#table-lista-evaluaciones tbody").append(
      "<tr>" +
            "<td class='td-nro-evaluaciones'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-evaluaciones'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-porcentaje'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-evaluaciones'></td>" +
            "<td class='td-eliminar-evaluaciones'></td>" +
        "</tr>");

    $(".i-agregar-evaluaciones").bind("click", Save);
    $("#i-add-fila-evaluaciones").bind("click", Add);
    $(".i-eliminar-fila-evaluaciones").bind("click", Delete);
};
function Save() {
    var par = $(this).parent().parent(); //tr
    var tdNombre = par.children("td:nth-child(1)");
    var tdPorcentaje = par.children("td:nth-child(2)");
    var tdInicio = par.children("td:nth-child(3)");
    var tdFin = par.children("td:nth-child(4)");
    var tdTipo = par.children("td:nth-child(5)");
    var tdAgregar = par.children("td:nth-child(6)");
    var tdEliminar = par.children("td:nth-child(7)");


    tdNombre.html(tdNombre.children("input[type=text]").val());
    tdPorcentaje.html(tdPorcentaje.children("input[type=text]").val());
    tdInicio.html(tdInicio.children("input[type=text]").val());
    tdFin.html(tdFin.children("input[type=text]").val());
    tdTipo.html(tdTipo.children("select").val());
    tdAgregar.html("<i class='fa fa-edit i-editar-evaluaciones'></i>"); //El ícono cambia al de editar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-evaluaciones'></i>");

    $(".i-editar-evaluaciones").bind("click", Edit);
    $(".i-eliminar-fila-evaluaciones").bind("click", Delete);
};
function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdNombre = par.children("td:nth-child(1)");
    var tdPorcentaje = par.children("td:nth-child(2)");
    var tdInicio = par.children("td:nth-child(3)");
    var tdFin = par.children("td:nth-child(4)");
    var tdTipo = par.children("td:nth-child(5)");
    var tdAgregar = par.children("td:nth-child(6)");
    var tdEliminar = par.children("td:nth-child(7)");

    tdNombre.html("<input class='form-control input-sm' type='text' value=" + tdNombre.html() + ">"); +
    tdPorcentaje.html("<input class='form-control input-sm' onkeypress='return isNumberKey(event)'" +
        "maxlength='2' type='text' value=" + tdPorcentaje.html() + ">");
    tdInicio.html("<input class='form-control input-sm datepicker' type='text' value=" + tdInicio.html() + ">");
    tdFin.html("<input class='form-control input-sm datepicker' type='text' value=" + tdFin.html() + ">");
    if (tdTipo.html() == "Prueba") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option selected value='Prueba'>Prueba</option>" +
                "<option value='Taller'>Taller</option>" +
                "<option value='Exposición'>Exposición</option>" +
                "<option value='Otro'>Otro</option>" +
                  " </select>");
    }
    if (tdTipo.html() == "Taller") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option value='Prueba'>Prueba</option>" +
                "<option selected value='Taller'>Taller</option>" +
                "<option value='Exposición'>Exposición</option>" +
                "<option value='Otro'>Otro</option>" +
                  " </select>");
    }
    if (tdTipo.html() == "Exposición") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option value='Prueba'>Prueba</option>" +
                "<option value='Taller'>Taller</option>" +
                "<option selected value='Exposición'>Exposición</option>" +
                "<option value='Otro'>Otro</option>" +
                  " </select>");
    }
    if (tdTipo.html() == "Otro") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option value='Prueba'>Prueba</option>" +
                "<option value='Taller'>Taller</option>" +
                "<option value='Exposición'>Exposición</option>" +
                "<option selected value='Otro'>Otro</option>" +
                  " </select>");
    }
    tdAgregar.html("<i class='ui-icon ui-icon-check i-agregar-evaluaciones'></i>"); //El ícono cambia al de agregar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-evaluaciones'></i>");

    $("#div-table-lista-evaluaciones-cargar tbody").find('.datepicker').datepicker();
    $("#div-table-lista-evaluaciones-cargar tbody").find('.bootstrap-timepicker').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $(".i-agregar-evaluaciones").bind("click", Save);
    $(".i-eliminar-fila-evaluaciones").bind("click", Delete);
};
function Delete() {
    var par = $(this).parent().parent(); //tr
    par.remove();

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-evaluaciones-cargar tr:last').remove();


    //Se agrega la última fila solo con el botón de añadir
    $("#div-table-lista-evaluaciones-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-evaluaciones'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-evaluaciones'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-porcentaje'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-evaluaciones'></td>" +
            "<td class='td-eliminar-evaluaciones'></td>" +
        "</tr>");

    $("#i-add-fila-evaluaciones").bind("click", Add);
};
function BorrarTodo() {
    //Se borra todas las filas de la tabla
    $("#div-table-lista-evaluaciones-cargar tbody tr").remove();

    /*Se inserta una fila solamente con el botón de añadir más alumnos*/
    $("#div-table-lista-evaluaciones-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-evaluaciones'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-evaluaciones'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-porcentaje'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-evaluaciones'></td>" +
            "<td class='td-eliminar-evaluaciones'></td>" +
        "</tr>");

    $("#i-add-fila-evaluaciones").bind("click", Add);
};
function SalvarTodo() {
    console.log("entro rods");
    //Se salvan todas las filas de las tablas
    $('#div-table-lista-evaluaciones-cargar tbody tr').each(function () {
        var tdNombre = $(this).children("td:nth-child(1)");
        var tdPorcentaje = $(this).children("td:nth-child(2)");
        var tdInicio = $(this).children("td:nth-child(3)");
        var tdFin = $(this).children("td:nth-child(4)");
        var tdTipo = $(this).children("td:nth-child(5)");
        var tdAgregar = $(this).children("td:nth-child(6)");
        var tdEliminar = $(this).children("td:nth-child(7)");

        tdNombre.html(tdNombre.children("input[type=text]").val());
        tdPorcentaje.html(tdPorcentaje.children("input[type=text]").val());
        tdInicio.html(tdInicio.children("input[type=text]").val());
        tdFin.html(tdFin.children("input[type=text]").val());
        tdTipo.html(tdTipo.children("select").val());
        tdAgregar.html("<i class='fa fa-edit i-editar-evaluaciones'></i>"); //El ícono cambia al de editar
        tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-evaluaciones'></i>");
    });
    console.log("entro rods 2");
    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-evaluaciones-cargar tr:last').remove();
    console.log("entro rods 3");
    /* Se inserta una fila solamente con el botón de añadir más alumnos. Esto se hace debido a que la última 
     * fila queda con un signo de más para agregar, y con el símbolo de editar y de borrar fila. Por eso se 
     * elimina*/
    $("#div-table-lista-evaluaciones-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-evaluaciones'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-evaluaciones'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-porcentaje'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-evaluaciones'></td>" +
            "<td class='td-eliminar-evaluaciones'></td>" +
        "</tr>");


    $(".i-editar-evaluaciones").bind("click", Edit);
    $(".i-eliminar-fila-evaluaciones").bind("click", Delete);
    $("#i-add-fila-evaluaciones").bind("click", Add);
}

$(document).ready(function () {
    /* Botones de edición de las filas de la tabla de lista de alumnos */
    $("#i-add-fila-evaluaciones").bind("click", Add);
    $(".i-eliminar-fila-evaluaciones").bind("click", Delete);
    $(".i-editar-evaluaciones").bind("click", Edit);
    $("#i-eliminar-todas-filas").bind("click", BorrarTodo);
    $("#i-salvar-todas-filas").bind("click", SalvarTodo);
});