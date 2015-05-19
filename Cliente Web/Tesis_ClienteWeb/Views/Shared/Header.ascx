<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Tesis_ClienteWeb.Models.MaestraModel>"  %>

<!-- Header -->
<nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
    <!-- Sección de logo y nombre sistema -->
    <div class="navbar-header">
        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
        </button>

        <!-- Imagen logo -->
        <a class="navbar-brand" href="../Index/Inicio" id="a-img-logo">
            <img id="imagen-logo" src="../../Content/Images/Maestra/logosinletras.png"/>
            <img id="letras-imagen-logo" src="../../Content/Images/Maestra/letrasblanco1.png"/>
        </a>
    </div>

    <!-- Sección de íconos menú top -->
    <ul class="nav navbar-top-links navbar-right">
        <!-- Lista de colegios-->   
        <% if(Model.ADMINISTRADOR) { %>
        <li>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, new { @class = "form-control" + 
                " selectpicker class-colegios",  @id = "colegios-administrador" })%>
        </li>
        <% } %>

        <!-- Ícono de sección Maestra -->   
        <% if (Model.ACCESO_MAESTRAS)
           { %>
        <li>
            <a href="../Administrador/Menu" class="iconos-seleccionados">
                <i class="fa fa-cogs fa-fw icono-blanco"></i>
                <span class="icono-mensaje-header label label-default">Menú de maestras</span>
            </a>
        </li>
        <% } %>

        <!-- Ícono de Próximos Eventos -->
        <li class="dropdown">
            <a href="#" class="dropdown-toggle iconos-seleccionados" data-toggle="dropdown">
                <i class="fa fa-calendar fa-fw icono-blanco"></i>  
                <i class="fa fa-caret-down icono-blanco"></i>
                <span class="icono-mensaje-header label label-default">Próximos eventos</span>
            </a>

            <ul class="dropdown-menu dropdown-tasks">
                <% if(Model.ADMINISTRADOR) { %> <!-- Sección administrador -->
                    <li>
                        <a href="../Eventos/CalendarioEventos">
                            <div>
                                <i style="color: red" class="fa fa-ban"></i>
                                No hay eventos para administradores
                            </div>
                        </a>
                    </li>
                <% }
                   else if (Model.ListaEventosHeader.Count() == 0) { %> <!-- No posee eventos asociados -->
                    <li>
                        <a>
                            <div>
                                <i style="color: red" class="fa fa-ban"></i>
                                Aún no hay próximos eventos asociados
                            </div>
                        </a>
                    </li>
                <% }
                   else { %> <!-- Sección no administrador -->
                    <li>
                        <a href="../Eventos/CalendarioEventos">
                            <div class="header-next-notifications">
                                <i class="fa fa-calendar fa-fw"></i> 
                                <%: Model.ListaEventosHeader[0].Name + " (Desde " +
                                Model.ListaEventosHeader[0].StartDate.ToShortDateString() + ", hasta " +
                                Model.ListaEventosHeader[0].FinishDate.ToShortDateString() + ")"%>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a href="../Eventos/CalendarioEventos">
                            <div class="header-next-notifications">
                                <i class="fa fa-calendar fa-fw"></i> 
                                <%: Model.ListaEventosHeader[1].Name + " (Desde " +
                                Model.ListaEventosHeader[1].StartDate.ToShortDateString() + ", hasta " +
                                Model.ListaEventosHeader[1].FinishDate.ToShortDateString()  + ")"%>
                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a href="../Eventos/CalendarioEventos">
                            <div class="header-next-notifications">
                                <i class="fa fa-calendar fa-fw"></i> 
                                <%: Model.ListaEventosHeader[2].Name + " (Desde " +
                                Model.ListaEventosHeader[2].StartDate.ToShortDateString() + ", hasta " +
                                Model.ListaEventosHeader[2].FinishDate.ToShortDateString() + ")"%>
                            </div>
                        </a>
                    </li>
                <% } %>
            </ul>
        </li>
        
        <!-- Ícono de progreso de los lapsos -->
        <li class="dropdown">
            <a class="dropdown-toggle iconos-seleccionados" data-toggle="dropdown" href="#">
                <i class="fa fa-tasks fa-fw icono-blanco"></i>  
                <i class="fa fa-caret-down icono-blanco"></i>
                <span class="icono-mensaje-header label label-default">Progreso de lapsos</span>
            </a>

            <ul class="dropdown-menu dropdown-tasks">
                <li> <!-- 1er Lapso -->
                    <a>
                        <div>
                            <p>
                                <strong>1er Lapso</strong>
                                <span class="pull-right text-muted">
                                    <%: Model.ListaPorcentajesPeriodosHeader[0]%>
                                    % completado
                                </span>
                            </p>

                            <div class="progress progress-striped active">
                                <div class="progress-bar progress-bar-success" 
                                    role="progressbar"
                                    aria-valuenow="<%: Model.ListaPorcentajesPeriodosHeader[0]%>" 
                                    aria-valuemin="0" 
                                    aria-valuemax="100" 
                                    style="width:<%: Model.ListaPorcentajesPeriodosHeader[0]%>%">
                                </div>
                            </div>
                        </div>
                    </a>
                </li>
                <li class="divider"></li>
                <li> <!-- 2do Lapso -->
                    <a>
                        <div>
                            <p>
                                <strong>2do Lapso</strong>
                                <span class="pull-right text-muted">
                                    <%: Model.ListaPorcentajesPeriodosHeader[1]%>
                                    % completado
                                </span>
                            </p>

                            <div class="progress progress-striped active">
                                <div class="progress-bar progress-bar-info" 
                                    role="progressbar" 
                                    aria-valuenow="<%: Model.ListaPorcentajesPeriodosHeader[1]%>" 
                                    aria-valuemin="0" 
                                    aria-valuemax="100" 
                                    style="width:<%: Model.ListaPorcentajesPeriodosHeader[1]%>%">
                                </div>
                            </div>
                        </div>
                    </a>
                </li>
                <li class="divider"></li>
                <li> <!-- 3er Lapso -->
                    <a>
                        <div>
                            <p>
                                <strong>3er Lapso</strong>
                                <span class="pull-right text-muted">
                                    <%: Model.ListaPorcentajesPeriodosHeader[2]%>
                                    % completado
                                </span>
                            </p>

                            <div class="progress progress-striped active">
                                <div class="progress-bar progress-bar-warning" 
                                    role="progressbar" 
                                    aria-valuenow="<%: Model.ListaPorcentajesPeriodosHeader[2]%>" 
                                    aria-valuemin="0" 
                                    aria-valuemax="100" 
                                    style="width:<%: Model.ListaPorcentajesPeriodosHeader[2]%>%">
                                </div>
                            </div>
                        </div>
                    </a>
                </li>
                <li class="divider"></li>
                <li> <!-- Año escolar -->
                    <a>
                        <div>
                            <p>
                                <strong>Año escolar</strong>
                                <span class="pull-right text-muted">
                                    <%: Model.ListaPorcentajesPeriodosHeader[3]%>
                                    % completado
                                </span>
                            </p>

                            <div class="progress progress-striped active">
                                <div class="progress-bar progress-bar-danger" 
                                    role="progressbar" 
                                    aria-valuenow="<%: Model.ListaPorcentajesPeriodosHeader[3]%>" 
                                    aria-valuemin="0" 
                                    aria-valuemax="100"
                                    style="width:<%: Model.ListaPorcentajesPeriodosHeader[3]%>%">
                                </div>
                            </div>
                        </div>
                    </a>
                </li>
            </ul>
        </li>

        <!-- Ícono de notificaciones -->
        <li class="dropdown" id="icono-notificaciones">
            <a class="dropdown-toggle iconos-seleccionados" data-toggle="dropdown" href="#">
                <i class="fa fa-bell fa-fw icono-blanco"></i>  
                
                <!-- # notificaciones -->
                <div id="div-container-nro-notificaciones">
                    <div id="nro-notificaciones" style="display: <%: Model.MostrarNroNotificaciones %>">
                        <%: Model.nroNotificacionesNoLeidas %>
                    </div>
                </div>

                <i class="fa fa-caret-down icono-blanco"></i>
                <span class="icono-mensaje-header label label-default">Ver notificaciones</span>
            </a>

            <ul class="dropdown-menu dropdown-alerts">
                <li>
                    <a href="#">
                        <div>
                            <i class="fa fa-twitter fa-fw"></i> 
                            Nuevas notificaciones
                        </div>
                    </a>
                </li>
                <li class="divider"></li>
                <li>
                    <a href="#">
                        <div>
                            <i class="fa fa-comment fa-fw"></i> 
                            Notificaciones del sistema
                        </div>
                    </a>
                </li>
                <li class="divider"></li>
                <li>
                    <a href="#">
                        <div>
                            <i class="fa fa-envelope fa-fw"></i> 
                            Enviar notificación
                        </div>
                    </a>
                </li>
               
                <li class="divider"></li>
                <li>
                    <a class="text-center" href="../Notificaciones/Buzon">
                        <strong>Ver todas las notificaciones</strong>
                        <i class="fa fa-angle-right"></i>
                    </a>
                </li>
            </ul>
        </li>

        <!-- Ícono de usuario -->
        <li class="dropdown">
            <a class="dropdown-toggle iconos-seleccionados" id="a-usuario" data-toggle="dropdown" href="#">
                <i class="fa fa-user fa-fw icono-blanco"></i>
                <%: Model._USERNAME %>
                <i class="fa fa-caret-down icono-blanco"></i>
            </a>

            <ul class="dropdown-menu dropdown-user">
                <li><a href="../Profile/Profile"><i class="fa fa-user fa-fw"></i>Editar perfil</a></li>
                <li><a href="#"><i class="fa fa-gear fa-fw"></i>Preferencias</a></li>
                <li class="divider"></li>
                <li><a href="#" id="logout-action"><i class="fa fa-sign-out fa-fw"></i> Cerrar sesión</a></li>
            </ul>
        </li>
    </ul>

    <!-- Menú barra de lado izquierdo -->
    <div class="navbar-default sidebar " role="navigation">
        <div class="sidebar-nav navbar-collapse">
            <ul class="nav" id="side-menu">
                <!-- Buscador -->
                <li class="sidebar-search">
                    <div class="input-group custom-search-form">
                        <input type="text" class="form-control" placeholder="Buscar..." />

                        <span class="input-group-btn">

                        <button class="btn btn-default" type="button">
                            <i class="fa fa-search"></i>
                        </button>
                    </span>
                    </div>
                </li>

                <!-- Menú de Notificaciones-->
                <li>
                    <a href="../Notificaciones/Buzon">
                        <i class="fa fa-envelope fa-fw"></i>
                        Notificaciones
                        <span id="nro-notificaciones2" style="display: <%: Model.MostrarNroNotificaciones %>">
                            <%: Model.nroNotificacionesNoLeidas %>
                        </span>
                    </a>
                </li>

                <!-- Menú de Alumnos -->
                <li>
                    <a href="#"><i class="fa fa-group fa-fw"></i>Alumnos<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Alumnos/Alumnos">Listado de alumnos</a></li>
                        <li>
                            <a href="#">Representantes<span class="fa arrow"></span></a>
                            <ul class="nav nav-third-level">
                                <li><a href="../Representantes/ListadoRepresentantes">Listado de representantes</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>

                <!-- Menú de Cursos -->
                <li>
                    <a href="#"><i class="fa fa-table fa-fw"></i>Cursos<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Cursos/ListaCursos">Listado de cursos</a></li>
                    </ul>
                </li>

                <!-- Menú de Estadísticas -->
                <li>
                    <a href="#"><i class="fa fa-bar-chart-o fa-fw"></i>Estadísticas<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Estadisticas/EstadisticasEvaluaciones">Estadísticas por evaluación</a></li>
                    </ul>
                    <ul class="nav nav-second-level">
                        <li><a href="../Estadisticas/EstadisticasMaterias">Estadísticas por materia</a></li>
                    </ul>
                    <ul class="nav nav-second-level">
                        <li><a href="../Estadisticas/EstadisticasCursos">Estadísticas por curso</a></li>
                    </ul>                    
                </li>
                
                <!-- Menú de Estrategias Pedagógicas -->
                <li>
                    <a href="#"><i class="fa fa-cogs fa-fw"></i>Estrategias Pedagógicas</a>
                </li>                   
                
                <!-- Menú de Evaluaciones -->
                <li>
                    <a href="#"><i class="fa fa-pencil fa-fw"></i>Evaluaciones<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Evaluaciones/CrearEvaluacionProfesor">Crear evaluaciones</a></li>
                        <li><a href="../Evaluaciones/ModificarEvaluacionProfesor">Modificar evaluaciones</a></li>
                        <li><a href="../Evaluaciones/Evaluaciones">Listado de evaluaciones</a></li>
                        <li>
                            <a href="#">Calificaciones<span class="fa arrow"></span></a>
                            <ul class="nav nav-third-level">
                                 <li><a href="/Calificaciones/GestionCalificaciones">Gestión de calificaciones</a></li>
                                 <li><a href="/Calificaciones/Definitivas">Notas Definitivas</a></li>
                            </ul>
                        </li>
                    </ul>
                </li>

                <!-- Menú de Eventos -->
                <li>
                    <a href="#"><i class="fa fa-calendar fa-fw"></i>Eventos<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Eventos/CalendarioEventos">Calendario de eventos</a></li>
                    </ul>
                </li>

                <!-- Menú de Materias -->
                <li>
                    <a href="#"><i class="fa fa-book fa-fw"></i> Materias<span class="fa arrow"></span></a>
                    <ul class="nav nav-second-level">
                        <li><a href="../Materias/Materias">Listado de materias</a></li>
                    </ul>
                </li>                                
            </ul>
        </div>        
    </div>
</nav>