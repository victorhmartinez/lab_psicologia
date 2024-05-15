using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;
using UnityEngine;
using Registro.Scripts.Comprobaciones;
using System.Collections.Generic;
using System.Linq;

namespace FirebaseWebGL.Examples.Auth
{
    public class AuthHandler : MonoBehaviour
    {
        public TMP_InputField emailInputField;
        public TMP_InputField passwordInputField;

        public GameObject Panel_Opcion_Escena;

        public TextMeshProUGUI outputText;
        public string username;

        private void Start()
        {
          
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                Comprobaciones.DisplayError(outputText, "The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            }

            FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo1");

        }


        public void CreateUserWithEmailAndPassword() =>
            FirebaseAuth.CreateUserWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");            

        public void SignInWithEmailAndPassword() =>
            FirebaseAuth.SignInWithEmailAndPassword(emailInputField.text, passwordInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void SignOut() =>
            FirebaseAuth.SignOut(gameObject.name, "DisplayInfo1", "DisplayErrorObject");

        public void SignInWithGoogle() =>
            FirebaseAuth.SignInWithGoogle(gameObject.name, "DisplayInfo", "DisplayErrorObject");
        
        public void SignInWithFacebook() =>
            FirebaseAuth.SignInWithFacebook(gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void SignInWithMicrosoft() =>
            FirebaseAuth.SignInWithMicrosoft(gameObject.name, "DisplayInfo", "DisplayErrorObject");

        public void OnAuthStateChanged()=>
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo", "DisplayInfo1");             

        public void DisplayUserInfo(string user)
        {
            var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
            DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}");
            DisplayUid(parsedUser.uid);
            Debug.Log($"Email: {parsedUser.email}, UserId: {parsedUser.displayName}");
            List<string> list = new List<string>();
            list = parsedUser.email.Split('@').ToList();
            username = list.First();
            DataUsers.Instance.GetUsersData(parsedUser.displayName, username, parsedUser.email);
            ActivarPanel();
        }

        public void DisplayData(string data)
        {
            outputText.text = "";
            Debug.Log(data);
        }

        public void DisplayUid(string data)
        {
            Debug.Log(data);
        }

        public void DisplayInfo1(string info)
        {
            Debug.Log(info);
        }

        public void DisplayInfo(string info)
        {
            outputText.text = "";
            Debug.Log(info);
            ActivarPanel();
        }

        public void DisplayErrorObject(string error)
        {
            var parsedError = StringSerializationAPI.Deserialize(typeof(FirebaseError), error) as FirebaseError;
            Comprobaciones.DisplayError(outputText, parsedError.message);
        }

        public void ActivarPanel()
        {
            Panel_Opcion_Escena.SetActive(true);         
        }      
    }
}