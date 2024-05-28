using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System;

public class ApiManager : MonoBehaviour
{
    [SerializeField]
    private string apiUrl = "http://localhost:3000/api";
    [SerializeField]
    private TextMeshProUGUI txtDatos;
    public List<Dialogos> dialogosList = new List<Dialogos>();
    public List<Dialogos> dialogosListDes = new List<Dialogos>();
    public List<Dialogos> dialogosListFin = new List<Dialogos>();
    public int nroCaso;
    // Evento para indicar que los di�logos se han cargado
    public event Action<List<Dialogos>> DialogosCargadosEvent;
    // Evento para indicar que el n�mero aleatorio ha sido generado
    public event Action<int> NumeroAleatorioGeneradoEvent;

    void Start()
    {
        //Consumimos la api mediante corrutinas por proceso asincrono
        Debug.Log("ApiManager Start() ejecutado.");

        // Generar un n�mero aleatorio entre 1 y 2
        nroCaso = UnityEngine.Random.Range(1, 3);
        Debug.Log("N�mero de caso generado: " + nroCaso);

        // Disparamos el evento para indicar que el n�mero aleatorio ha sido generado
        NumeroAleatorioGeneradoEvent?.Invoke(nroCaso);

        StartCoroutine(GetDialogosFromApi(nroCaso, "Inicial", (dialogos) => {
            dialogosList = dialogos;
            Debug.Log("Di�logos Inicial cargados: " + dialogosList.Count);
            DialogosCargadosEvent?.Invoke(dialogosList);
        }));

        StartCoroutine(GetDialogosFromApi(nroCaso, "Desarrollo", (dialogos) => {
            dialogosListDes = dialogos;
            Debug.Log("Di�logos Desarrollo cargados: " + dialogosListDes.Count);
            DialogosCargadosEvent?.Invoke(dialogosListDes);
        }));
        StartCoroutine(GetDialogosFromApi(nroCaso, "Final", (dialogos) => {
            dialogosListFin = dialogos;
            Debug.Log("Di�logos final cargados: " + dialogosListFin.Count);
            DialogosCargadosEvent?.Invoke(dialogosListFin);
        }));
    }

    IEnumerator GetDialogosFromApi(int caso, string fase, Action<List<Dialogos>> callback)
    {
        string url = apiUrl + "/get-dialogos?caso=" + caso + "&fase=" + fase;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener los dialogos: " + request.error);
            }
            else
            {
                string jsonData = request.downloadHandler.text;
                Debug.Log("Dialogos obtenidos correctamente para fase " + fase + ": " + jsonData);

                // Deserializar el JSON en la clase contenedora
                DialogosContainer dialogosContainer = JsonUtility.FromJson<DialogosContainer>("{\"dialogos\":" + jsonData + "}");

                // Acceder a la lista de dialogos
                List<Dialogos> listDialogos = dialogosContainer.dialogos;

                // Procesar preguntas si existen
                foreach (Dialogos dialogo in listDialogos)
                {
                    if (dialogo.tienePregunta)
                    {
                        yield return StartCoroutine(GetPreguntaById(dialogo.preguntaId, dialogo));
                    }
                }

                // Callback para asignar la lista
                callback?.Invoke(listDialogos);
            }
        }
    }

    IEnumerator GetPreguntaById(string preguntaId, Dialogos dialogo)
    {
        string url = apiUrl + "/get-questionsId/" + preguntaId;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener la pregunta con ID " + preguntaId + ": " + request.error);
            }
            else
            {
                string preguntaData = request.downloadHandler.text;
                Debug.Log("Pregunta obtenida correctamente.");

                // Deserializamos el JSON en un objeto Preguntas
                Preguntas pregunta = JsonUtility.FromJson<Preguntas>(preguntaData);
                dialogo.pregunta = pregunta;
            }
        }
    }
}
