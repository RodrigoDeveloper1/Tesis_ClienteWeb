<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MateriasModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de materias
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!------------------ Combobox de colegios ---------------->
    <% using (Html.BeginForm("CrearMateria", "Materias", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form",
                   @id = "btn-crear-materias"
               }))
       { %>
    <%: Html.AntiForgeryToken() %>

    <!-- Lista de colegios -->
    <div class="row">
        <div class="col-xs-7">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-crear" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>
    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Datos de la materia -->
    <div class="row">
        <!-- Formulario Crear Materia -->
        <div class="col-xs-5">
            <!-- Título: Crear Materia -->
            <div class="col-xs-12">
                <h4>Crear Materia: </h4>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Nombre de la materia -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.Name) %>
                <%: Html.TextBoxFor(m => m.Name, new { @PlaceHolder = "Nombre de la materia",
                            @class = "form-control", @id = "nombre-materia-crear"}) %>
            </div>

            <!-- Grado -->
            <div class="form-group col-xs-3">
                <%: Html.LabelFor(m => m.Grade) %>
                <%: Html.TextBoxFor(m => m.Grade, new { @class = "form-control", 
                    @id = "grado", @onkeypress="return isNumberKey(event)", @maxlength="2"}) %>
            </div>

            <!-- Código de la materia -->
            <div class="form-group col-xs-9">
                <%: Html.LabelFor(m => m.SubjectCode) %>
                <%: Html.TextBoxFor(m => m.SubjectCode,new  { @PlaceHolder = "Código de la materia",
                            @class = "form-control", @id = "codigo-materia-crear"}) %>
            </div>

            <!-- Pensum de la materia -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.Pensum) %>

                <%: Html.TextAreaFor(m => m.Pensum, new { @PlaceHolder = "Pensum de la materia", 
                    @class = "form-control", @rows = "3", @style = "resize: none", @id = "pensum-materia-crear" })%>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Botón: Agregar -->
            <div class="col-xs-12 text-center">
                <button class="btn btn-lg btn-default" type="submit" id="btn-crear-materia-nueva">
                    Agregar
                </button>
            </div>
            <% } %>
        </div>

        <!-- Panel de Lista de Materias -->
        <div class="col-xs-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de materias</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de materias -->
                    <div class="col-xs-12" id="div-tabla-lista-materias">
                        <table class="table" id="table-lista-materias">
                            <thead>
                                <tr>
                                    <th class="">Nombre</th>
                                    <th class="centrar">Código</th>
                                    <th class="centrar">Grado</th>
                                    <th class="centrar">Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="centrar"></td>
                                    <td class="centrar"></td>
                                    <td class="centrar"></td>
                                    <td class="centrar"></td>
                                </tr>
                            </tbody>
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

    <!-- Separador -->
    <div class="row">
        <div class="col-xs-12">
            <div class="separador"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Materias/CrearMateria.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Materias/CrearMaterias.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de materias
</asp:Content>