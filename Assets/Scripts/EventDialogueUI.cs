using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventDialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    private TMP_Text dialogueText;
    private Button continueButton;

    private void Awake()
    {
        continueButton = GetComponentInChildren<Button>();
        dialogueText = dialogue.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        continueButton.gameObject.SetActive(false);
        dialogue.SetActive(false);
        EventManager.Instance.OnEventStarted += ActivateDialogue;
        EventManager.Instance.OnVoteStarted += StartVote;
        EventManager.Instance.OnVoteEnded += EndVote;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnEventStarted -= ActivateDialogue;
        EventManager.Instance.OnVoteStarted -= StartVote;
        EventManager.Instance.OnVoteEnded -= EndVote;
    }
    
    private void ActivateDialogue(Event gameEvent)
    {
        dialogue.SetActive(true);
        continueButton.gameObject.SetActive(true);
        dialogueText.text = gameEvent.GetEventTextLine(currentLine);
    }

    private int currentLine = 0;

    public void PassLine()
    {
        ++currentLine;
        if (currentLine < EventManager.Instance.CurrentEvent.GetEventTextLineCount)
            ActivateDialogue(EventManager.Instance.CurrentEvent);
        else
            EventManager.Instance.StartVote();
    }

    private void StartVote(Event gameEvent)
    {
        currentLine = 0;
        continueButton.gameObject.SetActive(false);
        dialogueText.text = gameEvent.EventSummary;
    }

    private void EndVote(Event gameEvent)
    {
        continueButton.gameObject.SetActive(false);
        dialogue.SetActive(false);
    }
}
