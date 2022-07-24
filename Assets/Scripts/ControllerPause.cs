using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ControllerPause : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;
    public Button pauseButton;
    public Button restartButton;
    public Button resumeButton;
    public Button shieldButton;
    public Label message;
    public Label timer;

    [SerializeField] private string messageTextPause;

    public VisualElement backgroundPauseMenu;

    public VisualElement backgroundTimer;

    private bool DeleteShield = false;

    private bool DeleteBullet = false;
    // Start is called before the first frame update
    void Start() {
        
        var root = gameObject.GetComponent<UIDocument>().rootVisualElement;

        pauseButton = root.Q<Button>("pause");
        shieldButton = root.Q<Button>("shield");
        restartButton = root.Q<Button>("restart");
        resumeButton = root.Q<Button>("resume");
        message = root.Q<Label>("message");
        timer = root.Q<Label>("timer");
        

        messageTextPause = "Pause";
        
        backgroundPauseMenu = root.Q<VisualElement>("pause-menu");
        backgroundTimer = root.Q<VisualElement>("background-timer");
        
        pauseButton.clicked += PauseButtonPressed;
        restartButton.clicked += RestartButtonPressed;
		resumeButton.clicked += ResumeButtonPressed;
        
        backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = backgroundTimer.visible = timer.visible = false;
        pauseButton.visible = shieldButton.visible = true;
    }

    void Update() {
        message.text = messageTextPause;
    }

    void ChangeVisiblePauseMenu()
    {
        if (backgroundPauseMenu.visible == false) {
            backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = true;
            pauseButton.visible = false;
            
            return;
        }

        backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = false;
		pauseButton.visible = true;
    }
    
    void PauseButtonPressed()
    {
        ChangeVisiblePauseMenu();
        
        Debug.Log("Clicked pause-menu");
    }

    void RestartButtonPressed()
    {
        playerObject.GetComponent<ControllerPlayer>().Respawn();
        enemyObject.GetComponent<Enemy>().Respawn();

        playerObject.SetActive(true);
        enemyObject.SetActive(true);
        DeleteShield = DeleteBullet = true;

        Debug.Log("Clicked restart-button");

        ChangeVisiblePauseMenu();

    }
    
	void ResumeButtonPressed() {
		ChangeVisiblePauseMenu();
		Debug.Log("Clicked resume button");
	}

    public bool GetDeleteShield()
    {
        return DeleteShield;
    }

    public void SetDeleteShield()
    {
        DeleteShield = false;
    }
    
    public bool GetDeleteBullet()
    {
        return DeleteBullet;
    }

    public void SetDeleteBullet()
    {
        DeleteBullet = false;
    }
}
