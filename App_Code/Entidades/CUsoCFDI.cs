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

public partial class CUsoCFDI
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonUsoCFDIActivas(CConexion pConexion)
    {
        JArray JAUsoCFDI = new JArray();
        CUsoCFDI UsoCFDI = new CUsoCFDI();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("Baja", 0);
        foreach (CUsoCFDI oUsoCFDI in UsoCFDI.LlenaObjetosFiltros(pParametros, pConexion))
        {
            JObject JUsoCFDI = new JObject();
            JUsoCFDI.Add("Valor", oUsoCFDI.IdUsoCFDI);
            JUsoCFDI.Add("Descripcion", oUsoCFDI.ClaveUsoCFDI+" - "+oUsoCFDI.Descricpion);
            JAUsoCFDI.Add(JUsoCFDI);
        }
        return JAUsoCFDI;
    }

}
