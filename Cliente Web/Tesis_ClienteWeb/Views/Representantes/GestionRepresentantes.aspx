<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.GestionRepresentantesModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de representantes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
            new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
        </div>

        <!-- Lista de cursos -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idCurso) %>
            <%: Html.DropDownListFor(m => m.idCurso, Model.selectListCursos, "Seleccione el curso...", 
            new { @class = "form-control selectpicker class-cursos",  @id = "select-curso" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Tabla de alumnos y representantes -->
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Lista de alumnos y representantes</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de alumnos -->
                    <div class="col-xs-12" id="div-tabla-lista-alumnos">
                        <table class="table" id="table-lista-alumnos">
                            <thead>
                                <tr>
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">  
    <link href="../../Content/Css/Representantes/Representantes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Representantes/Representantes.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de alumnos
</asp:Content>
