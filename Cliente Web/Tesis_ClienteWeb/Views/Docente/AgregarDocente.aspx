<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.AgregarDocenteModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar docente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                    <div class="label label-info tip-mensaje" id="tip-lista-cursos-2">
                        Los cursos cargados en esta lista son aquellos que pertenecen a períodos escolares 
                        activos, a lo que corresponderán a un año escolar en curso. 
                    </div> 
                </div>
            </div>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!-- Lista de materias -->
        <div class="col-xs-6">
            <div class="form-inline">
                <%: Html.LabelFor(m => m.idMateria, new { @class="form-group cabecera-tips", @id="label-materias"})%> 
                <div class="form-group tip-informacion">
                    *
                    <div class="label label-info tip-mensaje" id="tip-lista-materias-1">
                        Se enlistan aquellas materias que no tienen docentes asignados. 
                    </div> 
                </div>
            </div>
            <%: Html.DropDownListFor(m => m.idMateria, Model.selectListMaterias, "Seleccione la materia...", 
            new { @class = "form-control selectpicker",  @id = "select-materia" })%>
        </div>

        <!--Separador con márgen -->
        <div class="form-group col-xs-12"></div>
	    <div class="col-xs-12"><div class="separador"></div></div>

        <div class="col-xs-12">
            <h4>Sección del docente: </h4>
        </div>

        <!-- Lista de docentes -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idDocente, new { @class="form-group", @id="label-docentes"})%> 
            <%: Html.DropDownListFor(m => m.idDocente, Model.selectListDocentes, "Seleccione el docente...", 
            new { @class = "form-control selectpicker",  @id = "select-docente" })%>
        </div>

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>

        <!-- Botón Asociar -->
        <div class="col-xs-6 text-right">
            <button type="button" class="btn btn-lg btn-default" id="btn-agregar-docente">Asociar</button>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Docente/AgregarDocente.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Docente/AgregarDocente.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Agregar docente
</asp:Content>
