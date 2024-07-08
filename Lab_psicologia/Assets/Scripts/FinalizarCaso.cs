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
    // Start is called before the first frame update
    void Start()
    {
        
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
}
