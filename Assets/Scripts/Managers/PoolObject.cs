using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour
{
    public bool IsActive
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public virtual void Reset()
    {
        transform.position = Vector3.zero;
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        Reset();
        this.gameObject.SetActive(false);
    }
}
