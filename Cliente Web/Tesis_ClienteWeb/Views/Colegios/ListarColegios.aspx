<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.ListarColegiosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gestión de colegios
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <!-- Panel de Lista de Colegios -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <strong>Lista de colegios</strong>
                </div>

                <div class="panel-body">
                    <!-- Tabla de colegios -->
                    <div class="col-xs-12" id="div-tabla-lista-colegios">
                        <table class="table" id="table-lista-colegios">
                            <thead>
                                <tr>
                                    <th>Nombre del colegio</th>
                                    <th>Fecha de creación</th>
                                    <th class="centrar">Estatus</th>
                                    <th class="centrar">Editar</th>
                                    <th class="centrar">Suspender</th>
                                    <th class="centrar">Habilitar</th>
                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (var colegio in Model.listaColegios) %>
                                <% { %> 
                                       <% string icono = (colegio.Status == true ? "fa fa-check status-ok" : 
                                              "fa fa-ban status-wrong"); %>

                                <tr>
                                    <td class="td-nombre"><%: colegio.Name %></td>
                                    <td class="td-fecha"><%: colegio.DateOfCreation %></td>
                                    <td class="td-estatus centrar"><i class="<%: icono %>"></i></td>
                                    <td class="td-editar centrar">
                                        <a class="fa fa-pencil" 
                                            href="EditarColegio/<%: colegio.SchoolId %>">
                                        </a>
                                    </td>
                                    <td class="td-eliminar centrar">
                                        <% if(colegio.Status) { %>
                                        <a class="btn fa fa-ban suspender-colegio" 
                                            data-id="<%: colegio.SchoolId %>">
                                        </a>
                                        <% } %>
                                        <% else { %>
                                        <i class="fa fa-ban"></i>
                                        <% } %>
                                    </td>
                                    <td class="td-habilitar centrar">
                                        <% if(!colegio.Status) { %>
                                        <a class="btn fa fa-check habilitar-colegio" 
                                            data-id="<%: colegio.SchoolId %>">
                                        </a>
                                        <% } %>
                                        <% else { %>
                                        <i class="fa fa-check"></i>
                                        <% } %>
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
    <link href="../../Content/Css/Colegios/Colegios.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Css/Colegios/ListarColegios.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Colegio/Colegio.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/ListarColegios.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Gestión de colegios
</asp:Content>