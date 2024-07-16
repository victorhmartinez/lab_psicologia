using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


public class SaveData : MonoBehaviour
{
    private string url_api = "https://api-lab-psicologia.onrender.com/api";

    public void writeNewUser(string name, string email, string username, string date)
    {
        User user = new User(name, email, username,  date);
        print(user.username);
        print(user.nombre);

        /*  RestClient.Post(url_api + "create-user", user).Then(
              response =>
              {
                  Debug.Log("usuario registrado con éxito");
              }
          );*/

    }
}

class User
{
    public string nombre;
    public string email;
    public string username;
    public string date;

    public User(string nombre, string email, string username, string date)
    {
        this.nombre = nombre;
        this.email = email;
        this.username = username;
        this.date = date;
    }
}
