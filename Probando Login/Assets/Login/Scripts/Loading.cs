using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

	public void CargarNivel(int SceneNumber)
    {
        StartCoroutine(CargarAsync(SceneNumber));
    
    }

    IEnumerator CargarAsync(int SceneNumber)
    {
        AsyncOperation Operación = SceneManager.LoadSceneAsync(SceneNumber);

        while (!Operación.isDone)
        {
            float Progreso = Mathf.Clamp01(Operación.progress / .9f);
            Debug.Log(Operación.progress);

            yield return null;
        }
    }

}