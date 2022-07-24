using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ControllerPlayer : MonoBehaviour
{

    private bool aliveShield, activeTimerShield;
    
    private float maxPositionTouch;
    private float minPositionTouch = 10.0f;
    private float thisPositionTouch;
    
    private VisualElement ActivePause;
    private VisualElement ActiveBackgroundTimer;
	private VisualElement HealthBarBackground;

    private Label ActiveLabelTimer;

    public GameObject UIDocumentMenu;
	public GameObject UIHealthBarUser;

    [SerializeField] private GameObject playerCannon;

    [SerializeField] [Range(1.0f, 25.0f)] private float maxRotation; 
    [SerializeField] [Range(1.0f, 25.0f)] private float minRotation;

    [SerializeField] [Range(0, 10.0f)] private float speedRotation;
    
    public Transform firePoint;
	public Transform shieldPoint;
    public GameObject bullet;
    public GameObject shield;
	public Button shieldButton;
    private StyleColor[] localColorShieldButton;

    [SerializeField] private bool directionLeft;
    [SerializeField] private bool directionRight;
    [SerializeField][Range(1f,100f)] private float health = 100f;
    [SerializeField] private float reloadTime = 5f;
    [SerializeField] private float reloadShield = 15f;
    
    private float localTimerReload = 0f;

    private float localTimerShield = 0f;
    private float localHealth;
    private string timerString = "0:00";
    // Start is called before the first frame update
    void Start()
    {
        aliveShield = activeTimerShield = false;

        localColorShieldButton = new StyleColor[6];
        var resolution = Screen.resolutions;
        foreach (var res in resolution)
        {
            maxPositionTouch = res.height;
        }

        var root = UIDocumentMenu.GetComponent<UIDocument>().rootVisualElement;
        var rootHealthBar = UIHealthBarUser.GetComponent<UIDocument>().rootVisualElement;
        
        ActivePause = root.Q<VisualElement>("pause-menu");
		shieldButton = root.Q<Button>("shield");
        ActiveBackgroundTimer = root.Q<VisualElement>("background-timer");
        ActiveLabelTimer = root.Q<Label>("timer");

        HealthBarBackground = rootHealthBar.Q<VisualElement>("background-user-health");
        
        thisPositionTouch = playerCannon.transform.position.y;
        localHealth = health;
		shieldButton.clicked += OnPressedShieldButton;
        if (directionLeft == true)
        {
            firePoint.Rotate(0, 180f, 0);    
        }

        if (directionRight == true)
        {
            firePoint.Rotate(0,0,0);
        }

    }

    // Update is called once per frame
    void Update() {
        
        if (localTimerReload >= 0.0f && !ActivePause.visible)
        {
            localTimerReload -= Time.deltaTime;
        }

        if (activeTimerShield && !ActivePause.visible) {

            if (localTimerShield >= 0.0f)
            {
                localTimerShield -= Time.deltaTime;

                ActiveLabelTimer.text = "0:";

                if (Math.Round(localTimerShield) < 10f)
                {
                    ActiveLabelTimer.text += "0";
                }
                
                ActiveLabelTimer.text += Math.Round(localTimerShield);
            }

            else
            {
                shieldButton.style.color = localColorShieldButton[0];
                shieldButton.style.borderBottomColor = localColorShieldButton[1];
                shieldButton.style.borderLeftColor = localColorShieldButton[2];
                shieldButton.style.borderRightColor = localColorShieldButton[3];
                shieldButton.style.borderTopColor = localColorShieldButton[4];
                shieldButton.style.backgroundColor = localColorShieldButton[5];
                
                activeTimerShield = aliveShield = ActiveBackgroundTimer.visible  = ActiveLabelTimer.visible = false;
            }
        }

        if (Input.touchCount > 0 && !ActivePause.visible) {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {
                case TouchPhase.Moved: {

                    if (playerCannon.transform.rotation.eulerAngles.z < minRotation) {
                        playerCannon.transform.rotation = Quaternion.Euler(0, 0, minRotation);
                    }
                    
                    if (playerCannon.transform.rotation.eulerAngles.z > maxRotation) {
                        playerCannon.transform.rotation = Quaternion.Euler(0, 0, maxRotation);
                    }
                    
                    if (touch.position.y >= thisPositionTouch && playerCannon.transform.rotation.eulerAngles.z >= minRotation && playerCannon.transform.rotation.eulerAngles.z <= maxRotation) {
                        playerCannon.transform.rotation = Quaternion.Euler(0, 0, playerCannon.transform.rotation.eulerAngles.z + speedRotation);
                        break;
                    }

                    if (touch.position.y < thisPositionTouch && playerCannon.transform.rotation.eulerAngles.z >= minRotation && playerCannon.transform.rotation.eulerAngles.z <= maxRotation) {
                        playerCannon.transform.rotation = Quaternion.Euler(0, 0, playerCannon.transform.rotation.eulerAngles.z - speedRotation);
                    }

                }break;
                case TouchPhase.Began: {
                    if (localTimerReload <= 0f)
                    {
                        localTimerReload = reloadTime;
                        Shoot();
                    }
                }break;
            }
        }
    }
    
    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

	void OnPressedShieldButton() {
        if (!aliveShield)
        {
            Instantiate(shield, shieldPoint.position, shieldPoint.rotation);
            
            aliveShield = ActiveBackgroundTimer.visible  = ActiveLabelTimer.visible = true;
            localTimerShield = reloadShield;

            localColorShieldButton[0] = shieldButton.style.color;
            localColorShieldButton[1] = shieldButton.style.borderBottomColor;
            localColorShieldButton[2] = shieldButton.style.borderLeftColor;
            localColorShieldButton[3] = shieldButton.style.borderRightColor;
            localColorShieldButton[4] = shieldButton.style.borderTopColor;
            localColorShieldButton[5] = shieldButton.style.backgroundColor;
            
            shieldButton.style.color = Color.white;
            shieldButton.style.borderBottomColor = shieldButton.style.borderLeftColor = shieldButton.style.borderRightColor = shieldButton.style.borderTopColor = Color.gray;
            shieldButton.style.backgroundColor = Color.gray;
        }
    }

    public void SetAliveShield()
    {
        if (aliveShield == true)
        {
            activeTimerShield = true;
        }
    }
    
    public void TakeDamage(float damage) {
        localHealth -= damage;

        if (Math.Abs((localHealth * 100) / health - 100) > health)
        {
            HealthBarBackground.style.borderRightWidth = health;
        }

        else
        {
            HealthBarBackground.style.borderRightWidth = Math.Abs((localHealth * 100) / health - 100);
        }

        
        if (localHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
