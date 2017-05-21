using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    public float IntroTimer = 5.0f;
    private float _currentTimer = 0;

	void Start ()
    {
        _currentTimer = IntroTimer;
	}
	
	void Update ()
    {
        _currentTimer -= Time.deltaTime;
        if(_currentTimer <= 0)
        {
            GetComponent<Fading>().BeginFade(1);
            StartCoroutine(GoToLevel());
        }
	}

    IEnumerator GoToLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
