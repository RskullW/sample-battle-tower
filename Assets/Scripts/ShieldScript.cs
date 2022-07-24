using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShieldScript : MonoBehaviour
{
	public Transform spawnPosition;
	public float healthShield = 60;
    private float localHealth;
    private GameObject UIDocument;

    private static short numShieldCreated = 0;
    
    // Start is called before the first frame update
    void Start()
    {
	    numShieldCreated++;
	    localHealth = healthShield;
		UIDocument = GameObject.Find("UIDocumentMenu");
	}

	// Update is called once per frame
    void Update()
    {
	    if (UIDocument.GetComponent<ControllerPause>().GetDeleteShield())
	    {
		    numShieldCreated--;
		    
			GameObject.Find("EnemyCollider").GetComponent<Enemy>().SetAliveShield();
		    GameObject.Find("PlayerCollider").GetComponent<ControllerPlayer>().SetAliveShield();

		    if (numShieldCreated == 0)
		    {
			    UIDocument.GetComponent<ControllerPause>().SetDeleteShield();
		    }
		    
		    Destroy(gameObject);
			
		    Debug.LogAssertion("[RESTART] Shield destroyed");

	    }
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
	    numShieldCreated--;

	    Destroy(gameObject);
    }
}
