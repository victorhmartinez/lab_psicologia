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
    private AudioClip audioNotaBeck;
    [SerializeField]
    private Calificacion calificacion;
    private bool estado = true;
    [Header("Escenario Trabajo")]
    [SerializeField]
    private GameObject escenarioTrabPsicologo;
    [Header("Propiedades de indicacacion de tiempo")]
    [SerializeField]
    private GameObject panelIndicacionTiempo;
    [SerializeField]
    private TextMeshProUGUI txtIndicaciones;
    [SerializeField]
    [TextArea(4, 2)]
    private string [] indicacionesSesion;
    [SerializeField]
    private Button btnContinuarFase;
    [SerializeField]
    private AudioClip audioIndicaciones;
    [SerializeField]
    private AudioSource audioSource;
    [Header("Finalizar el caso")]
    [SerializeField]
    private FinalizarCaso fnCaso;
    [Header("Animaciones")]
    [SerializeField]
    private GameObject panelAnimaciones, camaraAnimacion;
    [SerializeField]
    private TextMeshProUGUI txtAnimaciones;
    [SerializeField]
    private GameObject PerAbriPuerta;
    [SerializeField]
    private AnimationClip animAbrir;
    [SerializeField]
    private Animator animTerapeuta;
    [SerializeField]
    private GameObject[] abriendoPuerta;
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
            if (estado == true)
            {
                calificacion.incrementar(calificacion.valorPregunta);
                calificacion.incrementarFinal(calificacion.valorPregunta);
                calificacion.incrementarContador();
                estado = false;
            }
            Debug.Log("Los puntaje coiniciden");
            panelAlerta.SetActive(true);
            txtError.text = "Felicitaciones, has realizado correctamente el conteo de los puntajes de los puntos del inventario de beck";

            btnAceptarAlert.GetComponent<Button>().onClick.RemoveAllListeners();
            btnAceptarAlert.GetComponent<Button>().onClick.AddListener(() =>
            {
                panelBeck.SetActive(false);
                escenarioTrabPsicologo.SetActive(true);
                fnCaso.activarPreguntaBeck();

                    panelRetroalimentacionFase.SetActive(false);
                    panelIndicacionTiempo.SetActive(true);
                    StopAllCoroutines();
                    StartCoroutine(escribirTexto(indicacionesSesion[0], txtIndicaciones, btnContinuarFase.gameObject));
                    estado = true;
                    btnContinuarFase.onClick.RemoveAllListeners();
                    btnContinuarFase.onClick.AddListener(() =>
                    {
                        funcionBtnContinuar();
                    });

                });
            
             }
              
        
    }
    public void notaInventarioBecker()
    {
        btnAceptar.SetActive(false);
        objectGuia.SetActive(true);
       
     StartCoroutine(escribirTexto(notaBeck, txtNota, btnAceptar));

        escenarioTrabPsicologo.SetActive(true);
        audioSource.clip = audioNotaBeck;
        audioSource.Play();
        btnAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAceptar.GetComponent<Button>().onClick.AddListener(() => {
            objectGuia.SetActive(false);
            panelBeck.SetActive(true);
            escenarioTrabPsicologo.SetActive(false);

          
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
            yield return new WaitForSeconds(35f / 500);

        }
        if (btn != null)
        {
            btn.gameObject.SetActive(true);
        }
    }

    public void funcionBtnContinuar()
    {
        btnContinuarFase.gameObject.SetActive(false);
        txtIndicaciones.text = "";
        StopAllCoroutines();
        StartCoroutine(escribirTexto(indicacionesSesion[1], txtIndicaciones, btnContinuarFase.gameObject));
        audioSource.clip = audioIndicaciones;
        audioSource.Play();
       
        btnContinuarFase.onClick.RemoveAllListeners();
        btnContinuarFase.onClick.AddListener(() =>
        {
            panelIndicacionTiempo.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(esperarAnimaciones());
          

        });
    }
    public void continuarSesion()
    {
        escenarioTrabPsicologo.SetActive(false);
        panelRetroalimentacionFase.SetActive(true);
        lblTitulo.text = "Felicidades, has terminado la fase de desarrollo.";
        btnContinuar.onClick.RemoveAllListeners();
        btnContinuar.onClick.AddListener(() =>
        {


            panelRetroalimentacionFase.SetActive(false);
            panelIndicacionTiempo.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(escribirTexto(indicacionesSesion[0], txtIndicaciones, btnContinuarFase.gameObject));

            btnContinuarFase.onClick.RemoveAllListeners();
            btnContinuarFase.onClick.AddListener(() =>
            {
                funcionBtnContinuar();
            });

        });

    }
    IEnumerator esperarAnimaciones()
    {
        txtAnimaciones.text = "Paciente toca la puerta) \n" +
           "(Terapeuta abre la puerta e invita a pasar a la paciente)";
        PerAbriPuerta.SetActive(true);
        panelAnimaciones.SetActive(true);
        camaraAnimacion.SetActive(true);

        animTerapeuta.SetBool("abrir", true);
        yield return new WaitForSeconds(animAbrir.length / 2);
        animTerapeuta.SetBool("abrir", false);
        abriendoPuerta[0].SetActive(false);
        abriendoPuerta[1].SetActive(true);

        yield return new WaitForSeconds(3.0f);
        dialogosManager.iniciarFase("Final");
        dialogosManager.ubicarPersonajeCentro();
        dialogosManager.darFuncionBtnAceptar();
        camaraAnimacion.SetActive(false);
        panelAnimaciones.SetActive(false);
    }

}
