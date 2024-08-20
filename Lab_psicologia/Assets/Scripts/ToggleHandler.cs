using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Toggle myToggle;
    [SerializeField]
    private GameObject panel;

    void Start()
    {
        // Aseg�rate de que el Toggle est� asignado
        if (myToggle != null)
        {
            // Agrega el m�todo OnToggleChanged como listener para el evento onValueChanged
            myToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    // Este m�todo se ejecutar� cada vez que el valor del Toggle cambie
    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            panel.SetActive(false);
        }
        else
        {
            panel.SetActive(false);
        }
    }
}
