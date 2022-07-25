using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private GameObject UIDocumentPause;
    private GameObject UIHealth;
    [SerializeField][Range(1f,200f)] private float health = 200f;
    public Transform firePoint;
    private VisualElement backgroundHealth;
    private float localHealth;
    
    
    // SHIELD SETTINGS
    public GameObject shield;
    public Transform shieldPoint;
    private float timeReloadShield = 15f;
    private float localTimerShield;

    private bool aliveShield, activeTimerShield;

    // MOVING SETTINGS
    public GameObject cannon;

    private float localTimerMove = 3f;
    // ATTACKING SETTINGS
    public GameObject bullet;
    private float localTimerAttack = 5f;

    // CHECKING PAUSE ACTIVE
    private VisualElement activePause;
    private VisualElement ActiveEnd;
    
    // SOUND DESIGN
    public AudioSource soundShot;
    public AudioSource soundHit;
    public AudioSource soundDeath;

    void Start()
    {
        aliveShield = activeTimerShield = false;

        localHealth = health;
        
        UIDocumentPause = GameObject.Find("UIDocumentMenu");
        UIHealth = GameObject.Find("UIHealthBarEnemy");

        var root = UIHealth.GetComponent<UIDocument>().rootVisualElement;
        var rootPause = UIDocumentPause.GetComponent<UIDocument>().rootVisualElement;
        
        backgroundHealth = root.Q<VisualElement>("background-enemy-health");
        activePause = rootPause.Q<VisualElement>("pause-menu");
        ActiveEnd = GameObject.Find("UIDocumentEndGame").GetComponent<UIDocument>().rootVisualElement
            .Q<VisualElement>("backgroundEndGame");
        firePoint.Rotate(0, 180f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activePause.visible && !ActiveEnd.visible) {
            MoveCannon();
            GiveDamage();
            ShieldTimer();
        }
    }

    public void TakeDamage(float damage)
    {
        localHealth -= damage;

        if (Math.Abs((localHealth * 100) / health - 100) > health)
        {
            backgroundHealth.style.borderLeftWidth = health;
        }
        
        else
        {
            backgroundHealth.style.borderLeftWidth = Math.Abs((localHealth * 100) / health - 100);
        }
        
        if (localHealth <= 0)
        {
            backgroundHealth.style.borderLeftWidth = 100f;
            Death();
        }

        else
        {
            soundHit.Play();
        }
    }
    
    public float GetHealth()
    {
        return health;
    }
    
    public float GetLocalHealth()
    {
        return localHealth;
    }

    public void Death(bool restartClicked = false)
    {
        soundDeath.Play();
        gameObject.SetActive(false);
    }

    void GiveDamage()
    {
        if (!aliveShield)
        {	
            if (localTimerAttack <= 0f)
            {
                soundShot.Play();
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                localTimerAttack = Random.Range(2f, 5f);
            }

            else
            {
                localTimerAttack -= Time.deltaTime;
            }
        }
    }

    void ShieldTimer()
    {
        if (activeTimerShield) {

            if (localTimerShield >= 0.0f)
            {
                localTimerShield -= Time.deltaTime;
            }

            else
            {
                activeTimerShield = aliveShield = false;
                timeReloadShield = Random.Range(3f, 15f);
            }
        }

        else if (!aliveShield && !activeTimerShield)
        {
            Debug.Log("Create enemy shield");

            aliveShield = true;
            localTimerShield = timeReloadShield;

            Instantiate(shield, shieldPoint.position, shieldPoint.rotation);
        }
    }

    void MoveCannon()
    {
        if (localTimerMove <= 0f)
        {
            Debug.Log("Enemy moved");
            cannon.transform.rotation = Quaternion.Euler(0, 180, cannon.transform.rotation.eulerAngles.z + Random.Range(-2f, 2f));
            localTimerMove = Random.Range(1f, 2f);
        }

        else
        {
            localTimerMove -= Time.deltaTime;
        }
    }
    
    public void SetAliveShield()
    {
        if (aliveShield)
        {
            activeTimerShield = true;
            aliveShield = false;
        }
    }

    public bool CheckingAliveShield()
    {
        return aliveShield;
    }
    
    public void Respawn()
    {
        gameObject.SetActive(true);

        localHealth = health;
        localTimerAttack = 5f;
        activeTimerShield = aliveShield = false;
        localTimerShield = Random.Range(3f, 15f);
        backgroundHealth.style.borderLeftWidth = Math.Abs((localHealth * 100) / health - 100);

    }
}
