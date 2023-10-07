using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Components")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private int currentCandy = 0;
    [SerializeField] private int maxCandy = 100;
    [SerializeField] private AnimationCurve CandyToSpeedCurve;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public static event Action<PlayerController> OnPlayerCandyChanged;

    Vector2 inputVector;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        if (anim == null) anim = GetComponentInChildren<Animator>(); 
    }

    private void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        anim.SetFloat("hValue", inputVector.x); //Walking left and right
        anim.SetFloat("vValue", inputVector.y); //Walking forward

        if (inputVector.x < 0) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true; 

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
        displacement = displacement.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + displacement);
    }

    public void AddCandy(int amount)
    {
        currentCandy += amount;
        currentCandy = Mathf.Clamp(currentCandy, 0, maxCandy);
        OnPlayerCandyChanged?.Invoke(this);
    }

    public void LoseCandy(int amount)
    {
        currentCandy -= amount;
        currentCandy = Mathf.Clamp(currentCandy, 0, maxCandy);
        OnPlayerCandyChanged?.Invoke(this);
    }

    public int GetCurrentCandy()
    {
        return currentCandy;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;

    }
}
