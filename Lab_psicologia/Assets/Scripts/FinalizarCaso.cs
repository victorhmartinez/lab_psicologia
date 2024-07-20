using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalizarCaso : MonoBehaviour
{
    [SerializeField]
    private GameObject panelRetroalimentacionFase;
    [SerializeField]
    private Button btnContinuar;
    [SerializeField]
    private TextMeshProUGUI lblTitulo;
    [SerializeField]
    private SceneChange sceneChange;
    [SerializeField]
    private BeckInventory beckInventory;
    [Header("Retroalimentacion")]
    [SerializeField]
    private GameObject panelRetrolimentacion;
    [SerializeField]
    private TextMeshProUGUI txtRetroalimentacion;
    [SerializeField]
    private GameObject btnAceptarRetro;
    [SerializeField]
    [TextArea(4,2)]
    private string textoRetro;
    [SerializeField]
    private GameObject panelOpciones;
    [SerializeField]
    private AudioClip audioUltimo;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Toggle[] listToggle;

    [Header("Alerta")]
    [SerializeField]
    private GameObject panelAlerta;
    [SerializeField]
    private TextMeshProUGUI txtAlerta;
    [SerializeField]
    private Button btnAceptar;
    [SerializeField]
    private Calificacion calificacion;
    bool estado=true;
    // Start is called before the first frame update
    void Start()
    {
        listToggle[0].onValueChanged.AddListener(delegate
        {
            if (listToggle[0].isOn)
            {
                // Lógica cuando el Toggle está activado (true)
                Debug.Log("Toggle is On");
                opcionCorrecta();

            }
            else
            {
                // Lógica cuando el Toggle está desactivado (false)
                Debug.Log("Toggle is Off");
            }
        });
        listToggle[1].onValueChanged.AddListener(delegate
        {
            if (listToggle[1].isOn)
            {
                // Lógica cuando el Toggle está activado (true)
                Debug.Log("Toggle is On");
                opcionIncorrecta(listToggle[1]);

            }
            else
            {
                // Lógica cuando el Toggle está desactivado (false)
                Debug.Log("Toggle is Off");
            }
        });

        listToggle[2].onValueChanged.AddListener(delegate
        {
            if (listToggle[2].isOn)
            {
                // Lógica cuando el Toggle está activado (true)
                Debug.Log("Toggle is On");
                opcionIncorrecta(listToggle[2]);
            }
            else
            {
                // Lógica cuando el Toggle está desactivado (false)
                Debug.Log("Toggle is Off");
            }
        });
        listToggle[3].onValueChanged.AddListener(delegate
        {
            if (listToggle[3].isOn)
            {
                // Lógica cuando el Toggle está activado (true)
                Debug.Log("Toggle is On");
                opcionIncorrecta(listToggle[3]);

            }
            else
            {
                // Lógica cuando el Toggle está desactivado (false)
                Debug.Log("Toggle is Off");
            }
        });

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void activarRetroFinal()
    {
        panelRetroalimentacionFase.SetActive(true);
        lblTitulo.text = "Felicidades, has terminado este caso clinico";
        btnContinuar.onClick.RemoveAllListeners();
        btnContinuar.onClick.AddListener(() => {
            panelRetroalimentacionFase.SetActive(false);
            sceneChange.changeScena("Iniciar Sesion");
        });
    }

    public void activarPreguntaBeck()
    {
        panelRetrolimentacion.SetActive(true);
        btnAceptarRetro.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(escribirTexto(textoRetro, txtRetroalimentacion,panelOpciones));
        audioSource.clip = audioUltimo;
        audioSource.Play();
    }


    IEnumerator escribirTexto(string texto, TextMeshProUGUI txt,GameObject panelOpciones)
    {
        txt.maxVisibleCharacters = 0;
        txt.text = texto;
        txt.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txt.maxVisibleCharacters++;
            yield return new WaitForSeconds(35f / 500);

        }
        panelOpciones.SetActive(true);

   


    }

    public void opcionCorrecta()
    {
        
        panelAlerta.SetActive(true);
        panelOpciones.SetActive(false);
        txtAlerta.text = "El rango que seleccionaste de la tabla es el correcto";
        if (estado == true)
        {
            calificacion.incrementar(calificacion.valorPregunta);
            calificacion.incrementarFinal(calificacion.valorPregunta);
            calificacion.incrementarContador();
            estado = false;
        }
        btnAceptar.onClick.RemoveAllListeners();
        btnAceptar.onClick.AddListener(() =>
        {
            panelAlerta.SetActive(false);
            panelRetrolimentacion.SetActive(false);
            beckInventory.continuarSesion();
        });
    }

    public void opcionIncorrecta(Toggle toggle)
    {
        panelAlerta.SetActive(true);
        panelOpciones.SetActive(false);
        panelRetrolimentacion.SetActive(false);
        txtAlerta.text = "Incorrecto revisa los rangos de la tabla";
        btnAceptar.onClick.RemoveAllListeners();
        btnAceptar.onClick.AddListener(() =>
        {
            panelAlerta.SetActive(false);
            panelRetrolimentacion.SetActive(false);
            toggle.isOn = false;
            activarPreguntaBeck();
        });
    }

    // Este es el método que se ejecutará cuando el Toggle cambie de estado
 public void asignarMetodos()
    {
        for(int i=1; i <listToggle.Length-1; i++)
        {
            print(listToggle[i].gameObject.name);

            listToggle[i].onValueChanged.AddListener(delegate
            {
                print(listToggle[i].gameObject.name);
                if (listToggle[i].isOn)
                {
                    // Lógica cuando el Toggle está activado (true)
                    Debug.Log("Toggle is On");
                    opcionIncorrecta(listToggle[i]);

                }
                else
                {
                    // Lógica cuando el Toggle está desactivado (false)
                    Debug.Log("Toggle is Off");
                }
            });
        }



    }
}
