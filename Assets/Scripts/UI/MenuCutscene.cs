using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCutscene : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform bgRect;
    [SerializeField] private RectTransform ghostRect;
    [SerializeField] private RectTransform monsterRect;
    [SerializeField] private DialogueBubble ghostBubble;
    [SerializeField] private DialogueBubble monsterBubble;

    [SerializeField] private Dialogue[] initialDialogue;
    [SerializeField] private Dialogue[] finalDialogue;
    [SerializeField] private float backgroundScrollTime = 0.4f;
    [SerializeField] private float ghostScaleTime = 0.2f;

    private void OnEnable()
    {
        StartCoroutine(HandleCutscene());
    }

    private IEnumerator HandleCutscene()
    {

        float timeElapsed = 0.0f;

        Vector3 start = bgRect.anchoredPosition;
        Vector3 end = -start;

        while (timeElapsed <= backgroundScrollTime)
        {
            timeElapsed += Time.deltaTime;
            bgRect.anchoredPosition = Vector3.Lerp(start, end, timeElapsed / backgroundScrollTime);

            yield return null;
        }

        timeElapsed = 0.0f;

        ghostRect.localScale = Vector3.zero;
        ghostRect.gameObject.SetActive(true);

        while (timeElapsed <= ghostScaleTime)
        {
            timeElapsed += Time.deltaTime;
            ghostRect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / ghostScaleTime);
            yield return null;
        }

        yield return DoDialogue(initialDialogue);

        timeElapsed = 0.0f;

        monsterRect.localScale = Vector3.zero;
        monsterRect.gameObject.SetActive(true);

        while (timeElapsed <= ghostScaleTime)
        {
            timeElapsed += Time.deltaTime;
            monsterRect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timeElapsed / ghostScaleTime);
            yield return null;
        }

        yield return DoDialogue(finalDialogue);

        yield return null;

        SceneTransitionManager.Instance.LoadScene("Level 1");
    }

    public IEnumerator DoDialogue(Dialogue[] dialogue)
    {
        foreach(Dialogue d in dialogue)
        {
            if (d.ghostDialogue)
                yield return ghostBubble.HandleDialogue(d);

            else
                yield return monsterBubble.HandleDialogue(d);
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public bool ghostDialogue;
    [TextArea(2, 5)] public string dialogueText;

}
