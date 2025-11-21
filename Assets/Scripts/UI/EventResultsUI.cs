using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = Core.Event;

namespace UI
{
    public class EventResultsUI : MonoBehaviour
    {
        [SerializeField] private GameObject resultWindow;
        private TMP_Text dialogueText;
        private Button continueButton;
    
        private void Start()
        {
            EventManager.Instance.OnVoteEnded += PrintResults;
            dialogueText = resultWindow.gameObject.GetComponentInChildren<TMP_Text>();
            continueButton = GetComponentInChildren<Button>();
        
            resultWindow.SetActive(false);
            continueButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            EventManager.Instance.OnVoteEnded -= PrintResults;
        }

        private void PrintResults(Event gameEvent)
        {
            continueButton.gameObject.SetActive(true);
            resultWindow.SetActive(true);
            dialogueText.text = Results.Instance.ChosenSolutionForCurrentStep.solution + '\n' + "Le ";
            for (int i = 0; i < Results.Instance.ChosenSolutionForCurrentStep.supportingParties.Length; ++i)
            {
                dialogueText.text += Results.Instance.ChosenSolutionForCurrentStep.supportingParties[i].PartyName();
                if (i != Results.Instance.ChosenSolutionForCurrentStep.supportingParties.Length - 1)
                    dialogueText.text += " et le ";
            }
            if (Results.Instance.ChosenSolutionForCurrentStep.supportingParties.Length > 1)
                dialogueText.text += " ont voté pour cette décision.";
            else
                dialogueText.text += " a voté pour cette décision.";
        }

        public void EndResults()
        {
            continueButton.gameObject.SetActive(false);
            resultWindow.SetActive(false);
            EventManager.Instance.EndEvent();
        }
    }
}
