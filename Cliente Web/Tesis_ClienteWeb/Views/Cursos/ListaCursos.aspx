<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CursosModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista de Cursos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!---------------------------------- Fila de Tabla de Cursos -------------------------------------------->
    <div class="row">
        <div class="col-lg-12" id="div-table-lista-cursos">
            <table class="table" id="table-lista-cursos">
                <thead>
                    <tr>
                        <th class="th-nombre">Cursos</th>
                        <th class="th-periodo-escolar">Año escolar</th>
                        <th class="th-estatus">Grado</th>
                        <th class="th-estatus">Sección</th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var course in Model.listaCursos)
                       {
                                                        
                    %>
                    <tr id="<%: course.CourseId %>">
                        <td class="td-nombre"><%: course.Name %></td>
                        <td class="periodo-escolar"><%: Model.SchoolYear.StartDate.ToString("dd-MM-yyyy")%> - 
                                                    <%: Model.SchoolYear.EndDate.ToString("dd-MM-yyyy")%></td>
                        <td class="td-status"><%: course.Grade %></td>
                        <td class="td-status"><%: course.Section %></td>
                    </tr>
                    <% } %> 
                </tbody>
            </table>
        </div>

        <!-- Separador -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!---------------------------------------- Fila de detalles de curso ------------------------------------>
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Información del curso seleccionado</h4>
        </div>

        <!------------------------------------ Panel de detalles del curso ---------------------------------->
        <div class="col-xs-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Detalles del curso</strong>
                </div>
                <div class="panel-body">
                    <div class="col-xs-6" id= "Detalle-Curso-Panel-Div-Parte-1">
                        <p><strong>Grado del curso: </strong><span id="span-grado"></span></p>
                        <p><strong>Número de alumnos: </strong><span id="span-nro-alumnos"></span></p>
                        <p><strong>Lapso en curso: </strong><span id="span-lapso"></span></p>
                        <p>
                            <strong>Período escolar: </strong>
                            <span id="span-periodo-escolar"></span>
                        </p>
                    </div>
                    <div class="col-xs-6" id= "Detalle-Curso-Panel-Div-Parte-2" >
                        
                        <p>
                            <strong>Lapsos: </strong>
                        </p>
                        <ul>
                            <li>I Lapso: <span id="span-lapso-I"></span></li>
                            <li>II Lapso: <span id="span-lapso-II"></span></li>
                            <li>III Lapso: <span id="span-lapso-III"></span></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!--------------------------------- Fin de Panel de detalles del curso ------------------------------>

        <!------------------------------------ Tabla de lista de alumnos ------------------------------------>
        <div class="col-xs-5">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de alumnos</strong>
                </div>

                <div class="panel-body" id="div-panel-lista-alumnos">
                    <div class="col-xs-12" id="div-table-lista-alumnos">
                        <table class="table table-striped" id="table-lista-alumnos">
                            <thead>
                                <tr>
                                    <th class="th-numero-alumno">#</th>
                                    <th class="th-apellidos-alum">Apellidos</th>
                                    <th class="th-nombres-alum">Nombre(s)</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-numero-alumno"></td>
                                    <td class="td-apellidos-alum"></td>
                                    <td class="td-nombres-alum"></td>
                                </tr>                              
                            </tbody>
                        </table>
                    </div>
                    <div class="col-xs-12">
                        <div class="text-center"> 
                            <% using (Html.BeginForm("Alumnos", "Alumnnos", FormMethod.Get, new
                            {
                                @class = "form",
                                @role = "form"
                            }))
                            { %>
                            <button type="button" class="btn btn-primary">
                                <strong>Ver detalles</strong>
                            </button>
                            <% } %>
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--------------------------------- Fin de Fila de detalles de curso -------------------------------->

        <!-------------------------------- Panel de datos estadísticos del curso ---------------------------->
        <div class="col-xs-7" id="div-panel-datos-esta">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Datos estadísticos</strong>
                </div>
                <div class="panel-body">
                    <div class="col-xs-12" id="detalle_rendimiento">
                        <p>
                            <strong>Cantidad de materias: </strong>
                            <span></span>
                        </p>
                        <p>
                            <strong>Cantidad de evaluaciones: </strong>
                            <span></span>
                        </p>
                    </div>

                    <div class="col-xs-12">
                        <div class="text-center">
                                 <button type="submit" class="btn btn-primary" id="boton_est_curso">
                                    <strong>Ver más estadísticas</strong>
                                </button>
                              </div>
                    </div>
                </div>
            </div>
        </div>
        <!----------------------------- Fin de Panel de datos estadísticos del curso ------------------------>
    </div>
    <!------------------------------------- Fin de Fila de detalles de curso -------------------------------->

    <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">            
        <% using (Html.BeginForm("Inicio", "Index", FormMethod.Get, new
            {
                @class = "form",
                @role = "form"
            }))
            { %>
            <button class="btn btn-lg btn-default" type="submit">
                Cancelar
            </button>
        <% } %>
        </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Cursos/Cursos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Cursos/Cursos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Lista de cursos
</asp:Content>
