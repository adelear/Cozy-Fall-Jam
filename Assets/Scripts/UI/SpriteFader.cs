using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite spriteA;
    [SerializeField] private Sprite spriteB;

    [SerializeField] private bool useSpriteA = true; 
    [SerializeField] private DoorInteraction doorInteraction;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateHouseSprite();
    }

    private void UpdateHouseSprite()
    {
        if (doorInteraction.GetCanAskforCandy()) useSpriteA = true; 
        else useSpriteA = false; // Set to spriteB
        sr.sprite = useSpriteA ? spriteA : spriteB;
    }

    private void Update()
    {
        UpdateHouseSprite();
    }
}
