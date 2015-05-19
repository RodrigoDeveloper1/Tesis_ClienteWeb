<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    About
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

            <div class="panel panel-green">
                <div class="panel-heading center">
                    <h2>Conoce a nuestro equipo</h2>
                    <p class="lead">Este es el equipo que hizo posible esta herramienta, espero que la disfruten y le saquen provecho!</p>
                </div>
                <div class="gap"></div>
                <div id="team-scroller" class="carousel scale">
                    <div class="carousel-inner">
                        <div class="item active panel-body">
                            <div class="row">
                                <div class="col-xs-6">
                                    <div class="member">
                                        <p><img class="img-responsive img-thumbnail img-circle" src="../../Content/Images/About/rodrigo.png" alt="" ></p>
                                        <h3>Rodrigo Uzcátegui Urosa<small class="designation">CEO &amp; Founder</small></h3>
                                    </div>
                                </div>
                                <div class="col-xs-6">
                                    <div class="member">
                                        <p><img class="img-responsive img-thumbnail img-circle" src="../../Content/Images/About/fabio.png" alt="" ></p>
                                        <h3>Fabio Puchetti Pallarés<small class="designation">CEO &amp; Founder</small></h3>
                                    </div>
                                </div> 
                              
                            </div>
                        </div>                        
                    </div>
                    <a class="left-arrow" href="#team-scroller" data-slide="prev">
                        <i class="icon-angle-left icon-4x"></i>
                    </a>
                    <a class="right-arrow" href="#team-scroller" data-slide="next">
                        <i class="icon-angle-right icon-4x"></i>
                    </a>
                </div><!--/.carousel-->
            </div>
      

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
<link href="../../Content/Css/About/About.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
</asp:Content>
