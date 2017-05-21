using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour
{
    public Transform Grid = null;
    private Animator _anim = null;

    private float _currentMagnitude = 0.0f;
    public float MaxMagnitude = 3.0f;
    public float MagnitudeDecrease = 0.2f;

    private Vector2 _currentPosition = Vector2.zero;

	// Use this for initialization
	void Awake ()
    {
        _anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateShake();
    }
    void UpdateShake()
    {
        _currentMagnitude -= MagnitudeDecrease;
        if (_currentMagnitude < 0)
            return;
        float x = _currentMagnitude * (float)Random.Range(-1, 2);
        float y = _currentMagnitude * (float)Random.Range(-1, 2);
        _currentPosition = new Vector2(x, y);
        Grid.position = _currentPosition;
    }

    public void Shake()
    {
        _currentMagnitude = MaxMagnitude;
    }

    public void Pulse()
    {
        _anim.Play("Pulse");
    }
}
