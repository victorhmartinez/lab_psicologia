using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogosManager : MonoBehaviour

{
    [SerializeField]
    private GameObject uiDialogo;
    [SerializeField]
    private GameObject uiPreguntas;
  
    [SerializeField]
    private TextMeshProUGUI txtMensaje;
    [SerializeField]
    private TextMeshProUGUI txtPersonaje;
    [SerializeField]
    private TextMeshProUGUI txtPregunta;
    private List<Button> listButtons = new List<Button>();
    public int contador = 0;

    [SerializeField]
    private GameObject btn_Siguiente;
    [SerializeField]
    private Button btn_aceptar;
    [SerializeField]
    private Transform container_preguntas;
    [SerializeField]
    private Button btn_prefab;
    [SerializeField]
    private GameObject ui_retroalimentacion;
    [SerializeField]
    private TextMeshProUGUI txtRetroalimentacion;
    public List<Dialogos> dialogosList = new List<Dialogos>();
    private List<Dialogos> dialogosListDesarrollo = new List<Dialogos>();
    private List<Dialogos> dialogosListFin = new List<Dialogos>();
    [SerializeField]
    private ApiManager apiManager;
    [SerializeField]
    private FichaDiagnostico fichaDiagnostico;
    [SerializeField]
    private BeckInventory inventarioBeck;
    public string fase;
    [SerializeField]
    private GameObject dialagoPsicologo;
    [SerializeField]
    private GameObject dialagoPaciente;
    [SerializeField]
    private TextMeshProUGUI txtDialogoPsiscologo, txtDialogoPaciente;
    [SerializeField]
    private Button btnSigPaciente;
    [SerializeField]
    private Animator animDoctor,animPaciente;
    [SerializeField]
    private AudioSource audioSource;
  
    Preguntas pregunta;
    [SerializeField]
    private ManejadorCamara manejadorCamara;

    private double caliPorIncorrecto=0;
    [SerializeField]
    private Calificacion calificacion;
    private bool parado = true;
    [Header("Label nombres personajes")]
    [SerializeField]
    private TextMeshProUGUI txtNombrePaciente;
    [SerializeField]
    private TextMeshProUGUI txtNombrePsicologo;
    [Header("Ubicaciones personajes no se muevan")]
    [SerializeField]
    private Transform ubiSerntadoPsicologo;
    [SerializeField]
    private Transform ubiSerntadoPaciente;
    [SerializeField]
    private GameObject gameObjectPiscolog;
    [SerializeField]
    private GameObject gameObjectPaciente;
    [Header("Cambios escena")]
    [SerializeField]
    private FinalizarCaso finalizarCaso;

    void Start()
    {
        if (apiManager != null)
        {
            apiManager.DialogosCargadosEvent += OnDialogosInicialCargados;
            apiManager.DialogosCargadosDesarrolladoEvent += OnDialogosDesarrolloCargados;
            apiManager.DialogosCargadosFinalEvent += OnDialogosFinCargados;
        }
        dialagoPaciente.SetActive(false);
        dialagoPsicologo.SetActive(false);
    }

    private void OnDestroy()
    {
        if (apiManager != null)
        {
            apiManager.DialogosCargadosEvent -= OnDialogosInicialCargados;
            apiManager.DialogosCargadosDesarrolladoEvent -= OnDialogosDesarrolloCargados;
            apiManager.DialogosCargadosFinalEvent -= OnDialogosFinCargados;
        }
    }

    // M�todo llamado cuando se cargan los di�logos desde ApiManager
    private void OnDialogosInicialCargados(List<Dialogos> dialogos)
    {
        // Guardar los di�logos cargados
        dialogosList = dialogos;
        Debug.Log("Hola estas en el evento con las listas cargadas");
     
    }
    private void OnDialogosDesarrolloCargados(List<Dialogos> dialogos)
    {
        // Guardar los di�logos cargados
        dialogosListDesarrollo = dialogos;
        Debug.Log("Hola estas en el evento con las listas de desarrollo");

    }
    private void OnDialogosFinCargados(List<Dialogos> dialogos)
    {
        // Guardar los di�logos cargados
        dialogosListFin = dialogos;
        Debug.Log("Hola estas en el evento con las listas de fnal");

    }
    public void iniciarFase(string fase)
    {
        switch(fase) {
            case "Desarrollo":
                this.fase = fase;
                contador = 0;
                dialogosList = dialogosListDesarrollo;
                break;
            case "Final":
                this.fase = fase;
                contador = 0;
                dialogosList = dialogosListFin;
                break;

        }
    }

    IEnumerator escribirTexto(string texto, TextMeshProUGUI txt,GameObject btn)
    {
        txt.maxVisibleCharacters = 0;
        txt.text = texto;
        txt.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txt.maxVisibleCharacters++;
            yield return new WaitForSeconds(25f / 500);

        }
       
        if (contador<dialogosList.Count) {
            if (txt.gameObject.name != "txt_retroalimentacion")
            {
                yield return new WaitForSeconds(3f);
                if (dialogosList[contador].esImportante)
                {
                    btn.gameObject.SetActive(true);
                }
                else
                {
                    funcionalidadBtnSiguiente();
                }


            }
            else if (btn != null)
            {
                btn.gameObject.SetActive(true);
            }
        }
      
       
    }

    public void funcionalidadBtnSiguiente()
    {
        btnSigPaciente.gameObject.SetActive(false);
       
        Debug.Log("Voy reutilizar");
        if (contador<dialogosList.Count)
        {
            if (dialogosList[contador].tienePregunta)
            {
                dialagoPaciente.SetActive(false);
                dialagoPsicologo.SetActive(false);

                pregunta = dialogosList[contador].pregunta;
                cargarPreguntas(pregunta);
            }
            else
            {
                contador++;
                if (contador < dialogosList.Count)
                {
                    buscarPersonaje(dialogosList[contador].personaje);
                    txtPersonaje.text = dialogosList[contador].personaje;
                    llamarUiDialogos();

                }
                else
                {
                    if (fase == "Desarrollo")
                    {
                        Debug.Log("entre aqui");
                        txtDialogoPaciente.gameObject.SetActive(false);
                        txtDialogoPsiscologo.gameObject.SetActive(false);
                        dialagoPsicologo.SetActive(false);
                        dialagoPaciente.SetActive(false);
                        uiDialogo.SetActive(false);
                        inventarioBeck.notaInventarioBecker();
                    }else if (fase == "Final")
                    {
                        dialagoPsicologo.SetActive(false);
                        dialagoPaciente.SetActive(false);
                        uiDialogo.SetActive(false);
                        finalizarCaso.activarRetroFinal();
                    }
                }
            }
        }
      
       
       
    }

    public  void cargarPreguntas(Preguntas pregunta)
    {
        txtNombrePsicologo.gameObject.SetActive(false);
        txtNombrePaciente.gameObject.SetActive(false);
        manejadorCamara.activarCamaraGeneral();
        uiDialogo.SetActive(false);
        btn_aceptar.gameObject.SetActive(false);
        ui_retroalimentacion.SetActive(true);
        audioSource.clip = pregunta.audio;
        audioSource.Play();
        StopAllCoroutines();
        StartCoroutine(escribirPregunta(pregunta.pregunta, txtRetroalimentacion, pregunta));
    }
    //Cargamos los botones en el contianer de pregutnas
    public void ActivarBotones(int cantidad, Preguntas pregunta)
    {

        if (listButtons.Count >= cantidad)
        {
            for (int i = 0; i < listButtons.Count; i++)
            {
                if (i < cantidad)
                {
                    listButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = pregunta.respuestas[i].respuesta;
                    listButtons[i].onClick.RemoveAllListeners();
                    Respuestas respuestaObj = pregunta.respuestas[i];
                    string retroalimentacion = pregunta.respuestas[i].retroalimentacion;
                    bool esCorrecta = pregunta.respuestas[i].esCorrecta;
                    string respuesta = pregunta.respuestas[i].respuesta;
                    int cali = pregunta.calificacion;

                    double result = cali / pregunta.respuestas.Length;
                    caliPorIncorrecto = result;
                    calificacion.valorIncorrecto = result;
                    calificacion.valorPregunta = cali;
                    calificacion.incrementarFinal(cali);

                    listButtons[i].onClick.AddListener(() => darFuncionBtn(retroalimentacion, esCorrecta,respuesta,respuestaObj,cali));
                    listButtons[i].gameObject.SetActive(true);

                }
                else
                {
                    listButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            int cantidadRestante = (cantidad - listButtons.Count);
            for (int i = 0; i < cantidadRestante; i++)
            {
                var newButton = Instantiate(btn_prefab, container_preguntas).GetComponent<Button>();
                newButton.gameObject.SetActive(true);
                listButtons.Add(newButton);
            }
            ActivarBotones(cantidad, pregunta);

        }
    }
   // Metodo pata asignar la funcionalidad a los botones de las respuestas
    public void darFuncionBtn( string retroalimentacion, bool esCorrecta, string respuesta,Respuestas respuestasobj, int valor)
    {
        if (retroalimentacion != "")
        {
            audioSource.clip = respuestasobj.audio;
            audioSource.Play();
            ui_retroalimentacion.SetActive(true);
            txtRetroalimentacion.text = retroalimentacion;
            uiPreguntas.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(escribirTexto(retroalimentacion, txtRetroalimentacion, btn_aceptar.gameObject));
            if (respuesta == "Frente al terapeuta")
            {
                animDoctor.SetBool("sentarse", true);
                animPaciente.SetBool("sentarse", true);
                parado = false;
                manejadorCamara.cambiarPosiciones(parado);
            }
            darFuncionAceptar(esCorrecta, valor);
        }
        else
        {
            noRetroaliemntacion(esCorrecta, valor);
        }
        
        

    }

    public void noRetroaliemntacion(bool correcto, int valorSumar)
    {
        btn_aceptar.gameObject.SetActive(false);
        uiPreguntas.SetActive(false);
        if (correcto)
        {
            //Debug.LogWarning("Hizo click en algo correcto");
            calificacion.incrementar(valorSumar);     
                ui_retroalimentacion.SetActive(false);

                contador++;
                if (contador < dialogosList.Count)
                {
                    //uiDialogo.SetActive(true);

                    txtPersonaje.text = dialogosList[contador].personaje;
                    buscarPersonaje(dialogosList[contador].personaje);
                    llamarUiDialogos();
                    // StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtMensaje, btn_Siguiente));
                    container_preguntas.gameObject.SetActive(false);
                }
        }
        else
        {
         
           
                //  uiPreguntas.SetActive(true);
                ui_retroalimentacion.SetActive(true);
                cargarPreguntas(pregunta);
                btn_aceptar.gameObject.SetActive(false);

           
        }

    }
    public void  darFuncionBtnAceptar()
    {

        
        //uiDialogo.SetActive(true);
        buscarPersonaje(dialogosList[contador].personaje);
    
        txtPersonaje.text = dialogosList[contador].personaje;
        llamarUiDialogos();
        manejadorCamara.cambiarPosiciones(parado);
       
        btnSigPaciente.GetComponent<Button>().onClick.RemoveAllListeners();
        btnSigPaciente.GetComponent<Button>().onClick.AddListener(() => {
            funcionalidadBtnSiguiente();
        });
       
    }

    public void darFuncionAceptar(bool correcto, int valorSumar)
    {
        btn_aceptar.gameObject.SetActive(false);
        if (correcto)
        {
            //Debug.LogWarning("Hizo click en algo correcto");
            calificacion.incrementar(valorSumar);
            btn_aceptar.gameObject.SetActive(false);
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
                ui_retroalimentacion.SetActive(false);
               
                contador++;
                if (contador < dialogosList.Count)
                {
                    //uiDialogo.SetActive(true);
                  
                    txtPersonaje.text = dialogosList[contador].personaje;
                    buscarPersonaje(dialogosList[contador].personaje);
                    llamarUiDialogos();
                    // StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtMensaje, btn_Siguiente));
                    container_preguntas.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Se termino la fase inicial");
                    fichaDiagnostico.notaFichaDiagnostico();
                  
                }
               
                
                // contador++;
                /*   dialogosIncial = dialogo;
                   dialogosIncial.pregunta = dialogo.pregunta;
                   comenzarDialogos();*/
            });
            
        }
        else
        {
            //Debug.LogError("Hizo click en algo incorrecto");
            calificacion.decrementar(caliPorIncorrecto);
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
              //  uiPreguntas.SetActive(true);
                ui_retroalimentacion.SetActive(true);
                cargarPreguntas(pregunta);
                btn_aceptar.gameObject.SetActive(false);

            });
        }
      
    }


    IEnumerator escribirPregunta(string texto, TextMeshProUGUI txt,Preguntas pregunta)
    {
        txt.maxVisibleCharacters = 0;
        txt.text = texto;
        txt.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txt.maxVisibleCharacters++;
            yield return new WaitForSeconds(25f / 500);

        }
        uiPreguntas.SetActive(true);
        ActivarBotones(pregunta.respuestas.Length, pregunta);
        container_preguntas.gameObject.SetActive(true);

    }
    public void buscarPersonaje(string personajeHabalndo)
    {
     
        if (personajeHabalndo.Contains("Psicólogo"))
        {
            txtNombrePsicologo.text = personajeHabalndo;
            if (!parado)
            {
                txtNombrePaciente.gameObject.SetActive(false);
                txtNombrePsicologo.gameObject.SetActive(true);
            }
           
            dialagoPsicologo.SetActive(true);
            dialagoPaciente.SetActive(false);
        }
        else if (personajeHabalndo.Contains("Paciente"))
        {
            txtNombrePaciente.text = personajeHabalndo;
            if (!parado)
            {
                txtNombrePaciente.gameObject.SetActive(true);
                txtNombrePsicologo.gameObject.SetActive(false);
            }
          
            
            dialagoPaciente.SetActive(true);
            dialagoPsicologo.SetActive(false);
        }
    }
    
    public void llamarUiDialogos()
    {
        if (dialogosList[contador].personaje.Contains("Psicólogo"))

        {
            if (!parado)
            {
                animPaciente.SetBool("hablar", true);
                gameObjectPaciente.transform.position = ubiSerntadoPaciente.transform.position;
            }
            
            manejadorCamara.activarCamaraPsicologo();
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPsiscologo, btnSigPaciente.gameObject));
        }
        else
        {
            
            if (!parado)
            {
                animDoctor.SetBool("hablar", true);
                gameObjectPiscolog.transform.position = ubiSerntadoPsicologo.transform.position;
            }
        
            manejadorCamara.activarCamaraPaciente();
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPaciente, btnSigPaciente.gameObject));
        }
    }
}

