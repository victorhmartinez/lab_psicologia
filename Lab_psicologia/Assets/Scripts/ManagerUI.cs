using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject uiDialogo;
    [SerializeField]
    private GameObject uiPreguntas;
    [SerializeField]
    private Dialogo dialogosIncial;
    [SerializeField]
    private TextMeshProUGUI txtMensaje;
    [SerializeField]
    private TextMeshProUGUI txtPersonaje;
    [SerializeField]
    private TextMeshProUGUI txtPregunta;
    private List<Button> listButtons = new List<Button>();
    public    int contador = 0;
    [SerializeField]
    private GameObject btn_iniciar;
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


    // Start is called before the first frame update
    void Start()
    {
        uiDialogo.SetActive(false);
        uiPreguntas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void comenzarDialogos()
    {
        uiDialogo.SetActive(true);
        txtPersonaje.text = dialogosIncial.dialogos[contador].personaje.nombre;
        StartCoroutine(escribirTexto(dialogosIncial.dialogos[contador].dialogo,txtMensaje));
        contador++;
        btn_iniciar.SetActive(false);
    }

 

    public void cambiarConversacion()
    {
        btn_Siguiente.gameObject.SetActive(false);
        Debug.Log(dialogosIncial.dialogos.Length);
        if (contador >= dialogosIncial.dialogos.Length)
        {
           
            if (dialogosIncial.pregunta)
            {
                uiDialogo.SetActive(false);
                contador = 0;
                uiPreguntas.SetActive(true);
                txtPersonaje.text = dialogosIncial.pregunta.personaje.nombre;
                StopAllCoroutines();
                StartCoroutine(escribirTexto(dialogosIncial.pregunta.pregunta, txtPregunta));
                
                Pregunta pregunta = dialogosIncial.pregunta;
                ActivarBotones(pregunta.opciones.Length, pregunta);
            }
        }
        else
        {
            txtPersonaje.text = dialogosIncial.dialogos[contador].personaje.nombre;
            StopAllCoroutines();
            StartCoroutine(escribirTexto(dialogosIncial.dialogos[contador].dialogo, txtMensaje));
            contador++;
        }
       
    }

    public void ActivarBotones(int cantidad, Pregunta pregunta)
    {

        if (listButtons.Count >= cantidad)
        {
            for (int i = 0; i < listButtons.Count; i++)
            {
                if (i < cantidad)
                {
                    listButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = pregunta.opciones[i].opcion;
                    listButtons[i].onClick.RemoveAllListeners();

                    string retroalimentacion = pregunta.opciones[i].retroalimentacion;
                    bool esCorrecta = pregunta.opciones[i].esCorrecta;
                    Dialogo dialogo = null;
                    if (esCorrecta)
                    {
                      dialogo=pregunta.opciones[i].conveResultante;
                    }

                    listButtons[i].onClick.AddListener(() => darFuncionBtn(dialogo, retroalimentacion,esCorrecta));
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
    public void darFuncionBtn(Dialogo dialogo,string retroalimentacion, bool esCorrecta)
    {

       
            ui_retroalimentacion.SetActive(true);
          txtRetroalimentacion.text = retroalimentacion;
            uiPreguntas.SetActive(false);
           StopAllCoroutines();
           StartCoroutine(escribirTexto(retroalimentacion,txtRetroalimentacion));
      
        darFuncionAceptar(esCorrecta,dialogo);

    }

    IEnumerator escribirTexto(string texto, TextMeshProUGUI txt)
    {
        txt.maxVisibleCharacters = 0;
        txt.text = texto;
        txt.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txt.maxVisibleCharacters++;
            yield return new WaitForSeconds(15f / 500 );

        }

        btn_Siguiente.gameObject.SetActive(true);
    }

     public void darFuncionAceptar(bool correcto, Dialogo dialogo)
    {
        if (correcto)
        {
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
                ui_retroalimentacion.SetActive(false);
                dialogosIncial = dialogo;
                dialogosIncial.pregunta = dialogo.pregunta;
                comenzarDialogos();
            });
        }
        else
        {
            btn_aceptar.onClick.RemoveAllListeners();
            btn_aceptar.onClick.AddListener(() => {
                uiPreguntas.SetActive(true);
                ui_retroalimentacion.SetActive(false);
            });
        }
    }

}
