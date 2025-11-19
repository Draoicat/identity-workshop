using System;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Event[] events;
    
    public Action<Event> OnEventStarted { get; set; }
    
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
        StartCoroutine(WaitForVotingTime(events[CurrentEventIndex]));
        Debug.Log("Starting Vote of : " + events[CurrentEventIndex]);
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
    
    /*public const int STARTING_HOUR = 8;

    public const float MINUTE_TIME = 0.4f; //in game time speed
    public const int END_TIME = 1000;

    public float TimeSinceLastStep { get; private set; }
    public int TimeStep { get; private set; }

    public bool IsTimeRunning { get; private set; } = true;

    public int Hour => TimeStep / 60 + STARTING_HOUR;
    public int Minutes => TimeStep % 60;
    public float CurrentMinuteProgress => TimeSinceLastStep / MINUTE_TIME;

    public Event[] events;

    private void Start()
    {
        StartTime();
    }

    private void Update()
    {
        if (!IsTimeRunning) return;

        TimeSinceLastStep += Time.deltaTime;
        if (TimeSinceLastStep >= MINUTE_TIME)
        {
            TimeSinceLastStep = 0;
            TimeStep++;
            Debug.Log("Current Time : " + Hour + ":" + Minutes);
        }
        WatchEvents();
    }

    public Action<Event> OnEventStart { get; set; }

    private void WatchEvents()
    {
        foreach (var gameEvent in events)
        {
            if (TimeStep == gameEvent.Time)
            {
                OnEventStart?.Invoke(gameEvent);
                StopTime();
            }
        }
    }

    public void StartTime()
    {
        IsTimeRunning = true;
    }

    public void StopTime()
    {
        IsTimeRunning = false;
    }*/
}
