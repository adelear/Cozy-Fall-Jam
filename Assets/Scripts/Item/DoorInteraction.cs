using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private int maxCandyToGive;
    [SerializeField] private int minCandyToGive; 
    [SerializeField] private float askForCandyCooldown = 10;
    [SerializeField] public Slider slider; // temporary HUD Slider
    private PlayerController playerController;
    private bool isOnDoor;
    [SerializeField] private bool canAskForCandy;
    private float lastTimeAskedCandy;
    private float currentTime;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time - lastTimeAskedCandy;

        if (currentTime >= askForCandyCooldown)
        {
            if (!canAskForCandy) canAskForCandy = true;
        }
        else canAskForCandy = false; 

        if (isOnDoor && canAskForCandy)
        {
            if (Input.GetKeyDown(KeyCode.Space) && (currentTime >= askForCandyCooldown))
            {
                int candyToGive = Random.Range(minCandyToGive, maxCandyToGive); 
                playerController.AddCandy(candyToGive);
                canAskForCandy = false;
                lastTimeAskedCandy = Time.time;
            }
        }
         // this is Temporary HUD
      
            UpdateDoorHUDTimer(Mathf.Clamp(((currentTime) / askForCandyCooldown), 0, 1));
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Debug.Log("Player on door");
            playerController = other.GetComponent<PlayerController>();
            isOnDoor = true;
            //canAskForCandy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            Debug.Log("Player went away");
            isOnDoor = false;
        }
    }

    public void UpdateDoorHUDTimer(float curenthealth)
    {
        slider.value = curenthealth;
    }

    public bool GetCanAskforCandy()
    {
        return canAskForCandy; 
    }

}
