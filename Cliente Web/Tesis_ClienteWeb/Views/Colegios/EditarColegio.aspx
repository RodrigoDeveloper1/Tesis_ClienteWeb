<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EditarColegioModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar colegio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Formulario Crear colegio -->
    <div class="row">
        <!-- Datos del nuevo colegio-->
        <div class="col-xs-5">
            <% using (Html.BeginForm("EditarColegio", "Colegios", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <%: Html.HiddenFor(m => m.colegio.SchoolId) %>

            <!-- Nombre del colegio -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.colegio.Name) %>

                <%: Html.TextBoxFor(m => m.colegio.Name, new { @class = "form-control" }) %>
            </div>
        </div>

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
        <div class="row"><div class="col-xs-12"><div class="separador"></div></div></div>
        <div class="form-group col-xs-12"></div>
    </div>

    <!-- Botones -->
    <div class="row">
        <!-- Botón: Editar -->
        <div class="col-xs-5 text-right">
            <button class="btn btn-lg btn-default" type="submit" id="btn-agregar">
                Editar
            </button>
        </div>
        <% } %>

        <!-- Botón Cancelar -->
        <div class="col-xs-7 text-left">
            <% using (Html.BeginForm("ListarColegios", "Administrador", FormMethod.Get, new
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
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/ManejadorEstatus.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/EditarColegio.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Editar colegio
</asp:Content>