using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class CValidacion
{
    public bool ValidarCorreo(string pCorreo)
    {
        bool invalido = false;
        string[] caracteresInvalidos = { "|", "°", "!", "¡", "#", "{", "}", "[", "]", "^", "`", "´", "¨", "+", "*", "~", "¿", "?", "(", ")", "/", "\\", ",", "=", "&", "<", ">", "%", "\"", "$", ";", ",", ":", "ñ" };
        foreach (string caracter in caracteresInvalidos)
        {
            invalido = pCorreo.Contains(caracter);
            if (invalido == true)
            { break; }
        }
        if (invalido == false)
        {
            Regex patronCorreo = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            if (!patronCorreo.IsMatch(pCorreo))
            { return true; }
            else
            { return false; }
        }
        else
        { return true; }
    }

    public string LimpiarRFC(string pRFC)
    {
        string resultRFC = "";
        //resultRFC = pRFC.Replace(@"/([.*+?^=!:${}()|\[\]\/\\-]", "");
        resultRFC = Regex.Replace(pRFC, @"[.*+?^=!:${}()|\[\]\/\\-]", String.Empty);
        return resultRFC;
    }
}