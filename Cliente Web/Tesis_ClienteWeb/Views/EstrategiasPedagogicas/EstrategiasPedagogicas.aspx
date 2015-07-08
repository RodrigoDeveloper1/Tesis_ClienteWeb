<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EstrategiasPedagogicasModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Estrategias Pedagógicas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Combobox de cursos  y materias -->
    <div class="row">
        <!-- Lista de cursos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de lapsos -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idLapso) %>
            <%: Html.DropDownListFor(m => m.idLapso, Model.selectListLapsos, "Seleccione el lapso...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-lapso" })%>
        </div>

        <!-- Lista de materias -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.idMateria) %>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-materia" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>
    </div>

    <!-- Separador -->
    <div class="row">
        <div class="form-group col-xs-12"></div>
        <div class="col-xs-12"><div class="separador"></div></div>
    </div>

    <!-- Sección de botones -->
    <div class="row">
        <!-- Botón generar reporte -->
        <div class="col-xs-6 text-right">
            <button class="btn btn-lg btn-primary" id="generar-reporte">
                Generar
            </button>
        </div>

        <!-- Botón Cancelar -->
        <div class="col-xs-6">
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
    <!--link rel="Stylesheet" href="../../Content/Css/Administrador/Rol.css" type="text/css" /-->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/EstrategiasPedagogicas/EstrategiasPedagogicas.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Estrategias Pedagógicas
</asp:Content>