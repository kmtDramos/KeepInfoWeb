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


public partial class CEstructuraAddenda
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerJsonEstructuraAddenda(JObject pModelo, int pIdEstructuraAddenda, CConexion pConexion)
    {
        CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
        EstructuraAddenda.LlenaObjeto(pIdEstructuraAddenda, pConexion);
        pModelo.Add("IdEstructuraAddenda", EstructuraAddenda.IdEstructuraAddenda);
        pModelo.Add("EstructuraAddenda", EstructuraAddenda.EstructuraAddenda);
        pModelo.Add("IdTipoElemento", EstructuraAddenda.IdTipoElemento);
        return pModelo;
    }

    public static JArray ObtenerJsonTipoElementos(int pIdEstructuraAddenda, CConexion pConexion)
    {

        CTipoElemento TipoElemento = new CTipoElemento();
        JArray JTipoElementos = new JArray();



        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoElemento oTipoElemento in TipoElemento.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoElemento = new JObject();
            JTipoElemento.Add("IdTipoElemento", oTipoElemento.IdTipoElemento);
            JTipoElemento.Add("TipoElemento", oTipoElemento.TipoElemento);
            if (oTipoElemento.IdTipoElemento == pIdEstructuraAddenda)
            {
                JTipoElemento.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoElemento.Add(new JProperty("Selected", 0));
            }
            JTipoElementos.Add(JTipoElemento);
        }
        return JTipoElementos;
    }
}