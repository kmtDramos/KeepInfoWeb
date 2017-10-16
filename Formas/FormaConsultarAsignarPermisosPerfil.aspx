<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarAsignarPermisosPerfil.aspx.cs" Inherits="FormaConsultarAsignarPermisosPerfil" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Configurar Permisos de los Perfiles
            <div class="divTituloFormularioAcciones">
            </div>
        </div>
        <div class="divContenedorPermisos">
            <div id="divPermisosAsignados" class="divConsultaPermisos">
                <div class="divFondoPermisos">Permisos Asignados</div>
                <ul id="ulConsultaPermisosAsignados" runat="server"></ul>
            </div>
        </div>
        <div class="divAreaBotones">
            <input type="button" id="btnCerrar" value="Cerrar" class="button" onclick="javascript:SetFormaInicioAsignarPermisos('Elige una opcion del arbol para configurar los permisos de perfiles y paginas.');"/>
            <input type="button" id="btnEditar" value="Configurar Permisos" class="button" runat="server"/>
        </div>
    </div>
</form>