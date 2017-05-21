using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private LevelManager _level = null;
    private PoolManager _pool = null;
    private Animator _anim = null;

    public GameObject BulletSocket = null;
    private PoolObject _loadedBullet = null;

    public float StartReloadDelay = 3.0f;
    public float ReloadDelay = 0.1f;
    private float _currentReloadDelay = 0.0f;

    public float FireDelay = 0.1f;

    private int _remainingLives = 3;
    public int RemainingLives
    {
        get
        {
            return _remainingLives;
        }
    }

    public GameObject Life1 = null;
    public GameObject Life2 = null;
    public GameObject Life3 = null;

	void Awake ()
    {
        _level = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); // fetch the level manager once
        _pool = GameObject.FindGameObjectWithTag("PoolManager").GetComponent<PoolManager>(); // fetch the pool manager once
        _anim = GetComponent<Animator>();

        _level.SetPlayer(this);

        if (_pool == null)
            Debug.Log("ERROR! Pool manager not found"); // just keep this one in

        _currentReloadDelay = StartReloadDelay;
    }
	
	void Update ()
    {
        _currentReloadDelay -= Time.deltaTime;
	    if(_loadedBullet == null && _currentReloadDelay <= 0.0f && _remainingLives > 0)
        {
            _loadedBullet = _pool.ActivateObject("Bullet");
            _loadedBullet.transform.position = BulletSocket.transform.position;
            _loadedBullet.transform.rotation = BulletSocket.transform.rotation;
            _anim.Play("Player_Reload");
        }

        bool fire = false;
        if (_remainingLives <= 0)
            return; // player can't fire anymore

#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && _loadedBullet != null)
            fire = true;
#elif UNITY_ANDROID // currently to test on phone
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
            fire = true;
#endif
        if (fire)
            StartCoroutine("Fire", FireDelay);
	}

    void LateUpdate()
    {
        if (_loadedBullet == null)
            return;

        _loadedBullet.transform.position = BulletSocket.transform.position;
    }

    IEnumerator Fire(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_loadedBullet != null)
        {
            // unlink the bullet
            _loadedBullet.GetComponent<Rigidbody2D>().velocity = 300.0f * _loadedBullet.transform.right;
            _loadedBullet = null; // release bullet

            // fire it off
            _currentReloadDelay = ReloadDelay;
        }
    }

    public void LoseLife()
    {
        if (_remainingLives > 0)
        {
            --_remainingLives;
            // do animation based on the lost life
            int lifeAnim = _level.MaxAmountOfLives - _remainingLives;
            _anim.Play("LoseLife" + lifeAnim);
            if (_loadedBullet != null)
            {
                _loadedBullet.Deactivate();
                _loadedBullet = null;
            }

            StartCoroutine("DisableLife", lifeAnim);
        }
    }
    public void GainLife()
    {
        if (_remainingLives < _level.MaxAmountOfLives)
        {
            ++_remainingLives;
            // do animation based on the lost life
            int lifeAnim = _level.MaxAmountOfLives - _remainingLives;
            _anim.Play("GainLife" + lifeAnim);
            StartCoroutine("EnableLife", lifeAnim);
        }
    }
    IEnumerator DisableLife(int life)
    {
        yield return new WaitForSeconds(0.05f);
        switch (life)
        {
            case 1:
                Life1.SetActive(false);
                break;
            case 2:
                Life2.SetActive(false);
                break;
            case 3:
                Life3.SetActive(false);
                break;
        }
    }
    IEnumerator EnableLife(int life)
    {
        yield return new WaitForSeconds(0.05f);
        switch (life)
        {
            case 1:
                Life1.SetActive(true);
                break;
            case 2:
                Life2.SetActive(true);
                break;
            case 3:
                Life3.SetActive(true);
                break;
        }
    }

    public void Kill()
    {
        _remainingLives = 0;

        if (_loadedBullet != null)
        {
            _loadedBullet.Deactivate();
            _loadedBullet = null;
        }

        StartCoroutine("DisableLife", 3);
    }
}
