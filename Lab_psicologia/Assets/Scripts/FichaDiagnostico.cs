using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        }
        else
        {
            if (listResultados[nroCaso - 1] == criteriosObservadosEscritos)
            {
                if (verificarRespuestasCaso())
                {
                    
                   
                    panelAlerta.SetActive(true);
                    txtObservacion.text = "Felicitaciones, has realizado correctamente el conteo de los criterios del DMS-5";
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
                            dialogosManager.iniciarFase("Desarrollo");
                            dialogosManager.darFuncionBtnAceptar();
                            
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

        // Aquí puedes agregar lógica para enviar los datos a un servidor o almacenarlos localmente
    }

    public void notaFichaDiagnostico()
    {
        btnAceptar.SetActive(false);
        nroCaso = apiManager.nroCaso;
        objectGuia.SetActive(true);
        StartCoroutine(escribirTexto(notaTexto, txtNota, btnAceptar));
        btnAceptar.GetComponent<Button>().onClick.RemoveAllListeners();
        btnAceptar.GetComponent<Button>().onClick.AddListener(()=>{
            objectGuia.SetActive(false);
            panelFicha.SetActive(true);
         
            
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
            yield return new WaitForSeconds(15f / 500);

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
                txtObservacion.text = "Los criterios que seleccionaste no están de acuerdo con el caso clínico.";
                esCorrecta = false;
                break;
            }
        }
        return esCorrecta;
    }
    }

