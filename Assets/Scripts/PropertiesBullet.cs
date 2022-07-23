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
    public GameObject UIDocumentMenu;

    void Start()
    {
        UIDocumentMenu = GameObject.Find("UIDocumentMenu");
        
        var root = UIDocumentMenu.GetComponent<UIDocument>().rootVisualElement;
        ActivePause = root.Q<VisualElement>("pause-menu");

    }

    void Update()
    {
        if (!ActivePause.visible)
        {
            rigidbody.velocity = transform.right * speed;
        }

        else
        {
            rigidbody.velocity = transform.right * 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
