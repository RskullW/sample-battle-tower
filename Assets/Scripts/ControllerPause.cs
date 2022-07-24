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
    public Button restartButtonEnd;
    public Label endMessage;
    public Label message;
    public Label timer;

    [SerializeField] private string messageTextPause;

    public VisualElement backgroundPauseMenu;

    public VisualElement backgroundTimer;
    public VisualElement backgroundEnd;

    private bool DeleteShield = false;

    private bool DeleteBullet = false;

    private bool endGame = false;
    // Start is called before the first frame update
    void Start() {
        
        var root = gameObject.GetComponent<UIDocument>().rootVisualElement;
        var rootEnd = GameObject.Find("UIDocumentEndGame").GetComponent<UIDocument>().rootVisualElement;

        pauseButton = root.Q<Button>("pause");
        shieldButton = root.Q<Button>("shield");
        restartButton = root.Q<Button>("restart");
        restartButtonEnd = rootEnd.Q<Button>("restartEnd");
        resumeButton = root.Q<Button>("resume");
        message = root.Q<Label>("message");
        endMessage = rootEnd.Q<Label>("message");
        timer = root.Q<Label>("timer");
        

        messageTextPause = "Pause";
        
        backgroundPauseMenu = root.Q<VisualElement>("pause-menu");
        backgroundTimer = root.Q<VisualElement>("background-timer");
        backgroundEnd = rootEnd.Q<VisualElement>("backgroundEndGame");
        
        pauseButton.clicked += PauseButtonPressed;
        restartButton.clicked += RestartButtonPressed;
        restartButtonEnd.clicked += RestartButtonPressed;
        resumeButton.clicked += ResumeButtonPressed;

        
        backgroundPauseMenu.visible = resumeButton.visible = restartButton.visible = message.visible = backgroundTimer.visible = timer.visible = false;
        pauseButton.visible = shieldButton.visible = true;
    }

    void Update() {
        message.text = messageTextPause;
        EndGameMessage();
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
        shieldButton.visible = DeleteShield = DeleteBullet = true;

        Debug.Log("Clicked restart-button");

        endGame = backgroundEnd.visible = endMessage.visible = false;
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

    void EndGameMessage()
    {
        if (!endGame)
        {
            if (playerObject.GetComponent<ControllerPlayer>().GetLocalHealth() <= 0f)
            {
                pauseButton.visible = shieldButton.visible = false;
                endGame = backgroundEnd.visible = endMessage.visible = restartButton.visible = true;
                endMessage.text = "DEFEAT";
                endMessage.style.color = Color.red;
                
                Debug.LogAssertion("[END GAME] Defeat");
            }

            if (enemyObject.GetComponent<Enemy>().GetLocalHealth() <= 0f)
            {
                pauseButton.visible = shieldButton.visible = false;
                endGame = backgroundEnd.visible = endMessage.visible = restartButton.visible = true;
                endMessage.text = "VICTORY";
                endMessage.style.color = Color.green;
                
                Debug.LogAssertion("[END GAME] Victory");
            }
        }
    }
}
