using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BeckInventory : MonoBehaviour
{
    [SerializeField]
    private QuestionsBeck[] questions; // Lista de preguntas
    [SerializeField]
    private GameObject questionPrefab; // Prefab para cada pregunta
    [SerializeField]
    private Transform questionContainer; // Contenedor para las preguntas
    [SerializeField]
    private Button submitButton;
    [SerializeField]
    private TextMeshProUGUI resultText;

    private List<int> responses = new List<int>(); // Lista para almacenar las respuestas
    // Start is called before the first frame update
    void Start()
    {
        GenerateQuestions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenerateQuestions()
    {
        foreach (var question in questions)
        {
            GameObject questionObj = Instantiate(questionPrefab, questionContainer);
            TextMeshProUGUI questionText = questionObj.transform.Find("QuestionText").GetComponent<TextMeshProUGUI>();
            questionText.text = question.questionText;

            Dropdown dropdown = questionObj.GetComponentInChildren<Dropdown>();
            dropdown.ClearOptions();

            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            foreach (var option in question.options)
            {
                options.Add(new Dropdown.OptionData(option));
            }

            dropdown.AddOptions(options);
            int index = responses.Count;
            responses.Add(0); // Inicializar la respuesta para esta pregunta

            dropdown.onValueChanged.AddListener((value) => {
                responses[index] = value;
            });
        }
    }
}
