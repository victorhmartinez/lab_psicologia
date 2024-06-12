using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BeckInventory : MonoBehaviour
{
   [SerializeField]
    private TMP_InputField resultText;
    [SerializeField]
    private int resultado;
    [SerializeField]
    private int nroCaso;
    [SerializeField]
    private DialogosManager dialogosManager;
    [SerializeField]
    private ApiManager apiManager;
    [SerializeField]
    private GameObject panelBeck, btnAceptar, objectGuia;
    [SerializeField]
    private int [] listResultados;
    [SerializeField]
    [TextArea(5,8)]
    private string notaBeck;
    [SerializeField]
    private TextMeshProUGUI txtNota;
    [SerializeField]
    private GameObject [] uiCuestionarioBeck;

    [SerializeField]
    private TextMeshProUGUI txtError;
    [SerializeField]
    private GameObject panelAlerta;
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public void fnBtnEnviar() {
       
        resultado = int.Parse(resultText.text);
             if (resultado != listResultados[nroCaso - 1])
                {
                    Debug.Log("Los criterios no coinciden, realiza bien el conteo");
            panelAlerta.SetActive(true);
            txtError.text = "Los criterios no coinciden, realiza bien el conteo";
                }
                else
                {
                    Debug.Log("Los puntaje coiniciden");
                     dialogosManager.iniciarFase("Final");
                     panelBeck.SetActive(false);
                    dialogosManager.darFuncionBtnAceptar();

        }
              
        
    }
    public void notaInventarioBecker()
    {
        btnAceptar.SetActive(false);
        objectGuia.SetActive(true);
     StartCoroutine(escribirTexto(notaBeck, txtNota, btnAceptar));
        btnAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAceptar.GetComponent<Button>().onClick.AddListener(() => {
            objectGuia.SetActive(false);
            panelBeck.SetActive(true);


        });
        nroCaso = apiManager.nroCaso;
        for (int i = 0; i < uiCuestionarioBeck.Length; i++)
        {
            uiCuestionarioBeck[i].SetActive(false);
        }
        uiCuestionarioBeck[nroCaso - 1].SetActive(true);
    }

    IEnumerator escribirTexto(string texto, TextMeshProUGUI txt, GameObject btn)
    {
        txt.maxVisibleCharacters = 0;
        txt.text = texto;
        txt.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txt.maxVisibleCharacters++;
            yield return new WaitForSeconds(15f / 500);

        }
        if (btn != null)
        {
            btn.gameObject.SetActive(true);
        }
    }
}
