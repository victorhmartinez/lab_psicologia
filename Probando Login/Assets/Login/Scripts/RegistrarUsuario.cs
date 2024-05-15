using System.Collections;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Proyecto26;
using System;
using TMPro;
using Registro.Scripts.Comprobaciones;
using FirebaseWebGL.Scripts.FirebaseBridge;


public class RegistrarUsuario : MonoBehaviour
{
    public TMP_InputField IF_Name_Fb;
    public TMP_InputField IF_Email_Fb;
    public TMP_InputField IF_Password_Fb;
    public TMP_InputField IF_Password_Conf_Fb;
    public TMP_InputField IF_Age_Fb;
    public TMP_InputField IF_City_Fb;

    public static string User_Name;
    public static string User_Email;
    public static string User_Password;
    public static string User_Password_Conf;
    public static string User_Age;
    public static string User_City;
    public static string User_Start_Time;
    public static string User_Finish_Time;

    public GameObject Loading_Background;
    public GameObject Panel_Ingrese_Datos_Completos;
    public GameObject Panel_Ingrese_Con_Microsoft;
    public GameObject Panel_Usuario_Creado_Exitosamente;
    public GameObject Panel_Opcion_Escena;
    

    public void Continue()
    {
        User_Name = IF_Name_Fb.text;
        User_Email = IF_Email_Fb.text;
        User_Password = IF_Password_Fb.text;
        User_Password_Conf = IF_Password_Conf_Fb.text;
        User_Age = IF_Age_Fb.text;
        User_City = IF_City_Fb.text;

        if (Comprobaciones.datos_registro(User_Name, User_Email, User_Age, User_City, User_Password, User_Password_Conf) == true)
        {
            if (Comprobaciones.email_no_utpl(User_Email) == true)
            {
                FirebaseAuth.CreateUserWithEmailAndPassword(User_Email, User_Password, gameObject.name, "CreationSuccess", "DisplayErrorObject");
            }
            else
            {
                Panel_Ingrese_Con_Microsoft.SetActive(true);
            }
        } else
        {
            Panel_Ingrese_Datos_Completos.SetActive(true);
        }
    }

    public void CreationSuccess()
    {
        StartCoroutine("Success");    
    }

    IEnumerator Success()
    {
        Panel_Usuario_Creado_Exitosamente.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("LogInDeUSuario");
    }

    public void Send_User()
    {
        //Debug.Log("Has realizado click en botón salir");
        DateTime FinishDate = DateTime.Now;
        User_Finish_Time = FinishDate.ToString();
    }

    public void Cerrar_Panel_Datos()
    {
        Panel_Ingrese_Datos_Completos.SetActive(false);
        Panel_Ingrese_Con_Microsoft.SetActive(false);
    }

    public void Timer_Loading()
    {
        Loading_Background.SetActive(true);
        Panel_Opcion_Escena.SetActive(false);
        CargarNivel(1);
    }


    public void CargarNivel(int SceneNumber)
    {
        StartCoroutine(CargarAsync(SceneNumber));

    }

    public Slider Slider;

    IEnumerator CargarAsync(int SceneNumber)
    {
        AsyncOperation Operación = SceneManager.LoadSceneAsync(SceneNumber);

        while (!Operación.isDone)
        {
            float Progreso = Mathf.Clamp01(Operación.progress / .9f);
            Debug.Log(Progreso);
            Slider.value = Progreso;

            yield return null;
        }
    }

    //Codigo para cambiar de escena y activar el tutorial
    public void Cambiar_Scena_Tutorial()
    {
        Loading_Background.SetActive(true);
        Panel_Opcion_Escena.SetActive(false);
        CargarNivel(4);
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        Debug.LogError(error);
    }
}
