using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void TakeDamage(float damage) {
        health -= damage;

        if (health <= 0)
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
