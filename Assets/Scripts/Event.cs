using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Event/EventAsset", order = 1)]
public class Event : ScriptableObject
{
    [SerializeField] private string[] eventText;
    [SerializeField] private string eventSummary;
    [SerializeField] private int votingTime;

    [SerializeField] private string[] votes = new string[5];
    
    public string GetEventTextLine(int line) => eventText[line];
    public string GetEventResult(Parties party) => votes[(int) party];
    public int GetEventTextLineCount => eventText.Length;
    public string EventSummary => eventSummary;
    public int VotingTime => votingTime;
}
