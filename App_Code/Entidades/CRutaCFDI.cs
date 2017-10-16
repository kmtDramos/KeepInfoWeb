using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CRutaCFDI
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerJsonRutaCFDI(JObject pModelo, int pIdRutaCFDI, CConexion pConexion)
    {
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        RutaCFDI.LlenaObjeto(pIdRutaCFDI, pConexion);
        pModelo.Add("IdRutaCFDI", RutaCFDI.IdRutaCFDI);
        pModelo.Add("RutaCFDI", RutaCFDI.RutaCFDI);
        pModelo.Add("IdSucursal", RutaCFDI.IdSucursal);
        if (RutaCFDI.TipoRuta == 1)
        {
            pModelo.Add("TipoRuta", "Física");
        }
        else
        {
            pModelo.Add("TipoRuta", "Virtual");
        }
        return pModelo;
    }

}