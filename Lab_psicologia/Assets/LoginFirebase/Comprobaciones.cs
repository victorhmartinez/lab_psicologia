using System;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Registro.Scripts.Comprobaciones
{
    public static class Comprobaciones
    {
        public static Boolean email_bien_escrito(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean email_no_utpl(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@(utpl\\.edu\\.ec)";
            if (Regex.IsMatch(email, expresion))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Boolean datos_registro(String name, String email, String age, String city, String password, String password_conf)
        {
            if(name != "" && email_bien_escrito(email) == true && age != "" && city != "" && password != "" && password_conf != "" && password == password_conf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void DisplayError(TextMeshProUGUI outputText, string error)
        {
            if (error.Contains("An account already exists with the same email address but different sign-in credentials."))
            {
                error = "Ya existe una cuenta creada con el mismo correo pero diferente proveedor. Ingrese con el proveedor que corresponda";
            }
            else if (error.Contains("The popup has been closed by the user before finalizing the operation."))
            {
                error = "Se cerró la ventana antes de finalizar la operación";
            }

            outputText.color = Color.red;
            outputText.text = error;
            Debug.LogError(error);
        }

    }
}