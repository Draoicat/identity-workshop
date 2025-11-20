using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Solution
{
    public string solution;
    public Parties[] supportingParties;
}

[CreateAssetMenu(fileName = "Data", menuName = "Event/EventAsset", order = 1)]
public class Event : ScriptableObject
{
    [SerializeField] private string[] eventText;
    [SerializeField] private string eventSummary;
    [SerializeField] private int votingTime;

    [SerializeField] private Solution[] solutions;
    

    [SerializeField] private Sprite illustration;
    
    
    
    public string GetEventTextLine(int line) => eventText[line];
    public int GetEventTextLineCount => eventText.Length;
    public string EventSummary => eventSummary;
    public int VotingTime => votingTime;

    public Sprite Illustration => illustration;
}
