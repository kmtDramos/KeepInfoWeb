using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Word = Microsoft.Office.Interop.Word;
using System.Globalization;
using System.Text.RegularExpressions;

public class CUtilerias
{
    public string SubstringCustom(string value, string startTag, string endTag)
    {
        if (value.Contains(startTag) && value.Contains(endTag))
        {
            int index = value.IndexOf(startTag) + startTag.Length;
            return value.Substring(index, value.IndexOf(endTag) - index);
        }
        else
            return null;
    }

    public string ReportePDFTemplate(string rutaPDF, string rutaTemplate, string rutaCSS, string imagenLogo, int IdImpresionTemplate, JArray datos, CConexion pConexion)
    {

        Word.Application word = new Word.Application();
        //Word.Application word = (Word.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
        Word.Document wordDoc = new Word.Document();

        Object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; /* \endofdoc es un bookmark predefinido */
        Object noSavings = Word.WdSaveOptions.wdDoNotSaveChanges;

        int ind = 0;

        try
        {
            wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            word.Visible = false;
            Object filepath = (Object)rutaTemplate;
            Object confirmconversion = System.Reflection.Missing.Value;
            Object readOnly = false;
            Object saveto = (Object)rutaPDF;
            Object oallowsubstitution = System.Reflection.Missing.Value;

            wordDoc = word.Documents.Add(ref filepath, ref oMissing, ref oMissing, ref oMissing);
            Word.Range wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            object oPos;
            double dPos = word.InchesToPoints(10);
            wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
            object oPageEnter = Word.WdBreakType.wdLineBreak;

            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdImpresionTemplate", IdImpresionTemplate);

            JObject Info = new JObject();
            Info = (JObject)datos.First;

            wordDoc.Activate();

            Object nullobj = Type.Missing;
            Object start = 0;
            Object end = wordDoc.Characters.Count;

            Word.Range rng = wordDoc.Range(ref start, ref end);
            Word.Find fnd = rng.Find;

            fnd.ClearFormatting();

            fnd.Text = "{IMAGEN_LOGO}";
            fnd.Forward = true;

            Object linktoFile = false;
            Object SaveWithDoc = true;
            Object replaceOption = Word.WdReplace.wdReplaceOne;
            Object rangeImg = Type.Missing;

            // Ubicamos la posición donde queremos insertar la imagen.
            fnd.Execute(ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref replaceOption, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj);

            //Insertamos la imagen en la posicion adecuada
            var shape = rng.InlineShapes.AddPicture(imagenLogo, ref linktoFile, ref SaveWithDoc, ref rangeImg);

            for (int i = 1; i <= wordDoc.Tables.Count; i++)
            {
                Word.Table wTable = wordDoc.Tables[i];
                Word.Cell pCell = wTable.Cell(1, 1);

                if (wTable.ID == "tblConceptos")
                {
                    wTable.AllowAutoFit = false;
                    for (int j = ind; j < datos.Count; j++)
                    {
                        JToken oConcepto = datos[ind];

                        wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        wrdRng.ParagraphFormat.SpaceAfter = 6;

                        oPos = wrdRng.get_Information
                                      (Word.WdInformation.wdVerticalPositionRelativeToPage);

                        if (dPos >= Convert.ToDouble(oPos))
                        {
                            Word.Range range = wTable.Range;

                            // Renglon Layout
                            int selectedRow = 3;

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

                            JObject Concepto = (JObject)oConcepto;
                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                            {
                                List<string> pListaCortada = CortarPalabra(Concepto[oImpresionEtiqueta.Campo].ToString(), oImpresionEtiqueta.Etiqueta, 200);
                                foreach (string oTexto in pListaCortada)
                                {
                                    Word.Find findConceptos = range.Find;
                                    findConceptos.ClearFormatting();
                                    findConceptos.Text = oImpresionEtiqueta.Etiqueta;
                                    findConceptos.Replacement.ClearFormatting();
                                    findConceptos.Replacement.Text = oTexto;
                                    object replaceAll = Word.WdReplace.wdReplaceAll;
                                    findConceptos.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                                    //if (pListaCortada.Count > 1)
                                    //{
                                    //    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                                    //    HistorialGenerico.Comentario = oTexto;
                                    //    HistorialGenerico.Fecha = DateTime.Now;
                                    //    HistorialGenerico.IdClaseGenerador = 64;
                                    //    HistorialGenerico.IdUsuario = 1;
                                    //    HistorialGenerico.IdGenerico = 0;
                                    //    HistorialGenerico.Agregar(pConexion);
                                    //}
                                }
                            }

                            ind++;
                        }
                        else
                        {
                            object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                            object oPageBreak = Word.WdBreakType.wdPageBreak;
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertBreak(ref oPageBreak);
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertFile(rutaTemplate, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            wrdRng.InsertParagraphAfter();
                        }
                    }
                    wTable.Rows[3].Delete();
                }
            }

            //Foco en el pie de pagina
            word.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;

            //Enter y crea parrafo
            word.Selection.TypeParagraph();

            String docNumber = "1";
            String revisionNumber = "0";

            //Inserta los numeros de pagina
            word.Selection.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            word.ActiveWindow.Selection.Font.Name = "Arial";
            word.ActiveWindow.Selection.Font.Size = 8;
            //word.ActiveWindow.Selection.TypeText("Documento #: " + docNumber + " - Revisión #: " + revisionNumber);
            word.ActiveWindow.Selection.TypeText("");

            //Tabs
            word.ActiveWindow.Selection.TypeText("\t");
            word.ActiveWindow.Selection.TypeText("\t");

            word.ActiveWindow.Selection.TypeText("Página ");
            Object CurrentPage = Word.WdFieldType.wdFieldPage;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            word.ActiveWindow.Selection.TypeText(" de ");
            Object TotalPages = Word.WdFieldType.wdFieldNumPages;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Foco en el documento
            wordDoc.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument;

            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
            {
                List<string> pListaCortada = CortarPalabra(Info[oImpresionEtiqueta.Campo].ToString(), oImpresionEtiqueta.Etiqueta, 200);
                foreach (string oTexto in pListaCortada)
                {
                    Word.Find findObject = word.Selection.Find;
                    findObject.ClearFormatting();
                    findObject.Text = oImpresionEtiqueta.Etiqueta;
                    findObject.Replacement.ClearFormatting();
                    findObject.Replacement.Text = oTexto;

                    object replaceAllC = Word.WdReplace.wdReplaceAll;
                    findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                        ref replaceAllC, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                    //if (pListaCortada.Count > 1)
                    //{
                    //    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    //    HistorialGenerico.Comentario = oTexto;
                    //    HistorialGenerico.Fecha = DateTime.Now;
                    //    HistorialGenerico.IdClaseGenerador = 64;
                    //    HistorialGenerico.IdUsuario = 1;
                    //    HistorialGenerico.IdGenerico = 0;
                    //    HistorialGenerico.Agregar(pConexion);
                    //}
                }
            }

            /*** Se guarda como PDF ***/
            Object fileFormat = Word.WdSaveFormat.wdFormatPDF;
            //Object fileFormat = Word.WdSaveFormat.wdFormatHTML;

            wordDoc.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing,
                           ref oMissing);

            return "0|../Archivos/Impresiones/" + Path.GetFileName(rutaPDF);
        }
        catch (Exception ex)
        {
            return "1|Error " + ex.StackTrace;
        }
        finally
        {
            wordDoc.Close(ref noSavings, ref oMissing, ref oMissing);
            NAR(wordDoc);
            word.Quit(ref oMissing, ref oMissing, ref oMissing);
            NAR(word);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public string ReportePDFTemplateConceptos(string rutaPDF, string rutaTemplate, string rutaCSS, string imagenLogo, int IdImpresionTemplate, JObject datos, CConexion pConexion, int pTipoImpresion)
    {
        JArray JAEncabezado = new JArray();
        JAEncabezado = (JArray)datos["Table"];

        JObject JEncabezado = new JObject();
        JEncabezado = (JObject)JAEncabezado.First;

        JArray JAConceptos = new JArray();
        JAConceptos = (JArray)datos["Table1"];

        JArray JAConceptosD = new JArray();
        JAConceptosD = (JArray)datos["Table2"];

        Word.Application word = new Word.Application();
        Word.Document wordDoc = new Word.Document();

        Object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; /* \endofdoc es un bookmark predefinido */
        Object noSavings = Word.WdSaveOptions.wdDoNotSaveChanges;

        int ind = 0;

        try
        {
            wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            word.Visible = false;
            Object filepath = (Object)rutaTemplate;
            Object confirmconversion = System.Reflection.Missing.Value;
            Object readOnly = false;
            Object saveto = (Object)rutaPDF;
            Object oallowsubstitution = System.Reflection.Missing.Value;

            wordDoc = word.Documents.Add(ref filepath, ref oMissing, ref oMissing, ref oMissing);
            Word.Range wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            object oPos;
            double dPos = word.InchesToPoints(10);
            wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
            object oPageEnter = Word.WdBreakType.wdLineBreak;

            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdImpresionTemplate", IdImpresionTemplate);
            wordDoc.Activate();

            Object nullobj = Type.Missing;
            Object start = 0;
            Object end = wordDoc.Characters.Count;

            Word.Range rng = wordDoc.Range(ref start, ref end);
            Word.Find fnd = rng.Find;

            fnd.ClearFormatting();

            //fnd.Text = "{IMAGEN_LOGO}";
            //fnd.Forward = true;

            Object linktoFile = false;
            Object SaveWithDoc = true;
            Object replaceOption = Word.WdReplace.wdReplaceOne;
            Object rangeImg = Type.Missing;

            // Ubicamos la posición donde queremos insertar la imagen.
            fnd.Execute(ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref replaceOption, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj);

            //Insertamos la imagen en la posicion adecuada
            //var shape = rng.InlineShapes.AddPicture(imagenLogo, ref linktoFile, ref SaveWithDoc, ref rangeImg);

            for (int i = 1; i <= wordDoc.Tables.Count; i++)
            {
                Word.Table wTable = wordDoc.Tables[i];
                Word.Cell pCell = wTable.Cell(1, 1);

                if (wTable.ID == "tblConceptos")
                {

                    for (int j = ind; j < JAConceptos.Count; j++)
                    {

                        wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        wrdRng.ParagraphFormat.SpaceAfter = 6;

                        oPos = wrdRng.get_Information
                                      (Word.WdInformation.wdVerticalPositionRelativeToPage);

                        if (dPos >= Convert.ToDouble(oPos))
                        {
                            Word.Range range = wTable.Range;

                            // Renglon Layout
                            int selectedRow = 2;

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
                            JObject Concepto = (JObject)JAConceptos[ind];

                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                            {
                                Word.Find findConceptos = range.Find;
                                findConceptos.ClearFormatting();
                                findConceptos.Text = oImpresionEtiqueta.Etiqueta;
                                findConceptos.Replacement.ClearFormatting();

                                if (Concepto[oImpresionEtiqueta.Campo] != null)
                                {
                                    if (Concepto[oImpresionEtiqueta.Campo].ToString().Length > 500)
                                    {
                                        findConceptos.Replacement.Text = Concepto[oImpresionEtiqueta.Campo].ToString().Substring(0, 200) + "...";
                                    }
                                    else
                                    {
                                        findConceptos.Replacement.Text = Concepto[oImpresionEtiqueta.Campo].ToString();
                                    }

                                }
                                object replaceAll = Word.WdReplace.wdReplaceAll;
                                findConceptos.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            }

                            ind++;
                        }
                        else
                        {
                            object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                            object oPageBreak = Word.WdBreakType.wdPageBreak;
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertBreak(ref oPageBreak);
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertFile(rutaTemplate, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            wrdRng.InsertParagraphAfter();
                        }
                    }
                    wTable.Rows[2].Delete();
                }

                if (wTable.ID == "tblConceptosD")
                {
                    ind = 0;
                    for (int j = ind; j < JAConceptosD.Count; j++)
                    {

                        wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        wrdRng.ParagraphFormat.SpaceAfter = 6;

                        oPos = wrdRng.get_Information
                                      (Word.WdInformation.wdVerticalPositionRelativeToPage);

                        if (dPos >= Convert.ToDouble(oPos))
                        {
                            Word.Range range = wTable.Range;

                            // Renglon Layout
                            int selectedRow = 2;

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
                            JObject Concepto = (JObject)JAConceptosD[ind];

                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                            {
                                Word.Find findConceptos = range.Find;
                                findConceptos.ClearFormatting();
                                findConceptos.Text = oImpresionEtiqueta.Etiqueta;
                                findConceptos.Replacement.ClearFormatting();

                                if (Concepto[oImpresionEtiqueta.Campo] != null)
                                {
                                    findConceptos.Replacement.Text = Concepto[oImpresionEtiqueta.Campo].ToString();
                                }
                                object replaceAll = Word.WdReplace.wdReplaceAll;
                                findConceptos.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            }

                            ind++;
                        }
                        else
                        {
                            object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                            object oPageBreak = Word.WdBreakType.wdPageBreak;
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertBreak(ref oPageBreak);
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertFile(rutaTemplate, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            wrdRng.InsertParagraphAfter();
                        }
                    }
                    wTable.Rows[2].Delete();
                }
            }

            //Foco en el pie de pagina
            word.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;

            //Enter y crea parrafo
            word.Selection.TypeParagraph();

            String docNumber = "1";
            String revisionNumber = "0";

            //Inserta los numeros de pagina
            word.Selection.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            word.ActiveWindow.Selection.Font.Name = "Arial";
            word.ActiveWindow.Selection.Font.Size = 8;
            //word.ActiveWindow.Selection.TypeText("Documento #: " + docNumber + " - Revisión #: " + revisionNumber);
            word.ActiveWindow.Selection.TypeText("");

            //Tabs
            word.ActiveWindow.Selection.TypeText("\t");
            word.ActiveWindow.Selection.TypeText("\t");

            word.ActiveWindow.Selection.TypeText("Página ");
            Object CurrentPage = Word.WdFieldType.wdFieldPage;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            word.ActiveWindow.Selection.TypeText(" de ");
            Object TotalPages = Word.WdFieldType.wdFieldNumPages;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Foco en el documento
            wordDoc.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument;

            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
            {
                Word.Find findObject = word.Selection.Find;
                findObject.ClearFormatting();
                findObject.Text = oImpresionEtiqueta.Etiqueta;
                findObject.Replacement.ClearFormatting();

                if (JEncabezado[oImpresionEtiqueta.Campo] != null)
                {
                    findObject.Replacement.Text = JEncabezado[oImpresionEtiqueta.Campo].ToString();
                }

                object replaceAllC = Word.WdReplace.wdReplaceAll;
                findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref replaceAllC, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            }

            wordDoc.PageSetup.LeftMargin = 50;
            wordDoc.PageSetup.RightMargin = 50;
            //wordDoc.PageSetup.TopMargin = 1;
            //wordDoc.PageSetup.BottomMargin = 20;
            if (pTipoImpresion == 1)
            {
                wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }

            /*** Se guarda como PDF ***/
            Object fileFormat = Word.WdSaveFormat.wdFormatPDF;

            wordDoc.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing,
                           ref oMissing);

            return "0|../Archivos/Impresiones/" + Path.GetFileName(rutaPDF);
        }
        catch (Exception ex)
        {
            return "1|Error " + ex.Message;
        }
        finally
        {
            wordDoc.Close(ref noSavings, ref oMissing, ref oMissing);
            wordDoc = null;
            word.Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public string ReportePDFTemplateConceptosInventario(string rutaPDF, string rutaTemplate, string rutaCSS, string imagenLogo, int IdImpresionTemplate, JObject datos, CConexion pConexion, int pTipoImpresion)
    {
        JArray JAEncabezado = new JArray();
        JAEncabezado = (JArray)datos["Table"];

        JObject JEncabezado = new JObject();
        JEncabezado = (JObject)JAEncabezado.First;

        JArray JAConceptos = new JArray();
        JAConceptos = (JArray)datos["Table1"];

        JArray JAConceptosD = new JArray();
        JAConceptosD = (JArray)datos["Table2"];

        Word.Application word = new Word.Application();
        Word.Document wordDoc = new Word.Document();

        Object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc"; /* \endofdoc es un bookmark predefinido */
        Object noSavings = Word.WdSaveOptions.wdDoNotSaveChanges;

        int ind = 0;

        try
        {
            wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            word.Visible = false;
            Object filepath = (Object)rutaTemplate;
            Object confirmconversion = System.Reflection.Missing.Value;
            Object readOnly = false;
            Object saveto = (Object)rutaPDF;
            Object oallowsubstitution = System.Reflection.Missing.Value;

            wordDoc = word.Documents.Add(ref filepath, ref oMissing, ref oMissing, ref oMissing);
            Word.Range wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            object oPos;
            double dPos = word.InchesToPoints(10);
            wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
            object oPageEnter = Word.WdBreakType.wdLineBreak;

            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdImpresionTemplate", IdImpresionTemplate);
            wordDoc.Activate();

            Object nullobj = Type.Missing;
            Object start = 0;
            Object end = wordDoc.Characters.Count;

            Word.Range rng = wordDoc.Range(ref start, ref end);
            Word.Find fnd = rng.Find;

            fnd.ClearFormatting();

            //fnd.Text = "{IMAGEN_LOGO}";
            //fnd.Forward = true;

            Object linktoFile = false;
            Object SaveWithDoc = true;
            Object replaceOption = Word.WdReplace.wdReplaceOne;
            Object rangeImg = Type.Missing;

            // Ubicamos la posición donde queremos insertar la imagen.
            fnd.Execute(ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj, ref nullobj,
            ref nullobj, ref nullobj, ref replaceOption, ref nullobj,
            ref nullobj, ref nullobj, ref nullobj);

            //Insertamos la imagen en la posicion adecuada
            //var shape = rng.InlineShapes.AddPicture(imagenLogo, ref linktoFile, ref SaveWithDoc, ref rangeImg);

            for (int i = 1; i <= wordDoc.Tables.Count; i++)
            {
                Word.Table wTable = wordDoc.Tables[i];
                Word.Cell pCell = wTable.Cell(1, 1);

                if (wTable.ID == "tblConceptos")
                {

                    for (int j = ind; j < JAConceptos.Count; j++)
                    {

                        wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        wrdRng.ParagraphFormat.SpaceAfter = 6;

                        oPos = wrdRng.get_Information
                                      (Word.WdInformation.wdVerticalPositionRelativeToPage);

                        if (dPos >= Convert.ToDouble(oPos))
                        {
                            Word.Range range = wTable.Range;

                            // Renglon Layout
                            int selectedRow = 3;

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
                            JObject Concepto = (JObject)JAConceptos[ind];

                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                            {
                                Word.Find findConceptos = range.Find;
                                findConceptos.ClearFormatting();
                                findConceptos.Text = oImpresionEtiqueta.Etiqueta;
                                findConceptos.Replacement.ClearFormatting();

                                if (Concepto[oImpresionEtiqueta.Campo] != null)
                                {
                                    findConceptos.Replacement.Text = Concepto[oImpresionEtiqueta.Campo].ToString();
                                }
                                object replaceAll = Word.WdReplace.wdReplaceAll;
                                findConceptos.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            }

                            ind++;
                        }
                        else
                        {
                            object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                            object oPageBreak = Word.WdBreakType.wdPageBreak;
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertBreak(ref oPageBreak);
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertFile(rutaTemplate, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            wrdRng.InsertParagraphAfter();
                        }
                    }
                    wTable.Rows[3].Delete();
                }

                if (wTable.ID == "tblConceptosD")
                {
                    ind = 0;
                    for (int j = ind; j < JAConceptosD.Count; j++)
                    {

                        wrdRng = wordDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        wrdRng.ParagraphFormat.SpaceAfter = 6;

                        oPos = wrdRng.get_Information
                                      (Word.WdInformation.wdVerticalPositionRelativeToPage);

                        if (dPos >= Convert.ToDouble(oPos))
                        {
                            Word.Range range = wTable.Range;

                            // Renglon Layout
                            int selectedRow = 2;

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
                            JObject Concepto = (JObject)JAConceptosD[ind];

                            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
                            {
                                Word.Find findConceptos = range.Find;
                                findConceptos.ClearFormatting();
                                findConceptos.Text = oImpresionEtiqueta.Etiqueta;
                                findConceptos.Replacement.ClearFormatting();

                                if (Concepto[oImpresionEtiqueta.Campo] != null)
                                {
                                    findConceptos.Replacement.Text = Concepto[oImpresionEtiqueta.Campo].ToString();
                                }
                                object replaceAll = Word.WdReplace.wdReplaceAll;
                                findConceptos.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            }

                            ind++;
                        }
                        else
                        {
                            object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
                            object oPageBreak = Word.WdBreakType.wdPageBreak;
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertBreak(ref oPageBreak);
                            wrdRng.Collapse(ref oCollapseEnd);
                            wrdRng.InsertFile(rutaTemplate, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                            wrdRng.InsertParagraphAfter();
                        }
                    }
                    wTable.Rows[2].Delete();
                }
            }

            //Foco en el pie de pagina
            word.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekCurrentPageFooter;

            //Enter y crea parrafo
            word.Selection.TypeParagraph();

            String docNumber = "1";
            String revisionNumber = "0";

            //Inserta los numeros de pagina
            word.Selection.Paragraphs.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            word.ActiveWindow.Selection.Font.Name = "Arial";
            word.ActiveWindow.Selection.Font.Size = 8;
            //word.ActiveWindow.Selection.TypeText("Documento #: " + docNumber + " - Revisión #: " + revisionNumber);
            word.ActiveWindow.Selection.TypeText("");

            //Tabs
            word.ActiveWindow.Selection.TypeText("\t");
            word.ActiveWindow.Selection.TypeText("\t");

            word.ActiveWindow.Selection.TypeText("Página ");
            Object CurrentPage = Word.WdFieldType.wdFieldPage;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref CurrentPage, ref oMissing, ref oMissing);
            word.ActiveWindow.Selection.TypeText(" de ");
            Object TotalPages = Word.WdFieldType.wdFieldNumPages;
            word.ActiveWindow.Selection.Fields.Add(word.Selection.Range, ref TotalPages, ref oMissing, ref oMissing);

            //Foco en el documento
            wordDoc.ActiveWindow.ActivePane.View.SeekView = Word.WdSeekView.wdSeekMainDocument;

            foreach (CImpresionEtiquetas oImpresionEtiqueta in ImpresionEtiquetas.LlenaObjetosFiltros(Parametros, pConexion))
            {
                Word.Find findObject = word.Selection.Find;
                findObject.ClearFormatting();
                findObject.Text = oImpresionEtiqueta.Etiqueta;
                findObject.Replacement.ClearFormatting();

                if (JEncabezado[oImpresionEtiqueta.Campo] != null)
                {
                    findObject.Replacement.Text = JEncabezado[oImpresionEtiqueta.Campo].ToString();
                }

                object replaceAllC = Word.WdReplace.wdReplaceAll;
                findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref replaceAllC, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            }

            wordDoc.PageSetup.LeftMargin = 50;
            wordDoc.PageSetup.RightMargin = 50;
            //wordDoc.PageSetup.TopMargin = 1;
            //wordDoc.PageSetup.BottomMargin = 20;
            if (pTipoImpresion == 1)
            {
                wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            }
            else
            {
                wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }

            /*** Se guarda como PDF ***/
            Object fileFormat = Word.WdSaveFormat.wdFormatPDF;

            wordDoc.SaveAs(ref saveto, ref fileFormat, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                           ref oMissing, ref oMissing, ref oMissing, ref oallowsubstitution, ref oMissing,
                           ref oMissing);

            return "0|../Archivos/Impresiones/" + Path.GetFileName(rutaPDF);
        }
        catch (Exception ex)
        {
            return "1|Error " + ex.Message;
        }
        finally
        {
            wordDoc.Close(ref noSavings, ref oMissing, ref oMissing);
            wordDoc = null;
            word.Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
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

    //conversion de letras////////////////////////////////////////////////////////////////////////////////////
    public string ConvertLetter(string _textNumber, string _currency)
    {
        string Words = string.Empty;
        string Number = string.Empty;
        string auxNumber = string.Empty;
        string decimalPart = string.Empty;
        string integerPart = string.Empty;
        string Fl = string.Empty;
        string Fl_II = string.Empty;
        int numberAlone = -1;

        auxNumber = _textNumber.Replace("$", "").Replace(",", "").Replace("+", "").Trim();

        if (isFloatNumber(auxNumber))
        {

            //-------Si es un número negativo
            if (auxNumber.Substring(0, 1).Equals("-"))
            {
                Words = "MENOS ";
                auxNumber = auxNumber.Substring(1);
            }

            //-------Si tiene ceros a la izquierda

            for (int i = 0; i < auxNumber.Length; i++)
            {
                if (auxNumber.Substring(i, 1).Equals("0"))
                {
                    Number = auxNumber.Substring(i + 1);
                }
                else
                {
                    break;
                }
            }

            if (string.IsNullOrEmpty(Number)) { Number = auxNumber; }

            //-------Separa la parte entera de la decimal 

            string[] arrayNumber = splitString(Number, '.');

            integerPart = arrayNumber[0];

            if (arrayNumber[1].Length > 2)
            {
                decimalPart = arrayNumber[1].Substring(0, 2);
            }
            else if (arrayNumber[1].Length == 2)
            {
                decimalPart = arrayNumber[1];
            }
            else if (arrayNumber[1].Length == 1)
            {
                decimalPart = arrayNumber[1] + "0";
            }

            //-------Proceso de conversión

            if (float.Parse(Number) <= 100000000)
            {
                int sbt = 0;

                if (int.Parse(integerPart) != 0)
                {

                    for (int i = integerPart.Length; i > 0; i--)
                    {

                        numberAlone = int.Parse(integerPart.Substring(sbt, 1));


                        switch (i)
                        {

                            //--------Arma las centenas
                            case 6:
                            case 3:

                                switch (numberAlone)
                                {
                                    case 1:
                                        if (integerPart.Substring(sbt + 1, 1).Equals("0") &&
                                        integerPart.Substring(sbt + 2, 1).Equals("0"))
                                        { Words = Words + "CIEN "; }
                                        else { Words = Words + "CIENTO "; }
                                        break;

                                    case 2:
                                        Words = Words + "DOSCIENTOS ";
                                        break;

                                    case 3:
                                        Words = Words + "TRESCIENTOS ";
                                        break;

                                    case 4:
                                        Words = Words + "CUATROCIENTOS ";
                                        break;

                                    case 5:
                                        Words = Words + "QUINIENTOS ";
                                        break;

                                    case 6:
                                        Words = Words + "SEISCIENTOS ";
                                        break;

                                    case 7:
                                        Words = Words + "SETECIENTOS ";
                                        break;

                                    case 8:
                                        Words = Words + "OCHOCIENTOS ";
                                        break;

                                    case 9:
                                        Words = Words + "NOVECIENTOS ";
                                        break;
                                }

                                break;
                            //--------Arma las decenas
                            case 5:
                            case 2:

                                switch (numberAlone)
                                {
                                    case 1:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "DIEZ "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else if (integerPart.Substring(sbt + 1, 1).Equals("1"))
                                        { Words = Words + "ONCE "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else if (integerPart.Substring(sbt + 1, 1).Equals("2"))
                                        { Words = Words + "DOCE "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else if (integerPart.Substring(sbt + 1, 1).Equals("3"))
                                        { Words = Words + "TRECE "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else if (integerPart.Substring(sbt + 1, 1).Equals("4"))
                                        { Words = Words + "CATORCE "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else if (integerPart.Substring(sbt + 1, 1).Equals("5"))
                                        { Words = Words + "QUINCE "; Fl = "D"; if (i == 2) { Fl_II = "X"; } else { Fl_II = string.Empty; } }
                                        else
                                        { Words = Words + "DIECI"; }

                                        break;

                                    case 2:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "VEINTE "; }
                                        else
                                        { Words = Words + "VEINTI"; }

                                        break;

                                    case 3:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "TREINTA "; }
                                        else
                                        { Words = Words + "TREINTA Y "; }

                                        break;

                                    case 4:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "CUARENTA "; }
                                        else
                                        { Words = Words + "CUARENTA Y "; }

                                        break;

                                    case 5:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "CINCUENTA "; }
                                        else
                                        { Words = Words + "CINCUENTA Y "; }

                                        break;

                                    case 6:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "SESENTA "; }
                                        else
                                        { Words = Words + "SESENTA Y "; }

                                        break;

                                    case 7:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "SETENTA "; }
                                        else
                                        { Words = Words + "SETENTA Y "; }

                                        break;

                                    case 8:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "OCHENTA "; }
                                        else
                                        { Words = Words + "OCHENTA Y "; }

                                        break;

                                    case 9:

                                        if (integerPart.Substring(sbt + 1, 1).Equals("0"))
                                        { Words = Words + "NOVENTA "; }
                                        else
                                        { Words = Words + "NOVENTA Y "; }

                                        break;

                                }

                                break;


                            //--------Arma las unidades
                            case 7:
                            case 4:
                            case 1:

                                switch (numberAlone)
                                {
                                    case 1:

                                        if (!Fl.Equals("D"))
                                        {
                                            if (i == 4)
                                            {
                                                Words = Words + "UN ";
                                            }
                                            else
                                            {
                                                Words = Words + "UN "; //UNO
                                            }
                                        }
                                        else if (Fl.Equals("D") && string.IsNullOrEmpty(Fl_II))
                                        { Words = Words + "UNO "; }

                                        break;

                                    case 2:

                                        if (!Fl.Equals("D"))
                                        {
                                            Words = Words + "DOS ";
                                        }
                                        else if (Fl.Equals("D") && string.IsNullOrEmpty(Fl_II))
                                        { Words = Words + "DOS "; }

                                        break;

                                    case 3:

                                        if (!Fl.Equals("D"))
                                        {
                                            Words = Words + "TRES ";
                                        }
                                        else if (Fl.Equals("D") && string.IsNullOrEmpty(Fl_II))
                                        { Words = Words + "TRES "; }

                                        break;

                                    case 4:

                                        if (!Fl.Equals("D"))
                                        {
                                            Words = Words + "CUATRO ";
                                        }
                                        else if (Fl.Equals("D") && string.IsNullOrEmpty(Fl_II))
                                        { Words = Words + "CUATRO "; }

                                        break;

                                    case 5:

                                        if (!Fl.Equals("D"))
                                        {
                                            Words = Words + "CINCO ";
                                        }
                                        else if (Fl.Equals("D") && string.IsNullOrEmpty(Fl_II))
                                        { Words = Words + "CINCO "; }

                                        break;

                                    case 6:
                                        Words = Words + "SEIS ";
                                        break;

                                    case 7:
                                        Words = Words + "SIETE ";
                                        break;

                                    case 8:
                                        Words = Words + "OCHO ";
                                        break;

                                    case 9:
                                        Words = Words + "NUEVE ";
                                        break;
                                }

                                break;

                        }
                        if (i == 4)
                        {
                            Words = Words + "MIL ";
                        }

                        if (i == 7 && integerPart.Substring(0, 1).Equals("1"))
                        {
                            Words = Words + "MILLÓN ";
                        }
                        else if (i == 7 && !integerPart.Substring(0, 1).Equals("1"))
                        {
                            Words = Words + "MILLONES ";
                        }

                        sbt += 1;
                    }
                }
                else
                {
                    Words = "CERO ";
                }

                //-------Une la parte entera con la decimal y asigna la moneda

                //if (_currency.ToUpper().Trim().Equals("PESOS"))
                //{
                //    Words = Words + " PESOS " + decimalPart + "/100 M.N.";
                //}
                //else
                //{
                //    Words = Words + " USD " + decimalPart + "/100";
                //}
                Words = Words + " " + _currency.ToUpper() + " " + decimalPart + "/100";

            }
            else
            {
                Words = "NÚMERO FUERA DE RANGO [XXXXXXX.XX]";
            }

        }
        else
        {
            Words = "DATO NO NUMÉRICO";
        }

        Words = Words.Replace("ONCE UNO", "ONCE");
        Words = Words.Replace("DOCE DOS", "DOCE");
        Words = Words.Replace("TRECE TRES", "TRECE");
        Words = Words.Replace("CATORCE CUATRO", "CATORCE");
        Words = Words.Replace("QUINCE CINCO", "QUINCE");

        return Words;
    }
	
    //-------El código anterior requiere de las siguientes funciones:
    public bool isFloatNumber(string _numberText)
    {
        float Result = 0;
        bool numberResult = false;

        if (float.TryParse(_numberText, out Result))
        {
            numberResult = true;
        }

        return numberResult;
    }

    public string[] splitString(string _textString, char _character)
    {
        string[] split = null;

        if (!string.IsNullOrEmpty(_textString))
        {
            if (_textString.Contains(_character.ToString()))
            {
                split = _textString.Split(new char[] { _character });

                if (string.IsNullOrEmpty(split[0])) { split[0] = "0"; }

            }
            else
            {
                split = new string[2];
                split[0] = _textString;
                split[1] = "00";
            }
        }

        return split;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void WaitSeconds(double nSecs)
    {
        // Esperar los segundos indicados

        // Crear la cadena para convertir en TimeSpan
        string s = "0.00:00:" + nSecs.ToString().Replace(",", ".");
        TimeSpan ts = TimeSpan.Parse(s);

        // Añadirle la diferencia a la hora actual
        DateTime t1 = DateTime.Now.Add(ts);

        // Esta asignación solo es necesaria
        // si la comprobación se hace al principio del bucle
        DateTime t2 = DateTime.Now;

        // Mientras no haya pasado el tiempo indicado
        while (t2 < t1)
        {
            // Un respiro para el sitema
            //System.Windows.Forms.Application.DoEvents();
            // Asignar la hora actual
            t2 = DateTime.Now;
        }
    }

    public List<string> CortarPalabra(string pCadena, string pEtiqueta, int pNoCaracteres)
    {
        string etiqueta = pEtiqueta;
        string cadena = pCadena;
        int longitudCadena = cadena.Length;
        int cadenaMaxima = pNoCaracteres;
        decimal noVueltas = longitudCadena / cadenaMaxima;
        decimal residuo = longitudCadena % cadenaMaxima;

        int actual = 0;
        List<string> imprimir = new List<string>();
        if (pCadena != "")
        {
            for (int i = 1; i <= noVueltas; i++)
            {
                if (i < noVueltas)
                {
                    imprimir.Add(cadena.Substring(actual, cadenaMaxima) + etiqueta);
                }
                else
                {
                    if (residuo > 0)
                    {
                        imprimir.Add(cadena.Substring(actual, cadenaMaxima) + etiqueta);
                    }
                    else
                    {
                        imprimir.Add(cadena.Substring(actual, cadenaMaxima));
                    }
                }
                actual = actual + cadenaMaxima;
            }
        }
        else
        {
            imprimir.Add("");
        }

        if (residuo > 0)
        {
            imprimir.Add(cadena.Substring(actual, longitudCadena - actual));
        }

        return imprimir;
    }

    public string CeroIzquierda(int numero, int digitos)
    {
        string cantidad = "";

        cantidad = numero.ToString();

        for (int i = digitos; i >= cantidad.Length; i--)
        {
            cantidad = "0" + cantidad;
        }

        return cantidad;
    }

    public string ObtenerMes(int pMes)
    {
        string mes = "";
		pMes = (pMes > 12) ? pMes - 12 : pMes;
        switch (pMes)
        {
            case 1:
                mes = "Enero";
                break;
            case 2:
                mes = "Febrero";
                break;
            case 3:
                mes = "Marzo";
                break;
            case 4:
                mes = "Abril";
                break;
            case 5:
                mes = "Mayo";
                break;
            case 6:
                mes = "Junio";
                break;
            case 7:
                mes = "Julio";
                break;
            case 8:
                mes = "Agosto";
                break;
            case 9:
                mes = "Septiembre";
                break;
            case 10:
                mes = "Octubre";
                break;
            case 11:
                mes = "Noviembre";
                break;
            case 12:
                mes = "Diciembre";
                break;
        }

        return mes;
    }

    private void NAR(object o)
    {
        try
        {
            while (System.Runtime.InteropServices.Marshal.ReleaseComObject(o) > 0) ;
        }
        catch { }
        finally
        {
            o = null;
        }
    }

    public static void DelegarAccion(Action<CConexion, int, string, CUsuario> accion)
    {
        CConexion pConexion = new CConexion();
        string res = pConexion.ConectarBaseDatosSqlServer();
        if (res == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            int IdUsuario = (HttpContext.Current.Session["IdUsuario"] != null && HttpContext.Current.Session["IdUsuario"] != "") ? Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]) : 0;
            Usuario.LlenaObjeto(IdUsuario, pConexion);
            if (Usuario.IdUsuario != 0)
            {
                accion(pConexion, 0, "", Usuario);
            }
            else
            {
                accion(pConexion, 1, "Su sesión a expirado, favor de recargar la página.", Usuario);
            }
        }
        else
        {
            accion(pConexion, 1, res, new CUsuario());
        }
        pConexion.CerrarBaseDatosSqlServer();
    }

    public static string DD(int Numero) {
        return (Numero < 10 && Numero > 0) ? "0" + Numero.ToString() : Numero.ToString();
    }

	public static string TextoArchivo(string Archivo)
	{
		string Texto = "";

		try
		{
			StreamReader Contenido = new StreamReader(Archivo);
			Texto = Contenido.ReadToEnd();
			Contenido.Close();
		}
		catch (Exception Ex)
		{

		}

		return Texto;
	}

	public static void EnviarCorreo(string From, string To, string Subject, string Message)
	{
		MailMessage Mail = new MailMessage();
		Mail.From = new MailAddress(From);
		Mail.To.Add(new MailAddress(To));
		Mail.Subject = Subject;
		Mail.Body = Message;
		Mail.IsBodyHtml = true;
		Mail.Priority = MailPriority.Normal;
		SmtpClient Smtp = new SmtpClient();
		NetworkCredential credenciales = new NetworkCredential("autentificacion@keepmoving.com.mx", "kmt");
		Smtp.Host = "mail.keepmoving.com.mx";
		Smtp.Port = 587;
		Smtp.UseDefaultCredentials = false;
		Smtp.Credentials = credenciales;
		Smtp.Send(Mail);
	}

	public static void EnviarCorreoAdjunto(string From, string To, string Subject, string Message, Attachment Adjunto)
	{
		MailMessage Mail = new MailMessage();
		Mail.From = new MailAddress(From);
		Mail.To.Add(new MailAddress(To));
		Mail.Subject = Subject;
		Mail.Body = Message;
		Mail.IsBodyHtml = true;
		Mail.Attachments.Add(Adjunto);
		Mail.Priority = MailPriority.Normal;
		SmtpClient Smtp = new SmtpClient();
		Smtp.Send(Mail);
	}

	public static JArray ObtenerConsulta (CSelectEspecifico Consulta, CConexion pConexion)
	{
		JArray Registros = new JArray();

		Consulta.Llena(pConexion);

		if (Consulta.Registros.HasRows)
		{
			while (Consulta.Registros.Read())
			{
				JObject Registro = new JObject();
				for (int i = 0; i < Consulta.Registros.FieldCount; i++)
				{
					Registro.Add(Consulta.Registros.GetName(i), Convert.ToString(Consulta.Registros[i]));
				}
				Registros.Add(Registro);
			}
		}

		Consulta.CerrarConsulta();

		return Registros;
	}

	public static DateTime PrimerDiaMes()
	{
		return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
	}

}