<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AlumnosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cargar Alumnos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12">
            <div class="alert alert-danger" id="div-alerta-ext-file" role="alert">
                <strong>Solo se aceptan archivos de formato excel</strong>
            </div>
        </div>
    </div>

    <!-------------------------------- Combobox de cursos -------------------------------------->
    <div class="row">
        <div class="col-lg-4 " id="Div-seleccionarcurso-Alumnos">
            <select class="selectpicker" data-style="btn-primary" data-live-search="true">
                <option selected disabled>Seleccione un curso...</option>
                <% foreach (var course in Model.listaCursos)
                   { %>

                <option value=""><%: course.Name %></option>

                <% } %>
            </select>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!--------------------------------- Panel de lista de alumnos --------------------------------------->
    <div class="col-xs-12">
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
                        <input id="FileNameAlumnosExcel" type="text" class="form-control" readonly="" />
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
                                <td class="td-eliminar-alumno"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!-- Fin de Tabla de alumnos editable -->
            </div>
        </div>
    </div>

    <!-- Botón: Cargar alumnos -->
    <div class="row">
        <div class="col-xs-5 text-center">
            <button class="btn btn-primary" id="CargarAlumnosBoton">
                <strong>Cargar alumnos</strong>
            </button>
        </div>


        <div class="col-xs-5 text-center">
            <button class="btn btn-primary" id="CargarAlumnosPorExcelBoton">
                <strong>Cargar alumnos por excel</strong>
            </button>
        </div>
    </div>

    <div class="col-xs-12">
        <div class="separador"></div>
    </div>
    <div class="col-xs-12">
        <div class="separador"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Alumnos/ListaAlumnos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/Alumnos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Carga de alumnos
</asp:Content>
