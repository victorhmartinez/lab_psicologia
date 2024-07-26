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
    [SerializeField]
    private GameObject PerAbriPuerta;
    [SerializeField]
    private AnimationClip animAbrir;
    [SerializeField]
    private Animator animTerapeuta;
    [SerializeField]
    private GameObject[] abriendoPuerta;
    [SerializeField] private AudioClip audioPuerta;
    [SerializeField] private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        
        btnAceptar.gameObject.SetActive(false);
        if (gameObject.name == "Entrada3")
        {
            btnComenzar.SetActive(false);
            PerAbriPuerta.SetActive(false);
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
        audioSource.clip = audioPuerta;
        audioSource.Play();
        txtAnimaciones.text = "Paciente toca la puerta) \n" +
              "   (Terapeuta abre la puerta e invita a pasar a la paciente)";
        PerAbriPuerta.SetActive(true);
        cmaraAnimacion.SetActive(true);
        panelAnimacion.SetActive(true);
        animTerapeuta.SetBool("abrir", true);
        yield return new WaitForSeconds(animAbrir.length/2);
        animTerapeuta.SetBool("abrir", false);
        abriendoPuerta[0].SetActive(false);
        abriendoPuerta[1].SetActive(true);
       
        yield return new WaitForSeconds(3.2f);
        abriendoPuerta[1].SetActive(false);
        abriendoPuerta[0].SetActive(true);
        cmaraAnimacion.SetActive(false);
        panelAnimacion.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        PerAbriPuerta.SetActive(false);
        StartCoroutine(esperarIntro());
    }
}
