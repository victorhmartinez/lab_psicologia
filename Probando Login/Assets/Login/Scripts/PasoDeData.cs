using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasoDeData : MonoBehaviour
{
    private TMP_Text texto;


    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = $"Username: {DataUsers.Instance.username}\nFullname: {DataUsers.Instance.nombre}";
    }
}
