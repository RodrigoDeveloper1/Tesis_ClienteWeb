var idColegio = "";
var idAnoEscolar = "";

$(document).ready(function () {
    //Obtener lista de años escolares
    $("#select-colegio").change(function () {
        if ($(this).val() != "") {
            showProgress();

            $("#select-ano-escolar").find('option').remove().end().append("<option>Cargando años " +
                "escolares...</option>");
            $("#select-ano-escolar").selectpicker("refresh");

            $.ajax({
                type: "POST",
                url: "/Bridge/ObtenerAnosEscolaresSinPeriodos",
                data: {
                    idColegio: $(this).val()
                },
                success: function (data) {
                    if (data != null && data.length > 0) {
                        var lista = '<option value="">Seleccione el año escolar...</option>';

                        for (var i = 0; i < data.length; i++) {
                            lista += ('<option value="' + data[i].idAnoEscolar + '">' + data[i].AnoEscolar +
                                '</option>');
                        }

                        $("#select-ano-escolar").find('option').remove().end().append(lista);
                        $("#select-ano-escolar").selectpicker("refresh");

                        hideProgress();
                    }
                    else {
                        $("#select-ano-escolar").find('option').remove().end().append('<option>No se ' +
                            'encontraron años escolares sin períodos</option>');
                        $("#select-ano-escolar").selectpicker("refresh");

                        idAnoEscolar = "";

                        hideProgress();
                    }
                }
            });

            
        }
        else {
            $("#select-ano-escolar").find('option').remove().end().append('<option>Seleccione el año ' +
                'escolar </option>');
            $("#select-ano-escolar").selectpicker("refresh");

            idColegio = ""
            idAnoEscolar = "";
        }
    });

    //Obteniendo el id del año escolar
    $("#select-ano-escolar").change(function () {
        idAnoEscolar = $(this).val();
    });

    $("#btn-agregar").click(function () {
        if (idAnoEscolar == "")
            swal("¡Oops!", "Por favor, seleccione un colegio y/o un año escolar.", "warning");
        else
        {
            var fec_ini_1 = $('#fec-ini-1').val();
            var fec_fin_1 = $('#fec-fin-1').val();
            var fec_ini_2 = $('#fec-ini-2').val();
            var fec_fin_2 = $('#fec-fin-1').val();
            var fec_ini_3 = $('#fec-ini-3').val();
            var fec_fin_3 = $('#fec-fin-3').val();

            showProgress();

            $.ajax({
                type: "POST",
                url: "/Colegios/AsociarPeriodosEscolares",
                data: {
                    idAnoEscolar: idAnoEscolar,
                    fec_ini_1: fec_ini_1,
                    fec_fin_1: fec_fin_1,
                    fec_ini_2: fec_ini_2,
                    fec_fin_2: fec_fin_2,
                    fec_ini_3: fec_ini_3,
                    fec_fin_3: fec_fin_3
                },
                success: function (data) {
                    window.location.href = 'CrearPeriodoEscolar';
                }
            });
        }
    });
});