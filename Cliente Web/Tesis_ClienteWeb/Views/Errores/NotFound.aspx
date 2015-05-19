<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.LoginModel>" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>404 Not Found</title>

    <!-- Archivos .css -->
    <link rel="Stylesheet" href="../../Content/Css/Login/Login.css" type="text/css" />
    <link rel="Stylesheet" href="../../Content/Css/Login/Maestra.css" type="text/css" />
    <link rel="Stylesheet" href="../../Content/Css/Login/freelancer.css" type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-3.2.0/css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/bootstrap-3.2.0/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Plug-ins/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <!-- Archivos scripts -->
    <script src="../../Scripts/jquery-1.5.1.js" type="text/javascript"></script>
</head>

<body id="page-top" class="index">
 
    <!-- Sección de inicio de sesión -->
    <header>
        <div class="container">
            <div class="row">                
                <div class="col-lg-6">
                    <div class="intro-text">
                        <!--span class="name">Faro Atenas</span-->

                        <img src="../../Content/Images/Login/logoletrasblancas.png" alt="Logo Faro Atenas" 
                        class="" id="imagen-logo" />

                        <hr class="star-light" />                        
                    </div>
                                        
               </div>

                 <div class="col-lg-6">
                    <div class="intro-text">
                        <h1 class="name">¡Oops!</h1>   
                        <h3 class="">La página web que está buscando no se pudo encontrar. Compruebe la 
                            dirección e inténtelo de nuevo . Si usted está buscando un área en faroatenas.com 
                            puede regresar a la página principal
                        </h3>

                        <a class ="btn regresar" href="../Index/Inicio"> 
                            <img src="../../Content/Images/Otras/back.png" />
                        </a> 
                    </div>
               </div>
            </div>
        </div>
    </header>
</body>  
</html>