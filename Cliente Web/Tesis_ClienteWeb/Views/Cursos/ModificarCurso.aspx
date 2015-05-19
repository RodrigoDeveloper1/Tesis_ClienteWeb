<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CursosModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar curso
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Mensaje de error -->
    <div class="row">
        <div class="col-xs-12">
            <div class="alert alert-danger" id="div-alerta-ext-file" role="alert">
                <strong>Solo se aceptan archivos de formato excel</strong>
            </div>
        </div>
    </div>

    <!-- Títulos de secciones -->
    <div class="row">
        <div class="col-xs-6">
            <h4 class="subtitulos">Campos obligatorios (*)</h4>
        </div>

        <div class="col-xs-6">
            <h4 class="subtitulos">Lista de alumnos del curso</h4>
        </div>
    </div>

    <!-- Paneles de información -->
    <div class="row">
        <!-- Panel de campos obligatorios -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <!-- Cabecera de los paneles -->
                <div class="panel-heading">
                    <ul class="nav nav-tabs" id="ul-nav-campos-obligatorios" role="tablist">
                        <li class="active" id="li-datos-generales">
                            <a href="#"><strong>Datos generales*</strong></a>
                        </li>
                        <li id="li-periodo-escolar">
                            <a href="#"><strong>Datos del período escolar*</strong></a>
                        </li>
                    </ul>
                </div>

                <!-- Panel de Datos Generales -->
                <div class="panel-body" id="div-panel-datos-generales">
                    <% using (Html.BeginForm("ModificarCurso", "Cursos", FormMethod.Post, new
                    {
                        @class = "form",
                        @role = "form"
                    }))
                       { %>

                    <%: Html.AntiForgeryToken() %>
                    
                    <%: Html.HiddenFor(q => q.Course.CourseId) %>

                    <!-- Nombre del curso -->
                    <div class="form-group col-xs-12">
                        <%: Html.LabelFor(q => q.Course.Name)%>
                        <%: Html.TextBoxFor(q => q.Course.Name, new { @type = "text", @class = "form-control", 
                            @id = "input-nombre-auto", disabled="disabled" }) %>
                    </div>

                    <!-- Grado del curso-->
                    <div class="form-group col-xs-6">
                        <label for="select-grado-modif">Grado correspondiente:</label>

                        <select class="form-control selectpicker" id="select-grado">
                            <option value="0">Seleccionar un grado</option>
                            <option value="1">1er Grado</option>
                            <option value="2">2do Grado</option>
                            <option value="3">3er Grado</option>
                            <option value="4">4to Grado</option>
                            <option value="5">5to Grado</option>
                            <option value="6">6to Grado</option>
                            <option value="7">7mo Grado</option>
                            <option value="8">8vo Grado</option>
                            <option value="9">9no Grado</option>
                            <option value="10">4to Año</option>
                            <option value="11">5to Año</option>
                        </select>
                    </div>

                    <!-- Sección del curso -->
                    <div class="form-group col-xs-6">
                        <label for="select-seccion">Sección:</label>

                        <select class="form-control selectpicker" id="select-seccion">
                            <option value="0">Seccionar sección</option>
                            <option value="u">Sección única</option>
                            <option value="a">A</option>
                            <option value="b">B</option>
                            <option value="c">C</option>
                            <option value="d">D</option>
                            <option value="e">E</option>
                            <option value="f">F</option>
                        </select>
                    </div>                   

                    <!-- Nro de alumnos del curso -->
                    <div class="form-group col-xs-4">
                        <label for="input-nro-alumnos"># Alumnos (autogenerable):</label>
                        <%: Html.TextBoxFor(q => q.Course.StudentTotal, new { @type = "text", @class = "form-control", 
                            @id = "input-nro-alumnos", disabled="disabled"}) %>
                    </div>

                    <!-- Nro de asignaturas del curso -->
                    <div class="form-group col-xs-4">
                        <label for="input-nro-asignaturas"># Asignaturas (autogenerable):</label>

                    </div>

                    <!-- Estatus del curso -->
                    <div class="form-group col-xs-4">
                        <label for="input-estatus">Estatus (autogenerable):</label>
                        <%: Html.TextBoxFor(q => q.Course.Status, new { @type = "text", @class = "form-control", 
                            @id = "input-estatus", disabled="disabled" }) %>
                    </div>

                </div>

                <!-- Panel de Períodos Escolares -->
                <div class="panel-body" id="div-panel-periodo-escolar">

                    <!-- Fecha de Inicio -->
                    <div class="form-group col-xs-6">
                        <label for="fecha-inicio">Fecha inicio:</label>
                        <%: Html.TextBoxFor(q => q.Course.StartDate, new { @type = "text", @class = "datepicker form-control", 
                            @id = "fecha-inicio" }) %>
                    </div>

                    <!-- Fecha de Finalización -->
                    <div class="form-group col-xs-6">
                        <label for="fecha-finalizacion">Fecha finalización:</label>
                        <%: Html.TextBoxFor(q => q.Course.FinishDate, new { @type = "text", @class = "datepicker form-control", 
                            @id = "fecha-finalizacion" }) %>
                    </div>


                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>


                    <div class="row">
                        <div class="col-xs-3">
                            <label>Lapsos:</label>
                        </div>
                        <div class="col-xs-3">
                            <label>Fecha de inicio:</label>
                        </div>
                        <div class="col-xs-3">
                            <label>Fecha de finalización:</label>
                        </div>
                        <div class="col-xs-3">
                            <label>Nro. Días:</label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-3">
                            <label>1er Lapso:</label>
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-ini-1" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-fin-1" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="form-control" id="nro-dias-lapso-1" disabled />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-3">
                            <label>2do Lapso:</label>
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-ini-2" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-fin-2" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="form-control" id="nro-dias-lapso-2" disabled />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-3">
                            <label>3er Lapso:</label>
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-ini-3" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="datepicker form-control" id="fec-fin-3" />
                        </div>
                        <div class="col-xs-3">
                            <input type="text" class="form-control" id="nro-dias-lapso3" disabled />
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <!-- Fin de Panel de campos obligatorios -->

        <!-- Panel de lista de alumnos -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <div class="panel-heading"><strong>Seleccionar archivo de lista de alumnos:</strong></div>
                <div class="panel-body">
                    <!-- Seleccionador de archivos -->
                    <div class="col-xs-12">
                        <div class="form-group input-group">
                            <span class="input-group-btn">
                                <span class="btn btn-primary btn-file" id="span-btn-files">
                                    <strong>Examinar...</strong><input type="file" id="input-load-file" />
                                </span>
                            </span>
                            <input type="text" class="form-control" readonly="" />
                        </div>
                    </div>
                    <!-- Fin de Seleccionador de archivos -->

                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>

                    <!-- Tabla de alumnos editable -->
                    <div class="col-xs-12" id="div-table-lista-alumnos-cargar">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th class="th-nro-alumno">#</th>
                                    <th class="th-primerapellido-alumno">Primer Apellido</th>
                                    <th class="th-segundoapellido-alumno">Segundo Apellido</th>
                                    <th class="th-primernombre-alumno">Primer Nombre</th>
                                    <th class="th-segundonombre-alumno">Segundo Nombre</th>
                                    <th class="th-eliminar-alumno">
                                        <i class="fa fa-minus-circle" id="i-eliminar-todas-filas"></i>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-nro-alumno">
                                        <i class="ui-icon ui-icon-plusthick" id="i-add-fila-alumno"></i>
                                    </td>
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-nombres-alumno"></td>
                                    <td class="td-agregar-alumno"></td>
                                    <td class="td-eliminar-alumno"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <!-- Fin de Tabla de alumnos editable -->
                </div>
            </div>
        </div>
        <!-- Fin de Panel de lista de alumnos -->

        <!-- Panel de Lista de Asignaturas -->
        <div class="col-xs-6" id="div-panel-lista-asignaturas">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de asignaturas</strong>
                </div>

                <div class="panel-body">

                    <div class="col-xs-5">
                        <label for="select-from">Lista de materias</label>
                        <select class="form-control" name="selectfrom" id="select-from" multiple size="11">
                            <% foreach (var schoolsubject in Model.listaMaterias)
                               { %>

                            <option value=""><%: schoolsubject.Name %></option>

                            <% } %>
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

                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>

                    <div class="col-xs-12 text-center">
                        <button class="btn btn-primary">
                            <strong>Cargar evaluaciones por asignatura</strong>
                        </button>
                    </div>

                </div>
            </div>
        </div>
        <!-- Fin de Panel de Lista de Asignaturas -->
    </div>

    <!-- Botón: Crear curso -->
    <div class="row">
        <div class="col-xs-12 text-center">
            <button class="btn btn-lg btn-default" type="submit" >
                <strong>Modificar curso</strong>
            </button>
        </div>
    </div>
    <% } %>
    <!-- Separador -->
    <div class="row">
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Cursos/Cursos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Cursos/ModificarCurso.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Modificar curso
</asp:Content>
