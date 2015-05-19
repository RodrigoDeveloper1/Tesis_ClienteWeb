<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.UserListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Listado de usuarios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Panel de Lista de Usuarios -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de usuarios</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de usuarios -->
                    <div class="col-xs-12" id="div-tabla-lista-usuarios-grande">
                        <table class="table" id="table-lista-usuarios">
                            <thead>
                                <tr>
                                    <th>Nombre usuario</th>
                                    <th>Correo electrónico</th>
                                    <th>Correo confirmado</th>
                                    <th>Estatus</th>
                                    <th>Rol</th>
                                    <th class="centrar">Editar</th>
                                    <th class="centrar">Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.listaUsuariosPersonales.Count(); i++)
                                   {
                                       string Confirmacion = (Model.listaUsuariosPersonales[i].usuario
                                           .EmailConfirmed ? "Autenticado" : "En espera");

                                       string Estatus = (Model.listaUsuariosPersonales[i].usuario.LockoutEnabled
                                            ? "Bloqueado" : "Activo");
                                %>
                                <tr>
                                    <td class="td-username">
                                        <%: Model.listaUsuariosPersonales[i].usuario.UserName %>
                                    </td>
                                    <td class="td-email">
                                        <%: Model.listaUsuariosPersonales[i].usuario.Email %>
                                    </td>
                                    <td class="td-email-confirm"><%: Confirmacion %> </td>
                                    <td class="td-status"><%: Estatus %> </td>
                                    <td class="td-role">
                                        <%: Model.listaUsuariosPersonales[i].rol.Name %>
                                    </td>
                                    <td class="td-editar acciones centrar">
                                        <a class="fa fa-pencil"
                                            href="EditarUsuario/<%: Model.listaUsuariosPersonales[i].usuario.Id %>"></a>
                                    </td>
                                    <td class="td-eliminar acciones centrar">
                                        <a class="btn fa fa-minus-circle eliminarusuario" 
                                            data-id="<%: Model.listaUsuariosPersonales[i].usuario.Id %>">
                                        </a>                                        
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
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Usuario.css" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Usuario.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/ListarUsuarios.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Lista de usuarios
</asp:Content>