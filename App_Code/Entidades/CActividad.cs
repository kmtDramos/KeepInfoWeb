using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class CActividad
{
	//Constructores
	
	//Metodos Especiales
    public static void GenerarGridActividadesClienteOportunidad(string Titulo, Page Page, ClientScriptManager ClientScript) {

        // GridActividadesClienteOportunidad
        CJQGrid GridActividadesClienteOportunidad = new CJQGrid();
        GridActividadesClienteOportunidad.NombreTabla = "grdActividadesClienteOportunidad";
        GridActividadesClienteOportunidad.CampoIdentificador = "IdActvidad";
        GridActividadesClienteOportunidad.ColumnaOrdenacion = "FechaActividad";
        GridActividadesClienteOportunidad.TipoOrdenacion = "DESC";
        GridActividadesClienteOportunidad.Metodo = "ObtenerActividadesClienteOportunidad";
        GridActividadesClienteOportunidad.TituloTabla = Titulo;
        GridActividadesClienteOportunidad.GenerarFuncionFiltro = false;
        GridActividadesClienteOportunidad.GenerarFuncionTerminado = false;
        GridActividadesClienteOportunidad.Altura = 300;
        GridActividadesClienteOportunidad.Ancho = 700;
        GridActividadesClienteOportunidad.NumeroRegistros = 15;
        GridActividadesClienteOportunidad.RangoNumeroRegistros = "15,30,60";

        // IdActividad
        CJQColumn ColIdActividad = new CJQColumn();
        ColIdActividad.Nombre = "IdActvidad";
        ColIdActividad.Oculto = "true";
        ColIdActividad.Encabezado = "IdActvidad";
        ColIdActividad.Buscador = "false";
        GridActividadesClienteOportunidad.Columnas.Add(ColIdActividad);

        // TipoActividad
        CJQColumn ColTipoActividad = new CJQColumn();
        ColTipoActividad.Nombre = "TipoActividad";
        ColTipoActividad.Encabezado = "Tipo de actividad";
        ColTipoActividad.TipoBuscador = "Combo";
        ColTipoActividad.StoredProcedure.CommandText = "sp_TipoActividad_Combo";
        ColTipoActividad.Alineacion = "left";
        ColTipoActividad.Ancho = "100";
        GridActividadesClienteOportunidad.Columnas.Add(ColTipoActividad);

        // Usuario
        CJQColumn ColUsuario = new CJQColumn();
        ColUsuario.Nombre = "IdUsuario";
        ColUsuario.Encabezado = "Usuario";
        ColUsuario.Buscador = "false";
        ColUsuario.Alineacion = "left";
        ColUsuario.Ancho = "200";
        GridActividadesClienteOportunidad.Columnas.Add(ColUsuario);

        // IdOportunidad
        CJQColumn ColIdOportunidad = new CJQColumn();
        ColIdOportunidad.Nombre = "IdOportunidad";
        ColIdOportunidad.Encabezado = "#";
        ColIdOportunidad.Buscador = "false";
        ColIdOportunidad.Alineacion = "left";
        ColIdOportunidad.Ancho = "80";
        GridActividadesClienteOportunidad.Columnas.Add(ColIdOportunidad);

        // Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "Oportunidad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "false";
        ColOportunidad.Alineacion = "left";
        ColOportunidad.Ancho = "250";
        GridActividadesClienteOportunidad.Columnas.Add(ColOportunidad);

        // Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "350";
        GridActividadesClienteOportunidad.Columnas.Add(ColDescripcion);

        // Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaActividad";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridActividadesClienteOportunidad.Columnas.Add(ColFecha);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdActividadesClienteOportunidad", GridActividadesClienteOportunidad.GeneraGrid(), true);

    }
}
