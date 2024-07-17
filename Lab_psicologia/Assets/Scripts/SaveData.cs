using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;


public class SaveData : MonoBehaviour
{
    private string url_api = "http://localhost:3000/api";

    public void writeNewUser(string name, string email, string username, string date)
    {
        User user = new User(name, email, username, date);
        print(user.username);
        print(user.nombre_completo);

        RestClient.Post(url_api + "/create-user", user).Then(
              response =>
              {
                  Debug.Log("Usuario registrado con éxito");
              }
          ).Catch(err =>
          {
              Debug.LogError("Error al registrar el usuario: " + err.Message);
          }
          );
    }
}

class User
{
    public string nombre_completo;
    public string email;
    public string username;
    public string date;

    public User(string nombre_completo, string email, string username, string date)
    {
        this.nombre_completo = nombre_completo;
        this.email = email;
        this.username = username;
        this.date = date;
    }
}
