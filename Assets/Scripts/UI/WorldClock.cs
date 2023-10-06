using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldClock : MonoBehaviour
{
    [SerializeField] private Image clockFill;
    [SerializeField] private RectTransform clockHandle;
    [SerializeField] private float maxTime;



    private float currentTime;

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() == GameState.GAME)
        {
            currentTime += Time.deltaTime;
            clockHandle.eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.forward * 360, currentTime / maxTime);
            clockFill.fillAmount = Mathf.Lerp(0, 1, currentTime / maxTime);

            if (currentTime >= maxTime)
            {
                GameManager.Instance.SwitchState(GameState.DEFEAT);
            }
        }
    }    
}