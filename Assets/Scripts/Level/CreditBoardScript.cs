using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditBoardScript : MonoBehaviour
{
    public Text CreditText = null;
    private Animator _anim = null;

    private int _currentAmountOfCredits = 0;
    public int CurrentAmountOfCredits
    {
        set
        {
            _currentAmountOfCredits = value;
            _showCredits = true;
        }
    }

    private bool _showCredits = false;

    public float TimerInterval = 1.5f;
    private float _currentTimer = 0.0f;

	void Awake ()
    {
        _anim = GetComponent<Animator>();
    }
	
	void Update ()
    {
	    if(_showCredits)
        {
            _showCredits = false;
            _currentTimer = TimerInterval;
            _anim.Play("Show");
            CreditText.text = _currentAmountOfCredits.ToString();
        }
        else if(_currentTimer >= 0)
        {
            _currentTimer -= Time.deltaTime;
            if(_currentTimer <= 0)
            {
                _anim.Play("Hide");
            }
        }
	}
}
