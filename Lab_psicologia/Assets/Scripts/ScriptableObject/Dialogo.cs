
using UnityEngine;
[CreateAssetMenu(fileName ="Conversacion",menuName = "Sistema de dialogo / Nueva conversación")]
public class Dialogo : ScriptableObject
{
       [System.Serializable]
       public struct Linea
    {
        public Personaje personaje;
        [TextArea(3, 5)]
        public string dialogo;
    }
    public bool desbloqueada;
    public bool finalizada;
    public bool reUsar;
    public Linea[] dialogos;

    public Pregunta pregunta;
}
