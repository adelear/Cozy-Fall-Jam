using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Components")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float currentCandy = 0;
    [SerializeField] private float maxCandy = 100;
    [SerializeField] private AnimationCurve CandyToSpeedCurve;

    Vector2 inputVector;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddCandy(20);
        }
    }

    private void FixedUpdate()
    {
        float t = Mathf.Clamp01(currentCandy / maxCandy);
        float speed = moveSpeed * CandyToSpeedCurve.Evaluate(t);
        Vector3 displacement = new Vector3(inputVector.x, 0, inputVector.y);
        displacement = displacement * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }

    public void AddCandy(int amount)
    {
        currentCandy += amount;
        currentCandy = Mathf.Clamp(currentCandy, 0, maxCandy);
    }

    public void LoseCandy(int amount)
    {
        currentCandy -= amount;
        currentCandy = Mathf.Clamp(currentCandy, 0, maxCandy);
    }

    public float GetCurrentCandy()
    {
        return currentCandy;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;

    }
}
