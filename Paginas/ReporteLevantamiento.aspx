<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteLevantamiento.aspx.cs" Inherits="ReporteLevantamiento" Title="ReporteLevantamiento" %>
<asp:Content ID="headCatalogoLevantamiento" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>

    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/ReporteLevantamiento.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoLevantamiento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server"> 
    <div id="divContenido">
        <table>
            <tr>
                <td>
                    <div id="divFiltrosReporteLevantamiento" idAgente="" idCliente="" idAsignado="">
                        <table>
                            <tr>
				                <td>Fecha Inicial:</td>
				                <td><input type="text" id="txtFechaInicial" style="width:120px;" value="<%=DateTime.Today.AddDays(-DateTime.Today.Day+1).ToShortDateString() %>"</td>
			                </tr>
			                <tr>
				                <td>Fecha Final:</td>
				                <td><input type="text" id="txtFechaFinal" style="width:120px;" value="<%=DateTime.Today.ToShortDateString() %>"</td>
			                </tr>
                            <tr>
				                <td>Sucursal:</td>
							<td>
								<select id="cmbSucursal" style="width:100%;">
									<option value="-1">-Todas-</option>
								</select>
							</td>
			                </tr>
			                <tr>
				                <td>Vendedor:</td>
				                <td>
					                <input type="text" id="txtAgente" style="width:180px;"/>
				                </td>
			                </tr>
                            <tr>
                                <td>Familia</td>
							    <td>
								    <select id="cmbDivision" style="width:100%;">
									    <option value="-1">-Todas-</option>
								    </select>
							    </td>
                            </tr>
                            <tr>
                                <td>Estatus:</td>
							    <td>
								    <select id="cmbEstatus" style="width:100%;">
									    <option value="-1">-Todas-</option>
								    </select>
							    </td>
                            </tr>
                            <tr>
                                <td>Cliente:</td>
				                <td>
					                <input type="text" id="txtCliente" style="width:180px;"/>
				                </td>
                            </tr>
                            <tr>
                                <td>No. Oportunidad:</td>
				                <td>
					                <input type="text" id="txtIdOportunidad" style="width:180px;"/>
				                </td>
                            </tr>
                            <tr>
                                <td>Asignado:</td>
				                <td>
					                <input type="text" id="txtAsigando" style="width:180px;"/>
				                </td>
                            </tr>
                            <tr>
							    <td></td>
							    <td>
								    <button id="btnActualizarReporteLevantamiento" class="buttonLTR">Buscar</button>
							    </td>
						    </tr>
                        </table>
                    </div>
                </td>
                <td>
                    <div id="divKpiReporteLevantamiento">
                        Resumen Ejecutivo
                    </div>
                </td>
            </tr>
        </table>
        <div id="divGridReporteLevantamiento" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdReporteLevantamiento"></table>
                <div id="pagReporteLevantamiento"></div>                
            </div>
        </div>
    </div>
</asp:Content>