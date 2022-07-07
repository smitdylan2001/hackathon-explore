using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject[] _minigameObjects;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _startingSeconds = 60;

    private int _score = 0;
    private float _secondsLeft;
    private Transform _cameraTransform;


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _secondsLeft = _startingSeconds;
        _cameraTransform = Camera.main.transform;
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
        AddTime(60);
        UpdateScore();
    }

    private void AddTime(int seconds)
    {
        _secondsLeft += seconds;
        UpdateTimer();
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

    public void spawnMinigame(Vector3 position)
    {
        if (_minigameObjects.Length == 0)
        {
            Debug.LogError("No minigames founnd!");
            return;
        }
        var go = Instantiate(_minigameObjects[Random.Range(0, _minigameObjects.Length)]);
        go.transform.position = position;
        go.transform.LookAt(_cameraTransform);
    }

}
