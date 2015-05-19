<%@ Page Title="IndexInicio" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
Inherits="System.Web.Mvc.ViewPage<Tesis_ClienteWeb.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Inicio
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Fila de paneles -->
    <div class="row">
        <!-- Panel de Notificaciones -->
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-envelope fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge"></div>
                            <div><a class="panel-title" href="../Notificaciones/Buzon">Notificaciones Emitidas</a></div>
                        </div>
                    </div>
                </div>

                <a href="../Notificaciones/Buzon">
                    <div class="panel-footer">
                        <span class="pull-left">Ver detalles</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

        <!-- Panel de Calendario -->
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-calendar fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge"></div>
                            <div><a class="panel-title" href="../Eventos/CalendarioEventos">Calendario</a></div>
                        </div>
                    </div>
                </div>
                <a href="../Eventos/CalendarioEventos">
                    <div class="panel-footer">
                        <span class="pull-left">Ver detalle</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

        <!-- Panel de Evaluaciones -->
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-pencil fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge"></div>
                            <div><a class="panel-title" href="../Evaluaciones/Evaluaciones">Evaluaciones</a></div>
                        </div>
                    </div>
                </div>
                <a href="../Evaluaciones/Evaluaciones">
                    <div class="panel-footer">
                        <span class="pull-left">Ver detalles</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

        <!-- Panel de información de estudiantes -->
        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-group fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <div class="huge"></div>
                            <div><a class="panel-title" href="../Alumnos/Alumnos">Alumnos</a></div>
                        </div>
                    </div>
                </div>
                <a href="../Alumnos/Alumnos">
                    <div class="panel-footer">
                        <span class="pull-left">Ver detalles</span>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

        <!-- Separador con línea -->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Fila de Gráficos -->
    <div class="row">
        <!-- Título de estadísticas -->
        <div class="col-xs-12">
            <h4 class="subtitulos">Estadísticas</h4>
        </div>

        <!-- Gráfico de torta -->
        <div class="col-xs-6">
            <div class="panel panel-green" id="panel-rankingalumnos-inicio">
                <div class="panel-heading">
                    <i class="fa fa-pie-chart fa-fw"></i>
                    <strong>% alumnos aprobados vs reprobados.</strong>

                    <div class="pull-right col-xs-1">
                        <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-inicio-torta"></a>
                    </div>

                    <div class="separador-dialogos"></div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="morris-bar-AprovvsReprovInicio-div">
                            <div id="morris-bar-AprovvsReprovInicio" class="rankingalumnos-inicio"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Gráfico de barras -->
        <div class="col-xs-6">
            <div class="panel panel-yellow" id="panel-aprovsrepro-inicio">
                <div class="panel-heading">
                    <i class="fa fa-bar-chart-o fa-fw"></i>
                    <strong>Top 10 resultados destacados.</strong>

                    <div class="pull-right col-xs-1">
                       <a href="#" class="fa fa-file-pdf-o fa-2x" id="reporte-inicio-barras"></a>
                    </div>

                    <div class="separador-dialogos"></div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        <div class="col-xs-12" id="morris-bar-rankingalumnos-div">
                            <div id="morris-bar-rankingalumnos"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Fila de Próximos eventos -->
    <div class="row">
        <!-- Panel de próximos eventos -->
        <div class="col-xs-12">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <i class="fa fa-calendar fa-fw"></i>
                    Pr&oacute;ximos eventos de la semana
                    <div class="separador-dialogos"></div>
                </div>

                <!-- Tabla de próximos eventos -->
                <div class="panel-body">
                    <table class="table table-striped" id="table-eventos-semana">
                        <thead>
                            <tr>
                                <th class="th-dia-index">Día</th>
                                <th class="th-fecha-index">Fecha</th>
                                <th class="th-hora-index">Hora</th>
                                <th class="th-tipos-eventos-index">Tipo de evento</th>
                                <th class="th-nombre-evaluacion-index">Nombre</th>
                            </tr>
                        </thead>

                        <tbody>
                            <% foreach (var evento in Model.ListaEventos)
                               { %>
                            <tr <%: evento.EventId %>>
                                <td class="td-dia-index">
                                    <%: evento.StartDate
                                    .ToString("dddd",new System.Globalization.CultureInfo("es-ES")) %>
                                </td>
                                <td class="td-fecha-index"><%: evento.StartDate.ToString("dd-MM-yyyy") %></td>
                                <td class="td-hora-index"><%: evento.StartHour%></td>
                                <td class="td-tipos-eventos-index"><%: evento.EventType%></td>
                                <td class="td-nombre-evaluacion-index"><%: evento.Name %></td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Separador-->
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>

    <!-- Separador con línea -->
    <div class="row">
        <div class="col-lg-12">
            <div class="separador"></div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
    <link href="../../Content/Css/Index/Inicio.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
    <script src="../../Scripts/Views/Maestra/morris.min.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Maestra/morris-data.js" type="text/javascript"></script>
    <script src="../../Scripts/Views/Index/Index.js" type="text/javascript"></script>    
    <script src="../../Scripts/Views/Index/Reportes.js" type="text/javascript"></script>    
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
    Inicio
</asp:Content>