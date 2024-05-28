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
    private int criteriosObservados;
    private int criteriosObservadosEscritos;
    void Start()
    {
        submitButton.onClick.AddListener(SubmitFicha);
    }

    void SubmitFicha()
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
            txtObservacion.text="Recuerda revisar bien los criterios seleccionados con los ingresados.";
        }

        // Aquí puedes agregar lógica para enviar los datos a un servidor o almacenarlos localmente
    }
}

