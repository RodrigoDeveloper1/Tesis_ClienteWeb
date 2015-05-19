$(window).load(function () {
    if (window.location.pathname != "/Index/Inicio")
        $('#loading-gif').fadeOut(1000);
});

// Fabio Puchetti 07/03/2015 - función que valida que solo se ingresen datos numéricos
 function isNumberKey(evt)
{
     evt = (evt) ? evt : window.event;
     var charCode = (evt.which) ? evt.which : evt.keyCode;
     if (charCode > 31 && (charCode < 48 || charCode > 57)) {
         return false;
     }
     return true;
}

$(document).ready(function () {
    $('.selectpicker').selectpicker('deselectAll');    
    $('.selectpicker').datepicker('refresh');

    $.datepicker.setDefaults({
        dateFormat: 'dd-mm-yy',
        DayOfWeek: ["Domingo", "Lunes", "Martes", "Mi&eacute;rcoles", "Jueves", "Viernes", "S&aacute;bado"],
        dayNamesMin: ["Dom", "Lun", "Mar", "Mie", "Jue", "Vie", "Sab"],
        minDate: '01-01-2000',
        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre",
                     "Octubre", "Noviembre", "Diciembre"]
    });
});