using Core;
using UnityEngine;
using Event = Core.Event;

public class NextEventButton : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.OnEventStarted += DesactivateButton;
        EventManager.Instance.OnEventFinished += ActivateButton;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnEventStarted -= DesactivateButton;
        EventManager.Instance.OnEventFinished -= ActivateButton;
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
