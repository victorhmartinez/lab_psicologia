using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VistaFicha : MonoBehaviour
{
    [SerializeField]
    private GameObject uiResFicha;
    [SerializeField]
    private GameObject ScrollViewContent;
    [SerializeField] 
    private GameObject Ficha;
    [SerializeField]
    private GameObject vista;
    [SerializeField]
    private GameObject btnContinuar;
    private bool estado= true;
    public GameObject alerta;
    public string[] FichaC1 =
    {
        "el paciente durante la entrevista mencion� que se ha sentido desanimado regularmente por no poder manejar todo por su cuenta.",
        "el paciente mencion� que ha perdido el inter�s en las actividades que sol�a realizar.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con la p�rdida de peso.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con problemas de sue�o.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con problemas psicomotores.",
        "el paciente manifest� que se ha sentido exhausto y frustrado.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con sentimientos de culpabilidad.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con su concentraci�n.",
        "el paciente durante la entrevista no mencion� alg�n s�ntoma relacionado con intentos suicidas."
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void presentarLista(bool[] array, bool[] arrayRespuestas)
    {
        uiResFicha.SetActive(true);
        for (int i = 0; i < array.Length; i++)
        {
            string opcion = "No";
            string esCorrecto = "Incorrecto, ";
            Color colorP = new Color();
            colorP = Color.red;
            colorP.a = 184F;

            int valor = i + 1;
            if (array[i] == arrayRespuestas[i])
            {
                colorP = Color.green;
                esCorrecto = "Correcto, ";
            }

            vista.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = valor.ToString();
            
            if (arrayRespuestas[i]==true)
            {
                opcion = "Si";
                
            }
            vista.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = opcion;
            vista.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = esCorrecto + FichaC1[i];
            vista.gameObject.GetComponent<RawImage>().color = colorP;

            GameObject panel = (GameObject)Instantiate(vista);
            panel.transform.SetParent(ScrollViewContent.transform);
            panel.transform.localPosition = Vector3.zero;
            panel.transform.localScale = Vector3.one;

        }
        btnContinuar.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            ActivarDesactivarPantalla();
        }) ;
        estado = false;
        GameObject panel1 = (GameObject)Instantiate(btnContinuar);
        panel1.transform.SetParent(ScrollViewContent.transform);
        panel1.transform.localPosition = Vector3.zero;
        panel1.transform.localScale = Vector3.one;
    }
    public void ActivarDesactivarPantalla()
    {
        uiResFicha.SetActive(estado);
        estado = !estado;
    }
}
