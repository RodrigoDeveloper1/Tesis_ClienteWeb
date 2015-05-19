<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.RegisterUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar usuario
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Registrar Usuario -->
        <div class="col-xs-7">
            <% using (Html.BeginForm("AgregarUsuario", "Administrador", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <!-- Username -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Username) %>

                <%: Html.TextBoxFor(m => m.Username, new { @PlaceHolder = "Nombre de usuario",
                @class = "form-control", @id = "text-box-usuario"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Nombre -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.nombre) %>

                <%: Html.TextBoxFor(m => m.nombre, new { @PlaceHolder = "Nombre",
                @class = "form-control", @id = ""}) %>
            </div>

            <!-- Apellido -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.apellido) %>

                <%: Html.TextBoxFor(m => m.apellido, new { @PlaceHolder = "Apellido",
                @class = "form-control", @id = ""}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Correo electrónico -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Email) %>

                <%: Html.TextBoxFor(m => m.Email, new { @PlaceHolder = "Correo electrónico",
                @class = "form-control", @id = "text-box-correo", @type = "email" }) %>
            </div>

            <!-- Confirmar Correo electrónico -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.ConfirmEmail) %>

                <%: Html.TextBoxFor(m => m.ConfirmEmail, new { @PlaceHolder = "Confirmar correo electrónico",
                @class = "form-control", @id = "text-box-correo-confirmar", @type = "email" }) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Contraseña -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.Password) %>

                <%: Html.PasswordFor(m => m.Password, new { @PlaceHolder = "Contraseña",
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
                <%: Html.DropDownListFor(m => m.rolId, Model.listaRoles, "Seleccione el rol", new { 
                @class = "form-control selectpicker", @id = "select-rol" })%>
            </div>

            <!-- Lista de colegios -->
            <div class="form-group col-xs-6">
                <%: Html.LabelFor(m => m.idColegio) %>
                <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio", 
                new { @class = "form-control selectpicker",  @id = "select-colegio" })%>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Botón: Registrar -->
            <div class="col-xs-12 text-center">
                <button class="btn btn-lg btn-default" type="submit" id="btn-registrar-usuario">
                    Registrar
                </button>
            </div>
        </div>

        <!-- Panel de Lista de Usuarios -->
        <div class="col-xs-5">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de usuarios</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de usuarios -->
                    <div class="col-xs-12" id="div-tabla-lista-usuarios">
                        <table class="table" id="table-lista-usuarios">
                            <thead>
                                <tr>
                                    <th>Nombre usuario</th>
                                    <th>Rol</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.listaUsuariosPersonales.Count(); i++)%>
                                <% { %>

                                <tr>
                                    <td class="td-username">
                                        <%: Model.listaUsuariosPersonales[i].usuario.UserName %>
                                    </td>
                                    <td class="td-role">
                                        <%: Model.listaUsuariosPersonales[i].rol.Name %>
                                    </td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <% } %>
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
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Usuario.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Administrador.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Agregar usuario
</asp:Content>