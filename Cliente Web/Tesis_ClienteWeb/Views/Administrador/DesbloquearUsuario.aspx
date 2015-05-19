<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.UserListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Desbloquear usuarios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Panel lista de Usuarios Habilitados -->
        <div class="col-xs-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de usuarios habilitados</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de usuarios habilitados -->
                    <div class="col-xs-12" id="div-tabla-lista-usuarios-habilitados">
                        <table class="table" id="table-lista-usuarios-habilitados">
                            <thead>
                                <tr>
                                    <th>Nombre usuario</th>
                                    <th>Correo electrónico</th>                                    
                                    <th class="centrar">Bloquear</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.listaUsuariosHabilitados.Count(); i++)
                                {                                  
                                       %>
                                 <% string idUserHabilitadoAux = Model.listaUsuariosHabilitados[i].Id; %>

                                <tr>
                                    <td class="td-username">
                                        <%: Model.listaUsuariosHabilitados[i].UserName %>
                                    </td>
                                    <td class="td-email">
                                        <%: Model.listaUsuariosHabilitados[i].Email %>
                                    </td>
                                  
                                   
                                    <td class="td-eliminar acciones centrar">
                                       <%: Html.CheckBoxFor(m => m.listaUsuariosHabilitados[i].LockoutEnabled, 
                                        new 
                                        { 
                                            @value = idUserHabilitadoAux,
                                            @id = idUserHabilitadoAux,
                                            @Name = "checkbox_habilitados"
                                        })%>
                                    </td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Botón Bloquear -->
            <div class="col-xs-12 text-center">
                <div class="form" role="form">
                    <button class="btn btn-lg btn-danger" id="btn-bloquear" type="submit">
                        Bloquear
                    </button>
                </div>
            </div>
        </div>
                
        <!-- Panel lista de Usuarios Inhabilitados -->
        <div class="col-xs-6">
            <div class="panel panel-danger">
                <div class="panel-heading">
                    <strong>Lista de usuarios bloqueados</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de usuarios bloqueados -->
                    <div class="col-xs-12" id="div-tabla-lista-usuarios-bloqueados">
                        <table class="table" id="table-lista-usuarios-bloqueados">
                            <thead>
                                <tr>
                                    <th>Nombre usuario</th>
                                    <th>Correo electrónico</th>                                    
                                    <th class="centrar">Desbloquear</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% for (int i = 0; i < Model.listaUsuariosBloqueados.Count(); i++)
                                { %>

                                <% string idUserBloqueadoAux = Model.listaUsuariosBloqueados[i].Id; %>
                                <tr>
                                    <td class="td-username">
                                        <%: Model.listaUsuariosBloqueados[i].UserName %>
                                    </td>
                                    <td class="td-email">
                                        <%: Model.listaUsuariosBloqueados[i].Email %>
                                    </td>
                                  
                                   
                                    <td class="td-eliminar acciones centrar">
                                        <%: Html.CheckBoxFor(m => m.listaUsuariosBloqueados[i].LockoutEnabled, 
                                        new 
                                        { 
                                            @value = idUserBloqueadoAux,
                                            @id = idUserBloqueadoAux,
                                            @Name = "checkbox_deshabilitados"
                                        })%>      
                                    </td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <!-- Botón desbloquear -->
            <div class="col-xs-12 text-center">
                <div class="form" role="form">
                    <button class="btn btn-lg btn-primary" id="btn-desbloquear" type="submit">
                        Desbloquear
                    </button>
                </div>
            </div>
        </div>
                
        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>

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

        <!--Separador Final -->
        <div class="form-group col-xs-12"></div>
            <div class="row">
	            <div class="col-xs-12">
		            <div class="separador"></div>
	            </div>
            </div>
        <div class="form-group col-xs-12"></div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Administrador/Usuario.css" type="text/css" />
    <link rel="Stylesheet" href="../../Content/Css/Administrador/DesbloquearUsuario.css" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Administrador/Usuario.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Administrador/DesbloquearUsuario.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Desbloquear usuarios
</asp:Content>