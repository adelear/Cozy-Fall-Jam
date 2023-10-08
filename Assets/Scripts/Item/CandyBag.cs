using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandyBag : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Vector3 minScale = new Vector3(0.3f, 1f, 1f);
    [SerializeField] private Vector3 maxScale = new Vector3(1f, 1f, 1f);
    private Transform bagTransform; 

    void Start()
    {
        bagTransform = GetComponent<Transform>(); 
        Debug.Log("Current Level: " + levelManager.GetCurrentLevel()); 
    }

    private void Update()
    {
        float currentCandy = playerController.GetCurrentCandy();
        float maxCarriedTreats = LevelManager.Instance.GetMaxCarriedTreats();

        // Calculate the scale factor based on the candy ratio
        float candyRatio = Mathf.Clamp01(currentCandy / maxCarriedTreats);
        float newScaleX = Mathf.Lerp(minScale.x, maxScale.x, candyRatio);

        // Apply the new X-scale to the bag's transform
        Vector3 newScale = new Vector3(newScaleX, 1, 1);
        bagTransform.localScale = newScale;    
    }
}
