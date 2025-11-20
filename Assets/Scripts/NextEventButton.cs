using UnityEngine;

public class NextEventButton : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    private void Start()
    {
        eventManager.OnEventStarted += DesactivateButton;
        eventManager.OnEventFinished += ActivateButton;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        eventManager.OnEventStarted -= DesactivateButton;
        eventManager.OnEventFinished -= ActivateButton;
    }

    private void ActivateButton(Event _)
    {
        gameObject.SetActive(true);
    }

    private void DesactivateButton(Event _)
    {
        gameObject.SetActive(false);
    }
}
