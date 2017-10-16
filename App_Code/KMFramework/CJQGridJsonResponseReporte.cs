using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public class CJQGridJsonResponseReporte
{
    #region Passive attributes.

    private int _noPaginas;
    private int _paginaActual;
    private int _noRegistros;
    private List<CJQGridItem> _elementos;

    #endregion

    #region Properties

    /// <summary>
    /// Cantidad de páginas del JQGrid.
    /// </summary>
    public int NoPaginas
    {
        get { return _noPaginas; }
        set { _noPaginas = value; }
    }
    /// <summary>
    /// Página actual del JQGrid.
    /// </summary>
    public int PaginaActual
    {
        get { return _paginaActual; }
        set { _paginaActual = value; }
    }
    /// <summary>
    /// Cantidad total de elementos de la lista.
    /// </summary>
    public int NoRegistros
    {
        get { return _noRegistros; }
        set { _noRegistros = value; }
    }
    /// <summary>
    /// Lista de elementos del JQGrid.
    /// </summary>
    public List<CJQGridItem> Elementos
    {
        get { return _elementos; }
        set { _elementos = value; }
    }

    #endregion

    #region Active attributes
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="pItems">Lista de elementos a mostrar en el JQGrid</param>

    public CJQGridJsonResponseReporte(DataSet dataSet)
    {
        _noPaginas = Convert.ToInt32(dataSet.Tables[0].Rows[0]["NoPaginas"]);
        _paginaActual = Convert.ToInt32(dataSet.Tables[0].Rows[0]["PaginaActual"]);
        _noRegistros = Convert.ToInt32(dataSet.Tables[0].Rows[0]["NoRegistros"]);
        _elementos = new List<CJQGridItem>();
        List<string> arregloJson = new List<string>();


        foreach (DataRow row in dataSet.Tables[1].Rows)
        {
            for (int i = 0; i < dataSet.Tables[1].Columns.Count; i++)
            {
                switch (dataSet.Tables[1].Columns[i].DataType.Name)
                {
                    case "Int32":
                        arregloJson.Add(Convert.ToString(row[i]));
                        break;
                    case "String":
                        arregloJson.Add(Convert.ToString(row[i]));
                        break;
                    case "Decimal":
                        arregloJson.Add(Convert.ToString(row[i]));
                        break;
                    case "Boolean":
                        arregloJson.Add(Convert.ToString(row[i]));
                        break;
                    case "DateTime":
                        arregloJson.Add(Convert.ToDateTime(row[i]).ToShortDateString());
                        break;
                    default:
                        arregloJson.Add(Convert.ToString(row[i]));
                        break;
                }
            }
            _elementos.Add(new CJQGridItem(Convert.ToInt64(row[0]), new List<string>(arregloJson)));
            arregloJson.Clear();
        }
    }

    #endregion
}
