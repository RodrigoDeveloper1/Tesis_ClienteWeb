<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AgregarAlumnosModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de alumnos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idColegio, new { @class="form-group cabecera-tips" })%>
                <div class="form-group tip-informacion"> 
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-colegios-2">
                        Se muestran solo aquellos colegios activos. 
                    </div> 
                </div>
            </div>
            
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
            new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-3">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>

        <!-- Lista de cursos -->
        <div class="col-xs-3">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idCurso, new { @class="form-group cabecera-tips" })%> 
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-cursos-2">
                        Los cursos cargados en esta lista son aquellos que pertenecen a períodos escolares 
                        activos, a lo que corresponderán a un año escolar en curso. 
                    </div> 
                </div>
            </div>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!--Separador normal -->
        <div class="form-group col-xs-12"></div>

        <!-- Separador con márgen -->
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>

        <!-- Tabla de alumnos y representantes -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de alumnos y representantes</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de alumnos -->
                    <div class="col-xs-12" id="div-tabla-lista-alumnos-cargar">
                        <table class="table" id="table-lista-alumnos">
                            <thead>
                                <tr>
                                    <th class="th-nro-lista">#</th>
                                    <th class="th-apellidos">Apellidos</th>
                                    <th class="th-nombres">Nombres</th>
                                    <th class="th-representante-1">Representante #1</th>
                                    <th class="th-representante-2">Representante #2</th>
                                    <th class="th-editar centrar">Editar</th>
                                    <th class="th-eliminar centrar">Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Separador -->
        <div class="row">
            <div class="col-xs-12">
                <div class="separador"></div>
            </div>
        </div>    	

        <!-- Botón Cancelar -->
        <div class="col-xs-12 text-center">            
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
    <link rel="Stylesheet" href="../../Content/Css/Alumnos/GestionAlumnos.css" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Alumnos/GestionAlumnos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de alumnos
</asp:Content>