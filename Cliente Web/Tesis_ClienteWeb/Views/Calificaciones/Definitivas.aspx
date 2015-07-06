<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.CalificacionesModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Definitivas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Lista de cursos & lista de materias -->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de materias -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-materia" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="row">
        <div class="form-group col-xs-12"></div>
        <div class="col-lg-12"><div class="separador"></div></div>
    </div>

    <!-- Tabla de notas definitivas -->
    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Notas Definitivas</strong>
                </div>

                <div class="panel-body">
                    <div class="col-xs-12" id="div-tabla-lista-notas">
                        <table class="table" id="table-lista-notas">
                            <thead>
                                <tr>
                                    <th class="th-num-lista ">#</th>
                                    <th class="th-nombre-alumno">Alumno</th>
                                    <th class="th-1er-lapso">1er Lapso</th>
                                    <th class="th-2do-lapso">2do Lapso</th>
                                    <th class="th-3er-lapso">3er Lapso</th>
                                    <th class="th-definitiva">Definitiva</th>
                                </tr>
                            </thead>
                            <tbody>
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

    <!-- Lista de botones -->
    <div class="row">
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Calificaciones/handsontable.full.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Calificaciones/Definitivas.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Calificaciones/Definitivas.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Calificaciones/handsontable.full.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Definitivas
</asp:Content>

