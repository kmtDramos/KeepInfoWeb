<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaAsignarPermisosPagina.aspx.cs" Inherits="FormaAltaAsignarPermisosPagina" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Configurar Permisos de las Paginas
            <div class="divTituloFormularioAcciones">
            </div>
        </div>
        <div class="divContenedorPermisos">
            <div id="divPermisosAsignados" class="divColumnaPermisos">
                <div class="divFondoPermisos">Permisos Asignados</div>
                <ul id="ulPermisosAsignados" class="droptrue" runat="server"></ul>
            </div>
            <div id="divPermisosDisponibles" class="divColumnaPermisos">
                <div class="divFondoPermisos">Permisos Disponibles</div>
                <ul id="ulPermisosDisponibles" class="droptrue" runat="server"></ul>
            </div>
        </div>
        <div class="divAreaBotones">
            <input type="button" id="btnCancelar" value="Cancelar" class="button" onclick="javascript:SetFormaInicioAsignarPermisos('Elige una opcion del arbol para configurar los permisos de perfiles y paginas.');"/>
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:AgregarPermisosPagina();"/>
        </div>
    </div>
</form>