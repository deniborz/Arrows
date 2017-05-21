using UnityEngine;
using System.Collections;

public class SpawnPointScript : MonoBehaviour {

    private bool _isUnlocked = false;
    public bool IsUnlocked
    {
        get
        {
            return _isUnlocked;
        }
    }
    public Animator BlockPathAnim = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Unlock()
    {
        _isUnlocked = true;
        BlockPathAnim.Play("OpenPath");
    }
}
