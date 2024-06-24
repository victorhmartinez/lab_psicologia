using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calificacion : MonoBehaviour
{
    public double puntuacionActual=0;
    public double valorIncorrecto;
    public double valorPregunta;

    public double valorCalificacionTotal = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
