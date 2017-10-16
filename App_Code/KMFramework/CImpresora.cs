using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public class CImpresora
{
    private Word.Application aplicacionWord = new Word.Application();
    private Word.Document documentoWord = new Word.Document();

    public string ReportePDFTemplateArreglos(string rutaPDF, string rutaTemplate, string rutaCSS, string imagenLogo, int IdImpresionTemplate, JObject datos, CConexion pConexion, int pTipoImpresion)
    {
        Object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; /* \endofdoc es un bookmark predefinido */
        Object noSavings = Word.WdSaveOptions.wdDoNotSaveChanges;

        try
        {
            documentoWord = aplicacionWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            aplicacionWord.Visible = false;
            Object filepath = (Object)rutaTemplate;
            Object confirmconversion = System.Reflection.Missing.Value;
            Object readOnly = false;
            Object saveto = (Object)rutaPDF;
            Object oallowsubstitution = System.Reflection.Missing.Value;

            documentoWord = aplicacionWord.Documents.Add(ref filepath, ref oMissing, ref oMissing, ref oMissing);
            documentoWord.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
            object oPageEnter = Word.WdBreakType.wdLineBreak;

            Object nullobj = Type.Missing;
            Object start = 0;
            Object end = documentoWord.Characters.Count;

            ImprimirEtiquetasArreglos(datos, IdImpresionTemplate, rutaTemplate, pConexion);

            //Foco en el pie de pagina
            aplicacionWord.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;

            //Enter y crea parrafo
            aplicacionWord.Selection.TypeParagraph();

            String docNumber = "1";
            String revisionNumber = "0";

            //Inserta los numeros de pagina
            aplicacionWord.Selection.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            aplicacionWord.ActiveWindow.Selection.Font.Name = "Arial";
            aplicacionWord.ActiveWindow.Selection.Font.Size = 8;
            aplicacionWord.ActiveWindow.Selection.TypeText("");

            //Tabs
            aplicacionWord.ActiveWindow.Selection.TypeText("\t");
            aplicacionWord.ActiveWindow.Selection.TypeText("\t");
            aplicacionWord.ActiveWindow.Selection.TypeText("Página ");
            Object CurrentPage = Word.WdFieldType.wdFieldPage;
            aplicacionWord.ActiveWindow.Selection.Fields.Add(aplicacionWord.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            aplicacionWord.ActiveWindow.Selection.TypeText(" de ");
            Object TotalPages = Word.WdFieldType.wdFieldNumPages;
            aplicacionWord.ActiveWindow.Selection.Fields.Add(aplicacionWord.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            documentoWord.PageSetup.LeftMargin = 10;
            documentoWord.PageSetup.RightMargin = 10;
            documentoWord.PageSetup.TopMargin = 10;
            documentoWord.PageSetup.BottomMargin = 10;

            if (pTipoImpresion == 1)
            {
                documentoWord.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                documentoWord.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }

            /*** Se guarda como PDF ***/
            Object fileFormat = Word.WdSaveFormat.wdFormatPDF;
            //Object fileFormat = Word.WdSaveFormat.wdFormatHTML;
            documentoWord.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing, ref oMissing);
            return "0|../Archivos/Impresiones/" + Path.GetFileName(rutaPDF);
        }
        catch (Exception ex)
        {
            return "1|Error " + ex.Message;
        }
        finally
        {
            documentoWord.Close(ref noSavings, ref oMissing, ref oMissing);
            documentoWord = null;
            aplicacionWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            aplicacionWord = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public void ImprimirEtiquetasArreglos(JObject pJDocumento, int pIdImpresionTemplate, string pRutaTemplate, CConexion pConexion)
    {
        CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdImpresionTemplate", pIdImpresionTemplate);

        object oEndOfDoc = "\\endofdoc"; /* \endofdoc es un bookmark predefinido */
        Object oMissing = System.Reflection.Missing.Value;
        Word.Range wrdRng = documentoWord.Bookmarks.get_Item(ref oEndOfDoc).Range;
        int ind = 0;
        object oPos;
        double dPos = aplicacionWord.InchesToPoints(10);

        foreach (JProperty oPropiedadDocumento in pJDocumento.Properties())
        {
            if (oPropiedadDocumento.Value.Type.ToString() == "Object")
            {
                JObject JTablaConceptos = new JObject();
                JTablaConceptos = (JObject)pJDocumento[oPropiedadDocumento.Name];
                foreach (JProperty oPropiedadTabla in JTablaConceptos.Properties())
                {
                    if (oPropiedadTabla.Name == "Tipo")
                    {
                        if (oPropiedadTabla.Value.ToString() == "Conceptos")
                        {
                            for (int i = 1; i <= documentoWord.Tables.Count; i++)
                            {

                                Word.Table wTable = documentoWord.Tables[i];
                                Word.Cell pCell = wTable.Cell(1, 1);

                                if (wTable.ID != null && wTable.ID == "tblReceptor")
                                {
                                    foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                                    {
                                        Word.Range rangePlantilla = wTable.Range;

                                        int rc = wTable.Rows.Count;
                                        // Selecciona y copia el renglon Layout.
                                        rangePlantilla.Start = 1;
                                        rangePlantilla.End = wTable.Rows[rc].Cells[wTable.Rows[rc].Cells.Count].Range.End;
                                        rangePlantilla.Find.ClearFormatting();
                                        rangePlantilla.Find.Text = oImpresionEtiqueta.Etiqueta;
                                        rangePlantilla.Find.Replacement.ClearFormatting();


                                        if (pJDocumento[oImpresionEtiqueta.Campo] != null)
                                        {
                                            rangePlantilla.Find.Replacement.Text = pJDocumento[oImpresionEtiqueta.Campo].ToString();
                                        }
                                        object replaceAll = Word.WdReplace.wdReplaceAll;
                                        rangePlantilla.Find.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                    }
                                }

                                if (wTable.ID != null && wTable.ID == JTablaConceptos["NombreTabla"].ToString())
                                {

                                    bool pRowsNum = false;
                                    if (wTable.Rows.Count > 1)
                                    {
                                        pRowsNum = true;
                                    }

                                    int contador = 1;
                                    // Layaout
                                    foreach (JObject JInventario in JTablaConceptos["Inventarios"])
                                    {
                                        contador = contador + 1;
                                        Word.Range range = wTable.Range;

                                        // Renglon Layout
                                        int selectedRow = 1;

                                        // Selecciona y copia el renglon Layout.
                                        range.Start = wTable.Rows[selectedRow].Cells[1].Range.Start;
                                        range.End = wTable.Rows[selectedRow].Cells[wTable.Rows[selectedRow].Cells.Count].Range.End;
                                        range.Copy();

                                        // Inserta renglon nuevo al final de la tabla.
                                        wTable.Rows.Add(ref oMissing);

                                        // Mueve el cursor a la primera celda del renglon nuevo.
                                        range.Start = wTable.Rows[wTable.Rows.Count].Cells[1].Range.Start;
                                        range.End = range.Start;

                                        // Pega los valores del renglon layout al renglon nuevo.
                                        range.Paste();

                                        int m = wTable.Rows.Count;

                                        Word.Row renglonPlantilla = wTable.Rows[m];
                                        Word.Cell celdaPlantilla = renglonPlantilla.Cells[1];

                                        for (int x = 1; x <= celdaPlantilla.Tables.Count; x++)
                                        {
                                            bool sRowsNum = false;
                                            if (celdaPlantilla.Tables[x].Rows.Count > 3)
                                            {
                                                sRowsNum = true;
                                            }
                                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                                            {
                                                Word.Range rangePlantilla = wTable.Range;

                                                // Selecciona y copia el renglon Layout.
                                                rangePlantilla.Start = celdaPlantilla.Tables[x].Rows[1].Cells[1].Range.Start;
                                                rangePlantilla.End = celdaPlantilla.Tables[x].Rows[1].Cells[celdaPlantilla.Tables[x].Rows[1].Cells.Count].Range.End;
                                                rangePlantilla.Find.ClearFormatting();
                                                rangePlantilla.Find.Text = oImpresionEtiqueta.Etiqueta;
                                                rangePlantilla.Find.Replacement.ClearFormatting();

                                                if (JInventario[oImpresionEtiqueta.Campo] != null)
                                                {
                                                    rangePlantilla.Find.Replacement.Text = JInventario[oImpresionEtiqueta.Campo].ToString();
                                                }
                                                object replaceAll = Word.WdReplace.wdReplaceAll;
                                                rangePlantilla.Find.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                            }

                                            foreach (JObject JConcepto in JInventario["Lista"])
                                            {
                                                Word.Range sRange = celdaPlantilla.Tables[x].Range;

                                                // Selecciona y copia el renglon Layout.
                                                sRange.Start = celdaPlantilla.Tables[x].Rows[3].Cells[1].Range.Start;
                                                sRange.End = celdaPlantilla.Tables[x].Rows[3].Cells[celdaPlantilla.Tables[x].Rows[3].Cells.Count].Range.End;
                                                sRange.Copy();

                                                // Inserta renglon nuevo al final de la tabla.
                                                celdaPlantilla.Tables[x].Rows.Add(ref oMissing);

                                                // Mueve el cursor a la primera celda del renglon nuevo.
                                                sRange.Start = celdaPlantilla.Tables[x].Rows[celdaPlantilla.Tables[x].Rows.Count].Cells[1].Range.Start;
                                                sRange.End = sRange.Start;

                                                // Pega los valores del renglon layout al renglon nuevo.
                                                sRange.Paste();

                                                foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                                                {
                                                    Word.Range rowRange = wTable.Range;

                                                    // Selecciona y copia el renglon Layout.
                                                    sRange.Find.ClearFormatting();
                                                    sRange.Find.Text = oImpresionEtiqueta.Etiqueta;
                                                    sRange.Find.Replacement.ClearFormatting();

                                                    if (JConcepto[oImpresionEtiqueta.Campo] != null)
                                                    {
                                                        sRange.Find.Replacement.Text = JConcepto[oImpresionEtiqueta.Campo].ToString();
                                                    }
                                                    object replaceAll = Word.WdReplace.wdReplaceAll;
                                                    sRange.Find.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                                }

                                            }
                                            if (sRowsNum)
                                            {
                                                JObject JTotal = new JObject();
                                                JTotal = (JObject)JInventario["Total"];

                                                Word.Range sRange = celdaPlantilla.Tables[x].Range;

                                                // Selecciona y copia el renglon Layout.
                                                sRange.Start = celdaPlantilla.Tables[x].Rows[4].Cells[1].Range.Start;
                                                sRange.End = celdaPlantilla.Tables[x].Rows[4].Cells[celdaPlantilla.Tables[x].Rows[4].Cells.Count].Range.End;
                                                sRange.Copy();

                                                // Inserta renglon nuevo al final de la tabla.
                                                celdaPlantilla.Tables[x].Rows.Add(ref oMissing);

                                                // Mueve el cursor a la primera celda del renglon nuevo.
                                                sRange.Start = celdaPlantilla.Tables[x].Rows[celdaPlantilla.Tables[x].Rows.Count].Cells[1].Range.Start;
                                                sRange.End = sRange.Start;

                                                // Pega los valores del renglon layout al renglon nuevo.
                                                sRange.Paste();

                                                foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                                                {
                                                    Word.Range rowRange = wTable.Range;

                                                    // Selecciona y copia el renglon Layout.
                                                    sRange.Find.ClearFormatting();
                                                    sRange.Find.Text = oImpresionEtiqueta.Etiqueta;
                                                    sRange.Find.Replacement.ClearFormatting();

                                                    if (JTotal[oImpresionEtiqueta.Campo] != null)
                                                    {
                                                        sRange.Find.Replacement.Text = JTotal[oImpresionEtiqueta.Campo].ToString();
                                                    }
                                                    object replaceAll = Word.WdReplace.wdReplaceAll;
                                                    sRange.Find.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                                }
                                                celdaPlantilla.Tables[x].Rows[4].Delete();
                                            }
                                            celdaPlantilla.Tables[x].Rows[3].Delete();
                                        }

                                    }

                                    if (pRowsNum)
                                    {

                                        Word.Range sRange = wTable.Range;

                                        // Selecciona y copia el renglon Layout.
                                        sRange.Start = wTable.Rows[2].Cells[1].Range.Start;
                                        sRange.End = wTable.Rows[2].Cells[wTable.Rows[2].Cells.Count].Range.End;
                                        sRange.Copy();

                                        // Inserta renglon nuevo al final de la tabla.
                                        wTable.Rows.Add(ref oMissing);

                                        // Mueve el cursor a la primera celda del renglon nuevo.
                                        sRange.Start = wTable.Rows[wTable.Rows.Count].Cells[1].Range.Start;
                                        sRange.End = sRange.Start;

                                        // Pega los valores del renglon layout al renglon nuevo.
                                        sRange.Paste();

                                        foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                                        {
                                            Word.Range rangePlantilla = wTable.Range;

                                            // Selecciona y copia el renglon Layout.
                                            rangePlantilla.Find.ClearFormatting();
                                            rangePlantilla.Find.Text = oImpresionEtiqueta.Etiqueta;
                                            rangePlantilla.Find.Replacement.ClearFormatting();


                                            if (pJDocumento[oImpresionEtiqueta.Campo] != null)
                                            {
                                                rangePlantilla.Find.Replacement.Text = pJDocumento[oImpresionEtiqueta.Campo].ToString();
                                            }
                                            object replaceAll = Word.WdReplace.wdReplaceAll;
                                            rangePlantilla.Find.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                                        }
                                        wTable.Rows[2].Delete();
                                    }
                                    wTable.Rows[1].Delete();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void descargarPDF(string NombreArchivo, HttpResponse Response)
    {
        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + NombreArchivo);
        Response.WriteFile((HttpContext.Current.Server.MapPath("../Archivos/Impresiones/" + NombreArchivo)));
        Response.Flush();
        Response.End();
    }
}