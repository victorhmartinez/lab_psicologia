using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogosManager : MonoBehaviour

{
   
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
    [SerializeField]
    private AnimationClip animationEntregar;
    [Header("Panel Indicaciones")]
    [SerializeField]
    private GameObject panelIndiAniamciones;
    [SerializeField]
    private TextMeshProUGUI txtAnimaciones;
    [SerializeField]
    private GameObject[] listUbicacionesCamera;
    [SerializeField]
    private Camera mainCamera;

    bool estado=true;
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

       calificacion.preguntasCant = calcularCantidadP();

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
            
                animDoctor.SetBool("pararse", false);
                animPaciente.SetBool("pararse", false);
                animDoctor.SetBool("sentarse", false);
                animPaciente.SetBool("sentarse", false);
                dialogosList = dialogosListDesarrollo;
                this.fase = fase;
                contador = 0;
                parado = true;
                break;
            case "Final":
                animDoctor.SetBool("pararse", false);
                animPaciente.SetBool("pararse", false);
                animDoctor.SetBool("sentarse", false);
                animPaciente.SetBool("sentarse", false);
                this.fase = fase;
                contador = 0;
                dialogosList = dialogosListFin;
                parado = true;
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
                if(fase=="Inicial" && contador ==16)
                {
                    Debug.Log("Aqui llama a david");
                    dialagoPaciente.SetActive(false);
                    dialagoPsicologo.SetActive(false);
                    txtNombrePsicologo.gameObject.SetActive(false);
                    txtNombrePaciente.gameObject.SetActive(false);
                    manejadorCamara.activarCamaraGeneral();
                    ui_retroalimentacion.SetActive(true);
                    btn_aceptar.gameObject.SetActive(false);
                    uiPreguntas.SetActive(false);
                    StopAllCoroutines();
                    StartCoroutine(escribirTexto("¡Atención! Lo que estás a punto de observar podría ser clave para el desarrollo de este caso", txtRetroalimentacion, btn_aceptar.gameObject));
                    //Debug.LogWarning("Hizo click en algo correcto");
                    
                    
                    btn_aceptar.onClick.RemoveAllListeners();
                    btn_aceptar.onClick.AddListener(() => {
                        ui_retroalimentacion.SetActive(false);
                        btn_aceptar.gameObject.SetActive(false);
                        contador++;
                        buscarPersonaje(dialogosList[contador].personaje);
                        txtPersonaje.text = dialogosList[contador].personaje;
                        llamarUiDialogos();



                    });

                }else if (fase == "Inicial" && contador == 6)
                {
                    
                    dialagoPaciente.SetActive(false);
                    dialagoPsicologo.SetActive(false);
                    txtNombrePsicologo.gameObject.SetActive(false);
                    txtNombrePaciente.gameObject.SetActive(false);
                    manejadorCamara.activarCamaraGeneral();
                    panelIndiAniamciones.SetActive(true);
                    txtAnimaciones.text = "El terapeuta entrega el consentimiento informado al paciente) \n" +
                        "(El paciente simula leerlo y luego procede a firmar el consentimiento informado dado por el terapeuta. Posterior a ello se continua con la entrevista).";
                    animDoctor.SetBool("entregar", true);
                    StopAllCoroutines();
                    StartCoroutine(ejecutarAnimacionFirmar());
                    StartCoroutine(esperarAnimacion(panelIndiAniamciones,false,"Inicial",listUbicacionesCamera[1]));

                }else if (fase== "Desarrollo" && contador==1)
                {
                   animDoctor.SetBool("sentarse", true);
                    animPaciente.SetBool("sentarse", true);
                    parado = false;
                    manejadorCamara.cambiarPosiciones(parado);
                    panelIndiAniamciones.SetActive(true);
                    txtAnimaciones.text = "(Paciente pasa y se sienta frente al terapeuta";
                    StopAllCoroutines();
                    StartCoroutine(esperarAnimacion(panelIndiAniamciones, false, fase,null));
                }else if (fase == "Desarrollo" && contador == 8)
                {
                    dialagoPaciente.SetActive(false);
                    dialagoPsicologo.SetActive(false);
                    txtNombrePsicologo.gameObject.SetActive(false);
                    txtNombrePaciente.gameObject.SetActive(false);
                    manejadorCamara.activarCamaraGeneral();
                    panelIndiAniamciones.SetActive(true);
                    txtAnimaciones.text = "El terapeuta le presenta al paciente el test y empieza a simular que lo completa)";
                    animPaciente.SetBool("escribir", true);
                    StopAllCoroutines();
                    StartCoroutine(ejecutarAnimacionFirmar());
                    StartCoroutine(esperarAnimacion(panelIndiAniamciones, false, fase,listUbicacionesCamera[3]));
                } 
                else
                {
                    if (fase == "Final" && contador == 1)
                    {
                        animDoctor.SetBool("sentarse", true);
                        animPaciente.SetBool("sentarse", true);
                        parado = false;
                        manejadorCamara.cambiarPosiciones(parado);
                    }
                        contador++;
                    if (fase == "Final" && contador == dialogosList.Count)
                    {
                        panelIndiAniamciones.SetActive(true);
                        txtAnimaciones.text = "(El terapeuta acompaña al paciente hasta la puerta y el paciente sala de la sala)";
                        animPaciente.SetBool("despedirse", true);
                        StopAllCoroutines();
                        StartCoroutine(esperarAnimacion(panelIndiAniamciones, true, "Final",null));
                    }



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


                            dialagoPsicologo.SetActive(false);
                            dialagoPaciente.SetActive(false);

                            inventarioBeck.notaInventarioBecker();

                            txtNombrePaciente.gameObject.SetActive(false);
                            txtNombrePsicologo.gameObject.SetActive(false);

                        }
                        else if (fase == "Final")
                        {
                            dialagoPsicologo.SetActive(false);
                            dialagoPaciente.SetActive(false);

                            finalizarCaso.activarRetroFinal();
                        }
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
                    if (estado==true)
                    {
                        calificacion.incrementarFinal(cali);
                        calificacion.incrementarContador();
                        Debug.LogError("se sumo");
                        estado = false;
                    }
                    
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
       
            audioSource.clip = respuestasobj.audio;
            audioSource.Play();
            ui_retroalimentacion.SetActive(true);
           
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
            estado = true;
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
                    container_preguntas.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Se termino la fase inicial");

                    panelIndiAniamciones.SetActive(true);
                    txtAnimaciones.text = "(Paciente se despide del terapeuta y sale de la sala)" +
                    "\n(Terapeuta se dirige a su escritorio y simula a empieza a llenar el documento con los criterios diagnósticos descritos)";
                    StopAllCoroutines();
                    animPaciente.SetBool("despedirse", true);
                   
                    StartCoroutine(esperarAnimacion(panelIndiAniamciones, true,"Inicial",listUbicacionesCamera[2]));
                    
                    
                  
                }
               
                
               
            });
            
        }
        else
        {
            calificacion.decrementar(caliPorIncorrecto);
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
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
    IEnumerator esperarAnimacion(GameObject panel,bool faseInicial,string fase,GameObject ubicacionCamera)
    {

        
        if (ubicacionCamera != null)
        {
            mainCamera.transform.position = ubicacionCamera.transform.position;
            mainCamera.transform.rotation = ubicacionCamera.transform.rotation;
        }
      
        dialagoPaciente.SetActive(false);
        dialagoPsicologo.SetActive(false);
        txtNombrePsicologo.gameObject.SetActive(false);
        txtNombrePaciente.gameObject.SetActive(false);
        manejadorCamara.activarCamaraGeneral();
      

        yield return new WaitForSeconds(5.0f);
        animPaciente.SetBool("despedirse", false);
        panel.SetActive(false);
        if (!faseInicial )
        {
            contador++;
            buscarPersonaje(dialogosList[contador].personaje);

            llamarUiDialogos();
        }
        else
        {
            if (fase == "Inicial")
            {
                fichaDiagnostico.notaFichaDiagnostico();
            }
           
        }
        mainCamera.transform.position = listUbicacionesCamera[0].transform.position;
        mainCamera.transform.rotation = listUbicacionesCamera[0].transform.rotation;

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
   IEnumerator ejecutarAnimacionFirmar()
    {
        yield return new WaitForSeconds(0.3f);
        animDoctor.SetBool("entregar", false);
        yield return new WaitForSeconds(animationEntregar.length);
        animPaciente.SetBool("escribir", true);
        yield return new WaitForSeconds(0.3f);
        animPaciente.SetBool("escribir", false);
    }
    public void llamarUiDialogos()
    {
        if (dialogosList[contador].personaje.Contains("Psicólogo"))

        {
            if (!parado)
            {

                animDoctor.SetBool("hablar", true);
                ubicarPersonajeCentro();
            }
            
            manejadorCamara.activarCamaraPsicologo();
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPsiscologo, btnSigPaciente.gameObject));
        }
        else
        {
            
            if (!parado)
            {
                animPaciente.SetBool("hablar", true);
                ubicarPersonajeCentro();
             
            }
        
            manejadorCamara.activarCamaraPaciente();
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPaciente, btnSigPaciente.gameObject));
        }
    }

    public void ubicarPersonajeCentro()
    {
        gameObjectPiscolog.transform.position = ubiSerntadoPsicologo.transform.position;
        gameObjectPaciente.transform.position = ubiSerntadoPaciente.transform.position;
    }

    public int calcularCantidadP()
    {
        int cont = 0;
        for (int i = 0; i < dialogosList.Count; i++)
        {
            if (dialogosList[i].tienePregunta)
            {
                cont++;
            }
        }
        for (int i = 0; i < dialogosListDesarrollo.Count; i++)
        {
            if (dialogosListDesarrollo[i].tienePregunta)
            {
                cont++;
            }
        }
        for (int i = 0; i < dialogosListFin.Count; i++)
        {
            if (dialogosListFin[i].tienePregunta)
            {
                cont++;
            }
        }
        cont += 3;
        return cont;
    }
}

