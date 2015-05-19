<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Maestra.Master" 
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Contacto
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

               <div class="row">
               <div class="col-xs-8">
				<div class="panel panel-primary ">
					<div class="panel-heading">
						<h2 class="panel-title">Envíanos un correo...</h2>
					</div>
					<div class="panel-body">
						<form name="contactform" method="post" action=""  class="form-horizontal" role="form">
							<div class="form-group">
								<label for="inputName" class="col-xs-2 control-label">Nombre:</label>
								<div class="col-lg-10">
									<input type="text" class="form-control" id="inputName" name="inputName" placeholder="Su Nombre">
								</div>
							</div>
							<div class="form-group">
								<label for="inputEmail1" class="col-xs-2 control-label">Correo:</label>
								<div class="col-lg-10">
									<input type="text" class="form-control" id="inputEmail" name="inputEmail" placeholder="Su Correo">
								</div>
							</div>
							<div class="form-group">
								<label for="inputSubject" class="col-xs-2 control-label">Sujeto:</label>
								<div class="col-lg-10">
									<input type="text" class="form-control" id="inputSubject" name="inputSubject" placeholder="Sujeto Del Mensaje">
								</div>
							</div>
							<div class="form-group">
								<label for="inputPassword1" class="col-xs-2 control-label align-">Mensaje:</label>
								<div class="col-lg-10">
									<textarea class="form-control" rows="4" id="inputMessage" name="inputMessage" placeholder="Su Mensaje ..."></textarea>
								</div>
							</div>
							<div class="form-group">
								<div class="col-xs-offset-2 col-xs-10">
									<button type="submit" class="btn btn-default">
										Enviar Mensaje
									</button>
								</div>
							</div>
						</form>

					</div>
				</div>
                </div>
                <div class="col-xs-4">
					
					<div class="panel panel-primary ">
					<div class="panel-heading">
						<h2 class="panel-title">Contáctenos por:</h2>
					</div>
                    <div class="panel-body">
                                <ul class="social">
                                    <li><a href="#"><i class="fa fa-facebook icon-social"></i> Facebook</a></li>
                                    <li><a href="#"><i class="fa fa-google-plus icon-social"></i> Google Plus</a></li>
                                    <li><a href="#"><i class="fa fa-pinterest icon-social"></i> Pinterest</a></li>
									<li><a href="#"><i class="fa fa-linkedin icon-social"></i> Linkedin</a></li>
                                    <li><a href="#"><i class="fa fa-twitter icon-social"></i> Twitter</a></li>
                                    <li><a href="#"><i class="fa fa-youtube icon-social"></i> Youtube</a></li>
                                </ul>
                     

                            </div>
                           
                          </div>  
                          </div>  
			</div>
            
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CSSContent" runat="server">
<link href="../../Content/Css/Contacto/Contacto.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="JSContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="TituloPagina" runat="server">
Contáctanos
</asp:Content>
