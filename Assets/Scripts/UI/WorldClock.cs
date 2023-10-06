using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldClock : MonoBehaviour
{
    [SerializeField] private RectTransform clockFill;
    [SerializeField] private RectTransform clockHandle;
    [SerializeField] private float maxTime;



    private float currentTime;

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameState.GAME)
        {
            currentTime += Time.deltaTime;
            

            if (currentTime <= maxTime)
            {
                GameManager.Instance.SwitchState(GameState.DEFEAT);
            }
        }
    }
}
