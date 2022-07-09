using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Configuration;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Niantic.ARDK.AR.ARSessionEventArgs;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool MinigameActive { get; private set; }

    [SerializeField] private GameObject[] _minigameObjects, _objectsToDisableAtGameOver;
    [SerializeField] private GameObject _completedPrefab, _timerIncreaseObject, _gameOverObject;
    [SerializeField] private RectTransform _walkingIcon, _walkingIconEnd, _gameIcon, _distanceTransform;
    [SerializeField] private TextMeshProUGUI _scoreText, _timerText, _instructionText, _promptText, _endScore;
    [SerializeField] private int _startingSeconds = 60;
    [SerializeField] private float _minDistance = 30;
    [SerializeField] private string _gameInstruction = "Minigame: ", _searchInstruction = "Find: ";
    [SerializeField] private Image _completedIcon;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _spawnAudio, _winAudio, _failAudio;

    private int _score = 0;
    private float _secondsLeft;
    private Transform _cameraTransform;
    private LayerDetection _layerDetection;
    private Vector2 _startPos, _endPos;

    private List<Vector3> previousPositions = new List<Vector3>();

    float _distanceOffset;
    Image _offsetImage;
    [SerializeField] Image _targetImage;

    Vector3 _lastMinigamePos;
    Light _light;
    IARSession _session;
    void Awake()
    {
        Instance = this;
        _gameIcon.gameObject.SetActive(false);
        _timerIncreaseObject.SetActive(false);
    }

    private void Start()
    {
        MinigameActive = false;
        _secondsLeft = _startingSeconds;
        _cameraTransform = Camera.main.transform;
        _layerDetection = FindObjectOfType<LayerDetection>();
        _distanceOffset = _distanceTransform.localScale.y;
        _offsetImage = _distanceTransform.GetComponentInChildren<Image>();
        _startPos = _walkingIcon.position;
        _endPos = _walkingIconEnd.position;
        _light = GameObject.FindObjectOfType<Light>();
        ARSessionFactory.SessionInitialized += OnAnyARSessionDidInitialize;
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        _session = args.Session;
        _session.FrameUpdated += UpdateLighting;
    }

    void UpdateLighting(FrameUpdatedArgs args)
    {
        var light = args.Frame.LightEstimate;
        if (light == null) return;
        _light.intensity = light.AmbientIntensity;
        if (Application.platform == RuntimePlatform.Android) _light.color = new Color(light.ColorCorrection[0], light.ColorCorrection[1], light.ColorCorrection[2]);
        else if (Application.platform == RuntimePlatform.IPhonePlayer) _light.color = Mathf.CorrelatedColorTemperatureToRGB(light.AmbientColorTemperature);
        else Debug.LogWarning("No platform match, platform is " + Application.platform);
    }

    private void OnDestroy()
    {
        _session.FrameUpdated -= UpdateLighting;
    }
    void FixedUpdate()
    {
        _secondsLeft -= Time.fixedDeltaTime *1.02f;

        if(_secondsLeft <= 0)
        {
            _secondsLeft = 0;
            _gameOverObject.SetActive(true);
            _endScore.text = _score.ToString();
            PlaySFX(_failAudio);

            foreach (var go in _objectsToDisableAtGameOver) go.SetActive(false);
            GetComponent<LayerDetection>().enabled = false;
            enabled = false;

            UpdateTimer();
            return;
        }

        UpdateTimer();
        UpdateDistanceMeter();
    }

    private void UpdateDistanceMeter()
    {
        if (!_distanceTransform || previousPositions.Count == 0 || MinigameActive)
        {
            _distanceTransform.localScale = new Vector3(_distanceTransform.localScale.x,
                                               _distanceOffset,
                                               _distanceTransform.localScale.z);
            _offsetImage.color = Color.green;
            return;
        }

        float minDistance = GetMinimumDistance();

        float distancePercent = minDistance / _minDistance;

        _distanceTransform.localScale = new Vector3(_distanceTransform.localScale.x, 
                                                Mathf.Clamp(distancePercent * _distanceOffset,0, _distanceOffset), 
                                                _distanceTransform.localScale.z);


        _offsetImage.color = Color.Lerp(Color.red, Color.green, distancePercent);

        _walkingIcon.position = (distancePercent * (_endPos - _startPos)) + _startPos;
        if (_walkingIcon.position.x > _endPos.x) _walkingIcon.position = _endPos;
        if (distancePercent > 0.98)
        {
            _completedIcon.gameObject.SetActive(true);
        }
        else
        {
            _completedIcon.gameObject.SetActive(false);
        }
    }
    public async void IncreaseScore(int amount = 1)
    {
        MinigameActive = false;
        _score += amount;

        _layerDetection.GotoNextLayer();

        //Show high line with time completed?

        UpdateScore();
        SpawnCompletedNote();
        _timerIncreaseObject.SetActive(false);
        _timerIncreaseObject.SetActive(true);
        _completedIcon.gameObject.SetActive(false);
        _minDistance *= 1.04f;
        await Task.Delay(700);

        AddTime(60);

        PlaySFX(_winAudio);
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

    public void SpawnMinigame(Vector3 position)
    {
        if (MinigameActive) return;

        if (_minigameObjects.Length == 0)
        {
            Debug.LogError("No minigames founnd!");
            return;
        }

        if (GetMinimumDistance() < _minDistance * 0.96)
        {

            Debug.Log("too close");

            //TODO: Show warning you need to move further?

            return;

        }

        var go = Instantiate(_minigameObjects[Random.Range(0, _minigameObjects.Length)]);
        go.transform.position = position;
        go.transform.LookAt(_cameraTransform);

        previousPositions.Add(position);
        _lastMinigamePos = position;

        MinigameActive = true;

        SetInstruction(_gameInstruction);
        SetPrompt(go.GetComponent<MiniExplanation>().MinigameInfo, null);
        _gameIcon.gameObject.SetActive(true);
        _walkingIcon.gameObject.SetActive(false);

        PlaySFX(_spawnAudio);
    }

    private void SpawnCompletedNote()
    {
        var go = Instantiate(_completedPrefab);
        go.transform.position = _lastMinigamePos;

        go.GetComponentInChildren<TextMeshPro>().text = "Completed:\n" + System.DateTime.Now.ToString("t");
    }

    private float GetMinimumDistance()
    {
        float minDistance = float.MaxValue;
        float distance;
        foreach (var pos in previousPositions)
        {
            distance = Vector3.Distance(pos, _cameraTransform.position);
            minDistance = distance < minDistance ? distance : minDistance;
        }

        return minDistance;
    }

    public void SetPrompt(string text, Sprite sprite)
    {
        _promptText.text = text;
        if(sprite)
        {
            _targetImage.sprite = sprite;
            SetInstruction(_searchInstruction);
        }
        _gameIcon.gameObject.SetActive(false);
        _walkingIcon.gameObject.SetActive(true);
    }

    public void SetInstruction(string text)
    {
        _instructionText.text = text;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaySFX(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}