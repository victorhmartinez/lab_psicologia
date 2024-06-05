using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PresentarInfoSalas : MonoBehaviour
{
    [SerializeField]
    private GameObject panelInformacion;
    [SerializeField]
    private TextMeshProUGUI txtTituloSala;
    [SerializeField]
    private TextMeshProUGUI txtCuerpoSala;
    [SerializeField]
    private GameObject btnAceptar;
    [SerializeField]
    private string titulo;
    [SerializeField]
    [TextArea(3,2)]
    private string descripcion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entro el jugador al trigger");
            panelInformacion.SetActive(true);
            txtTituloSala.text = titulo;
            StartCoroutine(escribirInformacion(descripcion, btnAceptar));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            btnAceptar.gameObject.SetActive(false);
            Debug.Log("Entro el jugador al trigger");
            panelInformacion.SetActive(false);
            
        }
    }
    IEnumerator escribirInformacion(string texto,GameObject button)
    {
        txtCuerpoSala.maxVisibleCharacters = 0;
        txtCuerpoSala.text = texto;
        txtCuerpoSala.richText = true;
        for (int i = 0; i < texto.ToCharArray().Length; i++)
        {
            txtCuerpoSala.maxVisibleCharacters++;
            yield return new WaitForSeconds(15f / 500);
        }
        button.SetActive(true);
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            //dialogosManager.darFuncionBtnAceptar();
        });
    }
}
