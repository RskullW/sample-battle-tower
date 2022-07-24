using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    public GameObject UIHealth;
    private VisualElement backgroundHealth;
    private float localHealth;
    void Start()
    {
        localHealth = health;

        var root = UIHealth.GetComponent<UIDocument>().rootVisualElement;
        backgroundHealth = root.Q<VisualElement>("background-enemy-health");


    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TakeDamage(float damage) {
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
            Death();
        }
    }
    
    public float GetHealth()
    {
        return health;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
