using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

public class CComboBox
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();
    private string[] columnas;
    private string nombre;
    private string cssClase;
    private string valorInicio;
    private int opcionSeleccionada;
    private string tabIndex;
    private string funcionOnChange;
    private bool habilitado;
    private int numeroInicial;
    private int numeroFinal;

    //Propiedades
    public string[] Columnas
    {
        get { return columnas; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            columnas = value;
        }
    }

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

    public string CssClase
    {
        get { return cssClase; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            cssClase = value;
        }
    }

    public string ValorInicio
    {
        get { return valorInicio; }
        set { valorInicio = value; }
    }

    public int OpcionSeleccionada
    {
        get { return opcionSeleccionada; }
        set
        {
            if (value == 0)
            {
                return;
            }
            opcionSeleccionada = value;
        }
    }

    public string TabIndex
    {
        get { return tabIndex; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            tabIndex = value;
        }
    }

    public string FuncionOnChange
    {
        get { return funcionOnChange; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            funcionOnChange = value;
        }
    }

    public bool Habilitado
    {
        get { return habilitado; }
        set
        { habilitado = value; }
    }

    public int NumeroInicial
    {
        get { return numeroInicial; }
        set
        {
            if (value == 0)
            {
                return;
            }
            numeroInicial = value;
        }
    }

    public int NumeroFinal
    {
        get { return numeroFinal; }
        set
        { numeroFinal = value; }
    }

    //Contructor
    public CComboBox()
    {
        opcionSeleccionada = 0;
        habilitado = true;
        numeroInicial = 1;
        numeroFinal = 10;
    }

    public string GeneraCombo(CConexion pConexion)
    {
        SqlDataReader drSelect;
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        drSelect = StoredProcedure.ExecuteReader();
        int idCampo = 0;

        string htmlComboBox = "";
        htmlComboBox = "<select id='" + nombre + "' class='" + cssClase + "'";

        if (funcionOnChange != "")
        {
            htmlComboBox = htmlComboBox + " onchange='javascript: " + funcionOnChange + ";'";
        }

        if (habilitado == false)
        {
            htmlComboBox = htmlComboBox + " disabled='disabled'";
        }

        if (tabIndex != "")
        {
            htmlComboBox = htmlComboBox + " tabindex='" + tabIndex + "'";
        }

        htmlComboBox = htmlComboBox + "><option value='0'>" + valorInicio + "</option>";

        while (drSelect.Read())
        {
            idCampo = Convert.ToInt32(drSelect[columnas[0]]);
            htmlComboBox = htmlComboBox + "<option value='";
            htmlComboBox = htmlComboBox + drSelect[columnas[0]] + "' ";
            if (idCampo == opcionSeleccionada)
            {
                htmlComboBox = htmlComboBox + "selected='selected'";
            }
            htmlComboBox = htmlComboBox + ">";
            htmlComboBox = htmlComboBox + drSelect[columnas[1]];
            htmlComboBox = htmlComboBox + "</opcion>";
        }

        htmlComboBox = htmlComboBox + "</select>";
        //htmlComboBox = htmlComboBox + "<asp:Label ID='lbl"+columnas[1]+"' CssClass='lblEtiqueta' runat='server'>"+columnas[1]+":</asp:Label>";
        return htmlComboBox;
    }

    public string GeneraComboNumerico(CConexion pConexion)
    {
        string htmlComboBox = "";
        htmlComboBox = "<select id='" + nombre + "' class='" + cssClase + "'";

        if (funcionOnChange != "")
        {
            htmlComboBox = htmlComboBox + " onchange='javascript: " + funcionOnChange + ";'";
        }

        if (habilitado == false)
        {
            htmlComboBox = htmlComboBox + " disabled='disabled'";
        }

        if (tabIndex != "")
        {
            htmlComboBox = htmlComboBox + " tabindex='" + tabIndex + "'";
        }

        htmlComboBox = htmlComboBox + "><option value='0'>" + valorInicio + "</option>";

        for (int i = numeroInicial; i <= numeroFinal; i++)
        {
            htmlComboBox = htmlComboBox + "<option value='";
            htmlComboBox = htmlComboBox + i + "' ";
            if (i == opcionSeleccionada)
            {
                htmlComboBox = htmlComboBox + "selected='selected'";
            }
            htmlComboBox = htmlComboBox + ">";
            htmlComboBox = htmlComboBox + i;
            htmlComboBox = htmlComboBox + "</opcion>";
        }

        htmlComboBox = htmlComboBox + "</select>";
        return htmlComboBox;
    }
}