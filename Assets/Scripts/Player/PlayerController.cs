using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [Header("Components")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private int currentCandy = 0;
    [SerializeField] private int maxCandy;
    [SerializeField] private AnimationCurve CandyToSpeedCurve;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip[] walkingSounds;
    [SerializeField] private AudioSource walkingSource;

    [Header("PlayerMechanics")]
    [SerializeField] private int teleRadius = 10;
    [SerializeField] private float teleDelay = 2.75f;
    [SerializeField] private float teleDuration = 1.0f;
    [SerializeField] private float teleRiseDuration = 1.0f;
    [SerializeField] private float teleSinkDuration = 1.0f;
    [SerializeField] private float teleMoveSpeed = 5.0f;
    [SerializeField] private float teleMoveShakeX = 5.0f;
    [SerializeField] private float teleMoveShakeY = 5.0f;
    [SerializeField] private bool canTele = true;
    [SerializeField] LayerMask teleMask;
    
    private float timeAtTele = 0;
    public bool isTeleporting = false; //sorry its 3:07 am

    public static event Action<PlayerController> OnPlayerCandyChanged;

    Vector2 inputVector;

    private void Awake()
    {
        maxCandy = LevelManager.Instance.GetMaxCarriedTreats(); 
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
        if (anim == null) anim = GetComponentInChildren<Animator>(); 
    }

    private void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        bool isMoving = inputVector.magnitude > 0;
        if (isMoving && walkingSource.isPlaying == false)
        {
            int rand = Random.Range(0, walkingSounds.Length);
            walkingSource.clip = walkingSounds[rand];
            walkingSource.Play();
        }
            

        if (!isMoving && walkingSource.isPlaying == true)
            walkingSource.Stop();

        anim.SetFloat("hValue", inputVector.x); //Walking left and right
        anim.SetFloat("vValue", inputVector.y); //Walking forward

        if (inputVector.x < 0) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true; 

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddCandy(20);
        }

        if (Input.GetMouseButtonUp(0) && canTele)
        {
            //if we are eligable to teleport - cooldown or currently teleporting
            if (Time.time < timeAtTele + teleDelay || isTeleporting) return;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, 500, teleMask))
            {
                //destination in teleport radius ?
                if (Vector3.Distance(hit.point, rb.position) > teleRadius) return;

                StartCoroutine(BeginTeleport(hit.point));
            }
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

    public float GetCandyRatio()
    {
        return (float)currentCandy / (float)maxCandy;
    }

    public Vector2 GetInputVector()
    {
        return inputVector;

    }

    private IEnumerator BeginTeleport(Vector3 destination)
    {
        isTeleporting = true;

        float tempDuration = 0;

        Vector3 tempPos = transform.position;

        while (tempDuration <= teleSinkDuration)
        {
            transform.position = Vector3.Lerp(tempPos, new Vector3(tempPos.x, tempPos.y - 10.0f, tempPos.z), tempDuration / teleSinkDuration);

            Vector3 xAxiNoise = Vector3.right * (UnityEngine.Random.Range(-teleMoveShakeX, teleMoveShakeY) * Time.deltaTime);

            transform.Translate(xAxiNoise);
          
            tempDuration += Time.deltaTime;

            yield return null;
        }

        transform.position = new Vector3(destination.x, transform.position.y, destination.z);

        tempPos = transform.position;

        //uncomment to make camera move when player enters underground.
        //isTeleporting = false;

        tempDuration = 0;

        while (tempDuration <= teleRiseDuration)
        {

            //Debug.Log("manipulating tform BUT IN REVERSE...");

            transform.position = Vector3.Lerp(tempPos, destination, tempDuration / teleRiseDuration);
            
            tempDuration += Time.deltaTime;

            yield return null;
        }

        //Debug.Log("End of teleport");
        isTeleporting = false;

    }
}
