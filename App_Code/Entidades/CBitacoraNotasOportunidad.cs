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



public partial class CBitacoraNotasOportunidad
{
    public static JArray ObtenerComentariosOportunidadDesc(int pIdOportunidad, CConexion pConexion)
    {
        JArray JAComentarios = new JArray();
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Oportunidad_BitacoraComentarios";
        Select.StoredProcedure.Parameters.AddWithValue("IdOportunidad", pIdOportunidad);
        Select.Llena(pConexion);
        
        while (Select.Registros.Read())
        {
            JObject JComentario = new JObject();
            JComentario.Add("Comentario", Select.Registros["Comentario"].ToString());
            JComentario.Add("Fecha", Select.Registros["Fecha"].ToString());
            JComentario.Add("Usuario", Select.Registros["Usuario"].ToString());
            JComentario.Add("Area", Select.Registros["Area"].ToString());
            JAComentarios.Add(JComentario);
        }
        Select.CerrarConsulta();
        return JAComentarios;
    }

}