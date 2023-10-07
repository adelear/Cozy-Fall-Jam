using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandyBag : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Sprite smallBag;
    [SerializeField] private Sprite mediumBag;
    [SerializeField] private Sprite largeBag;

    private SpriteRenderer sr; 

    private Sprite currentBag;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); 

        if (levelManager.GetCurrentLevel() > 5)
        {
            currentBag = largeBag;
        }
        else if (levelManager.GetCurrentLevel() > 2)
        {
            currentBag = mediumBag;
        }
        else
        {
            currentBag = smallBag;
        }
        sr.sprite = currentBag;

        Debug.Log("Current Level:" + levelManager.GetCurrentLevel()); 
    }
}
