using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _startingSeconds = 60;

    int _score = 0;
    float _secondsLeft;



    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _secondsLeft = _startingSeconds;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _secondsLeft -= Time.fixedDeltaTime;

        if(_secondsLeft <= 0)
        {
            //GameOver
            _secondsLeft = 0;
            UpdateTimer();
            return;
        }

        UpdateTimer();
    }

    public void IncreaseScore(int amount = 1)
    {
        _score += amount;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _timerText.text = _score.ToString();
    }

    private void UpdateTimer()
    {
        int seconds = Mathf.FloorToInt(_secondsLeft % 60);
        int minutes = Mathf.FloorToInt(_secondsLeft / 60);

        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
