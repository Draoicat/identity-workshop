using System;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] events;
    
    public Action<Event> OnEventStarted { get; set; }
    
    public Action OnCountdownStarted { get; set; }
    
    public Action<Event> OnVoteStarted { get; set; }
    
    public Action<Event> OnVoteEnded { get; set; }
    public Action<Event> OnEventFinished { get; set; }

    public int CurrentEventIndex { get; private set; } = 0;
    public Event CurrentEvent => events[CurrentEventIndex];

    private void Start()
    {
        StartEvent(events[CurrentEventIndex]);
    }

    private void StartEvent(Event gameEvent)
    {
        OnEventStarted?.Invoke(gameEvent);
        Debug.Log("Starting Event : " + gameEvent);
    }

    public void StartVote()
    {
        StartCoroutine(VoteCountdown());
        Debug.Log("Starting Vote of : " + events[CurrentEventIndex]);
    }

    private IEnumerator VoteCountdown()
    {
        OnCountdownStarted?.Invoke();
        yield return new WaitForSeconds(3f);
        StartCoroutine(WaitForVotingTime(events[CurrentEventIndex]));
    }
    
    private IEnumerator WaitForVotingTime(Event gameEvent)
    {
        OnVoteStarted?.Invoke(gameEvent);
        yield return new WaitForSeconds(gameEvent.VotingTime);
        Debug.Log("Finished Vote of : " + gameEvent);
        OnVoteEnded?.Invoke(gameEvent);
    }

    public void EndEvent()
    {
        Debug.Log("Ending Event : " + CurrentEvent);
        OnEventFinished?.Invoke(CurrentEvent);
        CurrentEventIndex++;
        if (CurrentEventIndex < events.Length) StartEvent(CurrentEvent);
        else Debug.Log("No More Events");
    }
}
