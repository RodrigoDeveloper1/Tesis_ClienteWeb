<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AlumnosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Alumnos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Lista de cursos -->
    <div class="row">
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="row">
        <div class="form-group col-xs-12"></div>
        <div class="form-group col-xs-12"></div>
    </div>

    <!-- Inicio tabla alumnos -->
    <div class="row">
        <!-- Panel de Lista de Alumnos -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de alumnos</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de Alumnos -->
                    <div class="col-xs-12" id="div-tabla-lista-alumnos">
                        <table class="table" id="table-lista-alumnos">
                            <thead>
                                <tr>
                                    <th class="th-primerapellido-alumno">#</th>
                                    <th class="th-primerapellido-alumno">Matrícula</th>
                                    <th class="th-primerapellido-alumno">1er Apellido</th>
                                    <th class="th-segundoapellido-alumno">2do Apellido</th>
                                    <th class="th-primernombre-alumno">1er Nombre</th>
                                    <th class="th-segundonombre-alumno">2do Nombre</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr> 
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-nombres-alumno"></td>
                                    <td class="td-nombres-alumno"></td>                            
                                </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Separador -->
    <div class="row">
        <div class="col-lg-12"><div class="separador"></div></div>
    </div>
    
    <div class="row">
        <div class="col-xs-12">
            <h4 class="subtitulos">Información del curso seleccionado</h4>
        </div>
        
        <!-- Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Detalles de cursos -->
        <div class="col-xs-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Detalles del curso</strong>
                </div>
                <div class="panel-body">
                    <div class="col-xs-6" id="Detalle-Curso-Panel-Div-Parte-1">
                        <p><strong>Grado del curso: </strong><span id="span-grado"></span></p>
                        <p><strong>Número de alumnos: </strong><span id="span-nro-alumnos"></span></p>
                        <p><strong>Lapso en curso: </strong><span id="span-lapso"></span></p>
                            <p>
                            <strong>Período escolar: </strong>
                            <span id="span-periodo-escolar"></span>
                        </p>                       
                       
                    </div>
                    <div class="col-xs-6" id="Detalle-Curso-Panel-Div-Parte-2">
                        
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
        
        <!-- Panel de datos estadísticos del curso -->
        <div class="col-xs-5" >
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
                           <button type="submit" class="btn btn-primary" id="boton_est_alumnos">
                                <strong>Ver más estadísticas</strong>
                            </button>                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Botón Cancelar -->
    <div class="row">
        <div class="col-xs-12 text-center">            
        <% using (Html.BeginForm("Inicio", "Index", FormMethod.Get, new { @class = "form", @role = "form" }))
        { %>
            <button class="btn btn-lg btn-default" type="submit">
                Cancelar
            </button>
        <% } %>
        </div>
    </div>

    <!-- Separador -->
    <div class="row">
        <div class="form-group col-xs-12"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Alumnos/ListaAlumnosDocentes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/Alumnos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Listado de alumnos
</asp:Content>
