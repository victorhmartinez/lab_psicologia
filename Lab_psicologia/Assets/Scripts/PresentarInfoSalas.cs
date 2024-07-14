using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PresentarInfoSalas : MonoBehaviour
{
    [SerializeField]
    private GameObject panelInformacion;
    [SerializeField]
    private TextMeshProUGUI txtTituloSala;
    [SerializeField]
    private TextMeshProUGUI txtCuerpoSala;
    [SerializeField]
    private GameObject btnAceptar;
    [SerializeField]
    private GameObject btnComenzar;
    [SerializeField]
    private string titulo;
    [SerializeField]
    [TextArea(3,2)]
    private string descripcion;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject introController;
    [Header("Animaciones")]
    [SerializeField]
    private GameObject panelAnimacion, cmaraAnimacion;
    [SerializeField]
    private TextMeshProUGUI txtAnimaciones;


    // Start is called before the first frame update
    void Start()
    {
        btnAceptar.gameObject.SetActive(false);
        if (gameObject.name == "Entrada3")
        {
            btnComenzar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            btnAceptar.gameObject.SetActive(false);
            Debug.Log("Entro el jugador al trigger");
            panelInformacion.SetActive(true);
            txtTituloSala.text = titulo;
            StartCoroutine(escribirInformacion(descripcion, btnAceptar));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            btnAceptar.gameObject.SetActive(false);
            Debug.Log("Entro el jugador al trigger");
            panelInformacion.SetActive(false);
            StopAllCoroutines();
            
        }
    }
    IEnumerator escribirInformacion(string texto,GameObject button)
    {
        txtCuerpoSala.maxVisibleCharacters = 0;
        txtCuerpoSala.text = texto;
        txtCuerpoSala.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txtCuerpoSala.maxVisibleCharacters++;
            yield return new WaitForSeconds(15f / 500);
        }
        if(gameObject.name== "Entrada3")
        {
            btnComenzar.SetActive(true);
        }
        else
        {
            button.SetActive(true);
        }
        
        
    }
    public void fnBtnEmpezar()
    {
        player.SetActive(false);
       
        panelInformacion.SetActive(false);

        if (gameObject.name == "Entrada3")
        {
            StartCoroutine(ejecutarAnimacion());
        }
        else
        {
            StartCoroutine(esperarIntro());
        }

        
    }
    IEnumerator esperarIntro()
    {
        yield return new WaitForSeconds(0.5f);
        introController.gameObject.SetActive(true);
    }

    IEnumerator ejecutarAnimacion()
    {
        cmaraAnimacion.SetActive(true);
        panelAnimacion.SetActive(true);
        txtAnimaciones.text = "Paciente toca la puerta) \n" +
            "   (Terapeuta abre la puerta e invita a pasar a la paciente)";
        yield return new WaitForSeconds(3.0f);
        cmaraAnimacion.SetActive(false);
        panelAnimacion.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        StartCoroutine(esperarIntro());
    }
}
