<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.MateriasModel>"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Modificar Materia
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.AntiForgeryToken() %>

    <!------------------ Combobox de colegios ---------------->
    <div class="row">
        <!-- Lista de colegios -->
        <div class="col-xs-6">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-modif" })%>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>
    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Formulario Modificar Materia -->
    <div class="row">
        <div class="col-xs-6">
            <!-- Título: Modificar materia -->
            <div class="col-xs-12">
                <h4>Modificar Materia: </h4>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Nombre -->
            <div class="form-group col-xs-12" id="nombre-materia-modif-div">
                <label for="nombre-materia-modif">Nombre de la materia:</label>
                <input class="form-control" id="nombre-materia-modif" placeholder="Nombre de la materia">
            </div>

            <!-- Código -->
            <div class="form-group col-xs-12" id="codigo-materia-modif-div">
                <label for="codigo-materia-modif">Código de la materia:</label>
                <input class="form-control" id="codigo-materia-modif" placeholder="Código de la materia">
            </div>

            <!-- Pensum -->
            <div class="form-group col-xs-12" id="pensum-materia-modif-div">
                <label for="pensum-materia-modif">Pensum de la materia:</label>
                <textarea rows="4" cols="50" class="form-control" id="pensum-materia-modif"
                    placeholder="Pensum de la materia">
                </textarea>
            </div>

            <!-- Botón: Modificar -->
            <div class="col-xs-12 text-center">
                <button class="btn btn-lg btn-default" id="btn-modificar-materia">
                    Modificar
                </button>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>
        </div>

        <!-- Panel de Lista de Materias -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de materias</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de materias -->
                    <div class="col-xs-12" id="div-tabla-lista-materias">
                        <table class="table" id="table-lista-materias-modif">
                            <thead>
                                <tr>
                                    <th class="centrar">Nombre</th>
                                    <th class="centrar">Código</th>
                                    <th class="centrar">Pensum</th>
                                </tr>
                            </thead>
                            <tbody>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Materias/CrearMateria.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Materias/ModificarMateria.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Materias/ModificarMaterias.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Modificar Materia
</asp:Content>
