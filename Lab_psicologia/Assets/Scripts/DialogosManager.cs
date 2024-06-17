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
  
    Preguntas pregunta;
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

    // Método llamado cuando se cargan los diálogos desde ApiManager
    private void OnDialogosInicialCargados(List<Dialogos> dialogos)
    {
        // Guardar los diálogos cargados
        dialogosList = dialogos;
        Debug.Log("Hola estas en el evento con las listas cargadas");
     
    }
    private void OnDialogosDesarrolloCargados(List<Dialogos> dialogos)
    {
        // Guardar los diálogos cargados
        dialogosListDesarrollo = dialogos;
        Debug.Log("Hola estas en el evento con las listas de desarrollo");

    }
    private void OnDialogosFinCargados(List<Dialogos> dialogos)
    {
        // Guardar los diálogos cargados
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
            yield return new WaitForSeconds(15f / 500);

        }
        if (contador<dialogosList.Count) {
            if (txt.gameObject.name != "txt_retroalimentacion")
            {

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

                        dialagoPsicologo.SetActive(false);
                        dialagoPaciente.SetActive(false);
                        uiDialogo.SetActive(false);
                        inventarioBeck.notaInventarioBecker();
                    }
                }
            }
        }
      
       
       
    }

    public  void cargarPreguntas(Preguntas pregunta)
    {
        uiDialogo.SetActive(false);
        btn_aceptar.gameObject.SetActive(false);
        ui_retroalimentacion.SetActive(true);
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

                    string retroalimentacion = pregunta.respuestas[i].retroalimentacion;
                    bool esCorrecta = pregunta.respuestas[i].esCorrecta;
                    //Dialogo dialogo = null;
                    if (esCorrecta)
                    {
                        //dialogo = pregunta.respuestas[i].conveResultante;
                    }

                    listButtons[i].onClick.AddListener(() => darFuncionBtn(retroalimentacion, esCorrecta));
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
    public void darFuncionBtn( string retroalimentacion, bool esCorrecta)
    {

     
        ui_retroalimentacion.SetActive(true);
        txtRetroalimentacion.text = retroalimentacion;
        uiPreguntas.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(escribirTexto(retroalimentacion, txtRetroalimentacion,btn_aceptar.gameObject));

        darFuncionAceptar(esCorrecta);

    }
    public void  darFuncionBtnAceptar()
    {
        uiDialogo.SetActive(true);
        buscarPersonaje(dialogosList[contador].personaje);
    
        txtPersonaje.text = dialogosList[contador].personaje;
        llamarUiDialogos();

       
        btnSigPaciente.GetComponent<Button>().onClick.RemoveAllListeners();
        btnSigPaciente.GetComponent<Button>().onClick.AddListener(() => {
            funcionalidadBtnSiguiente();
        });
       
    }

    public void darFuncionAceptar(bool correcto)
    {
        btn_aceptar.gameObject.SetActive(false);
        if (correcto)
        {
            btn_aceptar.gameObject.SetActive(false);
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
                ui_retroalimentacion.SetActive(false);
               
                contador++;
                if (contador < dialogosList.Count)
                {
                    uiDialogo.SetActive(true);
                  
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
            yield return new WaitForSeconds(15f / 500);

        }
        uiPreguntas.SetActive(true);
        ActivarBotones(pregunta.respuestas.Length, pregunta);
        container_preguntas.gameObject.SetActive(true);

    }
    public void buscarPersonaje(string personajeHabalndo)
    {
     
        if (personajeHabalndo.Contains("Psicólogo"))
        {
            dialagoPsicologo.SetActive(true);
            dialagoPaciente.SetActive(false);
        }
        else if (personajeHabalndo.Contains("Paciente"))
        {
            dialagoPaciente.SetActive(true);
            dialagoPsicologo.SetActive(false);
        }
    }
    
    public void llamarUiDialogos()
    {
        if (dialogosList[contador].personaje.Contains("Psicólogo"))
        {
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPsiscologo, btnSigPaciente.gameObject));
        }
        else
        {
            StartCoroutine(escribirTexto(dialogosList[contador].contenido, txtDialogoPaciente, btnSigPaciente.gameObject));
        }
    }
}
