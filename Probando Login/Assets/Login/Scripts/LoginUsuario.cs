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

public class LoginUsuario : MonoBehaviour
{
    public TMP_InputField IF_Email_Fb;
    public TMP_InputField IF_Password_Fb;
    public TextMeshProUGUI outputText;

    public Button btnLogin;

    public static string User_Email;
    public static string User_Password;
    public GameObject Panel_Espera;
    public GameObject Panel_Datos_Incorrectos;
    private String UriUtpl = "https://utplvirtopsia.ms2sgroup.com/virtopsia-admin/api/authentication";
    private String authKey = "dKp9FfIjJL85AfuS8aZzHYUxlQw09AHW6EoiE4o7sZds3qFVuwpCxXFegA6AxGZ";

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
        //StartCoroutine(CargarPanelEspera());
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
            Headers = new Dictionary<string, string>{
                {"Authorization", $"{authKey}"}, {"Content-Type", "application/json"}
            },
            BodyString = jsonUser
        }).Then(response =>
        {
            Debug.Log(response.Text);
            var responseJson = response.Text;
            /*
                En la variable data se recibe los datos del usuario para ser tratados de acuerdo a la necesidad
                Esta data puede ser filtrada
            */
            var data = fsJsonParser.Parse(responseJson);
            DataUsers.Instance.GetUsersData(data.AsDictionary["fullName"].ToString().Replace("\"", ""), data.AsDictionary["username"].ToString().Replace("\"", ""), data.AsDictionary["username"].ToString().Replace("\"", "") + "@utpl.edu.ec");
            /*
                Aquí se debe redirigir a la primera escena (SceneManager.LoadScene("SampleLogin");)
            */
            Panel_Espera.SetActive(false);
            SceneManager.LoadScene("SampleLogin");
        }).Catch(err => Panel_Datos_Incorrectos.SetActive(true));
    }


    public void Cerrar_Panel_Datos_Incorrectos () {
        Panel_Datos_Incorrectos.SetActive(false);
        Panel_Espera.SetActive(false);
        btnLogin.GetComponent<Button>().interactable = true;
    }

    public void Timer_Loading()
    {
        gameObject.GetComponent<SaveData>().writeNewUser(DataUsers.Instance.nombre, DataUsers.Instance.email, DataUsers.Instance.username, System.DateTime.Now.ToString("HH:mm:ss; dd MMMM yyyy"));
        SceneManager.LoadScene("SampleScene");
        CargarNivel("SampleScene");
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