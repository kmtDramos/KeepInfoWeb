<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Opciones.aspx.cs" Inherits="Opciones" Title="Seguridad"%>
<asp:Content ID="headSeguridadOpciones" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--DHX-->
    <link rel="stylesheet" type="text/css" href="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxtree.css" />
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxcommon.js"></script>
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxtree.js"></script>
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/ext/dhtmlxtree_json.js"></script> 
    <!--DHX-->
    <script type="text/javascript"  src="../js/Seguridad.Opciones.js"></script>
</asp:Content>
<asp:Content ID="bodySeguridadOpciones" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">
        <div id="col-1" class="col-1">
            <div id="treeboxbox_tree" class="etiquetaOculta"></div>
        </div>
        <div id="col-2" class="col-2"></div>
    </div>
</asp:Content>