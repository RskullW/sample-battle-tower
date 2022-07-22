using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ControllerPause : MonoBehaviour
{
    public Button pauseButton;
    public Button restartButton;
    public Button resumeButton;
    public Button shieldButton;
    public Label message;
    [SerializeField] private string messageTextPause;

    public VisualElement backgroundPauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        pauseButton = root.Q<Button>("pause");
        shieldButton = root.Q<Button>("shield");
        restartButton = root.Q<Button>("restart");
        resumeButton = root.Q<Button>("resume");
        message = root.Q<Label>("message");

        messageTextPause = "Pause";
        
        backgroundPauseMenu = root.Q<VisualElement>("pause-menu");
        
        pauseButton.clicked += PauseButtonPressed;
        restartButton.clicked += RestartButtonPressed;
        
        backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = false;
        pauseButton.visible = shieldButton.visible = true;
    }

    private void Update()
    {
        message.text = messageTextPause;
    }

    void ChangeVisiblePauseMenu()
    {
        if (backgroundPauseMenu.visible == false)
        {
            backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = true;
            pauseButton.visible = shieldButton.visible = false;
            
            return;
        }

        backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = false;
        pauseButton.visible = shieldButton.visible = true;
    }
    
    void PauseButtonPressed()
    {
        ChangeVisiblePauseMenu();
        
        Debug.Log("Clicked pause-menu");
    }

    void RestartButtonPressed()
    {
        ChangeVisiblePauseMenu();
        Debug.Log("Clicked restarted");
    }
    
}
