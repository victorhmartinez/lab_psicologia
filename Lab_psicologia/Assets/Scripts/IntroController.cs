using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroController : MonoBehaviour
{
   
    [SerializeField]
    private List<string> listIntros = new List<string>();
    [SerializeField]
    private TextMeshProUGUI txtIntroduccion;
    [SerializeField]
    private GameObject panelIntroduccion;
    [SerializeField]
    private ApiManager api; // Referencia al ApiManager
    [SerializeField]
    private DialogosManager dialogosManager; // Referencia al ApiManager
    [SerializeField]
    private GameObject btnAceptar;
    [SerializeField]

    void Start()
    {
         btnAceptar.SetActive(false);
        if (api != null)
        {
            Debug.Log("Suscribiendo al evento NumeroAleatorioGeneradoEvent.");
            api.NumeroAleatorioGeneradoEvent += OnNumeroAleatorioGenerado;
        }
        else
        {
            Debug.LogError("ApiManager no asignado en el Inspector.");
        }
    }

    private void OnDestroy()
    {
        if (api != null)
        {
            api.NumeroAleatorioGeneradoEvent -= OnNumeroAleatorioGenerado;
        }
    }

    private void OnNumeroAleatorioGenerado(int numeroAleatorio)
    {
        Debug.Log("Número aleatorio recibido en IntroController: " + numeroAleatorio);
        StartCoroutine(escribirIntro(listIntros[numeroAleatorio - 1]));
    }

    IEnumerator escribirIntro(string texto)
    {
        txtIntroduccion.maxVisibleCharacters = 0;
        txtIntroduccion.text = texto;
        txtIntroduccion.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txtIntroduccion.maxVisibleCharacters++;
            yield return new WaitForSeconds(15f / 500);
        }
        btnAceptar.SetActive(true);
        btnAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAceptar.GetComponent<Button>().onClick.AddListener(() =>
        {
            dialogosManager.darFuncionBtnAceptar();
        });
    }

}
