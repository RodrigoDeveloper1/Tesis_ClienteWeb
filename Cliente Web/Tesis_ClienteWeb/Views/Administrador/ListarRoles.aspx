<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.RoleListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Listado de roles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Panel de Lista de Roles -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de roles</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de roles -->
                    <div id="div-tabla-lista-roles">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>Rol</th>
                                    <th>Descripción</th>
                                    <th class="centrar"># Perfiles</th>
                                    <th class="centrar"># Usuarios</th>
                                    <th class="th-accion centrar">Editar</th>
                                    <th class="th-accion centrar">Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (var rol in Model.roleList) %>
                                <% { %>
                                <tr>
                                    <td class="td-name"><%: rol.Name %></td>
                                    <td class="td-descripcion"><%: rol.Description %></td>
                                    <td class="td-perfiles centrar"><%: rol.Profiles.Count  %></td>
                                    <td class="td-usuarios centrar"><%: rol.Users.Count %></td>
                                    <td class="td-editar acciones centrar">
                                        <a class="fa fa-pencil" 
                                            href="EditarRol/<%: rol.Id %>">
                                        </a>
                                    </td>
                                    <td class="td-eliminar acciones centrar">
                                        <a class="btn fa fa-minus-circle eliminarrol" 
                                            data-id="<%: rol.Id %>">
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
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Rol.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Rol.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/ListarRoles.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Listado de roles
</asp:Content>