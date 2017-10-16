using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public class CJQColumn
{
    //Atributos
    private string nombre;
    private string ancho;
    private string alineacion;
    private string encabezado;
    private string oculto;
    private string formato;
    private string ordenable;
    private string etiquetado;
    private string funcion;
    private string estilo;
    private string id;
    private string buscador;
    private string tipoBuscador;
    private string nombreBaja;
    private string imagen;
    private string editable;
    private string editableTexto;
    public SqlCommand StoredProcedure = new SqlCommand();

    //Propiedades
    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nombre = value;
        }
    }

    public string Ancho
    {
        get { return ancho; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            ancho = value;
        }
    }

    public string Alineacion
    {
        get { return alineacion; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            alineacion = value;
        }
    }

    public string Encabezado
    {
        get { return encabezado; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            encabezado = value;
        }
    }

    public string Oculto
    {
        get { return oculto; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            oculto = value;
        }
    }

    public string Formato
    {
        get { return formato; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            formato = value;
        }
    }

    public string Ordenable
    {
        get { return ordenable; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            ordenable = value;
        }
    }

    public string Etiquetado
    {
        get { return etiquetado; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            etiquetado = value;
        }
    }

    public string Funcion
    {
        get { return funcion; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            funcion = value;
        }
    }

    public string Estilo
    {
        get { return estilo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            estilo = value;
        }
    }

    public string Id
    {
        get { return id; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            id = value;
        }
    }

    public string Buscador
    {
        get { return buscador; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            buscador = value;
        }
    }

    public string TipoBuscador
    {
        get { return tipoBuscador; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            tipoBuscador = value;
        }
    }

    public string NombreBaja
    {
        get { return nombreBaja; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nombreBaja = value;
        }
    }

    public string Imagen
    {
        get { return imagen; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            imagen = value;
        }
    }

    public string Editable
    {
        get { return editable; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            editable = value;
        }
    }

    public string EditableTexto
    {
        get { return editableTexto; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            editableTexto = value;
        }
    }

    //Constructores
    public CJQColumn()
    {
        ancho = "200";
        alineacion = "Center";
        oculto = "false";
        formato = "";
        ordenable = "true";
        etiquetado = "";
        funcion = "";
        estilo = "";
        id = "";
        buscador = "true";
        tipoBuscador = "Autocompletar";
        nombreBaja = "";
        imagen = "";
        editable = "";
        editableTexto = "";
    }

    public CJQColumn(string pNombre, string pAncho, string pAlineacion, string pEncabezado, string pOculto, string pFormato, string pOrdenable)
    {
        nombre = pNombre;
        ancho = pAncho;
        alineacion = pAlineacion;
        encabezado = pEncabezado;
        oculto = pOculto;
        formato = pFormato;
        ordenable = pOrdenable;
        etiquetado = "";
        funcion = "";
        estilo = "";
        id = "";
        buscador = "true";
        tipoBuscador = "Autocompletar";
        imagen = "";
        editable = "";
        editableTexto = "";
    }

    public CJQColumn(string pNombre, string pAncho, string pAlineacion, string pEncabezado, string pOculto, string pFormato, string pOrdenable, string pEtiquetado, string pFuncion)
    {
        nombre = pNombre;
        ancho = pAncho;
        alineacion = pAlineacion;
        encabezado = pEncabezado;
        oculto = pOculto;
        formato = pFormato;
        ordenable = pOrdenable;
        etiquetado = pEtiquetado;
        funcion = pFuncion;
        estilo = "divConsultar";
        id = "";
        buscador = "true";
        tipoBuscador = "Autocompletar";
        nombreBaja = "";
        imagen = "";
        editable = "";
        editableTexto = "";
    }

    public CJQColumn(string pNombre, string pAncho, string pAlineacion, string pEncabezado, string pOculto, string pFormato, string pOrdenable, string pEtiquetado, string pFuncion, string pEstilo)
    {
        nombre = pNombre;
        ancho = pAncho;
        alineacion = pAlineacion;
        encabezado = pEncabezado;
        oculto = pOculto;
        formato = pFormato;
        ordenable = pOrdenable;
        etiquetado = pEtiquetado;
        funcion = pFuncion;
        estilo = pEstilo;
        id = "";
        buscador = "true";
        tipoBuscador = "Autocompletar";
        nombreBaja = "";
        imagen = "";
        editable = "";
        editableTexto = "";
    }

    public CJQColumn(string pNombre, string pAncho, string pAlineacion, string pEncabezado, string pOculto, string pFormato, string pOrdenable, string pEtiquetado, string pFuncion, string pEstilo, string pId)
    {
        nombre = pNombre;
        ancho = pAncho;
        alineacion = pAlineacion;
        encabezado = pEncabezado;
        oculto = pOculto;
        formato = pFormato;
        ordenable = pOrdenable;
        etiquetado = pEtiquetado;
        funcion = pFuncion;
        estilo = pEstilo;
        id = pId;
        buscador = "true";
        tipoBuscador = "Autocompletar";
        nombreBaja = "";
        imagen = "";
        editable = "";
        editableTexto = "";
    }
}