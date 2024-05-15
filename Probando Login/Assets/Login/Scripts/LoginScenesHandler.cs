using UnityEngine;
using UnityEngine.SceneManagement;

namespace FirebaseWebGL.Examples.Utils
{
    public class LoginScenesHandler : MonoBehaviour
    {
        public void GoToRegistroDeUsuario() => SceneManager.LoadScene("RegistroDeUsuario");

        public void GoToLogInDeUsuario() => SceneManager.LoadScene("LogInDeUsuario");
    }
}
