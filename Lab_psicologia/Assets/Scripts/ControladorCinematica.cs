using UnityEngine;

public class ControladorCinematica : MonoBehaviour
{
    [Header("Animaciones de recorrido")]
    [SerializeField] private Animator recorridoController;
    [SerializeField] private string nombreBoolRecorrido = "recorrer";
    [SerializeField] private string nombreEstadoRecorrido = "Recorrido"; // Estado de la animación en el Animator
    [Header("Cámaras")]
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
        recorridoController.SetBool(nombreBoolRecorrido, true); // Inicia la animación
    }

    void Update()
    {
        if (recorridoController.GetBool(nombreBoolRecorrido))
        {
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                FinalizarCinematica();
                Debug.Log("Cinemática saltada");
            }
        }

        // Verifica si la animación ha terminado
        if (!isSkipping && IsAnimationFinished())
        {
            FinalizarCinematica();
            Debug.Log("Cinemática finalizada");
        }
    }

    // Método para verificar si la animación ha terminado
    private bool IsAnimationFinished()
    {
        var animStateInfo = recorridoController.GetCurrentAnimatorStateInfo(0);
        return animStateInfo.IsName(nombreEstadoRecorrido) && animStateInfo.normalizedTime >= 1.0f;
    }

    // Método para finalizar la cinemática
    private void FinalizarCinematica()
    {
        isSkipping = true;
        recorridoController.SetBool(nombreBoolRecorrido, false); // Detener la animación
        player.SetActive(true);
        abrirPuertas(true);
        camaraRecorrido.enabled = false;
        Debug.Log("Cinemática finalizada");
        panelIndicacion.SetActive(false);

    }




    // Método para abrir o cerrar puertas
    void abrirPuertas(bool estado)
    {
        foreach (GameObject puerta in listPuertas)
        {
            puerta.SetActive(estado);
        }
    }
}
