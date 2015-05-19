


$(document).ready(function () {
    $("#btn-login").click(function (e) {

        console.log("Acción: click -> Botón Iniciar Sesión (#btn-login)");
        console.log("Ajax.GET: /Index/Inicio()");

        /*$.ajax({
            type: "POST",
            url: "/Login/Index",
            data: {
                url: "LoQueSea"
            },
            dataType: "html"
        });*/
    });
});