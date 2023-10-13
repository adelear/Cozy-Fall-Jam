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

    private void Start()
    {
        SetMaxTime(LevelManager.Instance.GetMaxTime()); 
    }
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

    public float GetMaxTime()
    {
        return maxTime; 
    }

    public void SetMaxTime(int t)
    {
        maxTime = t;
        maxTime = Mathf.Max(0.01f, maxTime);
    }

    public float GetCurrentTime()
    {
        return currentTime; 
    }
}
