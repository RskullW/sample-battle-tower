using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PropertiesBullet : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    public Rigidbody2D rigidbody;
    
    private VisualElement ActivePause;
    private VisualElement ActiveEnd;
    private VisualElement root;
    private GameObject UIDocumentMenu;
    
    private static short numBulletsCreated = 0;

    void Start()
    {
        numBulletsCreated++;
        UIDocumentMenu = GameObject.Find("UIDocumentMenu");
        
        root = UIDocumentMenu.GetComponent<UIDocument>().rootVisualElement;
        ActivePause = root.Q<VisualElement>("pause-menu");
        ActiveEnd = GameObject.Find("UIDocumentEndGame").GetComponent<UIDocument>().rootVisualElement
            .Q<VisualElement>("backgroundEndGame");
    }

    void Update()
    {
        if (!ActivePause.visible && !ActiveEnd.visible)
        {
            rigidbody.velocity = transform.right * speed;

             if (UIDocumentMenu.GetComponent<ControllerPause>().GetDeleteBullet())
            {
                numBulletsCreated--;
		    
                if (numBulletsCreated == 0)
                {
                    UIDocumentMenu.GetComponent<ControllerPause>().SetDeleteBullet();
                }
		    
                Destroy(gameObject);
		    
                Debug.LogAssertion("[RESTART] Bullet destroyed");

            }
        }

        else
        {
            rigidbody.velocity = transform.right * 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        ControllerPlayer player = hitInfo.GetComponent<ControllerPlayer>();
        ShieldScript shieldPlayer = hitInfo.GetComponent<ShieldScript>();
        
        if (player != null)
        {
            player.TakeDamage(damage);
        }
        
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (shieldPlayer != null)
        {
            shieldPlayer.TakeDamage(damage);
        }

        numBulletsCreated--;
        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
