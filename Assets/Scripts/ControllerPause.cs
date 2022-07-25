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
    public GameObject objectHealthPlayer;
    public GameObject objectHealthEnemy;
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
    
    // SOUND DESIGN

    public AudioSource soundDefeat;

    public AudioSource soundVictory;
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
            shieldButton.visible = pauseButton.visible = false;
            gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("background-timer").visible = false;
            gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("timer").visible = false;

            if (!backgroundPauseMenu.enabledSelf)
            {
                backgroundPauseMenu.SetEnabled(true);
            }

            return;
        }

        if (backgroundPauseMenu.enabledSelf)
        {
            backgroundPauseMenu.SetEnabled(false);
        }

        backgroundPauseMenu.visible = backgroundEnd.visible = resumeButton.visible = restartButton.visible = message.visible = false;
		shieldButton.visible = pauseButton.visible = true;

        if (playerObject.GetComponent<ControllerPlayer>().GetActiveShield())
        {
            gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("timer").visible = true;
            gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("background-timer").visible = true;
        }
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
        shieldButton.visible = DeleteShield = DeleteBullet = pauseButton.visible = true;
        endGame = backgroundEnd.visible = endMessage.visible = restartButton.visible = false;
    }

    public void ResumeButtonPressed() {
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
                soundDefeat.Play();
                pauseButton.visible = shieldButton.visible = false;
                endGame = backgroundEnd.visible = endMessage.visible = restartButton.visible = true;
                endMessage.text = "DEFEAT";
                endMessage.style.color = Color.red;
                gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("background-timer").visible = false;
                gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("timer").visible = false;
                Debug.LogAssertion("[END GAME] Defeat");
            }

            if (enemyObject.GetComponent<Enemy>().GetLocalHealth() <= 0f)
            {
                soundVictory.Play();
                pauseButton.visible = shieldButton.visible = false;
                endGame = backgroundEnd.visible = endMessage.visible = restartButton.visible = true;
                endMessage.text = "VICTORY";
                endMessage.style.color = Color.green;
                gameObject.GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("background-timer").visible = false;
                gameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("timer").visible = false;
                Debug.LogAssertion("[END GAME] Victory");
            }
        }
    }

}
