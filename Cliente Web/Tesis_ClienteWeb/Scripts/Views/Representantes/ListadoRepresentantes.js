
$(document).ready(function () {

    //Obtener tabla de cursos 
    $("#select-curso").change(function () {
        var lista = "";       

        if ($(this).val() != "") {
            $('#table-lista-alumnos').find('tbody').find('tr').remove();
          
            idCurso = $(this).val();

            $.ajax({
                url: "/Bridge/ObtenerTablaAlumnosPorIdCurso",
                type: "POST",
                data: {
                    "idCurso": idCurso
                }
            }).done(function (data) {
          
            for (var i = 0; i < data.length; i++) {
                lista += ('<tr id="'+ data[i].idEstudiante + '">' +
                            '<td class="td-apellidos-alumno">' + data[i].apellido1 + '</td>' +
                            '<td class="td-apellidos-alumno">' + data[i].apellido2 + '</td>' +
                            '<td class="td-nombres-alumno">' + data[i].nombre1 + '</td>' +
                            '<td class="td-nombres-alumno">' + data[i].nombre2 + '</td>' +
                          '</tr>');
            }
            $('#table-lista-alumnos').find('tbody').end().append(lista);
            });

        }
        else {

            $('#table-lista-alumnos').find('tbody').find('tr').remove();
           

            lista = ('<tr>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +
                    '</tr>');

            $('#table-lista-alumnos').find('tbody').find('tr').end().append(lista);      

        }
    });

    $('#table-lista-alumnos').on("click", "tr", function () {

        var state = $(this).hasClass('active');
        $('.active').removeClass('active');

        if (!state) {
            $(this).addClass('active');
        }


        var lista = "";


        if ($(this).attr('id') != "") {
            $('#table-lista-representantes').find('tbody').find('tr').remove();
            

            idEstudiante = $(this).attr('id');

            $.ajax({
                url: "/Representantes/ObtenerTablaRepresentantesPorIdAlumno",
                type: "POST",
                data: {
                    "idEstudiante": idEstudiante
                }
            }).done(function (data) {

                for (var i = 0; i < data.length; i++) {
                    lista += ('<tr id="' + data[i].idRepresentante + '">' +
                                '<td class="td-nombres-alumno">' + data[i].nombre1 + '</td>' +
                                '<td class="td-apellidos-alumno">' + data[i].apellido1 + '</td>' +
                                '<td class="td-apellidos-alumno">' + data[i].apellido2 + '</td>' +
                              '</tr>');
                }
                $('#table-lista-representantes').find('tbody').end().append(lista);
            });

        }
        else {

            $('#table-lista-representantes').find('tbody').find('tr').remove();
          

            lista = ('<tr>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-apellidos-alumno"></td>' +
                                    '<td class="td-nombres-alumno"></td>' +                                   
                    '</tr>');

            $('#table-lista-representantes').find('tbody').find('tr').end().append(lista);

        }



    });

});