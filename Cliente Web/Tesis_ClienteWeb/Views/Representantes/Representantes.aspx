<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.RepresentantesModel>"%>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Representantes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



 <!-------------------------------- Fila de Tabla de Representantes -------------------------------------->
    <div class="row">
        <div class="col-lg-12">
            <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a href="#">Lista de Representantes</a></li>
            </ul>

            <!---------------------------------- Tabla de Representantes ------------------------------------>
            <table class="table table-striped">
                <thead>
                    <tr>
                        <th class="th-nombre">Nombre del Representante</th>
                        <th class="th-opcion">Editar</th>
                        <th class="th-opcion">Eliminar</th>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (var representante in Model.listaRepresentantes)
                           { %>
                        <tr <%: representante.RepresentativeId %>>
                            <td class="td-nombre"><%: representante.Name %></td>
                            <td class="td-opcion">
                                <a class="btn fa fa-pencil" 
                                             href="/Representantes/ModificarRepresentantes/<%: representante.RepresentativeId %>">
                              </a>
                            </td>
                            <td class="td-opcion">
                               <a class="btn fa fa-minus-circle a-eliminar-rol eliminarrepresentante" 
                                            id="" data-id=<%: representante.RepresentativeId %>>
                            </a>
                            
                            </td>
                        </tr>
                        <% } %>
                </tbody>
            </table>
            <!------------------------------- Fin de Tabla de Representantes -------------------------------->
        </div>

        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
    <!------------------------------ Fin de Fila de Tabla de Representantes --------------------------------->
     <!-------------------------------------- Fila de Nuevo Representante ------------------------------------->
    <div class="row">
        <div class="col-lg-12">
            <h4 class="subtitulos">Nuevo Representante</h4>
        </div>

        <div class="col-lg-12">
            <ul class="list-inline">
                <li>
                    <h5>Cargar un nuevo representante:</h5>
                </li>
                <li>
                    <button type="button" class="btn btn-primary" id="btn-nueva-representante">
                        Nuevo Representante
                    </button>
                </li>
            </ul>            
        </div>
    </div>
    <!------------------------------------ Fin de Fila de Nuevo Representante -------------------------------->
   

    <!------------------------------------- Diálogo Nuevo Representante -------------------------------------->
        <div id="dialog-nueva-representante">
	    <div class="row div-interior-dialog">
            
             <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
             
            
            <div class="col-xs-8">
                <label id="" class="pull-left" for="input-nombre-auto">Nombre Representante:</label>
                <input type="text" class="form-control pull-right" id="nombrenuevorepresentante" />
            </div>
	    </div>
        </div>
        
    <!----------------------------------- Fin de Diálogo Nuevo Representante --------------------------------->

    

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">  
<link href="../../Content/Css/Representantes/Representantes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
<script src="../../Scripts/Views/Representantes/Representantes.js" type="text/javascript" language="javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Representantes
</asp:Content>
