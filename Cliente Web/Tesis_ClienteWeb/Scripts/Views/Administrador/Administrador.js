$(document).ready(function () {
    /* ****************************************************************************************************
     * Fuente: http://stackoverflow.com/questions/9353585/html-passwordfor-dont-let-to-paste-the-password *
     * Para evitar el copy/paste                                                                          *
     * ****************************************************************************************************/
    $('#text-box-password').bind("paste", function (e) {
        e.preventDefault();
    });
    $('#text-box-conf-password').bind("paste", function (e) {
        e.preventDefault();
    });
    $('#text-box-conf-password').bind("paste", function (e) {
        e.preventDefault();
    });
    $('#text-box-correo').bind("paste", function (e) {
        e.preventDefault();
    });
    $('#text-box-correo-confirmar').bind("paste", function (e) {
        e.preventDefault();
    });

    $("#btn-registrar-usuario").click(function () {
        showProgress();
    });
});