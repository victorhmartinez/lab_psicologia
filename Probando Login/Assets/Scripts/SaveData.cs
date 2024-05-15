using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Proyecto26;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private static fsSerializer serializer = new fsSerializer();
    private string url_firebase = "https://quimicalabutpl-default-rtdb.firebaseio.com/";

    public void writeNewUser(string name, string email, string username, string date)
    {
        User user = new User(name, email, username, date);
        RestClient.Post(url_firebase + "lab-quimica-utpl" + "/usuarios" + "/" + username + "/" + name  + "/Registros" + ".json", user).Then(
            response =>
            {
                Debug.Log("usuario registrado con éxito");
            }
        );

    }
}

class User {
    public string nombre;
    public string email;
    public string username;
    public string date;

    public User(string nombre, string email, string username, string date){
        this.nombre = nombre;
        this.email = email;
        this.username = username;
        this.date = date;
    }
}
