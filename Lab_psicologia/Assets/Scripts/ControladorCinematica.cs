using UnityEngine;

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

    void Start()
    {
        panelIndicacion.SetActive(true);
        player.SetActive(false);
        abrirPuertas(false);
        recorridoController.SetBool(nombreBoolRecorrido, true); // Inicia la animaci�n
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
        isSkipping = true;
        recorridoController.SetBool(nombreBoolRecorrido, false); // Detener la animaci�n
        player.SetActive(true);
        abrirPuertas(true);
        camaraRecorrido.enabled = false;
        Debug.Log("Cinem�tica finalizada");
        panelIndicacion.SetActive(false);

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
