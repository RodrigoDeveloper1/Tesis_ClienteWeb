<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar perfil
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Editar Perfil -->
        <div class="col-xs-6">
            <% using (Html.BeginForm("EditarPerfil", "Administrador", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            
            <%: Html.AntiForgeryToken() %>
            <%: Html.HiddenFor(m => m.profile.ProfileId)%>

            <!-- Nombre del perfil -->
            <div class="form-group col-xs-10">
                <%: Html.LabelFor(m => m.profile.Name) %>

                <%: Html.TextBoxFor(m => m.profile.Name, new { @PlaceHolder = "Nombre del perfil",
                            @class = "form-control", @id = "text-box-perfil", @disabled = "disabled"}) %>

                <%: Html.HiddenFor(m => m.profile.Name) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Controlador del perfil -->
            <div class="form-group col-xs-10">
                <%: Html.LabelFor(m => m.profile.ControllerName) %>

                <%: Html.TextBoxFor(m => m.profile.ControllerName, new { @PlaceHolder = "Nombre del controlador",
                            @class = "form-control", @id = "text-box-controller"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Acción del perfil -->
            <div class="form-group col-xs-10">
                <%: Html.LabelFor(m => m.profile.Action) %>

                <%: Html.TextBoxFor(m => m.profile.Action, new { @PlaceHolder = "Nombre de la acción",
                            @class = "form-control", @id = "text-box-accion"}) %>
            </div>
        </div>
                
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

        <!-- Botones -->
        <div class="col-xs-5">
            <!-- Botón: Editar -->
            <div class="col-xs-6 text-right">
                <button class="btn btn-lg btn-default" id="btn-editar-perfil" type="submit">
                    Editar
                </button>
            </div>
            <% } %>

            <!-- Botón: Cancelar -->
            <% using (Html.BeginForm("GestionPerfiles", "Administrador", FormMethod.Get, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <div class="col-xs-6 text-left">
                <button class="btn btn-lg btn-default" type="submit">
                    Cancelar
                </button>
            </div>
            <% } %>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Administrador.css" type="text/css" />    
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Perfil.css" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/Perfil.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Editar perfil
</asp:Content>