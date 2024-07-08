using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calificacion : MonoBehaviour
{
    public double puntuacionActual=0;
    public double valorIncorrecto;
    public double valorPregunta;

    public double valorCalificacionTotal = 0;

    [SerializeField]
    private TextMeshProUGUI txtPorcentaje;
    [SerializeField]
    private TextMeshProUGUI txtPuntaje;

    public double ValorPorcentaje=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        porcentaje();
        Vista();
    }
    public void incrementar(double cantidad)
    {
        puntuacionActual += cantidad;
    }
    public void decrementar(double cantidad)
    {
        puntuacionActual -= cantidad;
    }

    public void incrementarFinal(double cantidad)
    {
        valorCalificacionTotal += cantidad;
    }
    public void decrementarFinal(double cantidad)
    {
        valorCalificacionTotal -= cantidad;
    }


    public double resultado()
    {
        double result = (10 * puntuacionActual) / valorCalificacionTotal;
        return result;
    }
    public void porcentaje()
    {
        ValorPorcentaje = (100 * puntuacionActual) / valorCalificacionTotal;
    }
    public void Vista()
    {
        txtPuntaje.text = puntuacionActual+"";
        txtPorcentaje.text = ValorPorcentaje+" %";
    }
}
