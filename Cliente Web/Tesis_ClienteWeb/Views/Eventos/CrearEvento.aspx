<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.EventosModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Crear evento
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Datos del nuevo Evento-->   

    <div class="row">
     <!-- Lista de colegios -->
        <div class="col-xs-8">
            <%: Html.LabelFor(m => m.idColegio) %>
            <%: Html.DropDownListFor(m => m.idColegio, Model.selectListColegios, "Seleccione el colegio...", 
                new { @class = "form-control selectpicker class-cursos",  @id = "select-colegio-crear" })%>
        </div>  
        <!-- Año escolar -->
        <div class="col-xs-4">
            <%: Html.LabelFor(m => m.labelAnoEscolar) %>
            <%: Html.TextBoxFor(m => m.labelAnoEscolar, new { @class="form-control", @id="ano-escolar", 
                @disabled = "disabled"})%>
            <% Html.HiddenFor(m => m.idAnoEscolar); %>
        </div>
    </div>         
    <!--Separador -->
    <div class="form-group col-xs-12"></div>   
  
    <!---------------- Fin de Comboboxde colegios ------------>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <div class="row">       
          <!-- Panel de lista de eventos -->
        <div class="col-xs-12">
            <div class="panel panel-primary">
                <div class="panel-heading"><strong>Lista de eventos a agregar:</strong></div>
                <div class="panel-body">

                    <!-- Tabla de evaluaciones editable -->
                    <div class="col-xs-12" id="div-table-lista-eventos-cargar">
                        <table class="table table-striped">
                            <thead>
                                <tr>
                                    <th class="th-nombre centrar">Nombre</th>
                                    <th class="th-descripcion centrar">Descripción</th>
                                    <th class="th-inicio centrar">Inicio</th>
                                    <th class="th-fin centrar">Fin</th>                                   
                                    <th class="th-tipo centrar">Tipo</th>                                                         
                                    <th class="th-agregar-eventos">
                                        <i class="ui-icon ui-icon-check iconos-comentados" 
                                            id="i-salvar-todas-filas">
                                        </i>
                                    </th>
                                    <th class="th-eliminar-eventos">
                                        <i class="fa fa-minus-circle iconos-comentados" 
                                            id="i-eliminar-todas-filas">
                                        </i>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="td-nro-eventos">
                                        <i class="ui-icon ui-icon-plusthick" id="i-add-fila-eventos"></i>
                                    </td>
                                    <td class="td-nombre"></td>
                                    <td class="td-descripcion"></td>
                                    <td class="td-inicio"></td>
                                    <td class="td-fin"></td>
                                    <td class="td-tipo"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-xs-12">
          <a class="" href="../Eventos/CrearEventoAvanzado">
               <span class="">Crear Evento Avanzado</span>
          </a>
        </div>
    </div>

    <!--Separador -->
    <div class="form-group col-xs-12"></div>

    <!--Separador -->
    <div class="col-lg-12">
        <div class="separador"></div>
    </div>

    <!-- Botones -->
      
    <div class="row">
         <!-- Botón Agregar -->
        <div class="col-xs-6 text-right">
            <button type="button" class="btn btn-lg btn-default" id="btn-agregar-eventos">Agregar</button>
        </div>
       

        <!-- Botón Cancelar -->
        <div class="col-xs-6 text-left">
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
        <!--Separador -->
        <div class="form-group col-xs-12"></div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link rel="Stylesheet" href="../../Content/Css/Eventos/CrearEvento.css" type="text/css" />  
    <link rel="Stylesheet" href="../../Content/Css/Eventos/Eventos.css" type="text/css" />        
<link href="../../Content/Css/Eventos/bootstrap-timepicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Eventos/CrearEventos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Eventos/Eventos.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Colegio/InicializarDatePickers.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Eventos/bootstrap-timepicker.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Maestras - Crear eventos 
</asp:Content>