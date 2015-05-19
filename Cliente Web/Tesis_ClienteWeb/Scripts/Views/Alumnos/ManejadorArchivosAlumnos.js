/*********************************************************************************************************/
/* Referencias de:                                                                                       */
/* http://www.abeautifulsite.net/whipping-file-inputs-into-shape-with-bootstrap-3/                       */
/* http://www.codeproject.com/Questions/763540/How-to-browse-only-xls-and-xlsx-file-in-html-file         */
/* http://stackoverflow.com/questions/16963787/file-upload-with-jquery-ajax-and-handler-ashx-not-working */
/* https://cmatskas.com/upload-files-in-asp-net-mvc-with-javascript-and-c/                               */
/*                                                                                                       */
/*********************************************************************************************************/
$(document).on('change', '#span-btn-files :file', function (e) {
    var extensionesExcel = new Array(".xlsx", ".xls");
    var input = $(this); //Guardando el nombre del archivo. Ej: 'Prueba.xlsx'
    var extensionArchivo = input.val().substring(input.val().lastIndexOf('.')); //Obteniendo la extensión

    /*Verificando que el archivo seleccionado sea de extensión excel*/
    if (extensionesExcel.indexOf(extensionArchivo) < 0) {
        $("#div-alerta-ext-file").show(400).delay(2000).slideUp(400);
        $("#FileNameAlumnosExcel").val("");
    }
    else {
        var archivos = input.get(0).files;
        var archivoExcel = archivos[0];
        var label = input.val().replace(/\\/g, '/').replace(/.*\//, ''); //Nombre del archivo
        var dataForm = new FormData();
        dataForm.append('file', archivoExcel);

        $("#FileNameAlumnosExcel").val(label);

        showProgress();

        /*Código para cargar el archivo excel en el servidor*/
        $.ajax({
            url: "/Bridge/CargarArchivoExcelAlumnosEnTabla_AgustinianoCristoRey",
            type: "POST",
            contentType: false, //Opciones para decirle a Jquery que no procese la data o que no se preocupe
            processData: false, //por el tipo de contenido.
            data: dataForm
        }).done(function (data) {
            if (data[0].success) {
                hideProgress();
                AgregadoAutomatico(data); //Ubicado en el archivo 'GestionTablasAlumnos.js'
            }
            else 
                window.location.href = 'AgregarAlumno';
        });
    }
    /*Fin de la verificación*/
});
/********************************************************************************************************/
/* Fin de referencias                                                                                   */
/********************************************************************************************************/