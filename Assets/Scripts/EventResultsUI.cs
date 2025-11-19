using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventResultsUI : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private GameObject resultWindow;
    private TMP_Text dialogueText;
    private Button continueButton;

    private void Start()
    {
        eventManager.OnVoteEnded += PrintResults;
        dialogueText = resultWindow.gameObject.GetComponentInChildren<TMP_Text>();
        continueButton = GetComponentInChildren<Button>();
        
        resultWindow.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        eventManager.OnVoteEnded -= PrintResults;
    }

    private void PrintResults(Event gameEvent)
    {
        continueButton.gameObject.SetActive(true);
        resultWindow.SetActive(true);
        dialogueText.text = "Résultats : ";
        //todo show result of winning party
    }

    public void EndResults()
    {
        continueButton.gameObject.SetActive(false);
        resultWindow.SetActive(false);
        eventManager.EndEvent();
    }
}
