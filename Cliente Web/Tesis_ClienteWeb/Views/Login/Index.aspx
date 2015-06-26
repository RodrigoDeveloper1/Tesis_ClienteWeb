<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.LoginModel>" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Inicio de sesión</title>

    <!-- Archivos .css -->
    <link rel="Stylesheet" href="../../Content/Css/Login/Login.css" type="text/css" />
    <link rel="Stylesheet" href="../../Content/Css/Login/Maestra.css" type="text/css" />
    <link rel="Stylesheet" href="../../Content/Css/Login/freelancer.css" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-3.2.0/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-3.2.0/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="../../Content/Plug-ins/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <!-- Archivos scripts -->
    <script src="../../Scripts/jquery-1.5.1.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Login/Login.js" type="text/javascript"></script>
</head>

<body id="page-top" class="index">
    <!-- Cabecera -->
    <!--nav class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle" data-toggle="collapse" 
                data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>                
            </div>

            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">
                    <li class="hidden">
                        <a href="#page-top"></a>
                    </li>                    
                    <li class="page-scroll">
                        <a href="#about">Sobre Nosotros</a>
                    </li>
                    <li class="page-scroll">
                        <a href="../Contacto/Contacto">Cont&aacute;ctanos</a>
                    </li>
                </ul>
            </div>            
        </div>        
    </!--nav-->

    <!-- Sección de inicio de sesión -->
    <header>
        <div class="container" id="div-cuerpo-login">
            <!-- Link de descarga app móvil-->
            <div class="row">
                <div class="col-xs-offset-8 col-xs-4">
                    <a class="btn btn-info" role="button">
                        <i class="fa fa-key"></i>
                        <strong>Descargar app móvil</strong>
                    </a>
                </div>
            </div>

            <div class="row" >
                <div class="col-lg-12">
                    <div class="intro-text">
                        <!--span class="name">Faro Atenas</span-->

                        <img src="../../Content/Images/Login/logoletrasblancas.png" alt="Logo Faro Atenas" 
                        class="" id="imagen-logo" />

                        <hr class="star-light" />                        
                    </div>
                                        
                    <!-------------------------------- Credenciales de login -------------------------------->
                    <div id="login-body-credentials">
                        <% using (Html.BeginForm("Index", "Login", FormMethod.Post, new 
                           {
                               @class = "form form-horizontal", 
                               @id = "form-login-credentials", 
                               @role = "form" 
                           })) 
                           { %>
                        <%: Html.AntiForgeryToken() %>

                        <!-- Mensaje de error -->
                        <div class="row">
                            <div class="input-extra-large col-centered" id="mensaje-error">
                                <%: Html.ValidationSummary("", 
                                new { 
                                    @class = "alert alert-danger alerta-error", 
                                    @role="alert", 
                                    @style="display: " + Model.MostrarErrores}) 
                                %>
                            </div>
                        </div>

                        <!-- Nombre de usuario -->
                        <div class="input-extra-large col-centered">
                            <%: Html.TextBoxFor(m => m.Username, new { @class = "form-control", 
                                    @PlaceHolder = "Nombre de usuario" }) %>
                        </div>

                        <!-- Contraseña -->
                        <div class="input-extra-large col-centered">
                            <%: Html.PasswordFor(m => m.Password, new { @class = "form-control", 
                                    @PlaceHolder = "Contraseña" }) %>
                        </div>

                        <!-- Botón: Agregar -->
                        <button class="btn btn-lg btn-default" id="btn-login" type="submit" >
                            <i class="fa fa-lock fa-fw"></i>Iniciar Sesi&oacute;n
                        </button>
                        <% } %>
                    </div>
                    <!---------------------------- Fin de Credenciales de login ----------------------------->
                </div>
            </div>
        </div>
    </header>
</body>  
</html>