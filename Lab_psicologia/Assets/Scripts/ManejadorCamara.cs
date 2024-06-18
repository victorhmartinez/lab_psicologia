using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorCamara : MonoBehaviour
{
    [SerializeField]
    private GameObject camPaciente;
    [SerializeField]
    private GameObject camPsicolog;
    [SerializeField]
    private GameObject camaraGeneral;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void activarCamaraPaciente()
    {
        activarCam(camPsicolog, camPaciente);
        
    }
    public void activarCamaraPsicologo()
    {
        activarCam(camPaciente, camPsicolog);
    }
    public void activarCamaraGeneral()
    {
        camaraGeneral.SetActive(true);
        camPaciente.SetActive(false);
        camPsicolog.SetActive(false);
    }
    public void activarCam(GameObject cam1, GameObject cam2)
    {
        camaraGeneral.SetActive(false);
        cam1.SetActive(true);
        cam2.SetActive(false);
    }
}
