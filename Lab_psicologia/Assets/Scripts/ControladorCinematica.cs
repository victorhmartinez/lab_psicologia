using UnityEngine;
using UnityEngine.UI;

public class ControladorCinematica : MonoBehaviour
{
    [Header("Animaciones de recorrido")]
    [SerializeField] private Animator recorridoController;
    [SerializeField] private string nombreBoolRecorrido = "recorrer";
    [SerializeField] private string nombreEstadoRecorrido = "Recorrido"; // Estado de la animaci�n en el Animator
    [Header("C�maras")]
    [SerializeField] private Camera camaraRecorrido;
    [SerializeField] private GameObject player;
    [Header("Puertas para abrir cerrar")]
    [SerializeField] private GameObject[] listPuertas;
    [Header("Panel de indicaciones")]
    [SerializeField] private GameObject panelIndicacion;
    private bool isSkipping = false;
    [Header("Inidicaciones juego")]
    [SerializeField] private GameObject btnContinuar;
    [SerializeField] private Button btnSaltar;
    [SerializeField] private GameObject panelInstruccioneJuego;
    [SerializeField] private AudioClip audioRecorrido;
    [SerializeField] private AudioSource audioSorce;

    void Start()
    {
        panelInstruccioneJuego.SetActive(true);
        btnContinuar.GetComponent<Button>().onClick.AddListener(() =>
        {
            darFuncionalidaBotonSC();
        });
        btnSaltar.GetComponent<Button>().onClick.AddListener(() =>
        {
            darFuncionalidaBotonSC();
        });


    }

    void Update()
    {
        if (recorridoController.GetBool(nombreBoolRecorrido))
        {
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FinalizarCinematica();
                Debug.Log("Cinem�tica saltada");
            }
        }

        // Verifica si la animaci�n ha terminado
        if (!isSkipping && IsAnimationFinished())
        {
            FinalizarCinematica();
            Debug.Log("Cinem�tica finalizada");
        }
    }

    // M�todo para verificar si la animaci�n ha terminado
    private bool IsAnimationFinished()
    {
        var animStateInfo = recorridoController.GetCurrentAnimatorStateInfo(0);
        return animStateInfo.IsName(nombreEstadoRecorrido) && animStateInfo.normalizedTime >= 1.0f;
    }

    // M�todo para finalizar la cinem�tica
    private void FinalizarCinematica()
    {
        audioSorce.Stop();
        isSkipping = true;
        recorridoController.SetBool(nombreBoolRecorrido, false); // Detener la animaci�n
        player.SetActive(true);
        abrirPuertas(true);
        camaraRecorrido.gameObject.SetActive(false);
        Debug.Log("Cinem�tica finalizada");
        panelIndicacion.SetActive(false);

    }


    public void darFuncionalidaBotonSC()
    {
        panelInstruccioneJuego.SetActive(false);
        audioSorce.clip = audioRecorrido;
        audioSorce.Play();
        panelIndicacion.SetActive(true);
        player.SetActive(false);
        abrirPuertas(false);
        recorridoController.SetBool(nombreBoolRecorrido, true); // Inicia la animaci�n
    }

    // M�todo para abrir o cerrar puertas
    void abrirPuertas(bool estado)
    {
        foreach (GameObject puerta in listPuertas)
        {
            puerta.SetActive(estado);
        }
    }
}
