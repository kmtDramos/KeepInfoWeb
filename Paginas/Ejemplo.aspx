<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Ejemplo.aspx.cs" Inherits="Ejemplo" %>
<asp:Content ID="headEjemplo" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--jQuery-->
    <script type="text/javascript" src="../js/Desarrollo.Ejemplo.js"></script>
</asp:Content>
<asp:Content ID="bodyEjemplo" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <!--<input type="button" id="btnObtenerResultadosOracle" value="Obtener resultados de oracle" class="buttonLTR" />-->
            <input type="button" id="btnCortaCadenas" value="Cortar cadena" class="buttonLTR" />
        </div>
    </div>
</asp:Content>