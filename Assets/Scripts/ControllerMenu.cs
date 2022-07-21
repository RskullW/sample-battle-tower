using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    public Button startButton;
    public Button resumeButton;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("start-button");
        resumeButton = root.Q<Button>("resume-button");

        startButton.clicked += StartButtonPressed;
        resumeButton.clicked += ResumeButtonPressed;

        Debug.Log("START\n");
    }

    void StartButtonPressed() {
        SceneManager.LoadScene("Game");
    }

    void ResumeButtonPressed() {
        Debug.Log("ResumePressed");
    }
}
