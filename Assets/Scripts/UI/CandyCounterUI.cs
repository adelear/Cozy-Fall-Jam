using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CandyCounterUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI candyText;
    [SerializeField] private TextMeshProUGUI monsterCandyText;
    [SerializeField] private Image candyPlayerFill;
    [SerializeField] private Image candyMonsterFill;
    [SerializeField] private Image candyImage;

    [SerializeField] private float wobbleTime = 0.3f;
    [SerializeField] private float candyWobbleScale = 1.2f;
    [SerializeField] private AnimationCurve wobbleCurve;
    [SerializeField] private AudioClip candyWobbleSFX;

    private bool isWobbling = false;

    private void Start()
    {
        PlayerController.OnPlayerCandyChanged += Player_CandyChangedCallback;

        var data = LevelManager.Instance.GetCurrentLevelData();
        int remaining = data.requiredTreats - LevelManager.Instance.GetCandyGiven();
        monsterCandyText.text = $"{LevelManager.Instance.GetCandyGiven()} / {data.requiredTreats} Candy Given";

        candyMonsterFill.fillAmount = 0;
        candyPlayerFill.fillAmount = 0;
    }

    private void Player_CandyChangedCallback(PlayerController player)
    {
        var data = LevelManager.Instance.GetCurrentLevelData();
        float ratio = LevelManager.Instance.GetCandyGiven() / data.requiredTreats;

        candyPlayerFill.fillAmount = player.GetCandyRatio();
        candyMonsterFill.fillAmount = ratio;

        monsterCandyText.text = $"{LevelManager.Instance.GetCandyGiven()} / {data.requiredTreats} Candy Given";
        //monsterCandyText.text = $"Remaining Candy: {remaining}";

        //candyText.text = player.GetCurrentCandy().ToString();
        //if (isWobbling == false)
        //    StartCoroutine(HandleCandyEffect());
    }

    private IEnumerator HandleCandyEffect()
    {
        isWobbling = true;

        if (candyWobbleSFX != null)
            AudioManager.Instance.PlayAudioSFX(candyWobbleSFX);

        float timeElapsed = 0.0f;
        while (timeElapsed <= wobbleTime)
        {
            timeElapsed += Time.deltaTime;
            float t = wobbleCurve.Evaluate(timeElapsed / wobbleTime);
            candyImage.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * candyWobbleScale, t);

            yield return null;
        }

        isWobbling = false;

        
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerCandyChanged -= Player_CandyChangedCallback;
    } 
}