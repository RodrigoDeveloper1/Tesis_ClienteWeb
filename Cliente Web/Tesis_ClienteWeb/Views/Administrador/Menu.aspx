<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MenuViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Menú de maestras
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Sección Maestras de Alumnos/Estudiantes -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Alumnos/Estudiantes</h4>
        </div>      

        <!-- Agregar alumno -->
        <%if( Model.diccionarioAcceso[Model.MASTER_ACTION_ALUMNOS_AGREGAR_ALUMNOS]) { %>
        <div class="col-xs-1">
            <a class="fa fa-child fa-lg iconos-maestras" href="../Alumnos/AgregarAlumno">
                <span class="icono-mensaje label label-info">Agregar alumno</span>
            </a>
        </div>
        <% } %>
                
        <!-- Gestión de alumnos -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_ALUMNOS_GESTION_ALUMNOS]) { %>
        <div class="col-xs-1">
            <a class="fa fa-group fa-lg iconos-maestras" href="../Alumnos/GestionAlumnos">
                <span class="icono-mensaje label label-info">Gestión de alumnos</span>
            </a>
        </div>
        <% } %>

        <!-- Asociar representantes -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_ALUMNOS_ASOCIAR_REPRESENTANTES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-cab fa-lg iconos-maestras" href="../Alumnos/AsociarRepresentantes">
                <span class="icono-mensaje label label-info">Asociar representantes</span>
            </a>
        </div>
        <% } %>

        <!-- Separador -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    
    <!-- Sección Maestras de Colegios -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Colegios, años y períodos escolares</h4>
        </div>      
        
        <!-- Creación de colegios -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_COLEGIOS_CREAR_COLEGIO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-institution fa-lg iconos-maestras" href="../Colegios/CrearColegio">
                <span class="icono-mensaje label label-info">Creación de colegios</span>
            </a>
        </div>
        <% } %>

        <!-- Listado de colegios -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_COLEGIOS_LISTADO_COLEGIOS]) { %>
        <div class="col-xs-1">
            <a class="fa fa-building-o fa-lg iconos-maestras" href="../Colegios/ListarColegios">
                <span class="icono-mensaje label label-info">Listado de colegios</span>
            </a>
        </div>
        <% } %>
        
        <!-- Creación de nuevo año escolar -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_COLEGIOS_NUEVO_ANO_ESCOLAR]) { %>
        <div class="col-xs-1">
            <a class="fa fa-yelp fa-lg iconos-maestras" href="../Colegios/CrearAnoEscolar">
                <span class="icono-mensaje label label-info">Creación de nuevo año escolar</span>
            </a>
        </div>
        <% } %>

        <!-- Asignación de períodos escolares -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_COLEGIOS_ASIGNACION_PERIODOS_ESCOLARES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-cubes fa-lg iconos-maestras" href="../Colegios/CrearPeriodoEscolar">
                <span class="icono-mensaje label label-info">Asignación de períodos escolares</span>
            </a>
        </div>
        <% } %>

        <!-- Separador -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Cursos -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Cursos</h4>
        </div>      
        
        <!-- Crear Curso -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_CURSOS_CREAR_CURSO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-puzzle-piece fa-lg iconos-maestras" href="../Cursos/CrearCurso">
                <span class="icono-mensaje label label-info">Crear Curso</span>
            </a>
        </div>
        <% } %>

        <!-- Gestión de Cursos -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_CURSOS_GESTION_CURSO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-fire-extinguisher  fa-lg iconos-maestras" href="../Cursos/GestionCursos">
                <span class="icono-mensaje label label-info">Gestión de Cursos</span>
            </a>
        </div>
        <% } %>
        
        <!-- Separador -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Docentes -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Docentes</h4>
        </div>      
         
        <!-- Asociar docente -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_DOCENTE_ASOCIAR_DOCENTE]) { %>
        <div class="col-xs-1">
            <a class="fa fa-binoculars fa-lg iconos-maestras" href="../Docente/AgregarDocente">
                <span class="icono-mensaje label label-info">Asociar docente</span>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>        
    
    <!-- Sección Maestras de Evaluaciones -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Evaluaciones</h4>
        </div>    

        <!-- Crear evaluación -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_EVALUACION_CREAR_EVALUACION]) { %>
        <div class="col-xs-1">
            <a class="fa fa-gavel fa-lg iconos-maestras" href="../Evaluaciones/CrearEvaluacion">
                <span class="icono-mensaje label label-info">Crear Evaluación</span>
            </a>
        </div>
        <% } %>

        <!-- Modificar evaluaciones -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_EVALUACION_MODIFICAR_EVALUACION]) { %>
        <div class="col-xs-1">
            <a class="fa fa-list fa-lg iconos-maestras" href="../Evaluaciones/ModificarEvaluacion">
                <span class="icono-mensaje label label-info">Modificar Evaluaciones</span>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Eventos -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Eventos</h4>
        </div>      
        
        <!-- Crear eventos generales -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_EVENTOS_CREAR_EVENTOS_GENERALES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-dot-circle-o fa-lg iconos-maestras" href="../Eventos/CrearEvento">
                <h4 class="icono-mensaje label label-info">Crear Eventos Generales</h4>
            </a>
        </div>
        <% } %>

        <!-- Gestión de eventos generales -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_EVENTOS_GESTION_EVENTOS_GENERALES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-calendar  fa-lg iconos-maestras" href="../Eventos/GestionEventos">
                <h4 class="icono-mensaje label label-info">Gestión de Eventos Generales</h4>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Materias -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Materias</h4>
        </div>      
        
        <!-- Gestión de materias -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_MATERIAS_GESTION_MATERIAS]) { %>
        <div class="col-xs-1">
             <a class="fa fa-book fa-lg iconos-maestras" href="../Materias/CrearMateria">
                <span class="icono-mensaje label label-info">Gestión de materias</span>
            </a>
        </div>
        <% } %>

        <!-- Modificar materias -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_MATERIAS_MODIFICAR_MATERIAS]) { %>
        <div class="col-xs-1">
             <a class="fa fa-eraser fa-lg iconos-maestras" href="../Materias/ModificarMateria">
                <span class="icono-mensaje label label-info">Modificar materias</span>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Notificaciones -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Notificaciones</h4>
        </div>      
        
        <!-- Gestión de notificaciones automáticas -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_AUTOMATICAS]) { %>
        <div class="col-xs-1">
            <a class="fa fa-bell fa-lg iconos-maestras" href="../Notificaciones/NotificacionesAutomaticas">
                <span class="icono-mensaje label label-info">Gestión de notificaciones automáticas</span>
            </a>
        </div>
        <% } %>
        
        <!-- Gestión de notificaciones personalizadas -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_NOTIFICACIONES_GESTION_NOTIFICACIONES_PERSONALIZADAS]) { %>
        <div class="col-xs-1">
            <a class="fa fa-cubes fa-lg iconos-maestras" href="../Notificaciones/NotificacionesPersonalizadas">
                <span class="icono-mensaje label label-info">Gestión de notificaciones personalizadas</span>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
        
    

    <!-- Sección Maestras de Seguridad -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Seguridad</h4>
        </div>

        <!-- Agregar usuario -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_AGREGAR_USUARIO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-user fa-lg iconos-maestras" href="AgregarUsuario">
                <span class="icono-mensaje label label-info">Agregar usuario</span>
            </a>
        </div>
        <% } %>

        <!-- Listar usuarios -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_LISTAR_USUARIO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-users fa-lg iconos-maestras" href="ListarUsuarios">
                <span class="icono-mensaje label label-info">Listar usuarios</span>
            </a>
        </div>
        <% } %>

        <!-- Bloquear/desbloquear usuarios -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_BLOQUEAR_DESBLOQUEAR_USUARIO]) { %>
        <div class="col-xs-1">
            <a class="fa fa-ban fa-lg iconos-maestras" href="DesbloquearUsuario">
                <span class="icono-mensaje label label-info">Bloquear/Desbloquear usuarios</span>
            </a>
        </div>
        <% } %>

        <!-- Agregar rol -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_AGREGAR_ROL]) { %>
        <div class="col-xs-1">
            <a class="fa fa-male fa-lg iconos-maestras" href="AgregarRol">
                <span class="icono-mensaje label label-info">Agregar rol</span>
            </a>
        </div>
        <% } %>

        <!-- Listar roles -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_LISTAR_ROLES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-sitemap fa-lg iconos-maestras" href="ListarRoles">
                <span class="icono-mensaje label label-info">Listar roles</span>
            </a>
        </div>
        <% } %>

        <!-- Gestión de perfiles -->
        <%if (Model.diccionarioAcceso[Model.MASTER_ACTION_SEGURIDAD_GESTION_PERFILES]) { %>
        <div class="col-xs-1">
            <a class="fa fa-newspaper-o fa-lg iconos-maestras" href="GestionPerfiles">
                <span class="icono-mensaje label label-info">Gestión de perfiles</span>
            </a>
        </div>
        <% } %>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Razonamiento Vocacional -->
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Razonamiento Vocacional</h4>
        </div>      
        
        <!-- Gestión de notificaciones automáticas -->
        <div class="col-xs-1">
            <a class="fa fa-comments-o fa-lg iconos-maestras" href="../Notificaciones/NotificacionesAutomaticas">
                <span class="icono-mensaje label label-info">Razonamiento Vocacional</span>
            </a>
        </div>
        
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Sección Maestras de Sistema Experto -->
    <!--
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Sistema Experto</h4>
        </div>      
        
        <div class="col-xs-1">
            <a class="fa fa-bolt fa-lg iconos-maestras" href="SistemaExperto">
                <span class="icono-mensaje label label-info">Sistema Experto</span>
            </a>
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Administrador/Administrador.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Menú de maestras
</asp:Content>