/************************************************************************************************************/
/* Referencia de:                                                                                           */
/* http://mrbool.com/how-to-add-edit-and-delete-rows-of-a-html-table-with-jquery/26721#ixzz3DEXgbakd        */
/*                                                                                                          */
/************************************************************************************************************/
var idColegio;

function CargarTablaCursos() {
    var lista = "";
    $('#table-lista-cursos').find('tbody').find('tr').remove();   
    
    showProgress();
    $.post("/Cursos/ObtenerSelectListCursosPorColegio",
    {
        idColegio: idColegio
    },
    function (data) {
        if (data[0].success) {
            for (var i = 0; i < data.length; i++) {
                lista += ('<tr id=' + data[i].idCurso + ' >' +
                            ' <td class="td-nombre">' + data[i].nombre + '</td>' +
                            ' <td class="periodo-escolar">' + data[i].anoescolar + '</td>' +
                            ' <td class="td-status">' + data[i].grado + '</td>' +
                            ' <td class="td-status">' + data[i].seccion + '</td>' +
                            '<td class=td-opcion id=' + data[i].idCurso + '>' +
                                    '<a class= "fa fa-ban" ></a>' +
                                '</td>' +
                          '</tr>');
            }
            $('#table-lista-cursos').find('tbody').end().append(lista);
            hideProgress();
        }
        else {
            lista = ('<tr>' +
                           ' <td class="td-nombre"></td>' +
                            ' <td class="periodo-escolar"></td>' +
                            ' <td class="td-status"></td>' +
                            ' <td class="td-status"></td>' +
                            ' <td class="td-opcion"></td>' +
                    '</tr>');

            $('#table-lista-cursos').find('tbody').find('tr').end().append(lista);

            hideProgress();
            swal("¡No hay cursos!", "El colegio no posee cursos", "warning");
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
                        CargarTablaCursos();
                    }
                    else {
                        //Limpiando el text-box del año escolar
                        $("#ano-escolar").val("No posee año escolar activo");
                        $('#table-lista-cursos').empty();
                    }
                }
            });
        }
        else {
            $("#ano-escolar").val(""); //Limpiando el text-box del año escolar
            $('#table-lista-cursos').empty();
        }        
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

    // Eliminar Curso
    $('#table-lista-cursos').on('click', 'td.td-opcion', function (e) {
        var selectedId = $(this).attr('id');
        
        swal({
            title: "¿Estás Seguro?",
            text: "¡No serás capaz de recuperar este curso!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "¡Si, bórralo!",
            cancelButtonText: "¡No, cancelalo!",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                swal("¡Borrado!", "Su curso ha sido borrado.", "success");
                jQuery.get("/Cursos/EliminarCurso",
                    {
                        "id": selectedId
                    }).done(function (data) {
                        swal({
                            title: "¡Eliminado!",
                            text: "El curso ha sido eliminado.",
                            type: "success",
                            confirmButtonColor: "green",
                            closeOnConfirm: true,
                        },
                        function () {
                            window.location.href = 'GestionCursos';
                        });
                    });
            }
            else
                swal("¡Cancelado!", "Su curso está a salvo :)", "error");
        });
    });
});
