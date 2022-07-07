using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool MinigameActive {get; private set;}

    [SerializeField] private GameObject[] _minigameObjects;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _startingSeconds = 60;
    [SerializeField] private float _minDistance = 20;

    private int _score = 0;
    private float _secondsLeft;
    private Transform _cameraTransform;
    private LayerDetection _layerDetection;

    private List<Vector3> previousPositions = new List<Vector3>();

    [SerializeField] RectTransform _distanceTransform;
    float _distanceOffset;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MinigameActive = false;
        _secondsLeft = _startingSeconds;
        _cameraTransform = Camera.main.transform;
        _layerDetection = FindObjectOfType<LayerDetection>();
        _distanceOffset = _distanceTransform.localScale.y;
    }

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

    private void UpdateDistanceMeter()
    {
        if (!_distanceTransform || previousPositions.Count == 0) return;

        float minDistance = 0;
        float distance;
        foreach (var pos in previousPositions)
        {
            distance = Vector3.Distance(pos, _cameraTransform.position);
            minDistance = distance < minDistance ? minDistance : distance;
        }
        _distanceTransform.localScale = new Vector3(_distanceTransform.localScale.x,  (minDistance / _minDistance) * _distanceOffset, _distanceTransform.localScale.z);
    }
    public void IncreaseScore(int amount = 1)
    {
        MinigameActive = false;
        _score += amount;

        _layerDetection.GotoNextLayer();

        //Show high line with time completed?

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
        _scoreText.text = _score.ToString();
    }

    private void UpdateTimer()
    {
        int seconds = Mathf.FloorToInt(_secondsLeft % 60);
        int minutes = Mathf.FloorToInt(_secondsLeft / 60);

        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void spawnMinigame(Vector3 position)
    {
        if (MinigameActive) return;

        if (_minigameObjects.Length == 0)
        {
            Debug.LogError("No minigames founnd!");
            return;
        }

        foreach(var pos in previousPositions)
        {
            if(Vector3.Distance(pos, _cameraTransform.position) < 5)
            {
                Debug.Log("too close");

                //TODO: Show warning you need to move further?

                return;
            }
        }

        var go = Instantiate(_minigameObjects[Random.Range(0, _minigameObjects.Length)]);
        go.transform.position = position;
        go.transform.LookAt(_cameraTransform);

        previousPositions.Add(position);

        MinigameActive = true;
    }
}
