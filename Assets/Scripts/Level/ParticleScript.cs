using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem _particle = null;
    public float AnimationTime = 0.5f;
    private float _currentAnimTime = 0.0f;

	// Use this for initialization
	void Awake () {
        _particle = GetComponent<ParticleSystem>();

    }
	
	// Update is called once per frame
	void Update () {
	    if(_particle.isPlaying)
        {
            _currentAnimTime -= Time.deltaTime;
            if(_currentAnimTime <= 0)
                _particle.Stop();
        }
	}

    public void Fire()
    {
        if(!_particle.isPlaying)
            _particle.Play();

        _currentAnimTime = AnimationTime;
    }
}
