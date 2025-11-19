using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventDialogueUI : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
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
        eventManager.OnEventStarted += ActivateDialogue;
        eventManager.OnVoteStarted += StartVote;
        eventManager.OnVoteEnded += EndVote;
    }

    private void OnDestroy()
    {
        eventManager.OnEventStarted -= ActivateDialogue;
        eventManager.OnVoteStarted -= StartVote;
        eventManager.OnVoteEnded -= EndVote;
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
        if (currentLine < eventManager.CurrentEvent.GetEventTextLineCount)
            ActivateDialogue(eventManager.CurrentEvent);
        else
            eventManager.StartVote();
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
