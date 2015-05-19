var idColegio;
var UltimoNroEvento;

// Gestion tabla de Eventos
function Add() {
    UltimoNroEvento = parseInt($('#div-table-lista-eventos-cargar tr:last').prev().find("td:first").text()) + 1;

    if (!UltimoNroEvento)
        UltimoNroEvento = 1;
    /*Se borra la fila que contenía solo el símbolo de añadir un nuevo evento*/
    var par = $(this).parent().parent(); //tr
    par.remove();
    var linea = ("<tr>" +
            "<td class='td-nombre'><input class='form-control input-sm' type='text'/></td>" +
            "<td class='td-descripcion'><input class='form-control input-sm' " +
            "  type='text'/></td>" +
            "<td class='td-inicio'><input class='form-control input-sm datepicker' type='text'/></td>" +
            "<td class='td-fin'><input class='form-control input-sm datepicker' type='text'/></td>" +
           "<td class='td-tipo'><select class='form-control selectpicker input-sm'>" +
            "<option value='Evento de un día'>Evento de un día</option>" +
            "<option value='Evento de varios días'>Evento de varios días</option>" +           
            "<option value='Otro'>Otro</option>" +
              " </select>" +
              "</td>" +
            "<td class='td-agregar-eventos'><i class='ui-icon ui-icon-check i-agregar-eventos'></i></td>" +
            "<td class='td-eliminar-eventos'><i class='fa fa-minus-circle i-eliminar-fila-eventos'></i></td>" +
        "</tr>");

    /*Se inserta la primera fila para agregar una nueva eventos*/
    $("#div-table-lista-eventos-cargar tbody").append(linea);
    $("#div-table-lista-eventos-cargar tbody").find('.datepicker').datepicker();
    $("#div-table-lista-eventos-cargar tbody").find('.bootstrap-timepicker').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    /*Se inserta una segunda fila solamente con el botón de añadir más eventos*/
    $("#div-table-lista-eventos-cargar tbody").append(
      "<tr>" +
            "<td class='td-nro-eventos'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-eventos'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-descrpcion'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-eventos'></td>" +
            "<td class='td-eliminar-eventos'></td>" +
        "</tr>");

    $(".i-agregar-eventos").bind("click", Save);
    $("#i-add-fila-eventos").bind("click", Add);
    $(".i-eliminar-fila-eventos").bind("click", Delete);
};
function Save() {
    var par = $(this).parent().parent(); //tr
    var tdNombre = par.children("td:nth-child(1)");
    var tdDescripcion = par.children("td:nth-child(2)");
    var tdInicio = par.children("td:nth-child(3)");
    var tdFin = par.children("td:nth-child(4)");
    var tdTipo = par.children("td:nth-child(5)");
    var tdAgregar = par.children("td:nth-child(6)");
    var tdEliminar = par.children("td:nth-child(7)");


    tdNombre.html(tdNombre.children("input[type=text]").val());
    tdDescripcion.html(tdDescripcion.children("input[type=text]").val());
    tdInicio.html(tdInicio.children("input[type=text]").val());
    tdFin.html(tdFin.children("input[type=text]").val());
    tdTipo.html(tdTipo.children("select").val());
    tdAgregar.html("<i class='fa fa-edit i-editar-eventos'></i>"); //El ícono cambia al de editar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-eventos'></i>");

    $(".i-editar-eventos").bind("click", Edit);
    $(".i-eliminar-fila-eventos").bind("click", Delete);
};
function Edit() {
    var par = $(this).parent().parent(); //tr
    var tdNombre = par.children("td:nth-child(1)");
    var tdDescripcion = par.children("td:nth-child(2)");
    var tdInicio = par.children("td:nth-child(3)");
    var tdFin = par.children("td:nth-child(4)");
    var tdTipo = par.children("td:nth-child(5)");
    var tdAgregar = par.children("td:nth-child(6)");
    var tdEliminar = par.children("td:nth-child(7)");

    tdNombre.html("<input class='form-control input-sm' type='text' value=" + tdNombre.html() + ">"); +
    tdDescripcion.html("<input class='form-control input-sm'" +
        " type='text' value=" + tdDescripcion.html() + ">");
    tdInicio.html("<input class='form-control input-sm datepicker' type='text' value=" + tdInicio.html() + ">");
    tdFin.html("<input class='form-control input-sm datepicker' type='text' value=" + tdFin.html() + ">");
   

    if (tdTipo.html() == "Evento de un día") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option selected value='Evento de un día'>Evento de un día</option>" +
                "<option value='Evento de varios días'>Evento de varios días</option>" +
                "<option value='Otro'>Otro</option>" +
                  " </select>");
    }
    if (tdTipo.html() == "Evento de varios días") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option value='Evento de un día'>Evento de un día</option>" +
                "<option selected value='Evento de varios días'>Evento de varios días</option>" +
                "<option value='Otro'>Otro</option>" +
                  " </select>");
    }
       
    if (tdTipo.html() == "Otro") {
        tdTipo.html("<select class='form-control selectpicker input-sm'>" +
                "<option value='Evento de un día'>Evento de un día</option>" +
                "<option value='Evento de varios días'>Evento de varios días</option>" +
                "<option value='Reunión/Consejo de profesores'>Reunión/Consejo de profesores</option>" +
                "<option selected value='Otro'>Otro</option>" +
                  " </select>");
    }
    tdAgregar.html("<i class='ui-icon ui-icon-check i-agregar-eventos'></i>"); //El ícono cambia al de agregar
    tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-eventos'></i>");

    $("#div-table-lista-eventos-cargar tbody").find('.datepicker').datepicker();
    $("#div-table-lista-eventos-cargar tbody").find('.bootstrap-timepicker').timepicker({
        showSeconds: true,
        showMeridian: false,
        defaultTime: 'current'
    });

    $(".i-agregar-eventos").bind("click", Save);
    $(".i-eliminar-fila-eventos").bind("click", Delete);
};
function Delete() {
    var par = $(this).parent().parent(); //tr
    par.remove();

    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-eventos-cargar tr:last').remove();


    //Se agrega la última fila solo con el botón de añadir
    $("#div-table-lista-eventos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-eventos'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-eventos'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-descripcion'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-eventos'></td>" +
            "<td class='td-eliminar-eventos'></td>" +
        "</tr>");

    $("#i-add-fila-eventos").bind("click", Add);
};
function BorrarTodo() {
    //Se borra todas las filas de la tabla
    $("#div-table-lista-eventos-cargar tbody tr").remove();

    /*Se inserta una fila solamente con el botón de añadir más eventos*/
    $("#div-table-lista-eventos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-eventos'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-eventos'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-descripcion'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-eventos'></td>" +
            "<td class='td-eliminar-eventos'></td>" +
        "</tr>");

    $("#i-add-fila-eventos").bind("click", Add);
};
function SalvarTodo() {
    //Se salvan todas las filas de las tablas
    $('#div-table-lista-eventos-cargar tbody tr').each(function () {
        var tdNombre = $(this).children("td:nth-child(1)");
        var tdDescripcion = $(this).children("td:nth-child(2)");
        var tdInicio = $(this).children("td:nth-child(3)");
        var tdFin = $(this).children("td:nth-child(4)");
        var tdTipo = $(this).children("td:nth-child(5)");
        var tdAgregar = $(this).children("td:nth-child(6)");
        var tdEliminar = $(this).children("td:nth-child(7)");

        tdNombre.html(tdNombre.children("input[type=text]").val());
        tdDescripcion.html(tdDescripcion.children("input[type=text]").val());
        tdInicio.html(tdInicio.children("input[type=text]").val());
        tdFin.html(tdFin.children("input[type=text]").val());
        tdTipo.html(tdTipo.children("select").val());
        tdAgregar.html("<i class='fa fa-edit i-editar-eventos'></i>"); //El ícono cambia al de editar
        tdEliminar.html("<i class='fa fa-minus-circle i-eliminar-fila-eventos'></i>");
    });
    //Se borra la última fila (la que contiene solo el símbolo de añadir)
    $('#div-table-lista-eventos-cargar tr:last').remove();
    /* Se inserta una fila solamente con el botón de añadir más alumnos. Esto se hace debido a que la última 
     * fila queda con un signo de más para agregar, y con el símbolo de editar y de borrar fila. Por eso se 
     * elimina*/
    $("#div-table-lista-eventos-cargar tbody").append(
        "<tr>" +
            "<td class='td-nro-eventos'>" +
                "<i class='ui-icon ui-icon-plusthick' id='i-add-fila-eventos'></i>" +
            "</td>" +
            "<td class='td-nombre'></td>" +
            "<td class='td-descripcion'></td>" +
            "<td class='td-inicio'></td>" +
            "<td class='td-fin'></td>" +
            "<td class='td-tipo'></td>" +
            "<td class='td-agregar-eventos'></td>" +
            "<td class='td-eliminar-eventos'></td>" +
        "</tr>");


    $(".i-editar-eventos").bind("click", Edit);
    $(".i-eliminar-fila-eventos").bind("click", Delete);
    $("#i-add-fila-eventos").bind("click", Add);
}
// Agregar Eventos
function AgregarEventos() {
    SalvarTodo();
    $('#div-table-lista-eventos-cargar tr:last').remove(); //Para borrar la última fila con el signo '+'
    console.log("Entro vieja");
    var tdNombre;
    var tdDescripcion;
    var tdInicio;
    var tdFin;
    var tdTipo;
    var tdAgregar;
    var tdEliminar;
    var evento;
    var listaEventos = [];

    listaEventos.push(idColegio);
   
    $('#div-table-lista-eventos-cargar tbody tr').each(function () {
        tdNombre = $(this).children("td:nth-child(1)").html();
        tdDescripcion = $(this).children("td:nth-child(2)").html();
        tdInicio = $(this).children("td:nth-child(3)").html();
        tdFin = $(this).children("td:nth-child(4)").html();
        tdTipo = $(this).children("td:nth-child(5)").html();
        tdAgregar = $(this).children("td:nth-child(6)").html();
        tdEliminar = $(this).children("td:nth-child(7)").html();

        evento = [tdNombre, tdDescripcion, tdInicio, tdFin, tdTipo];

        listaEventos.push(evento);
    });

    var postData = { values: listaEventos };
    
    $.ajax({
        type: "POST",
        url: "/Eventos/CrearEvento",
        traditional: true,
        data: postData,
        success: function (r) {
            window.location.href = 'CrearEvento';
        }
    });
}

$(document).ready(function () {
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
                    }
                    else {
                        //Limpiando el text-box del año escolar
                        $("#ano-escolar").val("No posee año escolar activo");
                    }
                }
            });
        }
        else {
            //Limpiando el text-box del año escolar
            $("#ano-escolar").val("");
        }
    });

    /* Botones de edición de las filas de la tabla de lista de alumnos */
    $("#i-add-fila-eventos").bind("click", Add);
    $(".i-eliminar-fila-eventos").bind("click", Delete);
    $(".i-editar-eventos").bind("click", Edit);
    $("#i-eliminar-todas-filas").bind("click", BorrarTodo);
    $("#i-salvar-todas-filas").bind("click", SalvarTodo);

    $("#btn-agregar-eventos").bind("click", AgregarEventos)
});

        



    


