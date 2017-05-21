using UnityEngine;
using System.Collections;

public class LevelBoundScript : MonoBehaviour
{
    
	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            other.GetComponent<PoolObject>().Deactivate(); // just clean it up to lower need of new objects
        }
    }
}
