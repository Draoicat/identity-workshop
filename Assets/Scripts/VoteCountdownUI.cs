using System.Globalization;
using TMPro;
using UnityEngine;

public class VoteCountdownUI : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    private TMP_Text _text;

    private bool _countdownRunning = false;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        eventManager.OnCountdownStarted += StartCountdown;
        eventManager.OnVoteStarted += StopCountdown;
        _text.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        eventManager.OnCountdownStarted -= StartCountdown;
        eventManager.OnVoteStarted -= StopCountdown;
    }

    private void StartCountdown()
    {
        _text.gameObject.SetActive(true);
        _countdownRunning = true;
    }

    private void StopCountdown(Event gameEvent)
    {
        _text.gameObject.SetActive(false);
        _countdownRunning = false;
        _timeSpent = 0f;
    }

    public const float COUNTDOWN_TIME = 3f;
    private float _timeSpent = 0f;

    private void Update()
    {
        if (!_countdownRunning) return;
        _text.text = (COUNTDOWN_TIME - _timeSpent).ToString("F0", CultureInfo.InvariantCulture);
        _timeSpent += Time.deltaTime;
    }
}
