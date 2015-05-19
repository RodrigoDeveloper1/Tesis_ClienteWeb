<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EditUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Editar usuario
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Registrar Usuario -->
        <div class="col-xs-7">
            <% using (Html.BeginForm("EditarUsuario", "Administrador", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <!-- Id del usuario -->
            <%: Html.HiddenFor(m => m.idUsuario) %>

            <!-- Username -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Username) %>

                <%: Html.TextBoxFor(m => m.Username, new { 
                    @class = "form-control",
                    @disabled = "disabled",
                    @id = "text-box-usuario"}) %>

                <%: Html.HiddenFor(m => m.Username) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Antiguo correo electrónico -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.OldEmail) %>

                <%: Html.TextBoxFor(m => m.OldEmail, new { 
                    @class = "form-control", 
                    @id = "text-box-correo-viejo", 
                    @disabled = "disabled",
                    @type = "email" }) %>

                <%: Html.HiddenFor(m => m.OldEmail) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Nuevo correo electrónico -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Email) %>

                <%: Html.TextBoxFor(m => m.Email, new { @PlaceHolder = "Correo electrónico",
                @class = "form-control", @id = "text-box-correo", @type = "email" }) %>
            </div>

            <!-- Confirmar correo electrónico -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.ConfirmEmail) %>

                <%: Html.TextBoxFor(m => m.ConfirmEmail, new { @PlaceHolder = "Confirmar correo electrónico",
                @class = "form-control", @id = "text-box-correo-confirmar", @type = "email" }) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Antigua contraseña -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.OldPassword) %>

                <%: Html.PasswordFor(m => m.OldPassword, new { @PlaceHolder = "Antigua contraseña",
                @class = "form-control", @id = "text-box-password"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Nueva contraseña -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Password) %>

                <%: Html.PasswordFor(m => m.Password, new { @PlaceHolder = "Nueva contraseña",
                @class = "form-control", @id = "text-box-password"}) %>
            </div>

            <!-- Confirmar contraseña -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.ConfirmPassword) %>

                <%: Html.PasswordFor(m => m.ConfirmPassword, new { @PlaceHolder = "Confirmar contraseña",
                @class = "form-control", @id = "text-box-conf-password"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Lista de roles -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.rolId) %>
                <%: Html.DropDownListFor(m => m.rolId, Model.listaRoles, "Seleccionar Rol", new { 
                @class = "form-control selectpicker", @id = "select-rol" })%>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Botón: Editar -->
            <div class="col-xs-6 text-right">
                <button class="btn btn-lg btn-default" type="submit" id="btn-editar-usuario">
                    Editar
                </button>
            </div>

            <% } %>

            <!-- Botón Cancelar -->
            <div class="col-xs-6 text-left">
                <% using (Html.BeginForm("ListarUsuarios", "Administrador", FormMethod.Get, new
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Usuario.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/EditarUsuario.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Editar usuario
</asp:Content>