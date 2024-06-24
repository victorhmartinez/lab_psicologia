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
    private GameObject panelAlerta, btnAceptarAlert, panelRetroalimentacionFase;
    [SerializeField]
    private Button btnContinuar;
    [SerializeField]
    private TextMeshProUGUI lblTitulo;

    [SerializeField]
    private Calificacion calificacion;
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
                    calificacion.decrementar(calificacion.valorIncorrecto);
             }
             else
                {
            calificacion.incrementar(calificacion.valorPregunta);
            calificacion.incrementarFinal(calificacion.valorPregunta);
            Debug.Log("Los puntaje coiniciden");
            panelAlerta.SetActive(true);
            txtError.text = "Felicitaciones, has realizado correctamente el conteo de los puntajes de los puntos del inventario de beck";

            btnAceptarAlert.GetComponent<Button>().onClick.RemoveAllListeners();
            btnAceptarAlert.GetComponent<Button>().onClick.AddListener(() =>
            {
                panelBeck.SetActive(false);
                panelRetroalimentacionFase.SetActive(true);
                lblTitulo.text = "Felicidades, has terminado la fase de desarrollo.";
                btnContinuar.onClick.RemoveAllListeners();
                btnContinuar.onClick.AddListener(() =>
                {
                    panelRetroalimentacionFase.SetActive(false);
                    dialogosManager.iniciarFase("Final");
                    dialogosManager.darFuncionBtnAceptar();

                });
            });
            
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
