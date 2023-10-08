using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float hangTime = 0.3f;

    public IEnumerator HandleDialogue(Dialogue dialogue)
    {
        dialogueText.text = "";

        rectTransform.gameObject.SetActive(true);

        for (int i = 0; i < dialogue.dialogueText.Length; i++)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                dialogueText.text = dialogue.dialogueText;
                break;
            }
                

            dialogueText.text += dialogue.dialogueText[i];
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(hangTime);
        rectTransform.gameObject.SetActive(false);
    }
}
