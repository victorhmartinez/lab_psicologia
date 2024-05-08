
using UnityEngine;
[CreateAssetMenu(fileName = "Pregunta", menuName = "Sistema de dialogo / Pregunta    ")]
public class Pregunta : ScriptableObject
{
    [System.Serializable]
    public struct Opciones
    {
        public bool esCorrecta;
        [TextArea(2, 4)]
        public string opcion;
        [TextArea(2, 4)]
        public string retroalimentacion;
       
        public Dialogo conveResultante;
    }
     
    [TextArea(3, 5)]
    public string pregunta;
    public Opciones[] opciones;
    public Personaje personaje;
}
