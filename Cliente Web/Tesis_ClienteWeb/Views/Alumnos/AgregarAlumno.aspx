<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AgregarAlumnosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar alumnos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Mensaje de error personalizado para la carga de archivos excel -->
    <div class="row">
        <div class="col-xs-12">
            <div class="alert alert-danger" id="div-alerta-ext-file" role="alert">
                <strong>Solo se aceptan archivos de formato excel</strong>
            </div>
        </div>
    </div>
    
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idColegio, new { @class="form-group cabecera-tips" })%>
                <div class="form-group tip-informacion">
                    *

                    <div class="label label-info tip-mensaje" id="tip-lista-colegios-1">
                        Se muestran solo aquellos colegios activos. 
                    </div> 
                </div>
            </div>
            
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
            new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idCurso, new { @class="form-group cabecera-tips" })%> 
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-cursos-1">
                        Los cursos cargados en esta lista son aquellos que pertenecen a períodos escolares 
                        activos, a lo que corresponderán a un año escolar en curso. Se muestran solo aquellos 
                        cursos que no tienen cargados alumnos.
                    </div> 
                </div>
            </div>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!-- Panel de lista de alumnos -->
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

                    <!-- Separador -->
                    <div class="col-xs-12">
                        <div class="separador"></div>
                    </div>

                    <!-- Tabla de alumnos editable -->
                    <div class="col-xs-12" id="div-table-lista-alumnos-cargar">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th class="th-nro-alumno">#</th>
                                    <th class="th-matricula"># Matrícula</th>
                                    <th class="th-primerapellido-alumno">1er Apellido</th>
                                    <th class="th-segundoapellido-alumno">2do Apellido</th>
                                    <th class="th-primernombre-alumno">1er Nombre</th>
                                    <th class="th-segundonombre-alumno">2do Nombre</th>
                                    <th class="th-agregar-alumno">
                                        <i class="ui-icon ui-icon-check iconos-comentados" 
                                            id="i-salvar-todas-filas">
                                        </i>
                                    </th>
                                    <th class="th-eliminar-alumno">
                                        <i class="fa fa-minus-circle iconos-comentados" 
                                            id="i-eliminar-todas-filas">
                                        </i>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-nro-alumno">
                                        <i class="ui-icon ui-icon-plusthick" id="i-add-fila-alumno"></i>
                                    </td>
                                    <td class="td-matricula"></td>
                                    <td class="td-apellidos-alumno"></td>
                                    <td class="td-nombres-alumno"></td>
                                    <td class="td-eliminar-alumno"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>
        
        <!-- Botón Agregar -->
        <div class="col-xs-6 text-right">
            <button type="button" class="btn btn-lg btn-default" id="btn-agregar-alumno">Agregar</button>
        </div>

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
            <% using (Html.BeginForm("Menu", "Administrador", FormMethod.Get, new
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

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Alumnos/GestionTablasAlumnos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Alumnos/ManejadorArchivosAlumnos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/GestionTablasAlumnos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/AgregarAlumnos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Alumnos/ManejadorArchivosAlumnos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Agregar alumnos
</asp:Content>