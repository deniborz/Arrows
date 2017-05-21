using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private PoolManager _pool = null;
    private PlayerScript _player = null;
    private MenuScript _menu = null;
    private BackgroundScript _background = null;
    public SpawnerScript _spawn = null;
    public ScoreboardScript _score = null;
    public CreditBoardScript _credit = null;

    public int MaxAmountOfLives = 3;
    public int LevelsPerLifeBonus = 5;

    private int _currentLevel = 1; // only should be changed by the level manager
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
    }

    public float LevelProgressionScore = 200;

    private int _currentScore = 0;
    public int CurrentScore
    {
        get
        {
            return _currentScore;
        }
    }

    private int _currentCredits = 0;
    public int CurrentCredits
    {
        get
        {
            return _currentCredits;
        }
    }

    public float StartDelay = 0.5f;
    private bool _isPlaying = false;
    public bool IsPlaying
    {
        get
        {
            return _isPlaying;
        }
    }

    private float _currentTimer = 0.0f;
    public float CurrentTimer
    {
        get
        {
            return _currentTimer;
        }
    }
    public float PlayerTimerReset = 4.0f;
    public GameObject TimerObject = null;
    private Animator _timerAnim = null;

    public Text TimerText = null;
    public Text ScoreText = null;
    private Animator _scoreAnim = null;

    private float _currentSpawnDelay = 0.0f;
    public float CurrentSpawnDelay
    {
        get
        {
            return _currentSpawnDelay;
        }
    }
    public float SpawnDelay = 0.5f;
    public float MinimumSpawnDelay = 0.2f;
    public float SpawnDelayDecreaseAmount = 0.05f;

    private int _sweetCount = 0;
    private int _coolCount = 0;
    private int _awesomeCount = 0;
    private int _majesticCount = 0;
    private int _onfireCount = 0;
    private int _godlikeCount = 0;
    private int _cheatCount = 0;
    private int _soCloseCount = 0;
    private int _superCloseCount = 0;
    private int _wtfCount = 0;

    void Awake()
    {
        _pool = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolManager>();
        _menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuScript>();
        _background = GameObject.FindGameObjectWithTag("Background").GetComponent<BackgroundScript>();
        _spawn = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>();
        _score = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardScript>();
        _credit = GameObject.FindGameObjectWithTag("Creditboard").GetComponent<CreditBoardScript>();
    }

    void Start ()
    {

        _currentTimer = PlayerTimerReset;
        _currentSpawnDelay = SpawnDelay;
        if (TimerText != null)
            _timerAnim = TimerObject.GetComponent<Animator>();
        if (ScoreText != null)
            _scoreAnim = ScoreText.GetComponent<Animator>();
        StartCoroutine(StartLevel());

        _credit.CurrentAmountOfCredits = _currentCredits;
    }

	void Update ()
    {
	    if(_isPlaying)
            _currentTimer -= Time.deltaTime;

        if (_currentTimer <= 0.0f && _isPlaying)
        {
            _isPlaying = false;
            _player.Kill();
            StartCoroutine(StopLevel());
        }
        
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        if (ScoreText != null)
            ScoreText.text = CurrentScore.ToString();
    }

    public void AddScore(int amount, VisualType type)
    {
        if (amount <= 0)
            return; // just ignore these values

        StartCoroutine(PushScore(amount, type));

        int nextLevelScore = (int)(LevelProgressionScore * Mathf.Pow(_currentLevel, 2));
        if (_currentScore >= nextLevelScore)
            LevelUp();
    }
    public void AddHitScore(int amount, VisualType type)
    {
        _background.Pulse();
        AddScore(amount * CurrentLevel, type);
        _currentTimer = PlayerTimerReset; // add for having strategy
        if (_timerAnim != null)
            _timerAnim.Play("Countdown", -1, 0f);
    }

    IEnumerator PushScore(int amount, VisualType type)
    {
        if (_scoreAnim != null)
            _score.PushVisual(amount, type);
        yield return new WaitForSeconds(0.25f);
        _currentScore += amount;
        if (_scoreAnim != null && amount > 1)
            _scoreAnim.Play("Bump", -1, 0f);
    }

    void AddComboCount(VisualType type)
    {
        switch (type)
        {
            case VisualType.SWEET:
                ++_sweetCount;
                break;
            case VisualType.COOL:
                ++_coolCount;
                break;
            case VisualType.AWESOME:
                ++_awesomeCount;
                break;
            case VisualType.MAJESTIC:
                ++_majesticCount;
                break;
            case VisualType.ONFIRE:
                ++_onfireCount;
                break;
            case VisualType.GODLIKE:
                ++_godlikeCount;
                break;
            case VisualType.CHEAT:
                ++_cheatCount;
                break;

            case VisualType.SOCLOSE:
                ++_soCloseCount;
                break;
            case VisualType.SUPERCLOSE:
                ++_superCloseCount;
                break;

            case VisualType.WTF:
                ++_wtfCount;
                break;
        }
    }

    public void AddCredit()
    {
        ++_currentCredits;
        _credit.CurrentAmountOfCredits = _currentCredits;
    }

    void LevelUp()
    {
        ++_currentLevel; // level up
        switch(_currentLevel)
        {
            case 2:
                _spawn.UnlockLine();
                _spawn.SpawnCredit();
                break;
            case 3:
                _spawn.SpawnCredit();
                break;
            case 4:
                _spawn.UnlockLine();
                _spawn.SpawnCredit();
                break;
        }

        //Debug.Log("Leveled up to:" + _currentLevel);
        _currentSpawnDelay -= SpawnDelayDecreaseAmount;
        if (_currentSpawnDelay <= MinimumSpawnDelay)
            _currentSpawnDelay = MinimumSpawnDelay;
        //if (_currentLevel % LevelsPerLifeBonus == 0)
        //    GainLife(); // SHOULD BE CHANGED INTO COINS
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(StartDelay);

        _isPlaying = true;
        if (_timerAnim != null)
        {
            // is the one for the text, not used anymore
            //_timerAnim.Play("Bump");
            _timerAnim.Play("Countdown");
        }

        StartCoroutine(ScorePoints());
    }
    IEnumerator ScorePoints()
    {
        while(_isPlaying)
        {
            yield return new WaitForSeconds(1.0f);
            AddScore(1, VisualType.NONE);
        }
    }

    public void LoseLife()
    {
        _player.LoseLife();
        _background.Shake();

        if (_player.RemainingLives == 0)
            StartCoroutine(StopLevel());
    }
    public void LoseLife(Vector3 collisionPos)
    {
        PoolObject po = _pool.ActivateObject("BlockHitParticles");
        if (po == null)
            Debug.Log("No particles");
        Vector3 pos = collisionPos;
        pos.z = -3;
        po.transform.position = pos;
        po.GetComponent<ParticleScript>().Fire();
        LoseLife();
    }
    IEnumerator StopLevel()
    {
        _isPlaying = false;
        _menu.ShowMenu();
        _background.Shake();
        List<PoolObject> bulletPool = _pool.GetAllActiveObjects("Bullet");

        foreach(PoolObject b in bulletPool)
        {
            PoolObject po = _pool.ActivateObject("BlockHitParticles");
            Vector3 pos = b.transform.position;
            pos.z = -3;
            po.transform.position = pos;
            po.GetComponent<ParticleScript>().Fire();
            Rigidbody2D rigid = b.GetComponent<Rigidbody2D>();
            rigid.AddForce(new Vector2(Random.Range(-5000, 5000), 0));
            rigid.AddTorque(Random.Range(-360, 360), ForceMode2D.Impulse);       
        }
        yield return new WaitForSeconds(0.5f);
        bulletPool.ForEach(e => e.Deactivate()); // clean call do destroy all bullets
        _timerAnim.Play("PlayerDied");
    }
    public void GainLife()
    {
        if (_player.RemainingLives < 3)
        {
            _player.GainLife();
            // maybe some animation call
        }
    }

    public void SetPlayer(PlayerScript player)
    {
        _player = player;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    void SavePlayer()
    {
        int maxScore = 0;
        if(PlayerPrefs.HasKey("MaxScore"))
            maxScore = PlayerPrefs.GetInt("MaxScore");

        if(maxScore <= _currentScore)
        {
            PlayerPrefs.SetInt("MaxScore", _currentScore);
            if(_currentScore >= maxScore * 1.2f)
            {
                // give extra credit
            }
        }
        PlayerPrefs.SetInt("Credits", _currentCredits);

        int currentCount = 0;
        currentCount = PlayerPrefs.GetInt("SweetCount");
        PlayerPrefs.SetInt("SweetCount", currentCount + _sweetCount);
        currentCount = PlayerPrefs.GetInt("CoolCount");
        PlayerPrefs.SetInt("CoolCount", currentCount + _coolCount);
        currentCount = PlayerPrefs.GetInt("AwesomeCount");
        PlayerPrefs.SetInt("AwesomeCount", currentCount + _awesomeCount);
        currentCount = PlayerPrefs.GetInt("MajesticCount");
        PlayerPrefs.SetInt("MajesticCount", currentCount + _majesticCount);
        currentCount = PlayerPrefs.GetInt("OnFireCount");
        PlayerPrefs.SetInt("OnFireCount", currentCount + _onfireCount);
        currentCount = PlayerPrefs.GetInt("GodlikeCount");
        PlayerPrefs.SetInt("GodlikeCount", currentCount + _godlikeCount);
        currentCount = PlayerPrefs.GetInt("CheatCount");
        PlayerPrefs.SetInt("CheatCount", currentCount + _cheatCount);
    }
}
