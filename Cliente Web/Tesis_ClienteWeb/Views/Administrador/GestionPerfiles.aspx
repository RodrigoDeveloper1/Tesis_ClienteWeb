<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de perfiles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Formulario Crear Perfil -->
        <div class="col-xs-5">
            <% using (Html.BeginForm("AgregarPerfil", "Administrador", FormMethod.Post, new
               {
                   @class = "form",
                   @role = "form"
               }))
               { %>
            <%: Html.AntiForgeryToken() %>

            <!-- Nombre del perfil -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.profile.Name) %>

                <%: Html.TextBoxFor(m => m.profile.Name, new { @PlaceHolder = "Nombre del perfil",
                            @class = "form-control", @id = "text-box-perfil"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Controlador del perfil -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.profile.ControllerName) %>

                <%: Html.TextBoxFor(m => m.profile.ControllerName, new { @PlaceHolder = "Nombre del controlador",
                            @class = "form-control", @id = "text-box-controller"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12 separador-formularios"></div>

            <!-- Acción del perfil -->
            <div class="form-group col-xs-12">
                <%: Html.LabelFor(m => m.profile.Action) %>

                <%: Html.TextBoxFor(m => m.profile.Action, new { @PlaceHolder = "Nombre de la acción",
                            @class = "form-control", @id = "text-box-accion"}) %>
            </div>

            <!--Separador -->
            <div class="form-group col-xs-12"></div>

            <!-- Botón: Agregar -->
            <div class="col-xs-12 text-center">
                <button class="btn btn-lg btn-default" id="btn-agregar-perfil" type="submit">
                    Agregar
                </button>
            </div>
            <% } %>
        </div>
                
        <!-- Panel de Lista de Perfiles -->
        <div class="col-xs-7">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de perfiles</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de perfiles -->
                    <div class="col-xs-12" id="div-tabla-lista-perfiles">
                        <table class="table" id="table-lista-perfiles">
                            <thead>
                                <tr>
                                    <th>Nombre</th>
                                    <th>Controlador</th>
                                    <th>Acción/Método</th>
                                    <th>Editar</th>
                                    <th>Eliminar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (var perfil in Model.ProfileList) %>
                                <% { %>
                                <tr>
                                    <td class="td-name"><%: perfil.Name %></td>
                                    <td class="td-controller"><%: perfil.ControllerName %></td>
                                    <td class="td-action"><%: perfil.Action %></td>
                                    <td class="td-editar">
                                        <a class="fa fa-pencil" 
                                            href="EditarPerfil/<%: perfil.ProfileId %>">
                                        </a>
                                    </td>
                                    <td class="td-eliminar">
                                        <a class="btn fa fa-minus-circle eliminarperfil" 
                                            data-id="<%: perfil.ProfileId %>">
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
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Perfil.css" type="text/css" />    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Perfil.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de perfiles
</asp:Content>