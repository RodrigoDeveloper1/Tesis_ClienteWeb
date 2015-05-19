<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MateriasModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de materias
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Lista de colegios -->
    <div class="col-xs-8">
        <%: Html.LabelFor(m => m.idColegio) %>
        <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
        new { @class = "form-control selectpicker class-colegios",  @id = "select-colegio" })%>
    </div>
    
    <!-- Formulario Crear Materia -->
    <div class="col-xs-5">
        <% using (Html.BeginForm("AgregarMateria", "Materias", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
           { %>
        <%: Html.AntiForgeryToken() %>

        <!-- Nombre de la materia -->
        <div class="form-group col-xs-12">
            <%: Html.LabelFor(m => m.materia.Name) %>

            <%: Html.TextBoxFor(m => m.materia.Name, new { @PlaceHolder = "Nombre de la materia",
                            @class = "form-control", @id = "text-box-materia"}) %>
        </div>

        <!-- Pensum de la materia -->
        <div class="form-group col-xs-12">
            <%: Html.LabelFor(m => m.materia.Pensum) %>

            <%: Html.TextAreaFor(m => m.materia.Pensum, new { @class = "form-control", @rows = "3", 
                    @style = "resize: none" })%>
        </div>

        <!--Separador -->
        <div class="form-group col-xs-12"></div>
                
        <!-- Panel lista de cursos -->
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Lista de cursos activos</strong>
                </div>

                <div class="panel-body">
                    <div class="col-xs-12" id="div-tabla-lista-cursos">
                        <table class="table" id="table-lista-cursos">
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <!-- Botón: Agregar -->
        <div class="col-xs-12 text-center">
            <button class="btn btn-lg btn-default" type="submit">
                Agregar
            </button>
        </div>
        <% } %>
    </div>



    <!-- Panel de Lista de Materias -->
    <div class="col-xs-7">
        <div class="panel panel-default">
            <div class="panel-heading">
                <strong>Lista de materias</strong>
            </div>

            <div class="panel-body">
                <!-- Tabla de materias -->
                <div class="col-xs-12" id="div-tabla-lista-materias">
                    <table class="table" id="table-lista-materias">
                        <thead>
                            <tr>
                                <th class="centrar">Nombre</th>
                                <th class="centrar"># Cursos</th>
                                <th class="centrar">Editar</th>
                                <th class="centrar">Eliminar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (var materia in Model.listaMaterias) %>
                            <% { %>
                            <tr>
                                <td class="td-name"><%: materia.Name %></td>
                                <td class="td-course centrar"><%: materia.CASUs.Count %></td>
                                <td class="td-editar centrar">
                                    <a class="fa fa-pencil"
                                        href="EditarMateria/<%: materia.SubjectId %>"></a>
                                </td>
                                <td class="td-eliminar centrar">
                                    <a class="fa fa-minus-circle a-eliminar-materia"
                                        id="<%: materia.SubjectId %>"></a>
                                </td>
                            </tr>
                            <% } %>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Materias/CrearMateria.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Materias/Materias.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de materias
</asp:Content>
