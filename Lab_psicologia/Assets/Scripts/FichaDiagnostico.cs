using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FichaDiagnostico : MonoBehaviour
{
    public Toggle[] toggles;

    public TMP_InputField[] comentarios;
    public TMP_InputField resultadoText;
    public Button submitButton;
    [SerializeField]
    private TextMeshProUGUI txtObservacion;
    [SerializeField]
    private GameObject objectGuia;
    [SerializeField]
    private TextMeshProUGUI txtNota;
    [SerializeField]
    private GameObject btnAceptar;
    [SerializeField]
    private GameObject panelFicha;
    [SerializeField]
    private GameObject panelAlerta;
    [SerializeField]
    private DialogosManager dialogosManager;
    [SerializeField]
    [TextArea(3, 2)]
    private string notaTexto;
    private int criteriosObservados;
    private int criteriosObservadosEscritos;
    void Start()
    {
        submitButton.onClick.AddListener(SubmitFicha);
    }

    public void SubmitFicha()
    {
        
        criteriosObservados = 0;

        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                criteriosObservados++;
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
            Debug.Log("Estan bien contado los criterios");
            panelFicha.SetActive(false);
            dialogosManager.iniciarFase("Desarrollo");
            dialogosManager.darFuncionBtnAceptar();
        }

        // Aquí puedes agregar lógica para enviar los datos a un servidor o almacenarlos localmente
    }

    public void notaFichaDiagnostico()
    {
        btnAceptar.SetActive(false);
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


    }

