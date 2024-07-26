using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;



public class SaveData : MonoBehaviour
{
    private string url_api = "https://api-lab-psicologia.onrender.com/api/";
    [SerializeField]
    private  string username;
    [SerializeField]
    private string idPartida;
    [SerializeField]
    public string fechaIncio;
    public void writeNewUser(string name, string email, string username, string date)
    {
        User user = new User(name, email, username, date);
        print(user.username);
        print(user.nombre_completo);

        RestClient.Post(url_api + "/create-user", user).Then(
              response =>
              {
                  Debug.Log("Usuario registrado con éxito");
                  this.username = username;
              }
          ).Catch(err =>
          {
              Debug.LogError("Error al registrar el usuario: " + err.Message);
          }
          );
    }
    public void updatePartidaUser(string faseCasoEstudio, string fechaModificacion, string partidaCaso)
    {
        Partida avancePartida = new Partida(faseCasoEstudio, fechaModificacion, partidaCaso);
        print(avancePartida.faseCasoEstudio);
        print(avancePartida.partidaCasoUsuario);
       

        RestClient.Put(url_api + "/update-user/"+username, avancePartida).Then(
              response =>
              {
                  // Parseamos la respuesta para obtener el ID de la partida
                  Response jsonResponse = JsonUtility.FromJson<Response>(response.Text);
                  idPartida = jsonResponse.id;
                  Debug.Log("El usuario fue actualizado con su partida con éxito");
                 
              }
          ).Catch(err =>
          {
              Debug.LogError("Error al registrar el usuario: " + err.Message);
          }
          );
    }
    public void updateUserIntentEntry(string fecha, string progreso, double puntaje)
    {
        Intento intentoPartida = new Intento(fechaIncio, progreso, puntaje);
        print(intentoPartida.progreso);
        print(intentoPartida.puntaje);
        RestClient.Put(url_api + "/update-user-intent/" + username+"/"+idPartida, intentoPartida).Then(
          response =>
          {
                  
              Debug.Log("Intento de partida actulizado correctamente");

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
[System.Serializable]
public class Response
{
    public string message;
    public string id;
}