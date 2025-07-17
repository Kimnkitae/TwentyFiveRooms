using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.ComponentModel.Design;

public class Question00 : MonoBehaviour
{
    public GameObject wallToDisable;
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInputField;
    public string correctAnswer = "4";
    private bool playerInTrigger = false;

    void Start()
    {
        if (questionPanel != null)
        {
            questionPanel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (questionPanel != null)
            {
                questionPanel.SetActive(true);
            }

            if (answerInputField != null)
            {
                answerInputField.text = "";
                answerInputField.Select();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (wallToDisable != null)
            {
                if (wallToDisable.activeSelf)
                {
                    playerInTrigger = false;
                    if (questionPanel != null)
                    {
                        questionPanel.SetActive(false);

                    }
                }

            }
            else
            {
                playerInTrigger = false;
                if (questionPanel != null)
                {
                    questionPanel.SetActive(false);
                }
            }
        }
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    public void CheckAnswer()
    {
        if (answerInputField == null || string.IsNullOrEmpty(correctAnswer))
        {
            Debug.LogWarning("Ответ или правильный ответ не заданы!");
            return;
        }
        if (answerInputField.text.Trim().ToLower() == correctAnswer.ToLower())
        {
            Debug.Log("Правильный ответ! Стена исчезает!");
            if (wallToDisable != null)
            {
                Destroy(wallToDisable);
            }

            if (questionPanel != null)
            {
                Destroy(questionPanel);
            }
            gameObject.SetActive(false);
            
        }
        else
        {
            Debug.Log("Неправильный ответ. Попробуйте еще раз.");

        }
    }
}