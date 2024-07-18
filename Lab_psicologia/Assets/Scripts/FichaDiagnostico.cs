using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FichaDiagnostico : MonoBehaviour
{
    public Toggle[] toggles;
    public Toggle[] togglesNo;

    public TMP_InputField resultadoText;
    public Button submitButton;
    [SerializeField]
    private TextMeshProUGUI txtObservacion, lblTitulo;
    [SerializeField]
    private GameObject objectGuia;
    [SerializeField]
    private TextMeshProUGUI txtNota;
    [SerializeField]
    private GameObject btnAceptar;
    [SerializeField]
    private GameObject btnAceptarAlert;
    [SerializeField]
    private GameObject panelFicha;
    [SerializeField]
    private GameObject panelAlerta;
    [SerializeField]
    private DialogosManager dialogosManager;
    [SerializeField]
    [TextArea(3, 2)]
    private string notaTexto;
    public int criteriosObservados;
    public int criteriosObservadosEscritos;
    [SerializeField]
    private int nroCaso;
    [SerializeField]
    private int[] listResultados;
    [SerializeField]
    private ApiManager apiManager;
    [SerializeField]
    private bool[] listaRespuestaObtenidas;
    [SerializeField]
    private bool[] listRespuestaC1, listRespuestaC2;
    [SerializeField]
    private GameObject panelRetroalimentacionFase;
    [SerializeField]
    private Button btnContinuar;
    [SerializeField]
    private AudioClip audioFinFase;
    [SerializeField]
    private AudioSource audioSource;
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
    private AudioClip audioIndicaciones;
    [SerializeField]
    private int contadorIndicaciones;
    [SerializeField]
    private Button btnContinuarFase;

    [SerializeField]
    private Calificacion calificacion;
    [Header("Animaciones")]
    [SerializeField]
    private GameObject panelAnimaciones, camaraAnimacion;
    [SerializeField]
    private TextMeshProUGUI txtAnimaciones;
    private bool estado=true;
    void Start()
    {
        submitButton.onClick.AddListener(SubmitFicha);
    
    }

    public void SubmitFicha()
    {
        
        criteriosObservados = 0;

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn )

            {
                
                criteriosObservados++;
            }
            if (toggles[i].isOn || togglesNo[i].isOn)

            {

                listaRespuestaObtenidas[i] = true;
            }
            else
            {
                listaRespuestaObtenidas[i] = false;
            }
        }

        criteriosObservadosEscritos = int.Parse(resultadoText.text);
        if (criteriosObservados != criteriosObservadosEscritos)
        {
            panelAlerta.SetActive(true);
            txtObservacion.text="Recuerda revisar bien los criterios seleccionados con los ingresados.";
            calificacion.decrementar(calificacion.valorIncorrecto);
        }
        else
        {
            if (listResultados[nroCaso - 1] == criteriosObservadosEscritos)
            {
                if (verificarRespuestasCaso())
                {
                    
                   
                    panelAlerta.SetActive(true);
                    txtObservacion.text = "Felicitaciones, has realizado correctamente el conteo de los criterios del DMS-5";
                    if (estado==true)
                    {
                        calificacion.incrementar(calificacion.valorPregunta);
                        calificacion.incrementarFinal(calificacion.valorPregunta);
                        calificacion.incrementarContador();
                        estado = false;
                    }
                    btnAceptarAlert.GetComponent<Button>().onClick.RemoveAllListeners();
                    btnAceptarAlert.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        panelFicha.SetActive(false);
                        panelRetroalimentacionFase.SetActive(true);
                        lblTitulo.text = "Felicidades, has terminado la fase inicial.";
                        btnContinuar.onClick.RemoveAllListeners();
                        btnContinuar.onClick.AddListener(() =>
                        {
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
                    });
                }
               
               
            }
            else
            {
                panelAlerta.SetActive(true);
                txtObservacion.text = "Los criterios escritos no estan de acuerdo con el caso clinicio revisalos nuevamente.";
            }
            Debug.Log("Estan bien contado los criterios");
         
        }

        // Aqu� puedes agregar l�gica para enviar los datos a un servidor o almacenarlos localmente
    }

    public void notaFichaDiagnostico()
    {
        btnAceptar.SetActive(false);
        nroCaso = apiManager.nroCaso;
        objectGuia.SetActive(true);
        StartCoroutine(escribirTexto(notaTexto, txtNota, btnAceptar));
        audioSource.clip = audioFinFase;
        audioSource.Play();
        escenarioTrabPsicologo.SetActive(true); 
        btnAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAceptar.GetComponent<Button>().onClick.AddListener(()=>{
            objectGuia.SetActive(false);
            panelFicha.SetActive(true);
            escenarioTrabPsicologo.SetActive(false);
            btnAceptar.SetActive(false);
        });
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

    public bool verificarRespuestasCaso()
    {
        bool verificado=false;
        switch (nroCaso)
        {
            case 1:
                verificado= comprobarRespuest(listRespuestaC1);
                break;
            case 2:
                verificado =comprobarRespuest(listRespuestaC2);
                break;
        }

        return verificado;
    }
    public bool comprobarRespuest(bool [] list)
    {
        bool esCorrecta=true;
        for(int i =0; i < list.Length; i++)
        {
            if (list[i] != listaRespuestaObtenidas[i])
            {
                panelAlerta.SetActive(true);
                txtObservacion.text = "Los criterios que seleccionaste no est�n de acuerdo con el caso cl�nico.";
                esCorrecta = false;
                calificacion.decrementar(calificacion.valorIncorrecto);
                break;
            }
        }
        return esCorrecta;
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

            StartCoroutine(esperarAnimaciones());
            btnContinuarFase.gameObject.SetActive(false);
        });
       
    }

    IEnumerator esperarAnimaciones()
    {
        panelAnimaciones.SetActive(true);
        camaraAnimacion.SetActive(true);
        txtAnimaciones.text = "Paciente toca la puerta) \n" +
            "(Terapeuta abre la puerta e invita a pasar a la paciente)";
        yield return new WaitForSeconds(3.0f);
        dialogosManager.iniciarFase("Desarrollo");
        dialogosManager.ubicarPersonajeCentro();
        dialogosManager.darFuncionBtnAceptar();
        camaraAnimacion.SetActive(false);
        panelAnimaciones.SetActive(false);
    }
}



