using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Label = System.Reflection.Emit.Label;

public class ControllerPlayer : MonoBehaviour {

    
    private float maxPositionTouch;
    private float minPositionTouch = 10.0f;
    private float thisPositionTouch;
    private VisualElement ActivePause;

    public GameObject UIDocumentMenu;

    [SerializeField] private GameObject playerCannon;

    [SerializeField] [Range(1.0f, 25.0f)] private float maxRotation; 
    [SerializeField] [Range(1.0f, 25.0f)] private float minRotation;

    [SerializeField] [Range(0, 10.0f)] private float speedRotation;
    
    public Transform firePoint;
    public GameObject bullet;

    [SerializeField] private bool directionLeft;
    [SerializeField] private bool directionRight;
    // Start is called before the first frame update
    void Start() {
        var resolution = Screen.resolutions;
        foreach (var res in resolution)
        {
            maxPositionTouch = res.height;
        }

        var root = UIDocumentMenu.GetComponent<UIDocument>().rootVisualElement;

        ActivePause = root.Q<VisualElement>("pause-menu");
        
        thisPositionTouch = playerCannon.transform.position.y;
        
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
                    Shoot();
                }break;
            }
        }
    }
    
    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}
