<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CursosModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Creación de curso
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-crear" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-3">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>

        <!--Separador con márgen -->
        <div class="form-group col-xs-12"></div>
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Títulos de secciones -->
    <div class="row">
        <div class="col-xs-6">
            <h4 class="subtitulos">Datos del curso:</h4>
        </div>

        <div class="col-xs-6">
            <h4 class="subtitulos">Lista de cursos:</h4>
        </div>
    </div>

    <!-- Paneles de información -->
    <div class="row">
        <!-- Panel de datos del curso -->
        <div class="col-xs-6">
            <div class="panel panel-default">
                <!-- Panel de Datos Generales -->
                <div class="panel-body" id="div-panel-datos-generales">
                    <form class="form" role="form">
                        <!-- Nombre del curso -->
                        <div class="form-group col-xs-12">
                            <label for="input-nombre-auto">Nombre del curso (autogenerable):</label>
                            <input type="text" class="form-control" id="input-nombre-auto" disabled />
                        </div>
                        <!-- Grado del curso-->
                        <div class="form-group col-xs-6">
                            <label for="select-grado">Grado correspondiente:</label>

                            <select class="form-control selectpicker" id="select-grado">
                                <option value="0">Seleccione un grado</option>
                                <option value="1">1er Grado</option>
                                <option value="2">2do Grado</option>
                                <option value="3">3er Grado</option>
                                <option value="4">4to Grado</option>
                                <option value="5">5to Grado</option>
                                <option value="6">6to Grado</option>
                                <option value="7">7mo Grado</option>
                                <option value="8">8vo Grado</option>
                                <option value="9">9no Grado</option>
                                <option value="10">10mo Grado</option>
                                <option value="11">11mo Grado</option>
                            </select>
                        </div>
                        <!-- Sección del curso -->
                        <div class="form-group col-xs-6">
                            <label for="select-seccion">Sección:</label>

                            <select class="form-control selectpicker" id="select-seccion">
                                <option value="0">Seleccione una sección</option>
                                <option value="U">Sección única</option>
                                <option value="A">A</option>
                                <option value="B">B</option>
                                <option value="C">C</option>
                                <option value="D">D</option>
                                <option value="E">E</option>
                                <option value="F">F</option>
                            </select>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <!-- Panel de lista de cursos -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Listado de cursos agregados:</strong>
                </div>
                <div class="panel-body">

                    <!-- Tabla de alumnos editable -->
                    <div class="col-xs-12" id="div-table-lista-cursos">
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
                                <tr id="">
                                    <td class="td-nombre"></td>
                                    <td class="periodo-escolar"></td>
                                    <td class="td-status"></td>
                                    <td class="td-status"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <!-- Fin de Tabla de alumnos editable -->
                </div>
            </div>
        </div>
        
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Panel de Lista de Asignaturas -->
        <div class="col-xs-6" id="div-panel-lista-asignaturas">
            <div class="panel panel-primary">
                <!-- Cabecera del panel -->
                <div class="panel-heading">
                    <strong>Lista de asignaturas</strong>
                </div>

                <!-- Cuerpo del panel -->
                <div class="panel-body">
                    <form class="form" role="form">
                        <div class="col-xs-5" id="div-lista-materias">
                            <label for="select-from">Lista de materias</label>
                            <select class="form-control" name="selectfrom" id="select-from" multiple size="11">
                            </select>
                        </div>

                        <div class="col-xs-2 text-center">
                            <br />
                            <br />
                            <br />
                            <br />

                            <a href="JavaScript:void(0);" id="btn-add">
                                <span class="glyphicon glyphicon-forward"></span>
                            </a>

                            <br />
                            <br />

                            <a href="JavaScript:void(0);" id="btn-remove">
                                <span class="glyphicon glyphicon-backward"></span>
                            </a>
                        </div>

                        <div class="col-xs-5">
                            <label for="select-to">Materias añadidas</label>
                            <select class="form-control" name="selectto" id="select-to" multiple size="11">
                            </select>
                        </div>
                        <!--Separador -->
                        <div class="form-group col-xs-12"></div>
                        <div class="form-group col-xs-12"></div>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Botón: Crear curso -->
    <div class="row">
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-default " id="CrearCursoBoton">
                Crear curso
            </button>
        </div>
        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("Menu", "Administrador", FormMethod.Get, new
            {
                @class = "form",
                @role = "form"
            }))
               { %>
            <button class="btn btn-lg btn-default " type="submit">
                Cancelar
            </button>
            <% } %>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!-- Separador -->
    <div class="row">
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Cursos/Cursos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Cursos/CrearCurso.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Cursos/Cursos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Creación de curso
</asp:Content>
