using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
	public Transform spawnPosition;
	public float healthShield = 60;
    private float localHealth;
    
    // Start is called before the first frame update
    void Start()
	{
		localHealth = healthShield;
	}

	// Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
	    healthShield -= damage;

	    if (healthShield <= 0f)
	    {
		    Death();
	    }
    }

    void Death()
    {
	    if (GameObject.Find("EnemyCollider").GetComponent<Enemy>().CheckingAliveShield())
	    {
		    GameObject.Find("EnemyCollider").GetComponent<Enemy>().SetAliveShield();
	    }
	    
	    else if (GameObject.Find("PlayerCollider").GetComponent<ControllerPlayer>().CheckingAliveShield())
	    {
		    GameObject.Find("PlayerCollider").GetComponent<ControllerPlayer>().SetAliveShield();
	    }

	    Debug.Log("Shield destroyed");
	    Destroy(gameObject);
    }
}