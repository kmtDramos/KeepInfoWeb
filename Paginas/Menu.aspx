<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Menu" Title="Men&uacute;" %>
<asp:Content ID="headSeguridadMenu" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--DHX-->
    <link rel="stylesheet" type="text/css" href="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxtree.css" />
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxcommon.js"></script>
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/dhtmlxtree.js"></script>
    <script type="text/javascript" src="../js/dhtmlx/dhtmlxTree/codebase/ext/dhtmlxtree_json.js"></script> 
    <!--DHX-->
    <script type="text/javascript"  src="../js/Seguridad.Menu.js"></script>
</asp:Content>
<asp:Content ID="bodySeguridadMenu" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogOrdenarMenus" title="Ordenar Menús">
        <div id="divMenus" class="divContenedorListado">
            <div id="divTituloMenu" class="divTituloListado">Menús</div>
            <ul id="ulMenus" class="ulListado"></ul>
        </div>
    </div>
    <div id="dialogOrdenarSubmenus" title="Ordenar Submenús">
        <div id="divSubmenus" class="divContenedorListado">
            <div id="divTituloSubmenu" class="divTituloListado">Submenús</div>
            <ul id="ulSubmenus" class="ulListado"></ul>
        </div>
    </div>
    <div id="divContenido">
        <div id="col-1" class="col-1">
            <div id="treeboxbox_tree" class="etiquetaOculta"></div>
        </div>
        <div id="col-2" class="col-2"></div>
    </div>
</asp:Content>
