using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.Objects;
using Registro.Scripts.Comprobaciones;
using Proyecto26;
using System.Collections.Generic;
using FullSerializer;
using Newtonsoft.Json;

public class LoginUsuario : MonoBehaviour
{
    public TMP_InputField IF_Email_Fb;
    public TMP_InputField IF_Password_Fb;
    public TextMeshProUGUI outputText;

    public Button btnLogin;

    public static string User_Email;
    public static string User_Password;
    public GameObject Panel_Opcion_Escena;
    public GameObject Panel_Espera;
    public GameObject Panel_Datos_Incorrectos;
    private String UriUtpl = "https://campus3d.utpl.edu.ec/virtopsia-admin/api/authentication";

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            IniciarSesion();
        }
    }

    public void IniciarSesion()
    {
        btnLogin.GetComponent<Button>().interactable = false;
        Panel_Espera.SetActive(true);
        User_Email = IF_Email_Fb.text;
        User_Password = IF_Password_Fb.text;
        UserUtpl newUserPost = new UserUtpl(User_Email, User_Password);
        string jsonUser = JsonUtility.ToJson(newUserPost);
        //Debug.Log(JsonUtility.ToJson(newUserPost, true));
        RestClient.Request(new RequestHelper
        {
            Uri = UriUtpl,
            Method = "POST",
            Timeout = 30,
            BodyString = jsonUser
        }).Then(response =>
        {
            Debug.Log(response.Text);
            var responseJson = response.Text;
            /*
                En la variable data se recibe los datos del usuario para ser tratados de acuerdo a la necesidad
                Esta data puede ser filtrada
            */

            var userUtpl = JsonConvert.DeserializeObject<NewUserUtpl>(responseJson);
           
            DataUsers.Instance.SetUsersData(userUtpl.fullName, userUtpl.username,userUtpl.username + "@utpl.edu.ec");
            /*
                Aquí se debe redirigir a la primera escena (SceneManager.LoadScene("SampleLogin");)
            */
            Panel_Opcion_Escena.SetActive(true);
            Panel_Espera.SetActive(false);
        }).Catch(err => { Panel_Datos_Incorrectos.SetActive(true);
            Debug.Log(err);
        }); 
    }


    public void Cerrar_Panel_Datos_Incorrectos () {
        Panel_Datos_Incorrectos.SetActive(false);
        Panel_Espera.SetActive(false);
        btnLogin.GetComponent<Button>().interactable = true;
    }

    public void Timer_Loading()
    {
        gameObject.GetComponent<SaveData>().writeNewUser(DataUsers.Instance.nombre, DataUsers.Instance.email, DataUsers.Instance.username, System.DateTime.Now.ToString("HH:mm:ss; dd MMMM yyyy"));
       // SceneManager.LoadScene("SampleScene");
    }


    public void CargarNivel(String SceneName)
    {
        StartCoroutine(CargarAsync(SceneName));

    }

    public Slider Slider;

    IEnumerator CargarAsync(String SceneName)
    {
        AsyncOperation Operación = SceneManager.LoadSceneAsync(SceneName);
        Operación.allowSceneActivation = false;
        while (!Operación.isDone)
        {
            float Progreso = Mathf.Clamp01(Operación.progress / .9f);
            Debug.Log(Progreso);
            Slider.value = Progreso;
            yield return null;
        }
    }


}

[Serializable]
public class UserUtpl
{
    public string username;
    public string password;

    public UserUtpl(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class NewUserUtpl
{
    public string username {get; set;}
    public string fullName {get; set;}
    
}