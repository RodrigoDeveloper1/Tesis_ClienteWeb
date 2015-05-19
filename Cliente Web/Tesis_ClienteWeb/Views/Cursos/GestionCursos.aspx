<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CursosModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de cursos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-8">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-crear" })%>
        </div>

        <!-- Año escolar -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!-- Paneles de información -->
    <div class="row">
        <!-- Panel de lista de cursos -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Listado de cursos agregados:</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de alumnos editable -->
                    <div class="col-xs-12" id="div-table-lista-cursos">
                        <table class="table table-striped" id="table-lista-cursos">
                            <thead>
                                <tr>
                                    <th class="th-nombre">Cursos</th>
                                    <th class="th-periodo-escolar">Año escolar</th>
                                    <th class="th-estatus">Grado</th>
                                    <th class="th-estatus">Sección</th>
                                    <th class="th-opcion">Eliminar</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Cursos/Cursos.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Cursos/CrearCurso.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Cursos/GestionCursos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Cursos/GestionCursos.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Gestión de cursos
</asp:Content>
