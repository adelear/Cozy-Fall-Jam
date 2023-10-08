using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image dialogueContainer;
    [SerializeField] private DialogueBubble dialogueBubble;

    [SerializeField] private Color monsterImgColor;
    [SerializeField] private Color ghostImgColor;
    [SerializeField] private Color monsterTextColor;
    [SerializeField] private Color ghostTextColor;

    [SerializeField] private Dialogue[] Dialogues;

    private void OnEnable()
    {
        StartCoroutine(DoDialogue());
    }

    private IEnumerator DoDialogue()
    {
        foreach (Dialogue d in Dialogues)
        {
            dialogueContainer.color = d.ghostDialogue ? ghostImgColor : monsterImgColor;
            dialogueText.color = d.ghostDialogue ? ghostTextColor : monsterTextColor;

            yield return dialogueBubble.HandleDialogue(d);
        }

        yield return null;
        LevelManager.Instance.LoadNextLevel();
    }
}
